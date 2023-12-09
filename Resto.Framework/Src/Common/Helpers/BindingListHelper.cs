using System;
using System.Collections.Generic;
using System.ComponentModel;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    public static class BindingListHelper
    {
        public class BindingListBulkOperation<T> : IDisposable
        {
            private readonly bool suspendedEvents;
            private bool forceResetBindings = true;
            private readonly BindingList<T> list;

            public BindingListBulkOperation(BindingList<T> list)
            {
                this.list = list;
                suspendedEvents = list.RaiseListChangedEvents;
                list.RaiseListChangedEvents = false;
            }

            public void Dispose()
            {
                if (suspendedEvents)
                {
                    list.RaiseListChangedEvents = true;
                    if (forceResetBindings)
                    {
                        list.ResetBindings();
                    }
                }
            }

            public void PreventResetBindings()
            {
                forceResetBindings = false;
            }
        }

        public static BindingListBulkOperation<T> GetBulkWrapper<T>(this BindingList<T> list)
        {
            return new BindingListBulkOperation<T>(list);
        }

        /// <summary>
        /// Массово обработать коллекцию. Если коллекция уже массово обрабатывается - просто вызовет обработчик.
        /// В противном случае, предотвратит вызов ListChanged event'ов на протяжении работы, после чего вызовет ResetBindings() коллекции.
        /// </summary>
        /// <typeparam name="TItem">Тип элементов в коллекции.</typeparam>
        /// <param name="list">Изменяемая коллекция.</param>
        /// <param name="func">Обработчик, изменяющий элементы коллекции. Должен возвращать true, если он изменил обрабатываемый элемент и false в противном случае</param>
        public static void WrapBulkAction<TItem>(
            this BindingList<TItem> list, [NotNull] Func<BindingList<TItem>, bool> func)
        {
            bool isRaisingEvents = list.RaiseListChangedEvents;
            list.RaiseListChangedEvents = false;

            var changed = func(list);

            if (isRaisingEvents)
            {
                list.RaiseListChangedEvents = true;
                if (changed)
                {
                    list.ResetBindings();
                }
            }
        }

        /// <summary>
        /// Массово вызвать обработчик для каждого элемента коллекции.
        /// </summary>
        /// <typeparam name="TItem">Тип элементов в коллекции.</typeparam>
        /// <param name="list">Изменяемая коллекция.</param>
        /// <param name="func">Обработчик, изменяющий элементы коллекции. Должен возвращать true, если он изменил обрабатываемый элемент и false в противном случае</param>
        public static void ExecuteBulk<TItem>([NotNull] this BindingList<TItem> list, [NotNull] Func<TItem, bool> func)
        {
            WrapBulkAction(list, l =>
            {
                bool changed = false;
                foreach (var i in l)
                {
                    changed = func(i) || changed;
                }
                return changed;
            });
        }

        /// <summary>
        /// Заменить содержимое коллекции перечислением элементов.
        /// </summary>
        /// <typeparam name="TItem">Тип элементов в коллекции.</typeparam>
        /// <typeparam name="TAddedItem">Тип добавляемых элементов - подтип типа элементов коллекции</typeparam>
        /// <param name="list">Изменяемая коллекция.</param>
        /// <param name="items">Элементы.</param>
        public static void SetBulk<TItem, TAddedItem>(
            [NotNull] this BindingList<TItem> list, [NotNull] IEnumerable<TAddedItem> items)
            where TAddedItem : TItem
        {
            WrapBulkAction(list, l =>
            {
                l.Clear();
                l.AddRange(items);
                return true;
            });
        }

        /// <summary>
        /// Удалить перечисление элементов из коллекции.
        /// </summary>
        /// <typeparam name="T">Тип элементов в коллекции.</typeparam>
        /// <param name="list">Изменяемая коллекция.</param>
        /// <param name="items">Удаляемые элементы.</param>
        public static void RemoveBulk<T>([NotNull] this BindingList<T> list, [NotNull] IEnumerable<T> items)
        {
            WrapBulkAction(list, l =>
            {
                var initialCount = list.Count;
                foreach (var item in items)
                    l.Remove(item);
                return initialCount != list.Count;
            });
        }
    }
}