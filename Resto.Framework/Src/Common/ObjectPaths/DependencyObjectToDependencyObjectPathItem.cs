using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.ObjectPaths
{
    public static partial class ObjectPath
    {
        private sealed class DependencyObjectToDependencyObjectPathItem<T> : PathItem
        {
            private const string ValueIsNotDependencyObject =
                "Value returned by object accessor is not dependency object.";

            #region Fields
            private readonly Func<T, object> objectAccessor;

#if DEBUG
            private readonly Dictionary<DependencyObject, DependencyObject> parentToChild =
                new Dictionary<DependencyObject, DependencyObject>();
#endif
            #endregion

            #region Ctor
            internal DependencyObjectToDependencyObjectPathItem([NotNull] Func<T, object> objectAccessor) 
            {
                Debug.Assert(objectAccessor != null);

                this.objectAccessor = objectAccessor;
            }
            #endregion

            #region Methods
            internal override PathItem Clone()
            {
                return new DependencyObjectToDependencyObjectPathItem<T>(objectAccessor);
            }

            protected override void SubscribeForChanges([NotNull] DependencyObject dependencyObject)
            {
                Debug.Assert(dependencyObject != null);

                if (!HasChildren)
                    throw new InvalidOperationException("Path too short. This path must be combined with other path.");

#if DEBUG
                parentToChild.Add(dependencyObject, GetChildObject(dependencyObject));
#endif
            }

            protected override void UnsubscribeFromChanges([NotNull] DependencyObject dependencyObject)
            {
                Debug.Assert(dependencyObject != null);

#if DEBUG
                Debug.Assert(ReferenceEquals(parentToChild[dependencyObject], GetChildObject(dependencyObject)),
                             "Object accessor returned invalid object",
                             "Object accessor must return same object beetwen SubscribeForChanges and UnsubscribeFromChanges methods call.");

                parentToChild.Remove(dependencyObject);
#endif
            }

            protected override IEnumerable<DependencyObject> GetChildrenObjects([NotNull] DependencyObject dependencyObject)
            {
                Debug.Assert(dependencyObject != null);
                Debug.Assert(HasChildren);

                return GetChildObject(dependencyObject).AsSequence();
            }

            private DependencyObject GetChildObject([NotNull] DependencyObject dependencyObject)
            {
                Debug.Assert(dependencyObject != null);

                T obj;
                try
                {
                    obj = (T)(object)dependencyObject;
                }
                catch (InvalidCastException castException)
                {
                    throw new Exception(string.Format("Cannot convert dependencyObject to type '{0}'.", typeof(T)),
                                        castException);
                }

                var childObject = objectAccessor(obj);

                if (childObject == null)
                    throw new Exception("Object accessor return null.");

                DependencyObject childDependencyObject;
                try
                {
                    childDependencyObject = (DependencyObject)childObject;
                }
                catch (InvalidCastException castException)
                {
                    throw new Exception(ValueIsNotDependencyObject, castException);
                }

                return childDependencyObject;
            }
            #endregion

            #region Equility Members
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                    return false;
                if (ReferenceEquals(this, obj))
                    return true;
                if (obj.GetType() != typeof(DependencyObjectToDependencyObjectPathItem<T>))
                    return false;
                return Equals(((DependencyObjectToDependencyObjectPathItem<T>)obj).objectAccessor, objectAccessor);
            }

            public override int GetHashCode()
            {
                return objectAccessor.GetHashCode();
            }
            #endregion
        }
    }
}