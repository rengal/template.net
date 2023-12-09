using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Input;
using Resto.Framework.Attributes.JetBrains;
using Expression = System.Linq.Expressions.Expression;

namespace Resto.Framework.Common
{
    public static class ExpressionHelper
    {
        #region Constants

        private const string CommandSuffix = "Command";
        private const string EventSuffix = "Event";

        #endregion

        #region Parameterless Constructor
        public static Func<T> GetParameterlessCtorCall<T>([NotNull] ConstructorInfo ctorInfo)
            where T : class
        {
            var ctor = Expression.New(ctorInfo);
            var lambda = Expression.Lambda(ctor);
            return (Func<T>)lambda.Compile();
        }
        #endregion

        #region Fields
        [NotNull, Pure]
        public static Func<T, TResult> GetFieldGetter<T, TResult>([NotNull] FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
                throw new ArgumentNullException(nameof(fieldInfo));
            if (fieldInfo.DeclaringType == null)
                throw new ArgumentException($"Cannot create getter for type {typeof(T).FullName}, because field {fieldInfo.Name} doesn't have declaring type.");
            if (!fieldInfo.DeclaringType.IsAssignableFrom(typeof(T)))
                throw new ArgumentException($"Cannot create getter for type {typeof(T).FullName}, because field {fieldInfo.Name} is declared in {fieldInfo.DeclaringType.FullName}.");

            var instance = Expression.Parameter(typeof(T), "instance");
            var member = Expression.Field(instance, fieldInfo);
            var lambda = Expression.Lambda(typeof(Func<T, TResult>), member, instance);
            return (Func<T, TResult>)lambda.Compile();
        }

        [NotNull, Pure]
        public static Action<T, TValue> GetFieldSetter<T, TValue>([NotNull] FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
                throw new ArgumentNullException(nameof(fieldInfo));
            if (fieldInfo.DeclaringType == null)
                throw new ArgumentException($"Cannot create getter for type {typeof(T).FullName}, because field {fieldInfo.Name} doesn't have declaring type.");
            if (!fieldInfo.DeclaringType.IsAssignableFrom(typeof(T)))
                throw new ArgumentException($"Cannot create getter for type {typeof(T).FullName}, because field {fieldInfo.Name} is declared in {fieldInfo.DeclaringType.FullName}.");

            // readonly-поле нельзя записать через expression, там неотключаемая проверка, поэтому приходится генерировать IL в режиме skipVisibility
            if (fieldInfo.IsInitOnly)
            {
                var m = new DynamicMethod("Set_" + fieldInfo.Name, typeof(void), new[] { typeof(T), typeof(TValue) }, fieldInfo.DeclaringType, true);
                var il = m.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Stfld, fieldInfo);
                il.Emit(OpCodes.Ret);

                return (Action<T, TValue>)m.CreateDelegate(typeof(Action<T, TValue>));
            }

            // rw-поля предпочтительнее записывать через expression lambda, такие операции смогут инлайниться
            // (jit побаивается инлайнить методы, сгенерированные через emit, https://stackoverflow.com/questions/11023993/why-is-lambda-faster-than-il-injected-dynamic-method)
            var instance = Expression.Parameter(fieldInfo.DeclaringType, "instance");
            var value = Expression.Parameter(typeof(TValue), "value");
            var member = Expression.Field(instance, fieldInfo.Name);
            var lambda = Expression.Lambda(typeof(Action<T, TValue>), Expression.Assign(member, value), instance, value);
            return (Action<T, TValue>)lambda.Compile();
        }
        #endregion

        #region Static Fields

        public static Func<TResult> GetStaticFieldGetter<TResult>(Type declaringType, string fieldName)
        {
            var field = declaringType.GetField(fieldName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            var member = Expression.Field(null, field);
            var lambda = Expression.Lambda(typeof(Func<TResult>), member);
            var compiled = (Func<TResult>)lambda.Compile();
            return compiled;
        }

        public static Action<TValue> GetStaticFieldSetter<TValue>(Type declaringType, string fieldName)
        {
            var field = declaringType.GetField(fieldName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            var m = new DynamicMethod("Set_" + fieldName, typeof(void), new[] { typeof(TValue) }, declaringType);
            var il = m.GetILGenerator();
            //il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Stsfld, field);
            il.Emit(OpCodes.Ret);

            return (Action<TValue>)m.CreateDelegate(typeof(Action<TValue>));
        }

        #endregion

        #region Properties

        public static Func<T, TResult> GetPropertyGetter<T, TResult>([NotNull] PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException(nameof(propertyInfo));
            if (propertyInfo.DeclaringType == null)
                throw new ArgumentException($"Cannot create getter for type {typeof(T).FullName}, because property {propertyInfo.Name} doesn't have declaring type.");
            if (!propertyInfo.DeclaringType.IsAssignableFrom(typeof(T)))
                throw new ArgumentException($"Cannot create getter for type {typeof(T).FullName}, because property {propertyInfo.Name} is declared in {propertyInfo.DeclaringType.FullName}.");

            var param = Expression.Parameter(typeof(T), "_");
            var member = Expression.Property(param, propertyInfo);
            var lambda = Expression.Lambda(typeof(Func<T, TResult>), member, param);
            return (Func<T, TResult>)lambda.Compile();
        }

        public static Action<T, TValue> GetPropertySetter<T, TValue>([NotNull] PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
                throw new ArgumentNullException(nameof(propertyInfo));
            if (propertyInfo.DeclaringType == null)
                throw new ArgumentException($"Cannot create setter for type {typeof(T).FullName}, because property {propertyInfo.Name} doesn't have declaring type.");
            if (!propertyInfo.DeclaringType.IsAssignableFrom(typeof(T)))
                throw new ArgumentException($"Cannot create setter for type {typeof(T).FullName}, because property {propertyInfo.Name} is declared in {propertyInfo.DeclaringType.FullName}.");

            var instance = Expression.Parameter(typeof(T), "instance");
            var value = Expression.Parameter(typeof(TValue), "value");
            var member = Expression.Property(instance, propertyInfo);
            var lambda = Expression.Lambda(typeof(Action<T, TValue>), Expression.Assign(member, value), instance, value);
            return (Action<T, TValue>)lambda.Compile();
        }

        #endregion

        #region Property Names

        /// <summary>
        /// Возвращает имя свойства из переданного лямбда-выражения вида "someObject => someObject.someProperty".
        /// </summary>
        /// <remarks>
        /// В некоторых случаях вызов свойства заворачивается в вызов Convert
        /// (https://devio.wordpress.com/2010/07/27/get-property-name-as-string-value/
        /// https://long2know.com/2015/07/eliminating-string-literals-in-net/),
        /// учитываем эти случаи.
        /// </remarks>
        public static string GetPropertyName<T, TResult>(Expression<Func<T, TResult>> expr)
        {
            var body = expr.Body as MemberExpression ?? (MemberExpression)((UnaryExpression)expr.Body).Operand;
            return body.Member.Name;
        }

        private static string GetPropertyName<T>(Expression<Func<T>> expr)
        {
            return ((MemberExpression)expr.Body).Member.Name;
        }

        public static string GetCommandName(Expression<Func<RoutedCommand>> expr)
        {
            return RemoveSuffix(GetPropertyName(expr), CommandSuffix);
        }

        public static string GetEventName(Expression<Func<RoutedEvent>> expr)
        {
            return RemoveSuffix(GetPropertyName(expr), EventSuffix);
        }

        #endregion

        #region Private Functions

        private static string RemoveSuffix(string s, string suffix)
        {
            return s.EndsWith(suffix) ? s.Substring(0, s.Length - suffix.Length) : s;
        }

        #endregion
    }
}