using System.Collections.Generic;
using System.Linq;
using Resto.Common;
using Resto.Common.Extensions;
using Resto.Framework.Common;

namespace Resto.Data
{
    public sealed partial class ProductionOrderDocument
    {
        /// <summary>
        /// Признак того, что указанный в документе поставщик является исполнителем заказа.
        /// </summary>
        public bool IsSupplierTo
        {
            get { return supplier != null && departmentTo == null; }
        }

        /// <summary>
        /// Признак того, что указанный в документе поставщик является заказчиком.
        /// </summary>
        public bool IsSupplierFrom
        {
            get { return supplier != null && departmentTo != null; }
        }

        public override Store DocStore
        {
            get { return storeFrom; }
            set
            {
                storeFrom = value;
                departmentFrom = value.GetDepartment();
            }
        }

        public override Store DocStoreTo
        {
            get { return null; }
            set { }
        }

        public override DepartmentEntity DocDepartmentTo
        {
            get { return DepartmentTo; }
            set { DepartmentTo = value; }
        }

        public override Account DocAccount
        {
            get { return null; }
            set { }
        }

        public override IEnumerable<IncomingDocumentItem> DocItems
        {
            get
            {
                return Items
                    .Select(item => new IncomingDocumentItem(
                        item.Id,
                        item.Num,
                        item.Product,
                        ProductSizeServerConstants.INSTANCE.DefaultProductSize,
                        ProductSizeServerConstants.INSTANCE.DefaultAmountFactor,
                        item.Amount,
                        item.AmountUnit,
                        item.ContainerId.GetValueOrFakeDefault()))
                    .ToList();
            }
            set
            {
                Items.Set(value.Select(item =>
                    new ProductionOrderItem(
                        item.Id,
                        item.Number,
                        item.Product,
                        item.Amount,
                        item.Unit,
                        this,
                        null,
                        null,
                        null,
                        0,
                        null,
                        null)
                        {
                            ContainerId = item.Container.Id,
                        }));
            }
        }

        public override DocumentType DocumentType
        {
            get { return DocumentType.PRODUCTION_ORDER; }
        }
    }
}