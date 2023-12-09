using System;
using System.Diagnostics;
using System.Windows;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.ObjectPaths
{
    public static partial class ObjectPath
    {
        private sealed class TreePath : Path, ITreePath
        {
            #region Fields
            private readonly PathItem root;
            #endregion

            #region Ctors
            internal TreePath([NotNull] PathItem root)
            {
                Debug.Assert(root != null);
                Debug.Assert(root.Parent == null);

                this.root = root;
                root.SetDataChangedCallback(OnPathDataChanged);
            }
            #endregion

            #region Props
            PathItem ITreePath.Root
            {
                get { return root; }
            }
            #endregion

            #region Methods
            public override void StartTrackingChanges([NotNull] DependencyObject dependencyObject)
            {
                if (dependencyObject == null)
                    throw new ArgumentNullException(nameof(dependencyObject));

                root.StartTrackingChanges(dependencyObject);
            }

            public override void StopTrackingChanges([NotNull] DependencyObject dependencyObject)
            {
                if (dependencyObject == null)
                    throw new ArgumentNullException(nameof(dependencyObject));

                root.StopTrackingChanges(dependencyObject);
            }
            #endregion
        }
    }
}
