using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Resto.Common;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;

namespace Resto.Data
{
    /// <summary>
    /// Запись отчёта о закупках
    /// </summary>
    public partial class PurchasingReportRecord
    {
        private Container container;

        /// <summary>
        /// Отклонение цены (в процентах).
        /// Для процентных полей PivotGrid'а нужны значения из диапазона [0, 1],
        /// а сервер возвращает для данного поля абсолютные величины процентов ([0, 100])
        /// </summary>
        [UsedImplicitly]
        public decimal? NullableDeviationPercentDivided => CurrentDeviationPercent / 100m;

        /// <summary>
        /// Группа продукта
        /// </summary>
        public ProductGroup ProductGroup
        {
            get { return product.Parent; }
        }

        /// <summary>
        /// Сумма без НДС
        /// </summary>
        public decimal? SumWithoutVat
        {
            get { return sum - vatSum; }
        }

        /// <summary>
        /// Имя склада с именем ТП
        /// </summary>
        public string StoreNameWithDepartment
        {
            get
            {
                var department = store.DepartmentEntity;
                return department == null
                    ? store.NameLocal
                    : string.Format("{0} ({1})", store.NameLocal, department.NameLocal);
            }
        }

        /// <summary>
        /// Имя склада без имени ТП
        /// </summary>
        public string StoreNameWithoutDepartment
        {
            get { return store.NameLocal; }
        }

        /// <summary>
        /// Фасовка
        /// </summary>
        public Container Container
        {
            get { return container ?? (container = product.GetContainerById(containerId ?? Guid.Empty)); }
        }

        /// <summary>
        /// Цена за единицу
        /// </summary>
        public decimal PriceForUnit => CurrentPrice / Container.Count;

        /// <summary>
        /// Учет НДС в стоимости товара.
        /// </summary>
        private VatAccounting VatAccounting
        {
            get
            {
                var vatAccounting = CompanySetup.Corporation.VatAccounting;
                if (vatAccounting.NotIn(VatAccounting.VAT_INCLUDED_IN_PRICE, VatAccounting.VAT_NOT_INCLUDED_IN_PRICE))
                {
                    throw new UnsupportedEnumValueException<VatAccounting>(vatAccounting);
                }

                return vatAccounting;
            }
        }

        /// <summary>
        /// Отображаемая цена.
        /// </summary>
        /// <remarks>
        /// В зависимости от <see cref="VatAccounting"/> возвращает цену с учетом или без учета НДС
        /// </remarks>
        public decimal CurrentPrice => VatAccounting.Equals(VatAccounting.VAT_NOT_INCLUDED_IN_PRICE)
            ? PriceWithoutVat
            : Price;

        /// <summary>
        /// Отображаемый процент отклонения
        /// </summary>
        /// <remarks>
        /// Возвращает процент отклонения цены в зависимости от <see cref="VatAccounting"/>
        /// </remarks>
        public decimal? CurrentDeviationPercent => VatAccounting.Equals(VatAccounting.VAT_NOT_INCLUDED_IN_PRICE)
            ? DeviationPercentWithoutVat
            : DeviationPercent;

        /// <summary>
        /// Отображаемое отклонение от цены
        /// </summary>
        /// <remarks>
        /// Возвращает отклонения от цены в зависимости от <see cref="VatAccounting"/>
        /// </remarks>
        [UsedImplicitly]
        public decimal? CurrentDeviation => VatAccounting.Equals(VatAccounting.VAT_NOT_INCLUDED_IN_PRICE)
            ? DeviationWithoutVat
            : Deviation;
    }
}
