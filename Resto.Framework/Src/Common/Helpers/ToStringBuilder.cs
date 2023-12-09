using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Resto.Framework.Common
{
    /// <summary>
    /// Builder для формирования строкового представления объекта(ов) определенного типа.
    /// </summary>
    /// <typeparam name="T">Тип объекта.</typeparam>
    public class ToStringBuilder<T>
    {
        /// <summary>
        /// Список элементов для построения строкового представления объекта.
        /// Элемент коллекции может быть только либо типа <see cref="Expression{Func}"/> 
        /// (в этом случае трактуется как выражение геттера свойства), 
        /// либо типа, реализующего <see cref="IToStringItem"/>.
        /// </summary>
        /// <remarks>Начальная емкость 15 выбрана "на глаз". 
        /// Вряд ли будет реально больше 15 свойств для одного объекта, т.к. уже более ~10 плохо читается.</remarks>
        private readonly IList<object> items = new List<object>(15);

        [Obsolete("You should not use the ToString method of the ToStringBuilder class. " +
                  "In order to retrieve the string constructed by the builder call the GetString method instead.")]
        public override string ToString()
        {
            return base.ToString();
        }

        /// <summary>
        /// Добавляет к строковому представлению свойство объекта.
        /// </summary>
        /// <param name="propertyGetter">Геттер свойства.</param>
        /// <returns>Возвращает текущий объект <see cref="ToStringBuilder{T}"/> для построения цепочки вызовов.</returns>
        public ToStringBuilder<T> AddProperty(Expression<Func<T, object>> propertyGetter)
        {
            items.Add(propertyGetter);
            return this;
        }

        /// <summary>
        /// Добавляет к строковому представлению объекта какое-либо неизменяемое значение.
        /// </summary>
        /// <param name="name">Наименование.</param>
        /// <param name="value">Значение.</param>
        /// <returns>Возвращает текущий объект <see cref="ToStringBuilder{T}"/> для построения цепочки вызовов.</returns>
        public ToStringBuilder<T> AddValue(string name, object value)
        {
            items.Add(new ToStringValueItem(name, value));
            return this;
        }

        /// <summary>
        /// Добавляет к строковому представлению объекта значение, определяемое переданным делегатом-функцией.
        /// </summary>
        /// <param name="name">Наименование.</param>
        /// <param name="valueFunc">Делегат функции, извлекающей значение для добавления к строковому представлению.</param>
        /// <returns>Возвращает текущий объект <see cref="ToStringBuilder{T}"/> для построения цепочки вызовов.</returns>
        public ToStringBuilder<T> AddValueFunc(string name, Func<T, object> valueFunc)
        {
            items.Add(new ToStringFuncItem(name, valueFunc));
            return this;
        }

        /// <summary>
        /// Формирует строковое представление переданного объекта в соответствии с заданными правилами.
        /// </summary>
        /// <param name="obj">Объект, строковое представление которого необходимо сформировать.</param>
        public string GetString(T obj)
        {
            if (obj == null)
            {
                return StringExtensions.NullRepresentation;
            }

            var sb = new StringBuilder();
            sb.Append(obj.GetType().Name);
            sb.Append("{");

            bool isFirst = true;
            foreach (object item in items)
            {
                if (isFirst)
                {
                    isFirst = false;
                } 
                else
                {
                    sb.Append(", ");
                }

                var propertyExpression = item as Expression<Func<T, object>>;
                if (propertyExpression != null)
                {
                    AppendValue(sb,
                        ExpressionHelper.GetPropertyName(propertyExpression),
                        propertyExpression.Compile()(obj));
                    continue;
                }

                var toStringItem = (IToStringItem) item;
                AppendValue(sb, toStringItem.Name, toStringItem.GetValue(obj));
            }

            sb.Append("}");
            return sb.ToString();
        }

        private void AppendValue(StringBuilder sb, string name, object value)
        {
            sb.Append(name);
            sb.Append("=");
            sb.Append(GetValueString(value));
        }

        private string GetValueString(object value)
        {
            if (value == null)
            {
                return StringExtensions.NullRepresentation;
            }

            var sequence = value as IEnumerable;
            if (sequence != null)
            {
                return sequence.AsText();
            }

            var dt = value as DateTime?;
            if (dt == null)
            {
                return value.ToString();
            }

            // Делаем строку покороче:
            // если время равно ровно полуночи, то форматируем только дату.
            return dt.Value.ToString(dt.Value.TimeOfDay == TimeSpan.Zero 
                ? "dd.MM.yyyy" 
                : "dd.MM.yyyy HH:mm:ss");
        }

        #region Inner types

        /// <summary>
        /// Интерфейс элемента преобразования к строковому представлению.
        /// </summary>
        private interface IToStringItem
        {
            string Name { get; }
            object GetValue(T obj);
        }

        private class ToStringFuncItem : IToStringItem
        {
            public ToStringFuncItem(string name, Func<T, object> valueFunc)
            {
                Name = name;
                ValueFunc = valueFunc;
            }

            public string Name { get; set; }
            public Func<T, object> ValueFunc { get; set; }

            public object GetValue(T obj)
            {
                return ValueFunc(obj);
            }
        }

        private class ToStringValueItem : ToStringFuncItem
        {
            public ToStringValueItem(string name, object value)
                : base(name, obj => value)
            {
                Name = name;
            }
        }

        #endregion
    }
}
