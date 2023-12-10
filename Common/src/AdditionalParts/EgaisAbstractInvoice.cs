using Resto.Common.Extensions;
using Resto.Common.Properties;
using Resto.Framework.Common;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class EgaisAbstractInvoice
    {
        #region Properties

        /// <summary>
        /// Заголовок документа
        /// </summary>
        public override string DocumentFullCaption
        {
            get { return string.Format(Resources.EgaisInvoiceDocumentFullCaption, WbNumber, WbDate.GetValueOrFakeDefault()); }
        }

        /// <summary>
        /// Возвращает либо краткое либо полное (если <see cref="ShipperInfo.ShortName"/> == null) имя грузоотправителя.
        /// </summary>
        public string ShipperFullName
        {
            get
            {
                if (ShipperInfo == null)
                {
                    return null;
                }
                return ShipperInfo.ShortName.IsNullOrEmpty() ? ShipperInfo.FullName : ShipperInfo.ShortName;
            }
        }

        public override string DocumentStatusStr
        {
            get { return EgaisInvoiceConfirmStatusText; }
        }

        public virtual string EgaisInvoiceStatusText
        {
            get { return string.Empty; }
        }

        public virtual string EgaisInvoiceConfirmStatusText
        {
            get { return string.Empty; }
        }

        public bool IsUnpacked
        {
            get { return packed != null && !packed.Value; }
        }

        #endregion
    }
}