using System;

namespace Resto.Data
{
    /// <summary>
    /// Интерфейс описывающий сущность с информацией об обработке персональных данных.
    /// </summary>
    public interface IWithPdpInfo
    {   
        /// <summary>
        /// Дата начала действия согласия обработки персональных данных.
        /// </summary>
        DateTime? ConsentDateFrom { get; }

        /// <summary>
        /// Дата окончания действия согласия обработки персональных данных.
        /// </summary>
        DateTime? ConsentDateTo { get; }

        /// <summary>
        /// Дата начала срока обработки персональных данных.
        /// </summary>
        DateTime? ProcessingDateFrom { get; }

        /// <summary>
        /// Дата окончания срока обработки персональных данных.
        /// </summary>
        DateTime? ProcessingDateTo { get; }

        /// <summary>
        /// Установлен срок согласия на обработку персональных данных.
        /// </summary>
        bool HasConsentPeriod { get; }

        /// <summary>
        /// Установлен срок легальной обработки данных.
        /// </summary>
        bool HasProcessingPeriod { get; }

        /// <summary>
        /// Согласие на обработку персональных данных.
        /// </summary>
        bool? PersonalDataConsent { get; }

        /// <summary>
        /// Статус персональных данных.
        /// </summary>
        PersonalDataStatus PersonalDataStatus { get; }
    }
}