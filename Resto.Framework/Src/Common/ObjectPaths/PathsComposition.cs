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
        private sealed class PathsComposition : Path
        {
            #region Fields
            private readonly HashSet<ITreePath> paths;
            #endregion

            #region Ctors
            private PathsComposition([NotNull] IEnumerable<ITreePath> paths)
            {
                Debug.Assert(paths != null);

                this.paths = new HashSet<ITreePath>(paths);
            }
            #endregion

            #region Events
            public override event PathDataChangedHandler PathDataChanged
            {
                add
                {
                    if (!Subscribe(value))
                        return;

                    foreach (var path in paths)
                    {
                        path.PathDataChanged += SomePathDataChanged;
                    }
                }
                remove
                {
                    if (!UnSubscribe(value))
                        return;

                    foreach (var path in paths)
                    {
                        path.PathDataChanged -= SomePathDataChanged;
                    }
                }
            }
            #endregion

            #region Public Methods
            public override void StartTrackingChanges([NotNull] DependencyObject dependencyObject)
            {
                if (dependencyObject == null)
                    throw new ArgumentNullException(nameof(dependencyObject));

                foreach (var path in paths)
                {
                    path.StartTrackingChanges(dependencyObject);
                }
            }

            public override void StopTrackingChanges([NotNull] DependencyObject dependencyObject)
            {
                if (dependencyObject == null)
                    throw new ArgumentNullException(nameof(dependencyObject));

                foreach (var path in paths)
                {
                    path.StopTrackingChanges(dependencyObject);
                }
            }
            #endregion

            #region Methods
            private void SomePathDataChanged(Path path, DependencyObject dependencyObject)
            {
                OnPathDataChanged(dependencyObject);
            }

            [NotNull]
            internal static Path MergePaths([NotNull] IEnumerable<Path> paths)
            {
                Debug.Assert(paths != null);

                var rootPaths = paths
                    .OfType<ITreePath>().Concat(paths.OfType<PathsComposition>().SelectMany(pathsComposition => pathsComposition.paths))
                    .Select(tp => tp.Root)
                    .GroupBy(pi => pi)
                    .Select(group => MergeChildren(group.Key.Clone(), group.SelectMany(pi => pi.Children)))
                    .Select(pi => new TreePath(pi));

                return new PathsComposition(rootPaths);
            }

            private static PathItem MergeChildren([NotNull] PathItem parent, [NotNull] IEnumerable<PathItem> children)
            {
                Debug.Assert(parent != null);
                Debug.Assert(children != null);

                if (!children.Any())
                    return parent;

                var mergedChildren = children
                    .GroupBy(pi => pi)
                    .Select(group => MergeChildren(group.Key.Clone(), group.SelectMany(pi => pi.Children)));

                foreach (var child in mergedChildren)
                {
                    parent.AddChild(child);
                }

                return parent;
            }
            #endregion
        }
    }
}
