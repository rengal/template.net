using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Common;
using Resto.Common.Extensions;

namespace Resto.Data
{
    public partial class EdiOrderDocumentListRecord
    {
        /// <summary>
        /// Строковое представление даты доставки
        /// </summary>
        public override string DeliveryDateAsString
        {
            get
            {
                return DeliveryDate.HasValue ? DateTimeUtils.FormatDateTime(DeliveryDate.GetValueOrFakeDefault()) : "-";
            }
        }

        /// <summary>
        /// Имя поставщика
        /// </summary>
        public override string SellerAsString
        {
            get
            {
                return Seller.NameLocal;
            }
        }

        /// <summary>
        /// Имя плательщика
        /// </summary>
        public override string PayerAsString
        {
            get { return Payer == null ? string.Empty : Payer.Name; }
        }

        /// <summary>
        /// Строковое представление статуса заказа
        /// </summary>
        public override string StatusAsString
        {
            get
            {
                return EdiOrderDocument.GetLocalizedStatus(Status);
            }
        }
    }
}
