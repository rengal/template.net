using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.ObjectPaths
{
    /// <summary>
    /// Фабрика для создания объектов, описывающих пути в объектной системе
    /// (от объекта через его поля/свойства к другим объектам/коллекциям; через <see cref="DependencyProperty"/> к значениям/объектам)
    /// и позволяет отслеживать изменение данных по этому пути для заданного объекта.
    /// </summary>
    public static partial class ObjectPath
    {
        /// <summary>
        /// Создаёт путь от объекта типа <typeparamref name="T"/> к коллекции, получаемой вызовом <paramref name="collectionAccessor"/>.<br/>
        /// Путь можно использовать для отслеживания изменений в коллекции.
        /// </summary>
        /// <typeparam name="T">
        /// Тип объекта или интерфейса, путь от которого ведёт к коллекции.
        /// Объект должен быть унаследован от <see cref="DependencyObject"/>.
        /// </typeparam>
        /// <param name="collectionAccessor">
        /// Функция, возвращающая коллекцию, изменения в которой будут отслеживаться.<br/>
        /// Коллекция должна реализовывать интерфейс <see cref="INotifyCollectionChanged"/> и <see cref="IEnumerable"/>.<br/>
        /// Коллекция может содержать <c>null</c> значения.
        /// </param>
        /// <returns>Путь от объекта типа <typeparamref name="T"/> к коллекции</returns>
        /// <remarks>
        /// Путь, создаваемый этим методом, отслеживает изменения в коллекции (см. <see cref="INotifyCollectionChanged"/>).<br/>
        /// Между вызовами <see cref="Path.StartTrackingChanges"/> и <see cref="Path.StopTrackingChanges"/> для объекта Obj,
        /// функция <paramref name="collectionAccessor"/> для объекта Obj должна всегда возвращать один и тот же экземпляр коллекции
        /// и не должна возвращать <c>null</c>.<br/>
        /// Если этот путь продолжать с помощью одного из методов <c>LinearPath.Combine</c>, элементами коллекции должны быть объекты,
        /// тип которых унаследован от <see cref="DependencyObject"/>.
        /// </remarks>
        public static LinearPath Create<T>([NotNull] Func<T, INotifyCollectionChanged> collectionAccessor)
        {
            return LinearPath.CreateLinearPath(collectionAccessor);
        }

        /// <summary>
        /// Создаёт путь от объекта (тип которого унаследован от <see cref="DependencyObject"/>)
        /// к значению <paramref name="dependencyProperty"/>
        /// </summary>
        /// <param name="dependencyProperty">DependecnyProperty, изменения значения которой будут отслеживаться</param>
        /// <returns>Путь от объекта к значению <paramref name="dependencyProperty"/></returns>
        /// <remarks>
        /// Путь, создаваемый этим методом, отслеживает изменения значения <paramref name="dependencyProperty"/>
        /// для объекта, задаваемого с помощью <see cref="Path.StartTrackingChanges"/>
        /// </remarks>
        public static LinearPath Create([NotNull] DependencyProperty dependencyProperty)
        {
            return LinearPath.CreateLinearPath(dependencyProperty);
        }

        /// <summary>
        /// Создаёт путь от объекта типа <typeparamref name="T"/> к объекту, получаемой вызовом <paramref name="objectAccessor"/>.<br/>
        /// Данный путь не предназначен для отслеживания изменений и должен использоваться только как промежуточное звено к другому пути.
        /// </summary>
        /// <typeparam name="T">
        /// Тип объекта или интерфейса, путь от которого ведёт к объекту.
        /// Объект должен быть унаследован от <see cref="DependencyObject"/>.
        /// </typeparam>
        /// <param name="objectAccessor">
        /// Функция, возвращающая объект, тип которого унаследован от <see cref="DependencyObject"/>
        /// </param>
        /// <returns>
        /// Путь, создаваемый этим методом, никаких изменений не отслеживает.
        /// Он используется только как промежуточное звено к другому пути.
        /// Для него необходимо вызвать один из методов <c>LinearPath.Combine</c>.
        /// </returns>
        public static LinearPath Create<T>([NotNull] Func<T, object> objectAccessor)
        {
            return LinearPath.CreateLinearPath(objectAccessor);
        }

        /// <summary>
        /// Extension-метод для <see cref="Create"/>
        /// </summary>
        public static LinearPath CreatePath([NotNull] this DependencyProperty dependencyProperty)
        {
            return Create(dependencyProperty);
        }

        /// <summary>
        /// Объединяет заданные пути, ведущие от одного и того же типа, в один составной путь.
        /// </summary>
        /// <returns>Составной путь</returns>
        /// <remarks>
        /// Данный метод предназначен для объединения параллельных путей, начинающихся от одного и того же типа.<br/>
        /// Для построения последовательных путей используйте семейство методов <c>LinearPath.Combine</c>.
        /// </remarks>
        /// <seealso cref="Merge{T}(IEnumerable{T})"/>
        [NotNull]
        public static Path Merge([NotNull] this Path path1, [NotNull] Path path2, [NotNull] params Path[] paths)
        {
            if (paths == null)
                throw new ArgumentNullException(nameof(paths));

            return PathsComposition.MergePaths(paths.StartWith(path1, path2));
        }

        /// <summary>
        /// Объединяет заданные пути, ведущие от одного и того же типа, в один составной путь.
        /// </summary>
        /// <param name="paths"></param>
        /// <returns>Составной путь</returns>
        /// <remarks>
        /// Данный метод предназначен для объединения параллельных путей, начинающихся от одного и того же типа.<br/>
        /// Для построения последовательных путей используйте семейство методов <c>LinearPath.Combine</c>.
        /// </remarks>
        /// <seealso cref="Merge(Path,Path,Path[])"/>
        [NotNull]
        public static Path Merge<T>([NotNull] this IEnumerable<T> paths) where T : Path
        {
            return PathsComposition.MergePaths(paths);
        }

        public static IObservable<T> CreateObservableFor<T>([NotNull] this Path path, [NotNull] T trackingObject) where T : class
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            if (trackingObject == null)
                throw new ArgumentNullException(nameof(trackingObject));

            var depObj = trackingObject as DependencyObject;
            if (depObj == null)
                throw new ArgumentOutOfRangeException(nameof(trackingObject), trackingObject, "tracking object must be dependencyObject");

            return
                Observable.FromEventPattern<PathDataChangedHandler, EventArgs<DependencyObject>>(
                    handler => (sender, obj) => handler(sender, new EventArgs<DependencyObject>(obj)),
                    handler => { path.StartTrackingChanges(depObj); path.PathDataChanged += handler; },
                    handler => { path.StopTrackingChanges(depObj); path.PathDataChanged -= handler; })
                .Where(args => ReferenceEquals(args.EventArgs.Value, depObj))
                .Select(_ => trackingObject);
        }
    }
}