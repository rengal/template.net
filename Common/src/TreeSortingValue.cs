using System;
using System.Collections.Generic;
using System.Collections;

namespace Resto.Data
{
    /// <summary>
    /// Утилитный класс для реализации сортировки элементов, образующих структуру дерева (сокращенно элемент сортировки).
    /// </summary>
    public abstract class TreeSortingValue : IComparable, IComparable<TreeSortingValue>
    {
        /// <summary>
        /// Возвращает идентификатор узла дерева.
        /// </summary>
        public abstract IComparable NodeId
        {
            get;
        }

        /// <summary>
        /// Возвращает значение сортировки (по нему вычисляется порядок элемента по отношению к другим элементам).
        /// </summary>
        public abstract IComparable NodeValue
        {
            get;
        }

        /// <summary>
        /// Возвращает ссылку на элемент сортировки родительского узла.
        /// </summary>
        public abstract TreeSortingValue ParentNode
        {
            get;
        }

        private List<TreeSortingValue> sortingPath
        {
            get
            {
                List<TreeSortingValue> result = new List<TreeSortingValue>();
                TreeSortingValue currentSortingValue = this;
                result.Add(currentSortingValue);
                while (currentSortingValue.ParentNode != null)
                {
                    result.Insert(0, currentSortingValue.ParentNode);
                    currentSortingValue = currentSortingValue.ParentNode;
                }

                return result;
            }
        }

        /// <summary>
        /// Возвращает порядок расположения данного элемента сортировки по отношению к указанному с учетом направления сортировки.
        /// </summary>
        /// <param name="other">Элемент, с которым сравнивается текущий.</param>
        /// <param name="ascending">Направление сортировки:
        /// null - без сортировки (однако иерархия учитывается),
        /// true - сортировка по возрастанию,
        /// false - сортировка по убыванию.
        /// </param>
        /// <returns>Результат сравнения:
        /// -1 - данный элемент должен находиться выше указанного,
        /// 0 - данный элемент равен указанному (порядок не важен),
        /// 1 - данный элемент должен находиться ниже указаного.
        /// </returns>
        public int GetSortOrder(TreeSortingValue other, bool? ascending)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }
            else if (other == null)
            {
                // Специально обрабатываем случай - для новой строки грида при редактировании
                // - она будет в конце...
                return -1;
            }

            List<TreeSortingValue> thisSortingPath = sortingPath;
            List<TreeSortingValue> otherSortingPath = other.sortingPath;

            // Будем проводить иерархическое сравнение (начиная с корневых узлов).
            int currentNodeIndex = 0;

            // Коэффициент для учета направления сортировки.
            int sortOrderRate = ascending == null ? 0 : (ascending.Value ? 1 : -1);

            // Сравниваем по-элементно сверху-вниз, пока одно из условий сравнения не выполнится
            while (true)
            {
                // Сравниваем значение узлов текущего уровня.
                int nodeComparisonResult = Comparer.Default.Compare(thisSortingPath[currentNodeIndex].NodeValue, otherSortingPath[currentNodeIndex].NodeValue);

                if (nodeComparisonResult != 0)
                {
                    return nodeComparisonResult * sortOrderRate;
                }

                // Значения узлов одинаковые, нужно сравнить по идентификаторам...
                nodeComparisonResult = Comparer.Default.Compare(thisSortingPath[currentNodeIndex].NodeId, otherSortingPath[currentNodeIndex].NodeId);

                // Если идентификаторы разные
                if (nodeComparisonResult != 0)
                {
                    // значит и узлы разные - 
                    // проводить сравнение разных веток дальше не имеет смысла, разруливаем порядок по идентификаторам.
                    return nodeComparisonResult * sortOrderRate;
                }

                // Сюда попадаем, если вплоть до текущего уровня иерархии сравниваемые узлы идентичны.

                // Если путь ЭТОГО узла уже закончился
                if (thisSortingPath.Count == currentNodeIndex + 1)
                {
                    // и ДРУГОГО тоже
                    if (otherSortingPath.Count == currentNodeIndex + 1)
                    {
                        // спустились до самого низа - значит узлы идентичны.
                        return 0;
                    }
                    // иначе (ДРУГОЙ путь еще продолжается)
                    else
                    {
                        // ЭТОТ узел родительский ДРУГОГО.
                        return -1;
                    }
                }
                // иначе (ЭТОТ путь еще не закончился), если ДРУГОЙ в конце
                else if (otherSortingPath.Count == currentNodeIndex + 1)
                {
                    // то ЭТОТ узел дочерний для ЭТОГО.
                    return 1;
                }
                // ни один из путей еще не на исходе - нужно продолжать сравнение глубже...
                else
                {
                    currentNodeIndex++;
                }
            }
        }

        /// <summary>
        /// Возвращает порядок расположения одного элемента сортировки по отношению к другому с учетом направления сортировки.
        /// </summary>
        /// <param name="value1">Первый элемент сортировки.</param>
        /// <param name="value2">Второй элемент сортировки.</param>
        /// <param name="ascending">Направление сортировки:
        /// null - без сортировки (однако иерархия учитывается),
        /// true - сортировка по возрастанию,
        /// false - сортировка по убыванию.
        /// </param>
        /// <returns>Результат сравнения:
        /// -1 - первый элемент должен находиться выше второго,
        /// 0 - первый элемент равен второму (порядок не важен),
        /// 1 - первый элемент должен находиться ниже указаного.
        /// </returns>
        public static int GetSortOrder(
            TreeSortingValue value1,
            TreeSortingValue value2,
            bool? ascending)
        {
            if (value1 == null)
            {
                if (value2 == null)
                {
                    return 0;
                }

                return -value2.GetSortOrder(value1, ascending);
            }
            return value1.GetSortOrder(value2, ascending);
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return CompareTo((TreeSortingValue)obj);
        }

        #endregion

        #region IComparable<TreeSortingValue> Members

        public int CompareTo(TreeSortingValue other)
        {
            return GetSortOrder(other, true);
        }

        #endregion
    }
    
    /// <summary>
    /// Типизированный вариант класса <see cref="TreeSortingValue"/>.
    /// </summary>
    /// <typeparam name="TNodeKey">Тип идентификаторов узлов дерева.</typeparam>
    /// <typeparam name="TNodeValue">Тип значений, по которым производится сортировка узлов (например тип одного из полей узла).</typeparam>
    public abstract class TreeSortingValue<TNodeKey, TNodeValue>
        : TreeSortingValue, IComparable, IComparable<TreeSortingValue<TNodeKey, TNodeValue>>
        where TNodeKey : IComparable, IComparable<TNodeKey>
        where TNodeValue : IComparable, IComparable<TNodeValue>
    {
        #region IComparable<TreeSortingValue<TNodeKey, TNodeValue>> Members

        public int CompareTo(TreeSortingValue<TNodeKey, TNodeValue> other)
        {
            return GetSortOrder(other, true);
        }

        #endregion IComparable<TreeSortingValue<TNodeKey, TNodeValue>> Members

        #region IComparable Members

        public new int CompareTo(object obj)
        {
            return CompareTo((TreeSortingValue<TNodeKey, TNodeValue>)obj);
        }

        #endregion IComparable Members
    }
}
