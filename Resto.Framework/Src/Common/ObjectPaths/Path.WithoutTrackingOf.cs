using System;
using System.Reactive.Disposables;
using System.Windows;
using Resto.Framework.Common.ObjectPaths;

namespace Resto.Framework.Src.Common.ObjectPaths
{
    public static class PathExtensions
    {
        public static IDisposable WithoutTrackingOf(this ObjectPath.Path path, DependencyObject dependencyObject)
        {
            path.StopTrackingChanges(dependencyObject);
            return Disposable.Create(() => path.StartTrackingChanges(dependencyObject));
        }
    }
}
