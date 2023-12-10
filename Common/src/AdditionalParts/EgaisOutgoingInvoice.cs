using System;
using System.Linq;
using EnumerableExtensions;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common.Extensions;
using Resto.Common.Localization;
using Resto.Common.Properties;
using Resto.Framework.Common;

namespace Resto.Data
{
    /// <summary>
    /// Расходная накладная ЕГАИС
    /// </summary>
    public partial class EgaisOutgoingInvoice
    {
        #region Fields

        /// <summary>
        /// Накладная создана на основе другого документа
        /// </summary>
        public bool isCreatedFromDocument;

        #endregion Fields

        #region Properties

        public override bool IsEditable
        {
            get { return status.Editable; }
        }

        /// <summary>
        /// Готов к отправке
        /// </summary>
        public bool ReadyToSend
        {
            get
            {
                return IsReadyToSend(out _);
            }
        }

        public bool ReadyToConfirmOrReject
        {
            get
            {
                return IsReadyToConfirmOrReject(out _);
            }
        }

        public override string DocumentFullCaption
        {
            get
            {
                switch (OutgoingInvoiceType)
                {
                    case EgaisOutgoingInvoiceType.WB_INVOICE_FROM_ME:
                    {
                        return WbNumber.IsNullOrEmpty() 
                            ? string.Format(Resources.EgaisNewOutgoingInvoiceDocumnetFullCaption) 
                            : string.Format(Resources.EgaisOutgoingInvoiceDocumentFullCaption, WbNumber, WbDate);
                    }
                    case EgaisOutgoingInvoiceType.WB_RETURN_TO_ME:
                    case EgaisOutgoingInvoiceType.WB_INVOICE_TO_ME:
                        return string.Empty;
                    case EgaisOutgoingInvoiceType.WB_RETURN_FROM_ME:
                        return string.Format(Resources.EgaisReturnedInvoiceDocumentFullCaption, WbNumber, WbDate);
                    default:
                        throw new ArgumentOutOfRangeException(OutgoingInvoiceType.ToString());
                }
            }
        }

        public override EgaisDocumentTypes Type
        {
            get
            {
                return EgaisDocumentTypes.EGAIS_OUTGOING_INVOICE;
            }
        }

        public override string EgaisInvoiceStatusText
        {
            get
            {
                return Status.GetLocalName();
            }
        }

        public override string EgaisInvoiceConfirmStatusText
        {
            get
            {
                return ConfirmStatus.GetLocalName();
            }
        }

        public bool IsReturnedInvoice
        {
            get { return OutgoingInvoiceType == EgaisOutgoingInvoiceType.WB_RETURN_FROM_ME; }
        }

        public bool IsInvoiceFromMe
        {
            get { return OutgoingInvoiceType == EgaisOutgoingInvoiceType.WB_INVOICE_FROM_ME; }
        }

        public override decimal DocumentSum
        {
            get
            {
                return Items.Sum(item => GetSumForItemByStatus(item));
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Проверяет, можно ли отправить накладную контрагенту
        /// </summary>
        /// <param name="errorMessage">Сообщение об ошибке при валидации</param>
        /// <returns><c>true</c> - если накладную можно отправить</returns>
        public bool IsReadyToSend(out string errorMessage)
        {
            errorMessage = null;
            if (!IsEditable && status != EgaisOutgoingInvoiceStatus.REJECTED)
            {
                errorMessage = string.Format(Resources.IncorrectEgaisStatusForOperation,
                EgaisInvoiceStatusText);
                return false;
            }

            if (status == EgaisOutgoingInvoiceStatus.REJECTED)
            {
                errorMessage = string.Format(Resources.EgaisInvoiceRejectedErrorMessage, status);
                return false;
            }

            if (Deleted)
            {
                errorMessage = Resources.EgaisOutgoingInvoiceDeletedErrorMessage;
                return false;
            }

            if (Items.IsEmpty())
            {
                errorMessage = Resources.EgaisOutgoingInvocieEmptyProductListErrorMessage;
                return false;
            }

            if (Items.All(item => item.Amount == 0))
            {
                switch (OutgoingInvoiceType)
                {
                    case EgaisOutgoingInvoiceType.WB_INVOICE_FROM_ME:
                        errorMessage = Resources.EgaisOutgoingInvoiceAmountOfProductSentIsZeroErrorMessage;
                        break;
                    case EgaisOutgoingInvoiceType.WB_RETURN_TO_ME:
                    case EgaisOutgoingInvoiceType.WB_INVOICE_TO_ME:
                        // TODO: по мере реализации остальных типов накладных добавить соответствующие сообщения
                        break;
                    case EgaisOutgoingInvoiceType.WB_RETURN_FROM_ME:
                        errorMessage = Resources.EgaisOutgoingInvocieAmountProductSummaryIsZeroErrorMessage;
                        break;
                }
                return false;
            }

            // Запрещаем отправлять расходные накладные с незаполненными справками 2
            if (OutgoingInvoiceType == EgaisOutgoingInvoiceType.WB_INVOICE_FROM_ME)
            {
                if (Items.OfType<EgaisOutgoingInvoiceItem>().Any(outg => outg.PrevBRegId.IsNullOrWhiteSpace()))
                {
                    errorMessage = Resources.EgaisOutgoingInvoicePrevBregIdIsEmptyErrorMessage;
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Проверяет, возможно ли подтвердить или отменить накладную
        /// </summary>
        /// <param name="errorMessage">Сообщение об ошибке при валидации</param>
        /// <returns><c>true</c> - если накладную можно подтвердить или отменить</returns>
        public bool IsReadyToConfirmOrReject(out string errorMessage)
        {
            errorMessage = null;
            if (!status.Answerable)
            {
                errorMessage = string.Format(Resources.IncorrectEgaisStatusForOperation,
                    EgaisInvoiceStatusText);
                return false;
            }

            if (confirmStatus != EgaisOutgoingInvoiceConfirmStatus.CHANGED_BY_RECIPIENT)
            {
                errorMessage = string.Format(Resources.IncorrectEgaisStatusForOperation,
                    EgaisInvoiceConfirmStatusText);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Возвращает подтвержденное количество товара элемента накладной.
        /// Если в уже отправленой возвратной накладной по позиции отсутствует подтвержденное количество 
        /// считаем что оно равно количеству проставленому при формировании накладной (RMS-41519)
        /// </summary>
        public decimal GetActualAmountForItemByStatus([NotNull] AbstractExternalDocumentItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }

            if (item.ActualAmount == null &&
                ConfirmStatus != EgaisOutgoingInvoiceConfirmStatus.NOT_CONFIRMED)
            {
                return item.Amount.GetValueOrFakeDefault();
            }
            else
            {
                return item.ActualAmount.GetValueOrFakeDefault();
            }
        }

        /// <summary>
        /// Возвращает сумму элемента накладной.
        /// Если накладная еще не отправлена сумма считается как произведение количества на цену.
        /// Иначе сумма = произведению результата метода <see cref="GetActualAmountForItemByStatus"/> на цену.
        /// </summary>
        public decimal GetSumForItemByStatus([NotNull] AbstractExternalDocumentItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException();
            }

            if (ConfirmStatus == EgaisOutgoingInvoiceConfirmStatus.NOT_CONFIRMED)
            {
                return item.Amount.GetValueOrFakeDefault() * item.Price.GetValueOrFakeDefault();
            }
            else
            {
                return GetActualAmountForItemByStatus(item) * item.Price.GetValueOrFakeDefault();
            }
        }

        #endregion
    }
}