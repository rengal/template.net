using System;
using System.Collections.Generic;
using System.Reflection;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Data
{
    public class EntitiesRegistryBase
    {
        private static readonly Dictionary<string, Type> classes = new Dictionary<string, Type>();
        private static readonly Dictionary<Type, string> classNames = new Dictionary<Type, string>();

        static EntitiesRegistryBase()
        {
            AddType(typeof(Pair));
        }

        public static void AddType(Type type)
        {
            var className = ComputeClassName(type);
            AddType(type, className);
        }

        public static void AddType(Type type, string className)
        {
            classes[className] = type;
            classNames[type] = className;
        }

        public static void AddTypeWithFullName(Type type)
        {
            AddType(type, type.FullName);
        }

        public static void AddTypeWithFullName<T>()
        {
            AddTypeWithFullName(typeof(T));
        }

        public static Type GetType(string className)
        {
            if (classes.TryGetValue(className, out var type))
            {
                return type;
            }

            type = Type.GetType(className);
            if (type == null)
            {
                throw new ArgumentException("Undefined class name: " + className);
            }
            return type;
        }

        public static string GetClassName(Type type)
        {
            if (classNames.TryGetValue(type, out var className))
            {
                return className;
            }

            if (type.IsGenericType && classNames.ContainsKey(type.GetGenericTypeDefinition()))
            {
                return classNames[type.GetGenericTypeDefinition()];
            }

            var assemblyName = type.Assembly.FullName;
            return type.FullName + (assemblyName != null ? ", " + assemblyName.Substring(0, assemblyName.IndexOf(",")) : string.Empty);
        }

        private static string ComputeClassName([NotNull] Type type)
        {
            var attribute = type.GetCustomAttribute<DataClassAttribute>(false);

            if (attribute != null)
                return attribute.Name;
            throw new ArgumentException("Attribute [DataClass] must be defined for " + type);
        }
    }
}