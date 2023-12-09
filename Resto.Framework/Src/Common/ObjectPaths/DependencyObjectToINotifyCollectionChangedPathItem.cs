using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Resto.Framework.Attributes.JetBrains;
using Wintellect.PowerCollections;

namespace Resto.Framework.Common.ObjectPaths
{
    public static partial class ObjectPath
    {
        private sealed class DependencyObjectToINotifyCollectionChangedPathItem<T> : PathItem
        {
            private const string CollectionElementIsNotDependencyObject =
                "Value of collection element is not dependency object. Path too long. Some of 'Combine()' method call was invalid.";

            #region Fields
            private readonly Func<T, INotifyCollectionChanged> collectionAccessor;
            private readonly NotifyCollectionChangedEventHandler notifyCollectionChangedEventHandler;

            private HashSetMultiDictionary<INotifyCollectionChanged, DependencyObject> collectionToObjects = new HashSetMultiDictionary<INotifyCollectionChanged, DependencyObject>();
            /// <summary>
            /// Показывает, читает ли кто-нибудь в данный момент коллекцию collectionToObjects, и если да, то сколько активных читателей.
            /// Если значение равно нулю, коллекцию collectionToObjects можно менять. Иначе необходимо создать копию и вносить изменения в копию.
            /// </summary>
            private int collectionToObjectsReadersCount;

            private readonly Dictionary<INotifyCollectionChanged, Bag<DependencyObject>> collectionToCollectionItems = new Dictionary<INotifyCollectionChanged, Bag<DependencyObject>>();

#if DEBUG
            private readonly Dictionary<DependencyObject, INotifyCollectionChanged> parentToChild = new Dictionary<DependencyObject, INotifyCollectionChanged>();
#endif
            #endregion

            #region Ctor
            internal DependencyObjectToINotifyCollectionChangedPathItem([NotNull] Func<T, INotifyCollectionChanged> collectionAccessor)
            {
                Debug.Assert(collectionAccessor != null);

                this.collectionAccessor = collectionAccessor;
                notifyCollectionChangedEventHandler = OnCollectionChanged;
            }
            #endregion

            #region Methods
            internal override PathItem Clone()
            {
                return new DependencyObjectToINotifyCollectionChangedPathItem<T>(collectionAccessor);
            }

            protected override void SubscribeForChanges([NotNull] DependencyObject dependencyObject)
            {
                Debug.Assert(dependencyObject != null);

                if (collectionToObjectsReadersCount > 0) // полагаем, что добавление подписчика в рамках вызова другого подписчика — большая редкость
                {
                    collectionToObjects = new HashSetMultiDictionary<INotifyCollectionChanged, DependencyObject>(collectionToObjects);
                    collectionToObjectsReadersCount = 0;
                }

                var collection = GetCollection(dependencyObject);

#if DEBUG
                parentToChild.Add(dependencyObject, collection);
#endif

                var hasSubscription = collectionToObjects.ContainsKey(collection);
                collectionToObjects.Add(collection, dependencyObject);
                if (hasSubscription)
                    return;

                if (HasChildren)
                    collectionToCollectionItems.Add(collection, new Bag<DependencyObject>(EnumerateCollection(collection)));

                collection.CollectionChanged += notifyCollectionChangedEventHandler;
            }

            protected override void UnsubscribeFromChanges([NotNull] DependencyObject dependencyObject)
            {
                Debug.Assert(dependencyObject != null);

                if (collectionToObjectsReadersCount > 0) // полагаем, что добавление подписчика в рамках вызова другого подписчика — большая редкость
                {
                    collectionToObjects = new HashSetMultiDictionary<INotifyCollectionChanged, DependencyObject>(collectionToObjects);
                    collectionToObjectsReadersCount = 0;
                }

                var collection = GetCollection(dependencyObject);

#if DEBUG
                Debug.Assert(ReferenceEquals(parentToChild[dependencyObject], collection),
                             "Collection accessor returned invalid collection",
                             "Collection accessor must return same collection beetwen SubscribeForChanges and UnsubscribeFromChanges methods call.");

                parentToChild.Remove(dependencyObject);
#endif

                collectionToObjects.Remove(collection, dependencyObject);
                if (collectionToObjects.ContainsKey(collection))
                    return;

                if (HasChildren)
                    collectionToCollectionItems.Remove(collection);

                collection.CollectionChanged -= notifyCollectionChangedEventHandler;
            }

            protected override IEnumerable<DependencyObject> GetChildrenObjects([NotNull] DependencyObject dependencyObject)
            {
                Debug.Assert(dependencyObject != null);
                Debug.Assert(HasChildren);

                return collectionToCollectionItems[GetCollection(dependencyObject)];
            }

            private INotifyCollectionChanged GetCollection([NotNull] DependencyObject dependencyObject)
            {
                Debug.Assert(dependencyObject != null);

                T obj;
                try
                {
                    obj = (T)((object)dependencyObject);
                }
                catch (InvalidCastException castException)
                {
                    throw new Exception(string.Format("Cannot convert dependencyObject to type '{0}'.", typeof(T)),
                                        castException);
                }

                var collection = collectionAccessor(obj);

                if (collection == null)
                    throw new Exception("Collection accessor return null.");

                return collection;
            }

            private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
            {
                collectionToObjectsReadersCount++;
                var collectionToObjectsCopy = collectionToObjects;

                var collection = (INotifyCollectionChanged)sender;
                var referencingObjects = collectionToObjectsCopy[collection];

                Debug.Assert(referencingObjects.Count > 0);

                if (args.Action != NotifyCollectionChangedAction.Move && HasChildren)
                    ProcessCollectionChanged(collection, referencingObjects, args);

                if (referencingObjects.Count == 1)
                    OnDataChanged(referencingObjects.Single());
                else
                    OnDataChanged(referencingObjects);

                if (collectionToObjects == collectionToObjectsCopy)
                    collectionToObjectsReadersCount--;
            }

            private void ProcessCollectionChanged([NotNull] INotifyCollectionChanged collection,
                [NotNull] IReadOnlyCollection<DependencyObject> referencingObjects, NotifyCollectionChangedEventArgs args)
            {
                Debug.Assert(HasChildren);

                var collectionItems = collectionToCollectionItems[collection];

                if (!NewOrOldItemsInfoExists(args))
                {
                    foreach (var childObject in collectionItems)
                    {
                        foreach (var parentObject in referencingObjects)
                        {
                            StopChildrenTrackingChanges(parentObject, childObject);
                        }
                    }

                    collectionItems.Clear();

                    foreach (var childObject in EnumerateCollection(collection))
                    {
                        collectionItems.Add(childObject);
                        foreach (var parentObject in referencingObjects)
                        {
                            StartChildrenTrackingChanges(parentObject, childObject);
                        }
                    }

                    return;
                }

                if (args.OldItems != null)
                {
                    // Приведение типа безопасно, так как тип значения уже проверялся при подписке
                    foreach (var childObject in args.OldItems.Cast<DependencyObject>().Where(item => item != null))
                    {
                        collectionItems.Remove(childObject);
                        foreach (var parentObject in referencingObjects)
                        {
                            StopChildrenTrackingChanges(parentObject, childObject);
                        }
                    }
                }

                if (args.NewItems != null)
                {
                    foreach (var childObject in args.NewItems.Cast<object>().Where(item => item != null).Select(CastWithCheck))
                    {
                        collectionItems.Add(childObject);
                        foreach (var parentObject in referencingObjects)
                        {
                            StartChildrenTrackingChanges(parentObject, childObject);
                        }
                    }
                }
            }
            #endregion

            #region Equility Members
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                    return false;
                if (ReferenceEquals(this, obj))
                    return true;
                if (obj.GetType() != typeof(DependencyObjectToINotifyCollectionChangedPathItem<T>))
                    return false;
                return Equals(((DependencyObjectToINotifyCollectionChangedPathItem<T>)obj).collectionAccessor, collectionAccessor);
            }

            public override int GetHashCode()
            {
                return collectionAccessor.GetHashCode();
            }
            #endregion

            #region Helpers
            private static bool NewOrOldItemsInfoExists(NotifyCollectionChangedEventArgs args)
            {
                return (args.OldItems != null && args.OldItems.Count > 0) ||
                       (args.NewItems != null && args.NewItems.Count > 0);
            }

            private static IEnumerable<DependencyObject> EnumerateCollection([NotNull] INotifyCollectionChanged collection)
            {
                Debug.Assert(collection != null);

                return from object obj in (IEnumerable)collection
                       where obj != null
                       select CastWithCheck(obj);
            }

            private static DependencyObject CastWithCheck([NotNull] object obj)
            {
                Debug.Assert(obj != null);

                try
                {
                    return (DependencyObject)obj;
                }
                catch (InvalidCastException castException)
                {
                    throw new Exception(CollectionElementIsNotDependencyObject, castException);
                }
            }
            #endregion
        }
    }
}