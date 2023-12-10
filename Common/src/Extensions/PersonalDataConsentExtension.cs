using System;
using System.Diagnostics.CodeAnalysis;
using Resto.Data;
using Resto.Framework.Attributes.JetBrains;
using NotNullAttribute = Resto.Framework.Attributes.JetBrains.NotNullAttribute;
#pragma warning disable 1584,1711,1572,1581,1580

namespace Resto.Common.Extensions
{
    /// <summary>
    /// Класс, позволяющий определять текущий статус данных для пользователя.
    /// </summary>
    public static class PersonalDataConsentExtension
    {
        /// <summary>
        /// Возвращает статус <paramref name="customer"/>.
        /// Если <paramref name="isPdpConsentRequired"/> == <c>false</c> (настройка корпорации "Требовать согласие..."),
        /// возвращает <c>null</c>.
        /// </summary>
        /// <param name="customer">Гость</param>
        /// <param name="today">Сегодняшний день</param>
        /// <param name="isPdpConsentRequired">Настройка "Требовать согласие..."</param>
        [CanBeNull]
        public static PersonalDataConsent DetermineConsentStatus([NotNull] this IWithPdpInfo customer, DateTime today,
            bool isPdpConsentRequired)
        {
            return DetermineConsentStatus(isPdpConsentRequired, today,
                customer.PersonalDataStatus, customer.PersonalDataConsent,
                customer.ConsentDateFrom, customer.ConsentDateTo,
                customer.ProcessingDateFrom, customer.ProcessingDateTo);
        }

        /// <summary>
        /// Возвращает статус <paramref name="customer"/>.
        /// Если <paramref name="isPdpConsentRequired"/> == <c>false</c> (настройка корпорации "Требовать согласие..."),
        /// возвращает <c>null</c>.
        /// </summary>
        /// <remarks>
        /// Игнорируем кучу инспекций, чтобы код был как можно больше похож на java-реализацию,
        /// чтобы копипастить его туда-сюда.
        /// </remarks>
        /// <see cref="resto.front.customer.PersonalDataConsent#of"/>
        [CanBeNull]
        [SuppressMessage("ReSharper", "InvertIf")]
        [SuppressMessage("ReSharper", "ConvertIfStatementToSwitchStatement")]
        [SuppressMessage("ReSharper", "RedundantIfElseBlock")]
        public static PersonalDataConsent DetermineConsentStatus(
            bool isPdpConsentRequired,
            DateTime now,
            [NotNull] PersonalDataStatus status,
            bool? consent,
            DateTime? consentDateFrom,
            DateTime? consentDateTo,
            DateTime? processingDateFrom,
            DateTime? processingDateTo
        )
        {
            if (status == PersonalDataStatus.ANONYMIZED ||
                status == PersonalDataStatus.NONE)
            {
                return PersonalDataConsent.ANONYMIZED;
            }

            if (!isPdpConsentRequired)
            {
                return null;
            }

            var hasConsentPeriod = consentDateFrom != null && consentDateTo != null;
            var hasProcessingPeriod = processingDateFrom != null && processingDateTo != null;

            if (consentDateFrom == null ^ consentDateTo == null ||
                processingDateFrom == null ^ processingDateTo == null)
            {
                // Обе даты, задающие период, должны присутствовать.
                return PersonalDataConsent.INVALID;
            }

            if (!hasConsentPeriod && !hasProcessingPeriod)
            {
                if (consent == null)
                {
                    return PersonalDataConsent.MISSING_CONSENT_FLAG;
                }
                else if (consent == true)
                {
                    return PersonalDataConsent.MISSING_CONSENT_DATES;
                }
                else
                {
                    // Не давал или отозвал согласие на старой версии iiko.
                    return PersonalDataConsent.DENIED_CONSENT;
                }
            }

            if (hasConsentPeriod && hasProcessingPeriod)
            {
                return PersonalDataConsent.INVALID;
            }

            if (hasProcessingPeriod)
            {
                if (consent == false)
                {
                    return PersonalDataConsent.DENIED_CONSENT;
                }
                else if (now >= processingDateFrom)
                {
                    return now < processingDateTo
                        ? PersonalDataConsent.AWAITING_CONSENT_PROCESSING
                        : PersonalDataConsent.SUSPENDED_PROCESSING;
                }
            }
            else
            {
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (hasConsentPeriod)
                {
                    if (consent == null)
                    {
                        // Если есть срок, должно быть и согласие.
                        return PersonalDataConsent.INVALID;
                    }
                    else if (consent == true)
                    {
                        if (now >= consentDateFrom)
                        {
                            return now < consentDateTo
                                ? PersonalDataConsent.VALID_CONSENT
                                : PersonalDataConsent.EXPIRED_CONSENT;
                        }
                    }
                    else
                    {
                        return PersonalDataConsent.REVOKED_CONSENT;
                    }
                }
            }

            return PersonalDataConsent.INVALID;
        }
    }
}