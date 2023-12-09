using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common.ObjectPaths;

// TODO <ap> priotize exact needle matches
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable 1574
namespace Resto.Framework.Common.Search
{
    /// <summary>
    /// Алгоритм, определяющий критерии соответствия объектов класса <typeparamref name="TObj"/> поисковой строке.
    /// </summary>
    /// <typeparam name="TObj"></typeparam>
    public sealed partial class SearchAlgorithm<TObj>
    {
        private static readonly CompareInfo Comparer = CultureInfo.CurrentCulture.CompareInfo;

        private readonly List<PropertySearchInfo<TObj>> properties = new List<PropertySearchInfo<TObj>>();
        internal bool NeedParseDecimal { get; private set; }
        internal bool NeedParseDigits { get; private set; }
        internal bool NeedParsePhones { get; private set; }

        private static readonly List<Type> StringTypes = new List<Type> {
            typeof(string), typeof(char)
        };
        private static readonly List<Type> DecimalTypes = new List<Type> {
            typeof(decimal), typeof(char), typeof(sbyte), typeof(byte), typeof(short), typeof(ushort),
            typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double)
        };

        private bool freezed;

        private readonly List<ISearchTokenProcessor> algorithmCompilers = new List<ISearchTokenProcessor> {
            //Процессор для строковых токенов
            new SearchTokenProcessor {
                SearchMethods = new Dictionary<PropertySearchMode, Func<object, SearchToken, bool>> {
                    { PropertySearchMode.StringInclude, (o, sn) =>  SearchStringInclude(Convert.ToString(o), sn)},
                    { PropertySearchMode.StringStartsWith, (o, sn) => SearchStringStartsWith(Convert.ToString(o), sn) },
                    { PropertySearchMode.StringDigitsInclude, (o, sn) => SearchStringDigitsInclude(Convert.ToString(o), sn) }
                },
                PropFilter = (mode, type, propertyMode) => StringTypes.Contains(type) &&  !propertyMode.Contains(PropertySearchMode.Phones),
                IsActiveOnToken = needle => !needle.IsDecimal,
                ConvertedTypes = StringTypes.Except(typeof(string).AsSequence()).ToList(),
            },
            //Процессор для числовых токенов
            new SearchTokenProcessor {
                SearchMethods = new Dictionary<PropertySearchMode, Func<object, SearchToken, bool>> {
                    { PropertySearchMode.DecimalEquals, (o, n) => SearchDecimalEquals(Convert.ToDecimal(o), n) },
                    { PropertySearchMode.DecimalStartsWith, (o, n) => SearchDecimalStartsWith(Convert.ToDecimal(o), n) },
                    { PropertySearchMode.StringInclude, (d, n) => SearchStringInclude(Convert.ToString(d), n) },
                    { PropertySearchMode.StringStartsWith, (d, n) => SearchStringStartsWith(Convert.ToString(d), n) },
                    { PropertySearchMode.StringDigitsInclude, (d, n) =>SearchStringDigitsInclude(Convert.ToString(d), n)}
                },
                PropFilter = (mode, type, propertyMode) => DecimalTypes.Contains(type) ||
                    (!mode.Contains(SearchMode.IgnoreStringPropertiesOnDecimalSearch) && StringTypes.Contains(type)) && !propertyMode.Contains(PropertySearchMode.Phones),
                IsActiveOnToken = needle => needle.IsDecimal,
                ConvertedTypes = DecimalTypes.Except(typeof(decimal).AsSequence()).ToList(),
            },
            new SearchTokenProcessor
                {
                SearchMethods = new Dictionary<PropertySearchMode, Func<object, SearchToken, bool>> {
                    { PropertySearchMode.Phones, (o, sn) =>  SearchPhoneNumberInclude(Convert.ToString(o), sn)}
                },
                PropFilter = (mode, type, propertyMode) => (propertyMode & PropertySearchMode.Phones) > 0,
                IsActiveOnToken = needle => needle.IsPhone || !string.IsNullOrEmpty(needle.StringOnlyDigits) || needle.String == "+"
                }
        };

        private SearchMode mode;
        /// <summary>
        /// Параметры поиска.
        /// </summary>
        public SearchMode Mode
        {
            get { return mode; }
            set
            {
                if (freezed)
                    throw new InvalidOperationException("Cannot change Mode. Algorithm is freezed.");

                mode = value;
            }
        }

        /// <summary>
        /// Добавить критерий поиска <paramref name="mode"/> для свойства <paramref name="propertyExpr"/>.
        /// </summary>
        /// <typeparam name="TProp">Тип свойства, для которого добавляется критерий.</typeparam>
        /// <param name="propertyExpr">Лямбда-выражение, определяющее путь к свойству. Например, <c>o => o.Path.To().Property</c>.
        /// Если используются методы, то их вызовы не должны зависеть ни от чего, кроме значений свойств, иначе подписка
        /// на dependency свойства не будет работать.</param>
        /// <param name="mode">Критерий для поиска.</param>
        /// <param name="path">Путь, от которого зависит выражение.
        /// Например, для <c>o => o.PathToProp.ToString()</c> класса <c>O</c> это <c>O.PathToPropProperty</c>.</param>
        /// <returns><c>this</c></returns>
        public SearchAlgorithm<TObj> AddProperty<TProp>(
            Func<TObj, TProp> propertyExpr, PropertySearchMode mode, ObjectPath.Path path = null)
        {
            return AddProperty(propertyExpr, null, mode, null, path);
        }

        public SearchAlgorithm<TObj> AddProperty<TProp>(
            Func<TObj, TProp> propertyExpr, PropertySearchMode mode, string format, ObjectPath.Path path = null)
        {
            return AddProperty(propertyExpr, null, mode, format, path);
        }

        public SearchAlgorithm<TObj> AddProperty<TProp>(
            Func<TObj, TProp> propertyExpr, string propertyKey, PropertySearchMode mode, string format, ObjectPath.Path path)
        {
            properties.Add(PropertySearchInfo<TObj>
                               .CreateSingleSearcherPropertyInfo
                               (
                                    propertyExpr,
                                    propertyKey,
                                    mode,
                                    format,
                                    path
                               ));
            return this;
        }

        public SearchAlgorithm<TObj> AddProperty<TItem, TValue>(Func<TObj, IList<TItem>> propertyExpr, Func<TItem, TValue> listItemPropertyExpr, string propertyKey, PropertySearchMode mode, string format)
        {
            properties.Add(PropertySearchInfo<TObj>
                               .CreateListSearcherPropertyInfo
                               (
                                   propertyExpr,
                                   listItemPropertyExpr,
                                   propertyKey,
                                   mode,
                                   format
                               ));
            return this;
        }

        public SearchAlgorithm<TObj> AddProperty<TItem, TValue>(Func<TObj, IReadOnlyList<TItem>> propertyExpr, Func<TItem, TValue> listItemPropertyExpr, string propertyKey, PropertySearchMode mode, string format)
        {
            properties.Add(PropertySearchInfo<TObj>
                               .CreateListSearcherPropertyInfo
                               (
                                   propertyExpr,
                                   listItemPropertyExpr,
                                   propertyKey,
                                   mode,
                                   format
                               ));
            return this;
        }

        /// <summary>
        /// Скомпилировать алгоритм. Должен быть вызван после добавления критериев для
        /// свойств (<see cref="AddProperty{TProp}"/>) или изменения параметров поиска (<see cref="Mode"/>).
        /// </summary>
        /// <returns><c>this</c></returns>
        public SearchAlgorithm<TObj> Freeze()
        {
            NeedParseDecimal = properties.Any(prop => prop.Mode.Contains(PropertySearchMode.DecimalModes));
            NeedParseDigits =
                properties.Any(
                    prop =>
                    prop.Mode.Contains(PropertySearchMode.StringDigitsInclude) ||
                    prop.Mode.Contains(PropertySearchMode.Phones));
            NeedParsePhones = properties.Any(prop => prop.Mode.Contains(PropertySearchMode.Phones));

            algorithmCompilers.ForEach(c => c.Freeze(this));

            freezed = true;
            return this;
        }

        [CanBeNull]
        public ObjectPath.Path GetPropertiesPath()
        {
            var paths = properties
                .Where(prop => prop.PropertyPath != null)
                .Select(prop => prop.PropertyPath)
                .ToList();

            return paths.IsEmpty() ? null : paths.Merge();
        }

        internal bool Check(TObj obj, SearchToken token)
        {
            CheckFreezed();

            return algorithmCompilers.Any(c => c.IsActiveOnToken(token) && c.Check(obj, token));
        }

        internal bool CheckWithMatch(TObj obj, SearchToken token, SearchMatches<TObj> matches)
        {
            CheckFreezed();

            var isMatch = false;
            for (var i = 0; i < algorithmCompilers.Count; i++)
            {
                var algorithmCompiler = algorithmCompilers[i];
                if (algorithmCompiler.IsActiveOnToken(token))
                    isMatch = algorithmCompiler.Match(obj, matches, token) || isMatch;
            }

            return isMatch;
        }

        private void CheckFreezed()
        {
            if (!freezed)
                throw new InvalidOperationException("Call Freeze method before using.");
        }

        #region Search methods
        private static bool SearchStringStartsWith(string searchingPropertyStringRep, SearchToken token)
        {
            // <ap> WTF? Getting Thread.CurrentThread.CultureInfo takes 35% of time required for IndexOf
            return
                token.String.Length > 0 &&
                searchingPropertyStringRep != null &&
                Comparer.IsPrefix(searchingPropertyStringRep, token.String, CompareOptions.IgnoreCase);
        }

        private static bool SearchPhoneNumberInclude(string searchingPropertyStringRep, SearchToken token)
        {
            if (searchingPropertyStringRep == null || token.String.Length == 0)
            {
                return false;
            }
            //Поддерживаем сценарий ввода части телефона. Например оператор ищет без кода страны и кода города (1112233)
            //в этом случае нормализатор подставит +
            return token.String == "+" || (token.IsPhone &&
                    Comparer.IndexOf(searchingPropertyStringRep, token.NormalizedPhone, CompareOptions.Ordinal) != -1)
                   || (!string.IsNullOrEmpty(token.StringOnlyDigits) &&
                   Comparer.IndexOf(searchingPropertyStringRep, token.StringOnlyDigits, CompareOptions.Ordinal) != -1);
        }

        private static bool SearchStringInclude(string searchingPropertyStringRep, SearchToken token)
        {
            return
                searchingPropertyStringRep != null && token.String.Length > 0 &&
                Comparer.IndexOf(searchingPropertyStringRep, token.String, CompareOptions.IgnoreCase) != -1;
        }

        // TODO <ap> parse searchingPropertyStringRep only once depending on token (create preparsed new{propparsed=prop} object and pass its props?)
        private static bool SearchStringDigitsInclude(string searchingPropertyStringRep, SearchToken token)
        {
            var digits = token.StringOnlyDigits;
            if (searchingPropertyStringRep == null || digits.Length == 0)
                return false;

            var firstDigit = digits[0];
            if (digits.Length == 1)
            {
                for (var i = 0; i < searchingPropertyStringRep.Length; i++)
                {
                    if (searchingPropertyStringRep[i] == firstDigit)
                        return true;
                }
                return false;
            }

            var maxStartIndex = searchingPropertyStringRep.Length - digits.Length;
            for (var startIndex = 0; startIndex <= maxStartIndex; startIndex++)
            {
                if (searchingPropertyStringRep[startIndex] != firstDigit)
                    continue;
                for (int i = startIndex + 1, j = 1; i < searchingPropertyStringRep.Length; i++)
                {
                    var c = searchingPropertyStringRep[i];
                    if (!char.IsDigit(c))
                        continue;
                    if (c != digits[j])
                        break;
                    if (++j == digits.Length)
                        return true;
                }
            }
            return false;
        }

        private static bool SearchDecimalEquals(decimal searchingPropertyDecimalRep, SearchToken token)
        {
            return searchingPropertyDecimalRep == token.Decimal;
        }

        // TODO <ap> parse searchingPropertyStringRep only once depending on token
        private static bool SearchDecimalStartsWith(decimal searchingPropertyDecimalRep, SearchToken token)
        {
            return token.DecimalAbsLower == token.DecimalAbsUpper
                ? searchingPropertyDecimalRep == token.Decimal
                : (Math.Sign(searchingPropertyDecimalRep) == Math.Sign(token.Decimal) || Math.Sign(searchingPropertyDecimalRep) == 0 || Math.Sign(token.Decimal) == 0) &&
                    token.DecimalAbsLower <= Math.Abs(searchingPropertyDecimalRep) && Math.Abs(searchingPropertyDecimalRep) < token.DecimalAbsUpper;
        }
        #endregion
    }
}
