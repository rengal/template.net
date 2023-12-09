using System;
using System.Diagnostics;
using System.Windows;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.ObjectPaths
{
    /// <summary>
    /// Делегат для события <see cref="ObjectPath.Path.PathDataChanged"/>.
    /// </summary>
    /// <param name="path">Путь, в котором произошло изменение данных</param>
    /// <param name="dependencyObject">Корневой объект, от которого идёт путь и для данных которого произошло изменение</param>
    public delegate void PathDataChangedHandler(ObjectPath.Path path, DependencyObject dependencyObject);

    public static partial class ObjectPath
    {
        public abstract class Path
        {
            #region Fields
            private CountingHashSet<PathDataChangedHandler> handlers = new CountingHashSet<PathDataChangedHandler>(DelegateComparer<PathDataChangedHandler>.Instance);
            /// <summary>
            /// Показывает, читает ли кто-нибудь в данный момент коллекцию handlers, и если да, то сколько активных читателей.
            /// Если значение равно нулю, коллекцию handlers можно менять. Иначе необходимо создать копию и вносить изменения в копию.
            /// </summary>
            private int handlersReadersCount;
            #endregion

            #region Methods
            protected bool Subscribe([NotNull] PathDataChangedHandler handler)
            {
                if (handler == null)
                    throw new ArgumentNullException(nameof(handler));

                if (handlersReadersCount > 0) // полагаем, что добавление подписчика в рамках вызова другого подписчика — большая редкость
                {
                    handlers = new CountingHashSet<PathDataChangedHandler>(handlers, DelegateComparer<PathDataChangedHandler>.Instance);
                    handlersReadersCount = 0;
                }

                var isEmpty = handlers.Count == 0;

                handlers.AddOne(handler);

                return isEmpty;
            }

            protected bool UnSubscribe([NotNull] PathDataChangedHandler handler)
            {
                if (handler == null)
                    throw new ArgumentNullException(nameof(handler));

                if (handlersReadersCount > 0) // полагаем, что удаление подписчика в рамках вызова другого подписчика — большая редкость
                {
                    handlers = new CountingHashSet<PathDataChangedHandler>(handlers, DelegateComparer<PathDataChangedHandler>.Instance);
                    handlersReadersCount = 0;
                }

                handlers.RemoveOne(handler);

                return handlers.Count == 0;
            }

            protected void OnPathDataChanged([NotNull] DependencyObject dependencyObject)
            {
                Debug.Assert(dependencyObject != null);

                handlersReadersCount++;
                var handlersCopy = handlers;

                foreach (var handler in handlersCopy)
                {
                    handler(this, dependencyObject);
                }

                if (handlers == handlersCopy)
                    handlersReadersCount--;
            }
            #endregion

            #region Public Methods
            /// <summary>
            /// Начинает отслеживать изменения данных в пути для объекта <paramref name="dependencyObject"/>.<br/>
            /// Метод для одного и того же объекта можно вызывать несколько раз;
            /// для каждого вызова необходим парный вызов <see cref="StopTrackingChanges"/>.
            /// </summary>
            /// <param name="dependencyObject">
            /// Объект, для которого нужно начать отслеживать изменения.
            /// </param>
            /// <remarks>
            /// Отслеживание изменений начинается при первом вызове метода для объекта.
            /// При повторном вызове с тем же объектом только увеличивается счётчик для объекта.
            /// </remarks>
            /// <seealso cref="StopTrackingChanges"/>
            public abstract void StartTrackingChanges([NotNull] DependencyObject dependencyObject);

            /// <summary>
            /// Прекращает отслеживать изменения данных в пути для объекта <paramref name="dependencyObject"/>.<br/>
            /// Метод для одного и того же объекта нужно вызывать столько раз,
            /// сколько для этого объекта вызывался метод <see cref="StopTrackingChanges"/>.
            /// </summary>
            /// <param name="dependencyObject">
            /// Объект, для которого нужно прекратить отслеживание изменений.
            /// </param>
            /// <remarks>
            /// Отслеживание изменений прекращается при вызове, парном первому вызову <see cref="StartTrackingChanges"/>.
            /// </remarks>
            /// <seealso cref="StartTrackingChanges"/>
            public abstract void StopTrackingChanges([NotNull] DependencyObject dependencyObject);
            #endregion

            #region Events
            /// <summary>
            /// Событие, генерируемое в момент изменения данных в отслеживаемом пути
            /// </summary>
            public virtual event PathDataChangedHandler PathDataChanged
            {
                add => Subscribe(value);
                remove => UnSubscribe(value);
            }
            #endregion

            #region Operators
            public static implicit operator Path(DependencyProperty dependencyProperty)
            {
                return LinearPath.CreateLinearPath(dependencyProperty);
            }
            #endregion
        }
    }
}
