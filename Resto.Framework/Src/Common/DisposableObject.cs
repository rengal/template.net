using System;
using System.Diagnostics;

namespace Resto.Framework.Common
{
    /// <summary>
    /// Базовый класс для поддержки детерменированного уничтожения объектов
    /// </summary>
    [DebuggerStepThrough]
    public abstract class DisposableObject : MarshalByRefObject, IDisposable
    {

        private bool disposed;

        /// <summary>
        /// Вызывается для детерминированного уничтожения объекта
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                // Вызываем метод, реально выполняющий очистку.
                Dispose(true);

                // Поскольку очистка объекта выполняется явно,
                // запрещаем сборщику мусора вызов метода Finalize
                GC.SuppressFinalize(this);

                disposed = true;
            }
        }

        /// <summary>
        /// Этот открытый метод можно вызывать вместо Dispose
        /// </summary>
        public virtual void Close()
        {
            Dispose();
        }

        /// <summary>
        /// Метод освобождения неуправляемых ресурсов для реализации в потомках
        /// </summary>
        protected abstract void InternalDispose();

        /// <summary>
        /// Общий метод, реально выполняющий очистку.
        /// Его вызывают методы Finalize, Dispose и Close
        /// </summary>
        /// <param name="disposing">
        /// true - явное уничтожение/закрытие объекта; 
        /// false - неявное уничтожение при сборке мусора
        /// </param>
        protected void Dispose(bool disposing)
        {
            // Синхронизируем потоки для запрета одновременного вызова Dispose/Close
            lock (this)
            {
                if (disposing)
                {
                    // Здесь еще можно обращаться к полям, ссылающимся 
                    // на другие объекты - это безопасно для кода, так как для
                    // этих объектов метод Finalize еще не вызван
                    // Вызываем метод особождения ресурсов потомками
                    InternalDispose();
                }
            }
        }

        /// <summary>
        /// Возвращает true если объект уже был уничтожен
        /// </summary>
        public bool Disposed
        {
            get { return disposed; }
        }

        /// <summary>
        /// Генерировать исключение ObjectDisposedException если объект был уничтожен
        /// </summary>
        public void VerifyNotDisposed()
        {
            // если объект был уничтожен - генерируем исключение 
            // и передаем тип объекта в качестве сообщения
            if (Disposed)
                throw new ObjectDisposedException(GetType().ToString());
        }
    }
}
