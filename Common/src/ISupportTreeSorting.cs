using System;

namespace Resto.Data
{
    public interface ISupportTreeSorting<S>
    {
        /// <summary>
        /// Возвращает элемент сортировки, соответсвующий данному узлу.
        /// </summary>
        /// <param name="valueSelector">Значение селектора, помогающее выбрать значение для сортировки.</param>
        TreeSortingValue ExtractTreeSortingValue(S valueSelector);
    }

    /// <summary>
    /// Интерфейс узлов дерева, поддерживающего сортировку.
    /// </summary>
    /// <typeparam name="K">Тип ключа узлов дерева.</typeparam>
    /// <typeparam name="S">Тип параметра-селектора, указывающего на критерий сортировки узлов.</typeparam>
    /// <remarks>
    /// Этот интерфейс удобен в тех случаях, когда нет возможности наследовать эелементы дерева от <see cref="TreeSortingValue{TNodeKey, TNodeValue}"/>,
    /// но при этом удобнее получать элемент сортировки непосредственно из узла дерева, а не поддерживать эту связь вручную.
    /// В качестве селектора может выступать любой тип: как enum, так и название поля, по которому ведется сортировка; или даже
    /// делегат.
    /// </remarks>
    public interface ISupportTreeSorting<K, S> where K : IComparable, IComparable<K>
    {
        /// <summary>
        /// Возвращает элемент сортировки, соответсвующий данному узлу.
        /// </summary>
        /// <param name="valueSelector">Значение селектора, помогающее выбрать значение для сортировки.</param>
        TreeSortingValue<K, V> ExtractTreeSortingValue<V>(S valueSelector)
            where V : IComparable, IComparable<V>;
    }
}
