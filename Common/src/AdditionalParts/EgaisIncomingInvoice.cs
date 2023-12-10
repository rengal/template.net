using System.Linq;
using Resto.Common.Localization;
using Resto.Common.Properties;
using Resto.Framework.Common;

namespace Resto.Data
{
    /// <summary>
    /// Приходная накладная ЕГАИС
    /// </summary>
    public partial class EgaisIncomingInvoice
    {
        #region Properties

        public bool ReadyToConfirm
        {
            get
            {
                return IsReadyToConfirmInvoice(out _);
            }
        }

        public bool ReadyToReject
        {
            get
            {
                return IsReadyToRejectInvoice(out _);
            }
        }

        public override bool IsEditable
        {
            get { return Status.Editable; }
        }

        /// <summary>
        /// Накладная имеет статус, в котором акт согласия или отказа может быть отменен.
        /// </summary>
        /// <see cref="EgaisRequestRepeal"/>
        public bool IsRepealable => ConfirmStatus == EgaisIncomingInvoiceConfirmStatus.CONFIRMED ||
                                    ConfirmStatus == EgaisIncomingInvoiceConfirmStatus.DISCARDED;

        public bool AllowCreateTransferToShop
        {
            get { return status == EgaisIncomingInvoiceStatus.CONFIRMED; }
        }

        public bool AllowCreateWriteoffFromShop
        {
            get { return status == EgaisIncomingInvoiceStatus.CONFIRMED; }
        }

        public bool AllowCreateInvoice
        {
            get
            {
                // Для создания внутренней накладной требуется выполнение следующих условий:
                // - установлена дата приемки
                var isSetDateIncoming = DateIncoming != null;
                // - во всех элементах накладной проставлен продукт
                var isSetAllProducts = Items.All(item => item.Product != null);
                // - у текущего пользователя есть право создавать накладные по позициям с пустым производителем/импортером,
                // либо производители/импортеры по всем позициям заполнены
                var isSetAllProducers = ServerSession.CurrentSession.GetCurrentUser()
                    .HasPermissionWhereResponsible(Permission.CAN_CREATE_INVOICE_FROM_EGAIS_WITHOUT_PRODUCER) ||
                    Items.Cast<EgaisAbstractInvoiceItem>().All(item => !item.IsNativeProducerNotExist && !item.IsNativeImporterNotExist);

                return isSetDateIncoming && isSetAllProducts && isSetAllProducers;
            }
        }

        public bool AllowCreateReturnedInvoice
        {
            get
            {
                return status == EgaisIncomingInvoiceStatus.CONFIRMED &&
                       confirmStatus == EgaisIncomingInvoiceConfirmStatus.CONFIRMED;
            }
        }

        public override string DocumentFullCaption
        {
            get { return string.Format(Resources.EgaisInvoiceDocumentFullCaption, WbNumber, WbDate); }
        }

        public override EgaisDocumentTypes Type
        {
            get
            {
                return EgaisDocumentTypes.EGAIS_INCOMING_INVOICE;
            }
        }

        public override string EgaisInvoiceStatusText
        {
            get
            {
                return status.GetLocalName();
            }
        }

        public override string EgaisInvoiceConfirmStatusText
        {
            get
            {
                return confirmStatus.GetLocalName();
            }
        }

        /// <summary>
        /// Возвращает имя УТМ-подключения, либо, если оно = null, <see cref="sourceRarId"/>
        /// </summary>
        public string UtmName
        {
            get
            {
                if (Department == null || Department.EgaisConnectionsSettings == null)
                {
                    return sourceRarId;
                }

                var connectionSettings =
                    Department.EgaisConnectionsSettings.Connections.FirstOrDefault(
                        connection => connection.FsRarId == sourceRarId);
                return connectionSettings != null ? connectionSettings.Name : sourceRarId;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Проверяет, возможно ли подтвердить накладную
        /// </summary>
        /// <param name="errorMessage">Сообщение об ошибке при валидации</param>
        /// <returns><c>true</c> - если накладную можно подтвердить</returns>
        public bool IsReadyToConfirmInvoice(out string errorMessage)
        {
            if (!IsReadyToRejectInvoice(out errorMessage))
            {
                return false;
            }

            if (Items.All(item => item.ActualAmount == 0m))
            {
                errorMessage = Resources.AmountInEgaisInvoiceItemsRequired;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Проверяет, возможно ли отменить накладную
        /// </summary>
        /// <param name="errorMessage">Сообщение об ошибке при валидации</param>
        /// <returns><c>true</c> - если накладную можно отменить</returns>
        public bool IsReadyToRejectInvoice(out string errorMessage)
        {
            errorMessage = null;
            if (DateIncoming == null)
            {
                errorMessage = Resources.DateIncomingInEgaisInvoiceRequired;
                return false;
            }

            if (DocumentNumber.IsNullOrWhiteSpace())
            {
                errorMessage = Resources.DocumentNumberInEgaisInvoiceRequired;
                return false;
            }

            if (!status.Editable)
            {
                errorMessage = string.Format(Resources.IncorrectEgaisStatusForOperation,
                    EgaisInvoiceStatusText);
                return false;
            }

            // В отличие от расходных накладных, confirmStatus проставляется нами до отправки,
            // так что и проверять его не нужно бы.
            // Как минимум, если акт не был принят ЕГАИС, его можно отправить повторно.
            if (status != EgaisIncomingInvoiceStatus.CHANGED_ACT_REJECTED_BY_EGAIS &&
                confirmStatus != EgaisIncomingInvoiceConfirmStatus.RECEIVED)
            {
                errorMessage = string.Format(Resources.IncorrectEgaisStatusForOperation,
                    EgaisInvoiceConfirmStatusText);
                return false;
            }

            return true;
        }

        #endregion
    }
}