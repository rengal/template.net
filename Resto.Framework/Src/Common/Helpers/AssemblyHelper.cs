using System.Reflection;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    public static class AssemblyHelper
    {
        #region Shared Members

        private static readonly LogWrapper logWrapper = new LogWrapper(typeof(AssemblyHelper));

        #endregion Shared Members

        #region Methods

        /// <summary>
        /// Получить пользовательский атрибут для сборки
        /// </summary>
        /// <typeparam name="T">Тип атрибута</typeparam>
        /// <param name="assembly">Сборка</param>
        /// <returns>Атрибут</returns>
        [CanBeNull]
        public static T GetAssemblyCustomAttribute<T>(Assembly assembly) where T : Attribute
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }
            return assembly.GetCustomAttributes(typeof(T), false).Cast<T>().FirstOrDefault();
        }

        /// <summary>
        /// Получить версию сборки
        /// </summary>
        /// <param name="assembly">Сборка</param>
        /// <returns>Версия</returns>
        public static string GetAssemblyVersion(Assembly assembly)
        {
            if (assembly == null)
                return string.Empty;

            var attr = GetAssemblyCustomAttribute<AssemblyFileVersionAttribute>(assembly);

            return attr == null ? string.Empty : attr.Version;
        }

        /// <summary>
        /// Создать в текущем AppDomain экземпляр типа из заданой сборки 
        /// (должна раполагаться в одном каталоге с приложением)
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="assemblyName">Имя сборки</param>
        /// <param name="typeName">Имя типа</param>
        /// <returns>Типизированный экземпляр объекта</returns>
        public static T GetInstanceFrom<T>(string assemblyName, string typeName)
        {
            return GetInstanceFrom<T>(assemblyName, typeName, AppDomain.CurrentDomain);
        }

        private static T GetInstanceFrom<T>(string assemblyNameOrPath, string typeName, AppDomain domain, params object[] args)
        {
            // получаем путь к сборке
            var assemblyPath = assemblyNameOrPath;
            if (!File.Exists(assemblyNameOrPath))
            {
                assemblyPath = AppDomain.CurrentDomain.BaseDirectory + assemblyNameOrPath;
            }
            // если сборки нет - генерируем исключение
            if (!File.Exists(assemblyPath))
                throw new RestoException($"Assembly not found: '{assemblyPath}'");

            if (!AppDomain.CurrentDomain.GetAssemblies().Any(assembly => !assembly.IsDynamic && StringComparer.OrdinalIgnoreCase.Equals(assembly.Location, assemblyPath)))
                logWrapper.Log.DebugFormat("Try to load assembly {0} and create instance of {1}", assemblyPath, typeName);

            // получаем экземпляр объекта
            const BindingFlags constructorDefault = BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance;
            var instance = domain.CreateInstanceFromAndUnwrap(assemblyPath, typeName, false, constructorDefault, null, args, null, null);

            if (instance == null)
                throw new RestoException($"Can not create instance of type '{typeName}' from assembly '{assemblyPath}'");

            return (T)instance;
        }

        #endregion Methods
    }
}