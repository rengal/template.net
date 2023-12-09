using System.Collections.Generic;

namespace Resto.Framework.Common.Currency
{
    public interface ICurrency
    {
        /// <summary>
        /// Полное имя валюты
        /// </summary>
        string IsoName { get; set; }

        /// <summary>
        /// Сокращенное имя валюты (руб.)
        /// Используется для отображения в GUI для BackOffice и FrontOffice
        /// </summary>
        string ShortNameForGui { get; set; }

        /// <summary>
        /// Основная единица измерения валюты.
        /// </summary>
        /// <example>
        /// Пример: "руб."
        /// </example>
        string ShortName { get; set; }

        /// <summary>
        /// Дополнительная единица измерения валюты.
        /// </summary>
        /// <example>
        /// Пример: "коп."
        /// </example>
        string CentName { get; set; }

        /// <summary>
        /// Показывать ли на UI дробную часть
        /// </summary>
        bool ShowFractionalPart { get; set; }

        /// <summary>
        /// Показывать ли на UI название валюту
        /// </summary>
        bool ShowCurrency { get; set; }

        /// <summary>
        /// Показывать на UI валюту после суммы
        /// </summary>
        bool ShowCurrencyAfterSum { get; set; }

        /// <summary>
        /// Список номиналов купюр для numpad'а во фронте
        /// </summary>
        List<int> Denominations { get; set; }

        /// <summary>
        /// Номинал минимальной купюры/монеты. До этого значения округляются стоимости.
        /// Если 0, то округление не выполняется.
        /// </summary>
        decimal MinimumDenomination { get; set; }

        /// <summary>
        /// Код валюты
        /// </summary>
        string Code { get; set; }
    }
}