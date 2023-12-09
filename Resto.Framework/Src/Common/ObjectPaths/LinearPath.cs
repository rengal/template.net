using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.ObjectPaths
{
    public static partial class ObjectPath
    {
        public sealed class LinearPath : Path, ITreePath
        {
            #region Fields
            private readonly List<PathItem> pathItems;
            #endregion

            #region Props
            PathItem ITreePath.Root
            {
                get { return pathItems[0]; }
            }
            #endregion

            #region Ctors
            private LinearPath([NotNull] PathItem pathItem)
            {
                Debug.Assert(pathItem != null);

                pathItems = new List<PathItem>(1) { pathItem };
                pathItem.SetDataChangedCallback(OnPathDataChanged);
            }

            private LinearPath([NotNull] LinearPath linearPath, [NotNull] PathItem pathItem)
            {
                Debug.Assert(linearPath != null);
                Debug.Assert(pathItem != null);
                Debug.Assert(linearPath.pathItems.First().Parent == null);
                Debug.Assert(linearPath.pathItems.Last().Children.Count == 0);

                pathItems = new List<PathItem>(linearPath.pathItems.Count + 1);
                pathItems.AddRange(linearPath.pathItems.Select(pi => pi.Clone()));
                pathItems.Add(pathItem);

                for (var i = 0; i < pathItems.Count - 1; i++)
                {
                    pathItems[i].AddChild(pathItems[i + 1]);
                }

                pathItems[0].SetDataChangedCallback(OnPathDataChanged);
            }
            #endregion

            #region Public Methods
            public override void StartTrackingChanges([NotNull] DependencyObject dependencyObject)
            {
                if (dependencyObject == null)
                    throw new ArgumentNullException(nameof(dependencyObject));

                pathItems[0].StartTrackingChanges(dependencyObject);
            }

            public override void StopTrackingChanges([NotNull] DependencyObject dependencyObject)
            {
                if (dependencyObject == null)
                    throw new ArgumentNullException(nameof(dependencyObject));

                pathItems[0].StopTrackingChanges(dependencyObject);
            }

            public LinearPath Combine<T>([NotNull] Func<T, INotifyCollectionChanged> collectionAccessor)
            {
                if (collectionAccessor == null)
                    throw new ArgumentNullException(nameof(collectionAccessor));

                return new LinearPath(this, new DependencyObjectToINotifyCollectionChangedPathItem<T>(collectionAccessor));
            }

            public LinearPath Combine([NotNull] DependencyProperty dependencyProperty)
            {
                if (dependencyProperty == null)
                    throw new ArgumentNullException(nameof(dependencyProperty));

                return new LinearPath(this, new DependencyObjectToDependencyPropertyPathItem(dependencyProperty));
            }

            public LinearPath Combine<T>([NotNull] Func<T, object> objectAccessor)
            {
                if (objectAccessor == null)
                    throw new ArgumentNullException(nameof(objectAccessor));

                return new LinearPath(this, new DependencyObjectToDependencyObjectPathItem<T>(objectAccessor));
            }
            #endregion

            #region Creation Methods
            internal static LinearPath CreateLinearPath<T>([NotNull] Func<T, INotifyCollectionChanged> collectionAccessor)
            {
                Debug.Assert(collectionAccessor != null);

                return new LinearPath(new DependencyObjectToINotifyCollectionChangedPathItem<T>(collectionAccessor));
            }

            internal static LinearPath CreateLinearPath([NotNull] DependencyProperty dependencyProperty)
            {
                Debug.Assert(dependencyProperty != null);

                return new LinearPath(new DependencyObjectToDependencyPropertyPathItem(dependencyProperty));
            }

            internal static LinearPath CreateLinearPath<T>([NotNull] Func<T, object> objectAccessor)
            {
                Debug.Assert(objectAccessor != null);

                return new LinearPath(new DependencyObjectToDependencyObjectPathItem<T>(objectAccessor));
            }

            public static implicit operator LinearPath(DependencyProperty dependencyProperty)
            {
                return CreateLinearPath(dependencyProperty);
            }
            #endregion
        }
    }
}