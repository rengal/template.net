using System;
using System.Collections.Generic;
using System.Linq;

namespace Resto.Framework.Common.Search
{
    /// <summary>
    /// Класс для проверки соответствия объекта типа <typeparamref name="TObj"/> поисковой строке по
    /// алгоритму <see cref="SearchAlgorithm{TObj}"/>.
    /// </summary>
    /// <typeparam name="TObj">Класс объектов, по которым производится поиск.</typeparam>
    public sealed class Searcher<TObj>
    {
        private static readonly SearchMatches<TObj> EmptyMatches = new SearchMatches<TObj>(default(TObj));

        /// <summary>
        /// Максимальное количество результатов в последовательности. По умолчанию количество не ограничено (равно <see cref="int.MaxValue"/>).
        /// </summary>
        private readonly int maxMatches;

        private List<SearchToken> tokens = new List<SearchToken>();

        public Searcher()
            : this(int.MaxValue)
        { }

        public Searcher(int maxMatches)
        {
            this.maxMatches = maxMatches;
        }

        /// <summary>
        /// Подготовить поисковую строку для поиска.
        /// Последующие вызовы методов поиска, не принимающих аргумент <paramref name="searchString"/>, не потребуют анализа поисковой строки.
        /// Используйте, если методы будут вызываться несколько раз.
        /// </summary>
        /// <param name="alg">Алгоритм, по котрому проверяется соответствие.</param>
        /// <param name="searchString">Поисковая строка.</param>
        public void Prepare(SearchAlgorithm<TObj> alg, string searchString)
        {
            //Parse search string
            tokens = (!string.IsNullOrEmpty(searchString)
                          ? searchString.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                          : Array.Empty<string>())
                .Distinct()
                .Select(search => new SearchToken(search, alg.NeedParseDecimal, alg.NeedParseDigits, alg.NeedParsePhones))
                .ToList();
        }

        /// <summary>
        /// Проверить соответствие объекта <paramref name="obj"/> поисковой строке <paramref name="searchString"/> по алгоритму <paramref name="alg"/>.
        /// Последующие вызовы методов поиска, не принимающих аргумент <paramref name="searchString"/>, будут производить поиск по последней переданной строке.
        /// </summary>
        /// <param name="obj">Проверяемый объект.</param>
        /// <param name="alg">Алгоритм, по которому проверяется соответствие.</param>
        /// <param name="searchString">Поисковая строка.</param>
        /// <returns><c>true</c>, если объект соответствует поисковой строке; <c>false</c> в противном случае.</returns>
        public bool Check(TObj obj, SearchAlgorithm<TObj> alg, string searchString)
        {
            Prepare(alg, searchString);
            return Check(obj, alg);
        }

        /// <summary>
        /// Проверить соответствие объекта <paramref name="obj"/> по алгоритму <paramref name="alg"/> поисковой строке,
        /// которая была передана в один из методов, принимающих аргумент <c>searchString</c>.
        /// </summary>
        /// <param name="obj">Проверяемый объект.</param>
        /// <param name="alg">Алгоритм, по которому проверяется соответствие.</param>
        /// <returns><c>true</c>, если объект соответствует поисковой строке; <c>false</c> в противном случае.</returns>
        public bool Check(TObj obj, SearchAlgorithm<TObj> alg)
        {
            return tokens.Any() && tokens.All(token => alg.Check(obj, token));
        }

        /// <summary>
        /// Проверить соответствие объекта <paramref name="obj"/> поисковой строке <paramref name="searchString"/> по алгоритму <paramref name="alg"/>
        /// и вернуть результат со списком найденных значений.
        /// Последующие вызовы методов поиска, не принимающих аргумент <paramref name="searchString"/>, будут производить поиск по последней переданной строке.
        /// </summary>
        /// <param name="obj">Проверяемый объект.</param>
        /// <param name="alg">Алгоритм, по которому проверяется соответствие.</param>
        /// <param name="searchString">Поисковая строка.</param>
        /// <returns>Результат <see cref="SearchMatches{TObj}"/> со списком найденных значений.</returns>
        public SearchMatches<TObj> CheckMatch(TObj obj, SearchAlgorithm<TObj> alg, string searchString)
        {
            Prepare(alg, searchString);
            return CheckMatch(obj, alg);
        }

        /// <summary>
        /// Проверить соответствие объекта <paramref name="obj"/> по алгоритму <paramref name="alg"/> поисковой строке,
        /// которая была передана в один из методов, принимающих аргумент <c>searchString</c>,
        /// и вернуть результат со списком найденных значений.
        /// </summary>
        /// <param name="obj">Проверяемый объект.</param>
        /// <param name="alg">Алгоритм, по которому проверяется соответствие.</param>
        /// <returns>Объект <see cref="SearchMatches{TObj}"/> со списком найденных значений.</returns>
        public SearchMatches<TObj> CheckMatch(TObj obj, SearchAlgorithm<TObj> alg)
        {
            var match = new SearchMatches<TObj>(obj);
            for (var i = 0; i < tokens.Count; i++)
            {
                if (!alg.CheckWithMatch(obj, tokens[i], match))
                    return EmptyMatches;
            }
            match.Unique();
            return match;
        }

        /// <summary>
        /// Выбрать в последовательности <paramref name="source"/> объекты, соответствующие поисковой строке <paramref name="searchString"/> по алгоритму <paramref name="alg"/>.
        /// Последующие вызовы методов поиска, не принимающих аргумент <paramref name="searchString"/>, будут производить поиск по последней переданной строке.
        /// </summary>
        /// <param name="source">Последовательность, в которой производится поиск.</param>
        /// <param name="alg">Алгоритм, по которому проверяется соответствие.</param>
        /// <param name="searchString">Поисковая строка.</param>
        /// <returns>Отфильтрованная последовательность.</returns>
        public IEnumerable<TObj> Filter(IEnumerable<TObj> source, SearchAlgorithm<TObj> alg, string searchString)
        {
            Prepare(alg, searchString);
            return Filter(source, alg);
        }

        /// <summary>
        /// Выбрать в последовательности <paramref name="source"/> объекты по алгоритму <paramref name="alg"/>, соответствующие поисковой строке,
        /// которая была передана в один из методов, принимающих аргумент <c>searchString</c>.
        /// </summary>
        /// <param name="source">Последовательность, в которой производится поиск.</param>
        /// <param name="alg">Алгоритм, по которому проверяется соответствие.</param>
        /// <returns>Отфильтрованная последовательность.</returns>
        public IEnumerable<TObj> Filter(IEnumerable<TObj> source, SearchAlgorithm<TObj> alg)
        {
            return tokens.Any() ? source.Where(obj => tokens.All(token => alg.Check(obj, token))).Take(maxMatches) : Enumerable.Empty<TObj>();
        }

        /// <summary>
        /// Выбрать в последовательности <paramref name="source"/> объекты, соответствующие поисковой строке <paramref name="searchString"/> по алгоритму <paramref name="alg"/>,
        /// и вернуть список результатов со списками найденных значений.
        /// Последующие вызовы методов поиска, не принимающих аргумент <paramref name="searchString"/>, будут производить поиск по последней переданной строке.
        /// </summary>
        /// <param name="source">Последовательность, в которой производится поиск.</param>
        /// <param name="alg">Алгоритм, по которому проверяется соответствие.</param>
        /// <param name="searchString">Поисковая строка.</param>
        /// <returns>Список результатов <see cref="SearchMatches{TObj}"/> со списками найденных значений.</returns>
        public IEnumerable<SearchMatches<TObj>> FilterMatch(IEnumerable<TObj> source, SearchAlgorithm<TObj> alg, string searchString)
        {
            Prepare(alg, searchString);
            return FilterMatch(source, alg);
        }

        /// <summary>
        /// Выбрать в последовательности <paramref name="source"/> объекты по алгоритму <paramref name="alg"/>, соответствующие поисковой строке,
        /// которая была передана в один из методов, принимающих аргумент <c>searchString</c>,
        /// и вернуть список результатов со списками найденных значений.
        /// </summary>
        /// <param name="source">Последовательность, в которой производится поиск.</param>
        /// <param name="alg">Алгоритм, по которому проверяется соответствие.</param>
        /// <returns>Список результатов <see cref="SearchMatches{TObj}"/> со списками найденных значений.</returns>
        public IEnumerable<SearchMatches<TObj>> FilterMatch(IEnumerable<TObj> source, SearchAlgorithm<TObj> alg)
        {
            return source.Select(obj => CheckMatch(obj, alg)).Where(match => match.IsMatched).Take(maxMatches);
        }
    }
}
