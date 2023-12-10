using System.Collections.Generic;
using System.Linq;
using Resto.Common.Extensions;

namespace Resto.Data
{
    public abstract partial class AbstractProductsWriteoffDocument
    {
        protected abstract List<AbstractProductsWriteoffDocumentItem> AbstractItems { get; }

        public override Store DocStore
        {
            get { return store; }
            set { store = value; }
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
            get { return accountTo; }
            set { accountTo = value; }
        }

        public override IEnumerable<IncomingDocumentItem> DocItems
        {
            get
            {
                return AbstractItems
                    .Select(item =>
                        new IncomingDocumentItem(
                            item.Id,
                            item.Num,
                            item.Product,
                            item.ProductSize,
                            item.AmountFactor,
                            item.Amount,
                            item.AmountUnit,
                            item.ContainerId.GetValueOrFakeDefault()))
                    .ToList();
            }
        }

        /// <summary>
        /// Обнуляет склад, требуя от пользователя выбрать его самостоятельно
        /// </summary>
        public void SetEmptyStore()
        {
            store = null;
        }
    }
}