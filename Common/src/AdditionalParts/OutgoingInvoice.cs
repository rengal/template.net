using System;
using System.Linq;
using Resto.Framework.Common;

namespace Resto.Data
{
    public sealed partial class OutgoingInvoice
    {
        public OutgoingInvoice(Guid id, DateTime dateIncoming, string documentNumber, DocumentStatus status, User supplier,
                               Account revenueAccount, Account revenueDebitAccount, Account accountTo, Account discountsAccount, Store defaultStore)
            : this(id, dateIncoming, documentNumber, status, supplier, revenueAccount, revenueDebitAccount, accountTo, discountsAccount)
        {
            DefaultStore = defaultStore;
        }

        public override DocumentType DocumentType
        {
            get { return DocumentType.OUTGOING_INVOICE; }
        }

        /// <summary>
        /// Адрес грузополучателя для случаев, когда данная РН является накладной доставки
        /// </summary>
        /// <remarks>
        /// Пока что адрес грузополучателя нужен в редких случаях (при печати отчётов),
        /// поэтому можно его запрашивать не раньше, чем он действетельно будет нужен.
        /// Если в будущем понадобится массовое получение адресов (например, в грид накладных
        /// будет добавлена колонка "Адрес доставки"), то нужно будет заменить этот метод на
        /// соответствующий кеш. Только нужно будет подумать над стратегией обновления кеша,
        /// чтобы не получилось как в RMS-45332.
        /// </remarks>
        public string GetDeliveryAddress()
        {
            var addressesMap = ServiceClientFactory.DocumentService.GetDeliveryAddresses(Id.AsSequence().ToArray()).CallSync();
            return addressesMap != null && addressesMap.ContainsKey(Id) ? addressesMap[Id] : string.Empty;
        }
    }
}