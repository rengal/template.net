namespace Resto.Framework.Common.Currency
{
    public class CurrencySettings
    {
        private readonly string guiShort;
        private readonly string mainUnit;
        private readonly string additionalUnit;
        private readonly string currencyCombined;
        private readonly string code;
        private readonly string name;

        public CurrencySettings()
        {
            guiShort = CurrencyHelper.CurrencyProvider.Currency.ShortNameForGui;            
            mainUnit = CurrencyHelper.CurrencyProvider.Currency.ShortName;
            additionalUnit = CurrencyHelper.CurrencyProvider.Currency.CentName;
            currencyCombined = string.Format("{0} {1}", mainUnit, additionalUnit);
            code = CurrencyHelper.CurrencyProvider.Currency.Code;
            name = CurrencyHelper.CurrencyProvider.Currency.GetFullName();
        }

        /// <summary>
        /// Наименовани.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Код валюты.
        /// </summary>
        public string Code
        {
            get { return code; }
        }

        /// <summary>
        /// Основная единица измерения валюты.
        /// </summary>
        /// <example>
        /// Пример: "руб."
        /// </example>
        public string MainUnit
        {
            get { return mainUnit; }
        }

        /// <summary>
        /// Дополнительная единица измерения валюты.
        /// </summary>
        /// <example>
        /// Пример: "коп."
        /// </example>
        public string AdditionalUnit
        {
            get { return additionalUnit; }
        }

        /// <summary>
        /// Объединяет основую и доп-ю единицу измерения валюты (руб. коп.).
        /// </summary>
        public string CurrencyCombined
        {
            get { return currencyCombined; }
        }

        /// <summary>
        /// Используется для отображения в GUI для BackOffice и FrontOffice.
        /// </summary>
        public string GuiShort
        {
            get { return guiShort; }
        }
    }
}
