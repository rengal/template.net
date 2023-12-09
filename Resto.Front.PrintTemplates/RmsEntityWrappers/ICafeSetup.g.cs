// This file was generated with T4.
// Do not edit it manually.

// ReSharper disable RedundantUsingDirective

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Front.PrintTemplates.RmsEntityWrappers
{
    /// <summary>
    /// Настройки торгового предприятия
    /// </summary>
    public interface ICafeSetup
    {
        /// <summary>
        /// Шапка отчётов
        /// </summary>
        [NotNull]
        string ReportHeader { get; }

        /// <summary>
        /// Шапка пречека
        /// </summary>
        [NotNull]
        string BillHeader { get; }

        /// <summary>
        /// Подвал пречека
        /// </summary>
        [NotNull]
        string BillFooter { get; }

        /// <summary>
        /// Название торгового предприятия
        /// </summary>
        [NotNull]
        string CafeName { get; }

        /// <summary>
        /// Адрес торгового предприятия
        /// </summary>
        [NotNull]
        string CafeAddress { get; }

        /// <summary>
        /// Наименование юридического лица
        /// </summary>
        [NotNull]
        string LegalName { get; }

        /// <summary>
        /// Юридический адрес
        /// </summary>
        [NotNull]
        string LegalAddress { get; }

        /// <summary>
        /// ИНН
        /// </summary>
        [NotNull]
        string TaxId { get; }

        /// <summary>
        /// Телефон
        /// </summary>
        [NotNull]
        string Phone { get; }

        /// <summary>
        /// КПП
        /// </summary>
        [NotNull]
        string AccountingReasonCode { get; }

        /// <summary>
        /// Код подразделения
        /// </summary>
        [CanBeNull]
        string DepartmentCode { get; }

        /// <summary>
        /// Название валюты
        /// </summary>
        [NotNull]
        string CurrencyName { get; }

        /// <summary>
        /// Стандартное название валюты (ISO)
        /// </summary>
        [NotNull]
        string CurrencyIsoName { get; }

        /// <summary>
        /// Сокращённое название валюты
        /// </summary>
        [NotNull]
        string CurrencyShortName { get; }

        /// <summary>
        /// Сокращённое название валюты в графическом интерфейсе
        /// </summary>
        [NotNull]
        string ShortCurrencyName { get; }

        /// <summary>
        /// НДС включен в стоимость блюд
        /// </summary>
        bool IncludeVatInDishPrice { get; }

        /// <summary>
        /// Показывать сумму личных продаж официанта по отдельным блюдам
        /// </summary>
        bool DisplayWaiterRevenueByDishes { get; }

        /// <summary>
        /// Отображать относительное количество модификаторов
        /// </summary>
        bool DisplayRelativeNumberOfModifiers { get; }

        /// <summary>
        /// Наименования курсов
        /// </summary>
        [NotNull]
        IDictionary<int, string> CourseCustomNames { get; }

    }
}
