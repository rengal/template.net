using System;
using System.Collections.Generic;
using Resto.Common.Properties;

// ReSharper disable CheckNamespace
namespace Resto.Data
// ReSharper restore CheckNamespace
{
    public partial class EdiOrderDocument
    {
        #region Properties

        public override Store DocStore
        {
            get { return null; }
            set { }
        }

        public override Store DocStoreTo
        {
            get { return null; }
            set { }
        }

        public override DepartmentEntity DocDepartmentTo
        {
            get { return null; }
            set { }
        }

        public override Account DocAccount
        {
            get { return null; }
            set { }
        }

        public override IEnumerable<IncomingDocumentItem> DocItems
        {
            get { return null; }
            set { }
        }

        public override DocumentType DocumentType
        {
            get { return DocumentType.EDI_ORDER; }
        }

        public override bool AllowDuplicateDocumentNumber
        {
            get { return false; }
        }

        #endregion

        #region Methods

        public static string GetLocalizedStatus(EdiOrderStatus status)
        {
            switch (status)
            {
                case EdiOrderStatus.CREATED:
                    return Resources.OrderStatusCreated;
                case EdiOrderStatus.PROCESSED:
                    return Resources.OrderStatusProcessed;
                case EdiOrderStatus.TO_SEND_REQ:
                case EdiOrderStatus.TO_SEND:
                    return Resources.OrderStatusToSend;
                case EdiOrderStatus.SENT:
                    return Resources.OrderStatusSent;
                case EdiOrderStatus.CONFIRMED:
                    return Resources.OrderStatusConfirmed;
                case EdiOrderStatus.CONFIRMED_WITH_CHANGES:
                    return Resources.OrderStatusConfirmedWithChanges;
                case EdiOrderStatus.DESPATCHED:
                    return Resources.OrderStatusDespatched;
                case EdiOrderStatus.EXECUTED:
                    return Resources.OrderStatusExecuted;
                case EdiOrderStatus.DISCARDED:
                case EdiOrderStatus.CANCELED:
                case EdiOrderStatus.CANCELED_REQ:
                    return Resources.OrderStatusDiscarded;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
    }
}
