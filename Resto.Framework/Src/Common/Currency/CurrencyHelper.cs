using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Resto.Framework.Properties;
using System.Globalization;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.Currency
{
    /// <summary>
    /// Класс для получения настроек валюты в системе (GUI,отчёты).
    /// </summary>
    public static partial class CurrencyHelper
    {
        /// <summary>
        /// Список падежей для форматирования валюты
        /// </summary>
        public enum Case
        {
            /// именительный падеж (Кто? Что?) единственное число
            NominativeOne = 0, // рубль
            /// именительный падеж (Кто? Что?) множественное число
            NominativeMany, // рубли
            /// для чеков
            Forcheck,
            /// родительный падеж (Кого? Чего?)
            GenetiveOne, // рубля
            GenetiveMany, // рублей
            /// дательный падеж (Кому? Чему?)
            DativeOne, // рублю
            DativeMany, // рублям
            // винительный падеж (Кого? Что?)
            AccusativeOne, // рубль
            AccusativeMany, // рубли
            // творительный падеж (Кем? Чем?)
            InstrumentalOne, // рублем
            InstrumentalMany, // рублями
            // предложный падеж (О ком? О чем?)
            PrepositionalOne, // рубле
            PrepositionalMany // рублях
        }

        /// <summary>
        /// Информация о валюте
        /// </summary>
        internal sealed class CurrencyCasesInfo
        {
            public readonly bool Male;
            public readonly Dictionary<Case, string> SeniorCases = new Dictionary<Case, string>();
            public readonly Dictionary<Case, string> JuniorCases = new Dictionary<Case, string>();
            public readonly string DescriptionResName;

            private static string GetCase(Dictionary<Case, string> cases, Case currencyCase, CultureInfo culture)
            {
                var resourceName = cases.GetOrDefault(currencyCase);
                return resourceName == null
                    ? string.Empty
                    : GetResourceStringIfExists(resourceName, culture);
            }

            public string GetSeniorCase(Case currencyCase, CultureInfo culture)
            {
                return GetCase(SeniorCases, currencyCase, culture);
            }

            public string GetJuniorCase(Case currencyCase, CultureInfo culture)
            {
                return GetCase(JuniorCases, currencyCase, culture);
            }

            public CurrencyCasesInfo(bool male, string descriptionResName)
            {
                Male = male;
                DescriptionResName = descriptionResName;
            }
        }

        public static int DenominationsMinimumCount => DefaultDenominations.Count;
        public static readonly IReadOnlyList<int> DefaultDenominations = new[] { 1, 5, 10, 50, 100, 500, 1000, 5000 };
        private static readonly Dictionary<string, int> CurrencyToFractionalPartLength = new Dictionary<string, int>();
        private static readonly Dictionary<string, decimal> CurrencyToMaxPayInOutSum = new Dictionary<string, decimal>();

        private static readonly Dictionary<string, bool> CalculateSumOnCashRegisterDictionary = new Dictionary<string, bool>();

        private static readonly Dictionary<string, CurrencyCasesInfo> Currencies = new Dictionary<string, CurrencyCasesInfo>();

        public static Dictionary<string, string> GetRegisteredCurrencyDescriptions()
        {
            return Currencies.ToDictionary(kvp => kvp.Key, kvp => GetResourceStringIfExists(kvp.Value.DescriptionResName));
        }

        internal static IReadOnlyDictionary<string, CurrencyCasesInfo> RegisteredCurrencies
        {
            get { return new ReadOnlyDictionary<string, CurrencyCasesInfo>(Currencies); }
        }

        private static ICurrencyProvider cachedCurrencyProvider;
        private static Func<ICurrencyProvider> currencyProviderResolver;

        public static void SetCurrencyProviderResolver(Func<ICurrencyProvider> resolver)
        {
            cachedCurrencyProvider = null;
            currencyProviderResolver = resolver;
        }

        public static ICurrencyProvider CurrencyProvider
        {
            get
            {
                if (cachedCurrencyProvider != null)
                    return cachedCurrencyProvider;
                if (currencyProviderResolver == null)
                    throw new InvalidOperationException("Use SetCurrencyProviderResolver() first.");
                cachedCurrencyProvider = currencyProviderResolver();
                return cachedCurrencyProvider;
            }
        }

        private static string GetResourceStringIfExists(string resourceName)
        {
            return GetResourceStringIfExists(resourceName, CurrencyResources.Culture);
        }

        private static string GetResourceStringIfExists(string resourceName, CultureInfo culture)
        {
            return CurrencyResources.ResourceManager.GetString(resourceName, culture);
        }

        private static void SetCase(IDictionary<Case, string> store, Case currencyCase, string resourceName)
        {
            if (string.IsNullOrEmpty(resourceName))
                return;

            store.Add(currencyCase, resourceName);
        }

        private static void Register(string isoName, int fractionalPartLength,
                                     decimal maxPayInOutSum, bool calculateSumOnCashRegister,
                                     string descriptionResName, bool male,
                                     string seniorOneResName, string seniorTwoResName, string seniorFiveResName,
                                     string seniorForCheckResName, string seniorManyResName,
                                     string juniorOneResName, string juniorTwoResName, string juniorFiveResName,

                                     string dativeOneResName, string dativeManyResName, string accusativeOneResName,
                                     string accusativeManyResName,
                                     string instrumentalOneResName, string instrumentalManyResName,
                                     string prepositionalOneResName)
        {
            var info = new CurrencyCasesInfo(male, descriptionResName);

            SetCase(info.SeniorCases, Case.NominativeOne, seniorOneResName);
            SetCase(info.SeniorCases, Case.NominativeMany, seniorManyResName);
            SetCase(info.SeniorCases, Case.Forcheck, seniorForCheckResName);
            SetCase(info.SeniorCases, Case.GenetiveOne, seniorTwoResName);
            SetCase(info.SeniorCases, Case.GenetiveMany, seniorFiveResName);
            SetCase(info.SeniorCases, Case.DativeOne, dativeOneResName);
            SetCase(info.SeniorCases, Case.DativeMany, dativeManyResName);
            SetCase(info.SeniorCases, Case.AccusativeOne, accusativeOneResName);
            SetCase(info.SeniorCases, Case.AccusativeMany, accusativeManyResName);
            SetCase(info.SeniorCases, Case.InstrumentalOne, instrumentalOneResName);
            SetCase(info.SeniorCases, Case.InstrumentalMany, instrumentalManyResName);
            SetCase(info.SeniorCases, Case.PrepositionalOne, prepositionalOneResName);
            SetCase(info.SeniorCases, Case.PrepositionalMany, seniorForCheckResName);

            SetCase(info.JuniorCases, Case.NominativeOne, juniorOneResName);
            SetCase(info.JuniorCases, Case.GenetiveOne, juniorTwoResName);
            SetCase(info.JuniorCases, Case.GenetiveMany, juniorFiveResName);

            Currencies.Add(isoName, info);

            CurrencyToFractionalPartLength.Add(isoName, fractionalPartLength);
            CurrencyToMaxPayInOutSum.Add(isoName, maxPayInOutSum);
            CalculateSumOnCashRegisterDictionary[isoName] = calculateSumOnCashRegister;
        }

        /// <summary>
        /// Возвращает наименование валюты для отображения в графическом интерфейсе (в BackOffice, FrontOffice etc.)
        /// </summary>
        /// <returns></returns>
        public static string GuiCurrencyName => CurrencyProvider.Currency?.ShortNameForGui ?? string.Empty;

        /// <summary>
        /// Возвращает наименование основной ед. изм. валюты с учётом запрашиваемого типа, если тип не указан, возвращает полное наименование валюты
        /// </summary>
        public static string GetCurrencyNameType(Case? type)
        {
            return GetCurrencyNameType(CurrencyProvider.Currency, type, null);
        }

        public static string GetFullName(this ICurrency currency)
        {
            return GetCurrencyNameType(currency, null, null);
        }

        public static string GetFullName(this ICurrency currency, CultureInfo culture)
        {
            return GetCurrencyNameType(currency, null, culture);
        }

        private static string GetCurrencyNameType(this ICurrency currency, Case? type)
        {
            return GetCurrencyNameType(currency, type, null);
        }

        private static string GetCurrencyNameType(this ICurrency currency, Case? type, CultureInfo culture)
        {
            var isoName = currency.IsoName;
            if (!Currencies.TryGetValue(isoName, out var info))
                return string.Empty;

            return type == null
                ? GetResourceStringIfExists(info.DescriptionResName, culture)
                : info.GetSeniorCase(type.Value, culture);
        }

        public static decimal GetSumInAdditionalCurrency([NotNull] this ICurrency additionalCurrency, decimal additionalCurrencyRate, decimal currentCurrencySum)
        {
            if (additionalCurrency == null)
                throw new ArgumentNullException(nameof(additionalCurrency));

            return (currentCurrencySum / additionalCurrencyRate).CurrencySpecificMoneyFloor(additionalCurrency);
        }

        public static decimal GetSumInCurrentCurrency(decimal additionalCurrencyRate, decimal additionalCurrencySum)
        {
            return (additionalCurrencyRate * additionalCurrencySum).CurrencySpecificMoneyRound();
        }

        /// <summary>
        /// Преобразует число и возвращает его в строковой(печатной) форме.
        /// Эта функция используется при оформлении печатных форм для которых в нормативной документации
        /// присутствует требование об указании суммы прописью.
        /// Указывается также полное название денежных единиц в правильном склонении.
        /// </summary>
        /// <param name="value">сумма</param>
        /// <returns></returns>
        public static string CreateCurrencyStr(this decimal value)
        {
            return CreateCurrencyStr(value, CurrencyProvider.Currency);
        }

        public static string CreateCurrencyStr(this decimal value, ICurrency currency)
        {
            var isoName = currency.IsoName;
            if (!Currencies.ContainsKey(isoName))
                throw new ArgumentOutOfRangeException(nameof(value), string.Format(CurrencyResources.CurrencyConvertercCurrencyNotRegistered, currency));

            var info = Currencies[isoName];
            return MoneyToString(value, info.Male,
                                 info.GetSeniorCase(Case.NominativeOne, null), info.GetSeniorCase(Case.GenetiveOne, null), info.GetSeniorCase(Case.GenetiveMany, null),
                                 info.GetJuniorCase(Case.NominativeOne, null), info.GetJuniorCase(Case.GenetiveOne, null), info.GetJuniorCase(Case.GenetiveMany, null));
        }

        public static decimal GetMinimalCurrencyValue(this ICurrency currency)
        {
            return currency.MinimumDenomination != 0 ? currency.MinimumDenomination : (decimal)Math.Pow(10, -currency.FractionalPartLength());
        }

        /// <summary>
        /// Получает по имени валюты длину ее дробной части 
        /// </summary>
        /// <param name="currencyIsoName">Имя валюты</param>
        /// <returns></returns>
        public static int GetCurrencyFractionalPartLength(string currencyIsoName)
        {
            return CurrencyToFractionalPartLength[currencyIsoName];
        }

        /// <summary>
        /// Получает по объекту валюты длину ее дробной части 
        /// </summary>
        public static int FractionalPartLength(this ICurrency currency)
        {
            return CurrencyToFractionalPartLength[currency.IsoName];
        }

        /// <summary>
        /// Длина дробной части текущей валюты
        /// </summary>
        public static int CurrentCurrencyFractionalPartLength => CurrencyProvider.Currency.FractionalPartLength();

        /// <summary>
        /// Округление стоимости, заданное в BackOffice. Если 0, то округление не выполняется
        /// </summary>
        public static decimal CurrentCurrencyMinimumDenomination => CurrencyProvider.Currency.MinimumDenomination;

        public static IReadOnlyList<int> CurrencyDenominations
        {
            get
            {
                var currencyDenominations = CurrencyProvider.Currency?.Denominations;
                if (currencyDenominations == null || currencyDenominations.Count == 0)
                    return DefaultDenominations;
                if (currencyDenominations.Count == DenominationsMinimumCount)
                    return currencyDenominations;

                return currencyDenominations
                    .Concat(Enumerable.Repeat(0, DenominationsMinimumCount - currencyDenominations.Count))
                    .Take(DenominationsMinimumCount)
                    .ToList();
            }
        }

        /// <summary>
        /// Максимальная сумма внесения/изъятия для текущей валюты
        /// </summary>
        public static decimal MaxPayInOutSum => CurrencyToMaxPayInOutSum[CurrencyProvider.Currency.IsoName];

        /// <summary>
        /// Отдавать ли вычисление суммы товара ФРу
        /// </summary>
        public static bool CalculateItemSumOnCashRegister => CurrentCurrencyMinimumDenomination == 0m && CalculateSumOnCashRegisterDictionary[CurrencyProvider.Currency.IsoName];

        /// <summary>
        /// Округление денежных сумм в соответствии с длиной дробной части текущей валюты (<see cref="CurrentCurrencyFractionalPartLength"/>)
        /// </summary>
        /// <param name="money">Денежная величина, которую нужно округлить</param>
        /// <param name="midpointRoundingTowardsZero">Нужно ли округлять до ближайшего меньшего по модулю числа, когда число находится посредине между двумя другими числами.</param>
        /// <returns>Округленное значение</returns>
        /// Существует копия метода для плагинов Resto.Front.Api.Data.Payments.CurrencyHelper.CurrencySpecificFractionalMoneyRound(this decimal money)
        public static decimal CurrencySpecificFractionalMoneyRound(this decimal money, bool midpointRoundingTowardsZero = false)
        {
            return money.MoneyRound(CurrentCurrencyFractionalPartLength, 0m, midpointRoundingTowardsZero);
        }

        /// <summary>
        /// Округление денежных сумм в соответствии с округлением стоимости, заданным в BackOffice (<see cref="CurrentCurrencyMinimumDenomination"/>)
        /// </summary>
        /// <param name="money">Денежная величина, которую нужно округлить</param>
        /// <param name="midpointRoundingTowardsZero">Нужно ли округлять до ближайшего меньшего по модулю числа, когда число находится посредине между двумя другими числами.</param>
        /// <returns>Округленное значение</returns>
        /// Существует копия метода для плагинов Resto.Front.Api.Data.Payments.CurrencyHelper.CurrencySpecificMoneyRound(this decimal money)
        public static decimal CurrencySpecificMoneyRound(this decimal money, bool midpointRoundingTowardsZero = false)
        {
            return money.MoneyRound(CurrentCurrencyFractionalPartLength, CurrentCurrencyMinimumDenomination, midpointRoundingTowardsZero);
        }

        public static decimal CurrencySpecificMoneyRound(this decimal money, ICurrency currency)
        {
            return money.MoneyRound(currency.FractionalPartLength(), currency.MinimumDenomination);
        }

        public static decimal CurrencySpecificMoneyFloor(this decimal money, ICurrency currency)
        {
            return money.MoneyFloor(currency.FractionalPartLength(), currency.MinimumDenomination);
        }

        public static decimal CurrencySpecificMoneyCeiling(this decimal money)
        {
            return money.MoneyCeiling(CurrentCurrencyFractionalPartLength, CurrentCurrencyMinimumDenomination);
        }

        public static decimal MoneyPrecisionMoneyRound(this decimal money)
        {
            return money.MoneyRound(GetMoneyPrecision(), 0);
        }

        /// <summary>
        /// Корректирует суммы массива sums так, чтобы sums.Sum() оказалась равной expectedSum и округлена в соответствии с длиной дробной части текущей валюты, 
        /// и каждый из элементов был округлен в соответствии с длиной дробной части текущей валюты
        /// </summary>
        /// <param name="sums"></param>
        /// <param name="expectedSum"></param>
        /// <returns></returns>
        public static IReadOnlyList<decimal> CurrencySpecificFractionalDistributeUniformly(IReadOnlyList<decimal> sums, decimal expectedSum)
        {
            return MoneyUtils.DistributeUniformly(sums, expectedSum, CurrentCurrencyFractionalPartLength, 0m);
        }

        /// <summary>
        /// Корректирует суммы массива sums так, чтобы sums.Sum() оказалась равной expectedSum и округлена в соответствии с округлением стоимости, заданным в BackOffice
        /// и каждый из элементов был округлен в соответствии с округлением стоимости, заданным в BackOffice
        /// </summary>
        /// <param name="sums"></param>
        /// <param name="expectedSum"></param>
        /// <returns></returns>
        public static IReadOnlyList<decimal> CurrencySpecificDistributeUniformly(IReadOnlyList<decimal> sums, decimal expectedSum)
        {
            return MoneyUtils.DistributeUniformly(sums, expectedSum, CurrentCurrencyFractionalPartLength, CurrentCurrencyMinimumDenomination);
        }

        /// <summary>
        /// Проверяет, является указанное число допустимым денежным значением с учётом настроек текущей валюты.
        /// </summary>
        /// <param name="value">Значение, которое требуется проверить на пригодность к использованию в качестве денежной суммы.</param>        
        public static bool IsValidMoneyValue(this decimal value)
        {
            // TODO: возможно, стоит добавить проверки на верхнюю и нижнюю границы
            return value.CurrencySpecificMoneyRound() - value == 0;
        }

        public static bool IsValidMoneyValue(this decimal value, ICurrency currency)
        {
            return CurrencySpecificMoneyRound(value, currency) - value == 0;
        }

        /// <summary>
        /// Преобразует денежную величину <paramref name="money"/> в строку с учётом настроек текущей валюты.
        /// Учитываются настройки <see cref="Resto.Framework.Common.Currency.ICurrency.ShowCurrency"/> и <see cref="Resto.Framework.Common.Currency.ICurrency.ShowFractionalPart"/>
        /// </summary>
        /// <param name="money">Денежная величина, котороую нужно преобразовать в строку</param>
        /// <returns>
        /// Строковое представление денежной величины <paramref name="money"/> с учётом настроек текущей валюты.
        /// </returns>
        public static string MoneyToString(this decimal money)
        {
            return money.MoneyToString((bool?)null, null);
        }

        public static string MoneyToString(this decimal money, ICurrency currency)
        {
            return money.MoneyToString(currency, null, null);
        }

        public static string MoneyToStringWithCulture(this decimal money, ICurrency currency, CultureInfo cultureInfo)
        {
            return money.MoneyToStringWithCulture(currency, null, null, cultureInfo);
        }

        /// <summary>
        /// Преобразует денежную величину <paramref name="money"/> в строку
        /// с учётом параметра <paramref name="showCurrency"/>.
        /// </summary>
        /// <param name="money">Денежная величина, котороую нужно преобразовать в строку</param>
        /// <param name="showCurrency">
        /// Параметр, определяющий нужно ли использовать символ текущей валюты (<see cref="Resto.Framework.Common.Currency.ICurrency.ShortNameForGui"/>):
        /// <list type="table">
        ///     <listheader>
        ///         <term>Значение</term>
        ///         <description>Описание</description>
        ///     </listheader>
        ///     <item>
        ///         <term><c>true</c></term>
        ///         <description>Символ валюты используется</description>
        ///     </item>
        ///     <item>
        ///         <term><c>false</c></term>
        ///         <description>Символ валюты не используется</description>
        ///     </item>
        ///     <item>
        ///         <term><c>null</c></term>
        ///         <description>Использование символа валюты определяется настройкой <see cref="Resto.Framework.Common.Currency.ICurrency.ShowCurrency"/> текущей валюты</description>
        ///     </item>
        /// </list>
        /// </param>
        /// <returns>
        /// Строковое представление денежной величины <paramref name="money"/> с учётом переданного параметра.
        /// </returns>
        public static string MoneyToString(this decimal money, bool? showCurrency)
        {
            return money.MoneyToString(showCurrency, null);
        }

        public static string MoneyToString(this decimal money, ICurrency currency, bool? showCurrency)
        {
            return money.MoneyToString(currency, showCurrency, null);
        }

        /// <summary>
        /// Преобразует денежную величину <paramref name="money"/> в строку
        /// с учётом параметров <paramref name="showCurrency"/> и <paramref name="showFractionalPart"/>.
        /// </summary>
        /// <param name="money">Денежная величина, котороую нужно преобразовать в строку</param>
        /// <param name="showCurrency">
        /// Параметр, определяющий нужно ли использовать символ текущей валюты (<see cref="Resto.Framework.Common.Currency.ICurrency.ShortNameForGui"/>):
        /// <list type="table">
        ///     <listheader>
        ///         <term>Значение</term>
        ///         <description>Описание</description>
        ///     </listheader>
        ///     <item>
        ///         <term><c>true</c></term>
        ///         <description>Символ валюты используется</description>
        ///     </item>
        ///     <item>
        ///         <term><c>false</c></term>
        ///         <description>Символ валюты не используется</description>
        ///     </item>
        ///     <item>
        ///         <term><c>null</c></term>
        ///         <description>Использование символа валюты определяется настройкой <see cref="Resto.Framework.Common.Currency.ICurrency.ShowCurrency"/> текущей валюты</description>
        ///     </item>
        /// </list>
        /// </param>
        /// <param name="showFractionalPart">
        /// Параметр, определяющий нужно ли округлять денежную величину до целого числа
        /// (актуален только для валют, в которых есть дробные части основной денежной единицы — копейки, центы и т.д.):
        /// <list type="table">
        ///     <listheader>
        ///         <term>Значение</term>
        ///         <description>Описание</description>
        ///     </listheader>
        ///     <item>
        ///         <term><c>true</c></term>
        ///         <description>Не округлять</description>
        ///     </item>
        ///     <item>
        ///         <term><c>false</c></term>
        ///         <description>Округлять до целого числа</description>
        ///     </item>
        ///     <item>
        ///         <term><c>null</c></term>
        ///         <description>Необходимость округления определяется настройкой <see cref="Resto.Framework.Common.Currency.ICurrency.ShowFractionalPart"/> текущей валюты</description>
        ///     </item>
        /// </list>
        /// </param>
        /// <returns>
        /// Строковое представление денежной величины <paramref name="money"/> с учётом переданных параметров.
        /// </returns>
        public static string MoneyToString(this decimal money, bool? showCurrency, bool? showFractionalPart)
        {
            return MoneyToString(money, CurrencyProvider.Currency, showCurrency, showFractionalPart);
        }

        public static string MoneyToString(this decimal money, ICurrency currency, bool? showCurrency, bool? showFractionalPart)
        {
            return money.MoneyToStringWithCulture(currency, showCurrency, showFractionalPart, null);
        }

        private static string MoneyToStringWithCulture(this decimal money, ICurrency currency, bool? showCurrency, bool? showFractionalPart, CultureInfo cultureInfo)
        {
            var fractionalPartLength = showFractionalPart ?? currency.ShowFractionalPart
                                           ? currency.FractionalPartLength()
                                           : Math.Min(currency.FractionalPartLength(), 0);

            return (showCurrency ?? currency.ShowCurrency) && !currency.ShortNameForGui.IsNullOrWhiteSpace()
                       ? money.MoneyToStringWithCulture(fractionalPartLength, currency.MinimumDenomination, currency.ShortNameForGui, currency.ShowCurrencyAfterSum, cultureInfo)
                       : money.MoneyToStringWithCulture(fractionalPartLength, currency.MinimumDenomination, cultureInfo);
        }

        /// <summary>
        /// Возвращает строковое представление денежной величины.
        /// </summary>
        /// <param name="money">Числовое значение денежной величины.</param>
        /// <returns>Строковое представление денежной величины.</returns>
        /// <example>
        /// Пример возвращаемого значения (для величины 146.20 рублей):
        /// 146 руб. 20 коп.
        /// 
        /// Пример возвращаемого значения (для величины 146 рублей):
        /// 146 руб. 00 коп.
        /// 
        /// Пример возвращаемого значения (для величины 146 драм):
        /// 146 драм.
        /// </example>
        public static string MoneyToStringFull(this decimal money)
        {
            return MoneyToStringFull(money, CurrencyProvider.Currency);
        }

        public static string MoneyToStringFull(this decimal money, ICurrency currency)
        {
            var valueOfMainCurrencyUnit = Math.Floor(money);

            if (currency.FractionalPartLength() > 0)
            {
                var valueOfAdditionalCurrencyUnit = (int)((money - valueOfMainCurrencyUnit) * (decimal)Math.Pow(10, currency.FractionalPartLength()));

                // Определить формат для вывода числа, представляющего количество монет валюты (дробная часть денежной величины).
                // Это число должно быть целым и содержать столько же разрядов, как и длина дробной части валюты.
                var additionalCurrencyFormat = "D" + currency.FractionalPartLength();
                var valueOfAdditionalCurrencyUnitString = valueOfAdditionalCurrencyUnit.ToString(additionalCurrencyFormat);
                return string.Format("{0} {1} {2} {3}",
                    valueOfMainCurrencyUnit.ToString(CultureInfo.CurrentUICulture),
                    currency.ShortName,
                    valueOfAdditionalCurrencyUnitString.ToString(CultureInfo.CurrentUICulture),
                    currency.CentName);

            }
            return string.Format("{0} {1}", valueOfMainCurrencyUnit.ToString(CultureInfo.CurrentUICulture), CurrencyProvider.Currency.ShortName);
        }

        /// <summary>
        /// Преобразует денежную величину <paramref name="money"/> в строку для печати
        /// (с дробной частью, без разделителей групп символов).
        /// </summary>
        /// <param name="money">Денежная величина, котороую нужно преобразовать в строку</param>
        public static string FractionalMoneyToStringForPrint(this decimal money)
        {
            return money.MoneyToStringForPrint(CurrentCurrencyFractionalPartLength, 0m);
        }

        /// <summary>
        /// Преобразует денежную величину <paramref name="money"/> в строку для печати
        /// (с округлением стоимости, настроенным в BackOffice, без разделителей групп символов).
        /// </summary>
        /// <param name="money">Денежная величина, котороую нужно преобразовать в строку</param>
        public static string MoneyToStringForPrint(this decimal money)
        {
            return money.MoneyToStringForPrint(CurrentCurrencyFractionalPartLength, CurrentCurrencyMinimumDenomination);
        }

        public static string MoneyToStringForPrint(this decimal money, ICurrency currency)
        {
            return money.MoneyToStringForPrint(currency.FractionalPartLength(), currency.MinimumDenomination);
        }

        /// <summary>
        /// Преобразует число в строку денежного представления.
        /// Пример: 30 руб. 50 коп
        /// </summary>
        public static string MoneyToShortString(this decimal value, ICurrency currency = null)
        {
            if (currency == null)
                currency = CurrencyProvider.Currency;

            var n = (long)value;
            return string.Format(@"{0} {1} {2:00} {3}", n.ToString(CultureInfo.CurrentUICulture),
                currency.ShortName,
                GetReminder(value),
                currency.CentName);
        }

        /// <summary>
        /// Преобразует число в строку денежного представления и отбрасывает дробную часть
        /// Пример для 30.59: тридцать руб
        /// </summary>
        public static string MoneyToShortCurrencyString(this decimal value)
        {
            var currency = CurrencyProvider.Currency;
            var isoName = currency.IsoName;
            if (!Currencies.ContainsKey(isoName))
                throw new ArgumentOutOfRangeException(nameof(value), string.Format(CurrencyResources.CurrencyConvertercCurrencyNotRegistered, currency));

            var info = Currencies[isoName];
            return MoneyToString(value, info.Male,
                                 currency.ShortName, currency.ShortName, currency.ShortName,
                                 null, null, null).Trim();
        }

        /// <summary>
        /// Преобразует число в строку денежного представления прописью.
        /// </summary>
        /// <param name="val">Значение</param>
        /// <param name="male">Род валюты.</param>
        /// <param name="seniorOne">Наименование типа "один рубль"</param>
        /// <param name="seniorTwo">Наименование типа "два рубля"</param>
        /// <param name="seniorFive">Наименование типа "пять рублей"</param>
        /// <param name="juniorOne">Наименование типа "одна копейка"</param>
        /// <param name="juniorTwo">Наименование типа "две копейки"</param>
        /// <param name="juniorFive">Наименование типа "пять копеек"</param>
        /// <returns></returns>
        public static string MoneyToString(this decimal val, bool male,
            string seniorOne, string seniorTwo, string seniorFive,
            string juniorOne, string juniorTwo, string juniorFive)
        {
            var minus = false;
            if (val < 0) { val = -val; minus = true; }

            var remainder = GetReminder(val);
            var n = (long)val;

            var r = new StringBuilder();

            //if (0 == n) r.Append("0 ");
            if (0 == n) r.Append(CurrencyResources.CurrencyConverterZero);
            r.Append(n % 1000 != 0 ? MoneyToString(n, male, seniorOne, seniorTwo, seniorFive) : seniorFive);

            n /= 1000;

            r.Insert(0, MoneyToString(n, false, CurrencyResources.CurrencyConverterThousand1, CurrencyResources.CurrencyConverterThousand2, CurrencyResources.CurrencyConverterThousand3));
            n /= 1000;

            r.Insert(0, MoneyToString(n, true, CurrencyResources.CurrencyConverterMillion1, CurrencyResources.CurrencyConverterMillion2, CurrencyResources.CurrencyConverterMillion3));
            n /= 1000;

            r.Insert(0, MoneyToString(n, true, CurrencyResources.CurrencyConverterBillion1, CurrencyResources.CurrencyConverterBillion2, CurrencyResources.CurrencyConverterBillion3));
            n /= 1000;

            r.Insert(0, MoneyToString(n, true, CurrencyResources.CurrencyConverterTrillion1, CurrencyResources.CurrencyConverterTrillion2, CurrencyResources.CurrencyConverterTrillion3));
            n /= 1000;

            r.Insert(0, MoneyToString(n, true, CurrencyResources.CurrencyConverterTrilliard1, CurrencyResources.CurrencyConverterTrilliard2, CurrencyResources.CurrencyConverterTrilliard3));
            if (minus) r.Insert(0, CurrencyResources.CurrencyConverterMinus);

            //r.Append(remainder.ToString("00 "));
            //r.Append(CurrencyNumber.Case(remainder, juniorOneResName, juniorTwoResName, juniorFiveResName));

            if (!string.IsNullOrEmpty(juniorOne) && !string.IsNullOrEmpty(juniorTwo) &&
                !string.IsNullOrEmpty(juniorFive))
            {
                if (r[r.Length - 1] != ' ')
                {
                    r.Append(" ");
                }
                r.Append(remainder.ToString("00 "));
                r.Append(GetCase(remainder, juniorOne, juniorTwo, juniorFive));
            }

            //Делаем первую букву заглавной
            r[0] = char.ToUpper(r[0]);

            return r.ToString();
        }

        /// <summary>
        /// Преобразует число в строку денежного представления прописью.
        /// </summary>
        /// <param name="val">Значение</param>
        /// <param name="male">Род</param>
        /// <param name="one">Наименование типа "один тугрик"</param>
        /// <param name="two">Наименование типа "два тугрика"</param>
        /// <param name="five">Наименование типа "пять тугриков"</param>
        /// <returns></returns>
        public static string MoneyToString(this long val, bool male, string one, string two, string five)
        {
            string[] frac20 =
            {
                "", CurrencyResources.CurrencyNumber1, CurrencyResources.CurrencyNumber2, CurrencyResources.CurrencyNumber3, CurrencyResources.CurrencyNumber4,
                CurrencyResources.CurrencyNumber5, CurrencyResources.CurrencyNumber6, CurrencyResources.CurrencyNumber7, CurrencyResources.CurrencyNumber8,
                CurrencyResources.CurrencyNumber9, CurrencyResources.CurrencyNumber10i, CurrencyResources.CurrencyNumber11, CurrencyResources.CurrencyNumber12,
                CurrencyResources.CurrencyNumber13, CurrencyResources.CurrencyNumber14, CurrencyResources.CurrencyNumber15, CurrencyResources.CurrencyNumber16,
                CurrencyResources.CurrencyNumber17, CurrencyResources.CurrencyNumber18, CurrencyResources.CurrencyNumber19
            };

            var num = val % 1000;
            if (0 == num) return "";
            if (num < 0) throw new ArgumentOutOfRangeException(nameof(val), CurrencyResources.CurrencyNumberParemeterCantBeNegative);
            if (!male)
            {
                frac20[1] = CurrencyResources.CurrencyNumberOneFem;
                frac20[2] = CurrencyResources.CurrencyNumberTwoFem;
            }

            var r = new StringBuilder(GetHundreds(num / 100));

            if (num % 100 < 20)
            {
                r.Append(frac20[num % 100]);
            }
            else
            {
                r.Append(GetTens(num % 100 / 10));
                r.Append(frac20[num % 10]);
            }

            r.Append(GetCase(num, one, two, five));

            if (r.Length != 0) r.Append(" ");
            return r.ToString();
        }

        /// <summary>
        /// Возвращает формат представления денежной единицы в строковом виде.
        /// Пример: nx, где x - точность округления взятая из текущих настроек корпорации.
        /// </summary>
        public static string GetFormat()
        {
            return "n" + GetMoneyPrecision();
        }

        /// <summary>
        /// Возвращает шаблон представления денежной единицы в строковом виде в числовом формате.
        /// Пример: {0:nx}, где x - точность округления взятая из текущих настроек корпорации.
        /// </summary>
        public static string GetFormatLayout()
        {
            return "{0:" + GetFormat() + "}";
        }

        /// <summary>
        /// Возвращает шаблон представления денежной единицы в строковом виде в денежном формате.
        /// Пример: {0:Cx}, где x - точность округления взятая из текущих настроек корпорации.
        /// </summary>
        public static string GetCurrencyFormatLayout()
        {
            return "{0:C" + GetMoneyPrecision() + "}";
        }

        public static int GetMoneyPrecision()
        {
            return CurrencyProvider.MoneyPrecision;
        }

        /// <summary>
        /// Минимальное значение валюты с учетом точности округления.
        /// </summary>
        public static decimal CurrencyMinPrecision()
        {
            return (decimal)(1d / Math.Pow(10, GetMoneyPrecision()));
        }

        private static long GetReminder(decimal val)
        {
            var n = (long)val;
            return (long)((val - n + (decimal)0.005) * 100);
        }

        private static string GetHundreds(long hundredsCount)
        {
            switch (hundredsCount)
            {
                case 0: return "";
                case 1: return CurrencyResources.CurrencyNumber100;
                case 2: return CurrencyResources.CurrencyNumber200;
                case 3: return CurrencyResources.CurrencyNumber300;
                case 4: return CurrencyResources.CurrencyNumber400;
                case 5: return CurrencyResources.CurrencyNumber500;
                case 6: return CurrencyResources.CurrencyNumber600;
                case 7: return CurrencyResources.CurrencyNumber700;
                case 8: return CurrencyResources.CurrencyNumber800;
                case 9: return CurrencyResources.CurrencyNumber900;
                default: throw new ArgumentOutOfRangeException(nameof(hundredsCount), hundredsCount, "Hundreds count must be between 0 and 9.");
            }
        }

        private static string GetTens(long tensCount)
        {
            switch (tensCount)
            {
                case 0: return "";
                case 1: return CurrencyResources.CurrencyNumber10;
                case 2: return CurrencyResources.CurrencyNumber20;
                case 3: return CurrencyResources.CurrencyNumber30;
                case 4: return CurrencyResources.CurrencyNumber40;
                case 5: return CurrencyResources.CurrencyNumber50;
                case 6: return CurrencyResources.CurrencyNumber60;
                case 7: return CurrencyResources.CurrencyNumber70;
                case 8: return CurrencyResources.CurrencyNumber80;
                case 9: return CurrencyResources.CurrencyNumber90;
                default: throw new ArgumentOutOfRangeException(nameof(tensCount), tensCount, "Tens count must be between 0 and 9.");
            }
        }

        private static string GetCase(long val, string one, string two, string five)
        {
            var t = (val % 100 > 20) ? val % 10 : val % 20;

            switch (t)
            {
                case 1: return one;
                case 2:
                case 3:
                case 4: return two;
                default: return five;
            }
        }
    }
}
