using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Resto.Framework.Attributes.JetBrains;

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable InvokeAsExtensionMethod
namespace Resto.Framework.Common.Search
{
    public partial class SearchAlgorithm<TObj>
    {
        /// <summary>
        /// Компилятор части алгоритма, соответствующей обработке частей запросов <see cref="SearchToken"/>.
        /// В текущей реализации в зависимости от значения <see cref="SearchToken.IsDecimal"/> используются строковая или
        /// цифровая реализация.
        /// </summary>
        private sealed class SearchTokenProcessor : ISearchTokenProcessor
        {
            // ReSharper disable MemberCanBePrivate.Local
            public Func<SearchMode, Type, PropertySearchMode, bool> PropFilter { get; set; }
            public List<Type> ConvertedTypes { get; set; }
            // ReSharper restore MemberCanBePrivate.Local

            public Dictionary<PropertySearchMode, Func<object, SearchToken, bool>> SearchMethods { get; set; }
            public Func<SearchToken, bool> IsActiveOnToken { get; set; }

            private static bool CheckSingleProperty(TObj objToCheck, PropertySearchInfo<TObj> propertyToCheck, SearchToken searchToken)
            {
                var propertyValue = propertyToCheck.PropertyGetter(objToCheck);
                return propertyToCheck.SearchMethod(propertyValue, searchToken);
            }

            private static bool CheckListProperty(TObj objectToCheck, PropertySearchInfo<TObj> propertyToCheck, SearchToken searchToken)
            {
                var collection = ((IEnumerable)propertyToCheck.PropertyGetter(objectToCheck)).Cast<object>();
                return collection.Any(n => propertyToCheck.SearchMethod(n, searchToken));
            }

            public bool Check(TObj objToCheck, SearchToken searchToken)
            {
                CheckFreezed();

                return activeProperties.Any(p => p.Mode.Contains(PropertySearchMode.List) ? CheckListProperty(objToCheck, p, searchToken)
                                                                                          : CheckSingleProperty(objToCheck, p, searchToken));
            }

            public bool Match(TObj objectToMatch, SearchMatches<TObj> matches, SearchToken searchToken)
            {
                CheckFreezed();

                var isMatch = false;
                for (var i = 0; i < activeProperties.Length; i++)
                {
                    var activeProperty = activeProperties[i];
                    isMatch = activeProperty.MatchMethod(objectToMatch, activeProperty, matches, searchToken) || isMatch;
                }
                return isMatch;
            }

            private PropertySearchInfo<TObj>[] activeProperties;

            public void Freeze(SearchAlgorithm<TObj> algorithm)
            {
                activeProperties = algorithm.properties
                                            .Where(prop => PropFilter(algorithm.Mode, prop.Type, prop.Mode))
                                            .Do(prop => 
                                                prop.Freeze(SearchMethods[prop.Mode & PropertySearchMode.AllModes],
                                                            prop.Mode.Contains(PropertySearchMode.List)
                                                                ? MatchListProperty
                                                                : (Func<TObj, PropertySearchInfo<TObj>, SearchMatches<TObj>, SearchToken, bool>)MatchSingleProperty,
                                                            prop.ListItemPropertyGetter == null
                                                                ? null
                                                                : (Func<PropertySearchInfo<TObj>, SearchMatches<TObj>, SearchToken, object, bool>)
                                                                  Delegate.CreateDelegate(
                                                                      typeof (Func<PropertySearchInfo<TObj>, SearchMatches<TObj>, SearchToken, object, bool>),
                                                                      GetType()
                                                                          .GetMethod("MatchListPropertyGeneric", BindingFlags.Static | BindingFlags.NonPublic)
                                                                          .MakeGenericMethod(prop.ListItemType))
                                                           )
                                               )
                                               .ToArray();
            }

            private static bool MatchSingleProperty(TObj objToCheck, PropertySearchInfo<TObj> propertyToCheck,
                                            SearchMatches<TObj> matches, SearchToken searchToken)
            {
                var property = propertyToCheck.PropertyGetter(objToCheck);
                if (propertyToCheck.SearchMethod(property, searchToken))
                {
                    matches.AddMatch(new SearchMatch<TObj>(propertyToCheck, property));
                    return true;
                }
                return false;
            }

            private static bool MatchListProperty(TObj objToCheck, PropertySearchInfo<TObj> propertyToCheck,
                                            SearchMatches<TObj> matches, SearchToken searchToken)
            {
                // TODO: Выполнить рефакторинг, заменив все возможные виды доступа к данным для поиска 
                //       из методов MatchListProperty и MatchSingleProperty на набор обобщённых классов с общим интерфейсом.
                //       Это позволит избавиться от лишних приведений типов, от требования реализовывать IList и от условной логики

                // 1. Поиск по свойствам элементов коллекции
                var propertyValue = propertyToCheck.PropertyGetter(objToCheck);
                if (propertyToCheck.ListItemPropertyGetter != null)
                {
                    return propertyToCheck.MatchListPropertyGenericMethod(propertyToCheck, matches, searchToken, propertyValue);
                }

                var isMatch = false;

                // 2. Поиск по коллекции (содержит непосредственно данные для поиска)
                // NOTE: Полагаем, что тип свойства реализует и IList<T>, и IList, хотя формально требуем только IList<T>
                var valuesList = propertyValue as IList;
                if (valuesList != null)
                {
                    for (var i = 0; i < valuesList.Count; i++)
                    {
                        var listItem = valuesList[i];
                        if (propertyToCheck.SearchMethod(listItem, searchToken))
                        {
                            matches.AddMatch(new SearchMatch<TObj>(propertyToCheck, listItem));
                            isMatch = true;
                        }
                    }
                    return isMatch;
                }

                var collection = propertyValue as ICollection;
                if (collection != null && collection.Count == 0)
                    return false;

                // NOTE: Поиск по IEnumerable может сильно сказаться на производительности 
                //       при переборе коротких последовательностей (в среднем по 0 или 1 элементу)
                //       из-за накладных расходов на GetEnumerator/MoveNext/Dispose.
                //       Желательно использовать списки с поддержкой перебора по индексу (IList).
                foreach (var c in ((IEnumerable)propertyValue).Cast<object>())
                {
                    if (propertyToCheck.SearchMethod(c, searchToken))
                    {
                        matches.AddMatch(new SearchMatch<TObj>(propertyToCheck, c));
                        isMatch = true;
                    }
                }
                return isMatch;
            }

            [UsedImplicitly]
            private static bool MatchListPropertyGeneric<TItem>(PropertySearchInfo<TObj> propertyToCheck, SearchMatches<TObj> matches, SearchToken searchToken, object propertyValue)
            {
                var valuesList = (IList<TItem>)propertyValue;
                var isMatch = false;
                for (var i = 0; i < valuesList.Count; i++)
                {
                    var listItem = propertyToCheck.ListItemPropertyGetter(valuesList[i]);
                    if (propertyToCheck.SearchMethod(listItem, searchToken))
                    {
                        matches.AddMatch(new SearchMatch<TObj>(propertyToCheck, listItem));
                        isMatch = true;
                    }
                }
                return isMatch;
            }

            private void CheckFreezed()
            {
                if (activeProperties == null)
                    throw new InvalidOperationException("Algorithm compiled should be freezed in search process.");
            }
        }
    }
}
