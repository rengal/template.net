namespace Resto.Framework.Common.Currency
{
    public static partial class CurrencyHelper
    {
        /// <summary>
        /// Именительный падеж единственное число (Кто? Что?) [рубль]
        /// </summary>
        public static string GetNominativeOne(this ICurrency currency)
        {
            return GetCurrencyNameType(currency, Case.NominativeOne);
        }

        /// <summary>
        /// Именительный падеж единственное число (Кто? Что?) [рубль]
        /// </summary>
        public static string GetNominativeOne()
        {
            return GetNominativeOne(CurrencyProvider.Currency);
        }

        /// <summary>
        /// Именительный падеж, множественное число (Кто? Что?) [рубли]
        /// </summary>
        public static string GetNominativeMany(this ICurrency currency)
        {
            return GetCurrencyNameType(currency, Case.NominativeMany);
        }

        /// <summary>
        /// Именительный падеж, множественное число (Кто? Что?) [рубли]
        /// </summary>
        public static string GetNominativeMany()
        {
            return GetNominativeMany(CurrencyProvider.Currency);
        }

        /// <summary>
        /// Родительный падеж, единственное число (Кого? Чего?) [рубля]
        /// </summary>
        public static string GetGenetiveOne(this ICurrency currency)
        {
            return GetCurrencyNameType(currency, Case.GenetiveOne);
        }

        /// <summary>
        /// Родительный падеж, единственное число (Кого? Чего?) [рубля]
        /// </summary>
        public static string GetGenetiveOne()
        {
            return GetGenetiveOne(CurrencyProvider.Currency);
        }

        /// <summary>
        /// Родительный падеж, множественное число (Кого? Чего?) [рублей]
        /// </summary>
        public static string GetGenetiveMany(this ICurrency currency)
        {
            return GetCurrencyNameType(currency, Case.GenetiveMany);
        }

        /// <summary>
        /// Родительный падеж, множественное число (Кого? Чего?) [рублей]
        /// </summary>
        public static string GetGenetiveMany()
        {
            return GetGenetiveMany(CurrencyProvider.Currency);
        }

        /// <summary>
        /// Дательный падеж, единственное число (Кому? Чему?) [рублю]
        /// </summary>
        public static string GetDativeOne(this ICurrency currency)
        {
            return GetCurrencyNameType(currency, Case.DativeOne);
        }

        /// <summary>
        /// Дательный падеж, единственное число (Кому? Чему?) [рублю]
        /// </summary>
        public static string GetDativeOne()
        {
            return GetDativeOne(CurrencyProvider.Currency);
        }

        /// <summary>
        /// Дательный падеж, множественное число (Кому? Чему?) [рублям]
        /// </summary>
        public static string GetDativeMany(this ICurrency currency)
        {
            return GetCurrencyNameType(currency, Case.DativeMany);
        }

        /// <summary>
        /// Дательный падеж, множественное число (Кому? Чему?) [рублям]
        /// </summary>
        public static string GetDativeMany()
        {
            return GetDativeMany(CurrencyProvider.Currency);
        }

        /// <summary>
        /// Винительный падеж, единственное число (Кого? Что?) [рубль]
        /// </summary>
        public static string GetAccusativeOne(this ICurrency currency)
        {
            return GetCurrencyNameType(currency, Case.AccusativeOne);
        }

        /// <summary>
        /// Винительный падеж, единственное число (Кого? Что?) [рубль]
        /// </summary>
        public static string GetAccisativeOne()
        {
            return GetAccusativeOne(CurrencyProvider.Currency);
        }

        /// <summary>
        /// Винительный падеж, множественное число (Кого? Что?) [рубли]
        /// </summary>
        public static string GetAccusativeMany(this ICurrency currency)
        {
            return GetCurrencyNameType(currency, Case.AccusativeMany);
        }

        /// <summary>
        /// Винительный падеж, множественное число (Кого? Что?) [рубли]
        /// </summary>
        public static string GetAccusativeMany()
        {
            return GetAccusativeMany(CurrencyProvider.Currency);
        }

        /// <summary>
        /// Творительный падеж, единственное число (Кем? Чем?) [рублем]
        /// </summary>
        public static string GetInstrumentalOne(this ICurrency currency)
        {
            return GetCurrencyNameType(currency, Case.InstrumentalOne);
        }

        /// <summary>
        /// Творительный падеж, единственное число (Кем? Чем?) [рублем]
        /// </summary>
        public static string GetInstrumentalOne()
        {
            return GetInstrumentalOne(CurrencyProvider.Currency);
        }

        /// <summary>
        /// Творительный падеж, множественное число (Кем? Чем?) [рублями]
        /// </summary>
        public static string GetInstrumentalMany(this ICurrency currency)
        {
            return GetCurrencyNameType(currency, Case.InstrumentalMany);
        }

        /// <summary>
        /// Творительный падеж, множественное число (Кем? Чем?) [рублями]
        /// </summary>
        public static string GetInstrumentalMany()
        {
            return GetInstrumentalMany(CurrencyProvider.Currency);
        }

        /// <summary>
        /// Предложный падеж, единственное число (О ком? О чем?) [рубле]
        /// </summary>
        public static string GetPrepositionalOne(this ICurrency currency)
        {
            return GetCurrencyNameType(currency, Case.PrepositionalOne);
        }

        /// <summary>
        /// Предложный падеж, единственное число (О ком? О чем?) [рубле]
        /// </summary>
        public static string GetPrepositionalOne()
        {
            return GetPrepositionalOne(CurrencyProvider.Currency);
        }

        /// <summary>
        /// Предложный падеж, множественное число (О ком? О чем?) [рублях]
        /// </summary>
        public static string GetPrepositionalMany(this ICurrency currency)
        {
            return GetCurrencyNameType(currency, Case.PrepositionalMany);
        }

        /// <summary>
        /// Предложный падеж, множественное число (О ком? О чем?) [рублях]
        /// </summary>
        public static string GetPrepositionalMany()
        {
            return GetPrepositionalMany(CurrencyProvider.Currency);
        }
    }
}