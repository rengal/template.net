using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Data;

namespace Resto.Data
{
    partial class EgaisAbstractDocument
    {
        public abstract EgaisDocumentTypes Type { get; }

        /// <summary>
        /// ТП на УТМ которого пришла данная накладная
        /// </summary>
        [CanBeNull]
        public Department Department
        {
            get
            {
                if (DepartmentId == null)
                {
                    return null;
                }
                return EntityManager.INSTANCE.Get(DepartmentId.Value) as Department;
            }
        }

        /// <summary>
        /// Документ можно редактировать
        /// </summary>
        public virtual bool IsEditable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Документ можно удалять
        /// </summary>
        public virtual bool IsDeleteable
        {
            get { return true; }
        }

        public string DocumentRegId
        {
            get
            {
                var incomingInvoice = this as EgaisIncomingInvoice;
                if (incomingInvoice != null)
                {
                    return incomingInvoice.SourceRarId;
                }

                var outgoingInvoice = this as EgaisOutgoingInvoice;
                if (outgoingInvoice != null)
                {
                    return outgoingInvoice.SenderRarId;
                }

                var balanceDocument = this as EgaisBalanceDocument;
                if (balanceDocument != null)
                {
                    return balanceDocument.SourceRarId;
                }

                var internalDocument = this as EgaisAbstractInternalDocument;
                if (internalDocument != null)
                {
                    return internalDocument.SourceRarId;
                }

                var markConfirm = this as EgaisMarkConfirm;
                if (markConfirm != null)
                {
                    return markConfirm.SourceRarId;
                }

                return null;
            }
            set
            {
                var incomingInvoice = this as EgaisIncomingInvoice;
                if (incomingInvoice != null)
                {
                    incomingInvoice.SourceRarId = value;
                }

                var outgoingInvoice = this as EgaisOutgoingInvoice;
                if (outgoingInvoice != null)
                {
                    outgoingInvoice.SenderRarId = value;
                }

                var balanceDocument = this as EgaisBalanceDocument;
                if (balanceDocument != null)
                {
                    balanceDocument.SourceRarId = value;
                }

                var internalDocument = this as EgaisAbstractInternalDocument;
                if (internalDocument != null)
                {
                    internalDocument.SourceRarId = value;
                }

                var markConfirm = this as EgaisMarkConfirm;
                if (markConfirm != null)
                {
                    markConfirm.SourceRarId = value;
                }
            }
        }
    }
}
