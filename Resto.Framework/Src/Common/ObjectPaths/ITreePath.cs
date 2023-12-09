using System.Windows;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.ObjectPaths
{
    public static partial class ObjectPath
    {
        private interface ITreePath
        {
            PathItem Root { get; }

            void StartTrackingChanges([NotNull] DependencyObject dependencyObject);
            void StopTrackingChanges([NotNull] DependencyObject dependencyObject);
            event PathDataChangedHandler PathDataChanged;
        }
    }
}
