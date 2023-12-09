using System;
using System.Threading;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    /// <summary>
    /// Класс для определения уникальности экземпляра приложения.
    /// Может определять уникальность как для текущей терминальной сессии, так и для всех запущенных.
    /// Для одного приложения может вызываться только с одним именем
    /// </summary>
    public static class SingleInstance
    {
        private static readonly object syncObj = new object();

        // Экземпяр мьютекса. Уничтожается системой после закрытия процесса.
        private static Mutex mutex;

        // Имя экземпляра мьютекса, созданного для данной копии приложения.
        private static string mutexName;

        /// <summary>
        /// Является ли запускаемая копия приложения первой?
        /// </summary>
        /// <param name="uniqueInstanceName">Уникальное имя (идентификатор) приложения</param>
        /// <param name="instanceIsVisibleForAllTerminalSessions">true - искать во всех терминальных сессиях / false - искать только в текущей терминальной сессии</param>
        /// <returns>true/false</returns>
        public static bool IsFirstInstance(string uniqueInstanceName, bool instanceIsVisibleForAllTerminalSessions)
        {
            // в зависимости от параметров поиска формируем имя мьютекса для глобальной или локальной области видимости
            // (во всех терминальных сессиях или только в текущей)
            var newMutexName = Win32KernelObjectsHelper.GetKernelObjectName(uniqueInstanceName,
                                                                            instanceIsVisibleForAllTerminalSessions);
            lock (syncObj)
            {
                // если метод вызыван для другого имени мьютекса или с другой областью видимости, 
                // что противоречит логике использования данного класса - падаем с исключением
                if (!string.IsNullOrEmpty(mutexName) && string.Compare(mutexName, newMutexName, false) != 0)
                {
                    throw new ArgumentException("Must be unique for application", nameof(uniqueInstanceName));
                }

                // если мьютекс определен - это не первый вызов метода и проверки были пройдены ранее
                if (mutex != null) return true;

                mutexName = newMutexName;

                bool isNew;
                try
                {
                    // пытаемся открыть/создать мьютекс с полными правами доступа для всех авторизованных пользователей
                    mutex = new Mutex(true, mutexName, out isNew,
                                      Win32KernelObjectsHelper.CreateFullAccessMutexSecurity());
                    if (!isNew)
                    {
                        // если он не новый - значит уже есть запущенная копия. Закрываем свой хэндл
                        mutex.Close();
                        mutex = null;
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    // так может быть, если другое приложение создало одноименный мьютекс с ограниченными правами доступа
                    return false;
                }
                return isNew;
            }
        }

        /// <summary>
        /// Является ли запускаемая копия приложения первой? Поиск осуществляется во всех терминальных сессиях
        /// </summary>
        /// <param name="uniqueInstanceName">Уникальное имя (идентификатор) приложения</param>
        /// <returns>true/false</returns>
        public static bool IsFirstInstance(string uniqueInstanceName)
        {
            return IsFirstInstance(uniqueInstanceName, true);
        }

        /// <summary>
        /// Заблокировать текущий поток до освобождения семафора с указанным именем.
        /// </summary>
        /// <param name="uniqueInstanceName">Имя семафора.</param>
        public static void Wait([NotNull] string uniqueInstanceName)
        {
            if (uniqueInstanceName == null)
                throw new ArgumentNullException(nameof(uniqueInstanceName));

            try
            {
                using (var waitingMutex = new Mutex(true, Win32KernelObjectsHelper.GetKernelObjectName(uniqueInstanceName, true)))
                {
                    waitingMutex.WaitOne();
                }
            }
            catch (AbandonedMutexException)
            {
                // исключение генерируется, когда предыдущий владелец ресурса завершает работу, не освободив ресурс явно
                // игнорируем исключение, поскольку:
                // - предыдущий экземпляр при завершении работы не заботится о «потерянном» ресурсе
                // - предыдущий экземпляр мог завершить работу аварийно
            }
        }
    }
}