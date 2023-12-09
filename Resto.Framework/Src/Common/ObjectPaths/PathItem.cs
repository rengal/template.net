using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using Resto.Framework.Attributes.JetBrains;
using System.Linq;

namespace Resto.Framework.Common.ObjectPaths
{
    public static partial class ObjectPath
    {
        private abstract class PathItem
        {
            #region Fields
            private readonly HashSet<PathItem> children = new HashSet<PathItem>();

            private readonly HashSetMultiDictionary<DependencyObject, DependencyObject> childToParentObjects =
                new HashSetMultiDictionary<DependencyObject, DependencyObject>();

            private readonly CountingHashSet<DependencyObject> registeredObjects = new CountingHashSet<DependencyObject>();

            private Action<DependencyObject> dataChangedCallback;
            #endregion

            #region Props
            internal PathItem Parent { get; private set; }

            internal HashSet<PathItem> Children
            {
                get { return children; }
            }

            private bool HasParent
            {
                get { return Parent != null; }
            }

            protected bool HasChildren
            {
                get { return Children.Count > 0; }
            }
            #endregion

            #region Methods
            internal abstract PathItem Clone();

            protected abstract void SubscribeForChanges([NotNull] DependencyObject dependencyObject);
            protected abstract void UnsubscribeFromChanges([NotNull] DependencyObject dependencyObject);
            protected abstract IEnumerable<DependencyObject> GetChildrenObjects([NotNull] DependencyObject dependencyObject);

            protected void OnDataChanged([NotNull] DependencyObject dependencyObject)
            {
                Debug.Assert(dependencyObject != null);

                if (!HasParent)
                {
                    dataChangedCallback(dependencyObject);
                }
                else
                {
                    var parentObjects = Parent.childToParentObjects[dependencyObject];
                    if (parentObjects.Count == 1)
                        Parent.OnDataChanged(parentObjects.Single());
                    else
                        Parent.OnDataChanged(parentObjects);
                }
            }

            protected void OnDataChanged([NotNull, InstantHandle] IEnumerable<DependencyObject> dependencyObjects)
            {
                Debug.Assert(dependencyObjects != null);

                if (!HasParent)
                {
                    foreach (var dependencyObject in dependencyObjects)
                    {
                        dataChangedCallback(dependencyObject);
                    }
                }
                else
                {
                    Parent.OnDataChanged(dependencyObjects.SelectMany(obj => Parent.childToParentObjects[obj]).Distinct());
                }
            }

            protected void StartChildrenTrackingChanges([NotNull] DependencyObject parentObject, [NotNull] DependencyObject childObject)
            {
                Debug.Assert(parentObject != null);
                Debug.Assert(childObject != null);
                Debug.Assert(HasChildren);

                childToParentObjects.Add(childObject, parentObject);
                foreach (var childPathItem in Children)
                {
                    childPathItem.StartTrackingChanges(childObject);
                }
            }

            protected void StopChildrenTrackingChanges([NotNull] DependencyObject parentObject, [NotNull] DependencyObject childObject)
            {
                Debug.Assert(parentObject != null);
                Debug.Assert(childObject != null);
                Debug.Assert(HasChildren);

                childToParentObjects.Remove(childObject, parentObject);
                foreach (var childPathItem in Children)
                {
                    childPathItem.StopTrackingChanges(childObject);
                }
            }

            internal void AddChild([NotNull] PathItem child)
            {
                Debug.Assert(child != null);
                Debug.Assert(!child.HasParent);
                Debug.Assert(!children.Contains(child));

                children.Add(child);
                child.Parent = this;
            }

            internal void StartTrackingChanges([NotNull] DependencyObject dependencyObject)
            {
                Debug.Assert(dependencyObject != null);

                if (registeredObjects.AddOne(dependencyObject) > 1)
                    return;

                SubscribeForChanges(dependencyObject);

                if (HasChildren)
                {
                    foreach (var childObject in GetChildrenObjects(dependencyObject))
                    {
                        StartChildrenTrackingChanges(dependencyObject, childObject);
                    }
                }
            }

            internal void StopTrackingChanges([NotNull] DependencyObject dependencyObject)
            {
                Debug.Assert(dependencyObject != null);

                if (registeredObjects.RemoveOne(dependencyObject) > 0)
                    return;

                if (HasChildren)
                {
                    foreach (var childObject in GetChildrenObjects(dependencyObject))
                    {
                        StopChildrenTrackingChanges(dependencyObject, childObject);
                    }
                }

                UnsubscribeFromChanges(dependencyObject);
            }

            internal void SetDataChangedCallback([NotNull] Action<DependencyObject> callback)
            {
                Debug.Assert(callback != null);

                dataChangedCallback = callback;
            }
            #endregion
        }
    }
}
