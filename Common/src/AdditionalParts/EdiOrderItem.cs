using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common;
using Resto.Common.Extensions;
using Resto.Framework.Common;
using Resto.Framework.Data;

namespace Resto.Data
{
    /// <summary>
    /// Класс поля Edi заказа
    /// ВАЖНО: при изменении логики расчета количества и цены необходимо синхронизировать изменения с серверным кодом создания накладной
    /// </summary>
    public partial class EdiOrderItem
    {
        #region Fields

        /// <summary>
        /// Количество товара в заказе в упаковках
        /// </summary>
        private decimal orderedAmountInPacking;

        /// <summary>
        /// Общее количество товара в фасовке принятой для внутреннего учета.
        /// </summary>
        private decimal orderedAmountInInternalMeasureUnit;

        /// <summary>
        /// Поставщик
        /// </summary>
        private User supplier;

        /// <summary>
        /// Стоимость единицы товара с учетом НДС.
        /// </summary>
        private decimal? orderedPriceWithVat;

        /// <summary>
        /// Поле для <see cref="OrderedPriceWithoutVat"/>
        /// </summary>
        private decimal? orderedPriceWithoutVat;

        /// <summary>
        /// Допустимые единицы измерения товаров для EDI Kontur
        /// </summary>
        public static readonly MeasureUnit[] ValidEdiAmountUnits = { MeasureUnit.DefaultKgUnit, MeasureUnit.DefaultLitreUnit, MeasureUnit.DefaultPieceUnit };

        private ProductNumCompletionData productNum;

        [Transient]
        private decimal? storeBalance;

        #endregion

        #region CTOR
        
        public EdiOrderItem(EdiOrderDocument document, int num)
            : base(Guid.NewGuid(), num, null, 0m, null)
        {
            status = EdiOrderItemStatus.NOT_CONFIRMED;
            onePlaceAmount = 1;

            ediOrder = document;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Nullable продукт для внутренних нужд
        /// </summary>
        public new Product Product
        {
            get { return base.Product; }
            set
            {
                if (value == null)
                {
                    return;
                }

                base.Product = value;
            }
        }

        /// <summary>
        /// Артикул товара.
        /// </summary>
        public ProductNumCompletionData ProductCode
        {
            get { return productNum = ProductNumCompletionData.SameOrNew(productNum, Product); }
            set
            {
                if (value != null)
                {
                    Product = value.Product;
                }
            }
        }

        /// <summary>
        /// Признак новой записи без установленного продукта
        /// </summary>
        public bool IsEmptyRecord => Product == null && SupplierProduct == null;

        /// <summary>
        /// Фасовка
        /// </summary>
        public Container Container
        {
            get
            {
                if (ContainerId != null)
                {
                    return GetContainers().FirstOrDefault(item => item.Id == ContainerId.Value);
                }

                return null;
            }
            set
            {
                if (value != null)
                {
                    if (Equals(ContainerId, value.Id))
                    {
                        return;
                    }
                    ContainerId = value.Id;
                }
                else if (Product != null)
                {
                    ContainerId = Container.GetMeasureUnitContainer(Product.MainUnit).Id;
                }

                if (ContainerId != null)
                {
                    OnePlaceAmount = Container.Count;
                    orderedAmountInInternalMeasureUnit = OnePlaceAmount.GetValueOrDefault() * OrderedAmountInPacking;
                    UpdateEdiAmountUnitByContainer();
                    RecalculateAmountAndSum();
                }
            }
        }

        /// <summary>
        /// Количество товара в заказе в упаковках
        /// </summary>
        public decimal OrderedAmountInPacking
        {
            get
            {
                if (IsEmptyRecord)
                {
                    return 0m;
                }

                if (orderedAmountInPacking == 0m)
                {
                    orderedAmountInPacking = OrderedAmountInInternalMeasureUnit / GetActualOnePlaceAmount();
                }
                return orderedAmountInPacking;
            }
            set
            {
                orderedAmountInPacking = value;
                orderedAmountInInternalMeasureUnit = GetActualOnePlaceAmount() * orderedAmountInPacking;
                RecalculateAmountAndSum();
            }
        }

        /// <summary>
        /// Общее количество товара в фасовке принятой для внутреннего учета. 
        /// </summary>
        public decimal OrderedAmountInInternalMeasureUnit
        {
            get
            {
                if (Product == null)
                {
                    return 0m;
                }

                if (orderedAmountInInternalMeasureUnit == 0m)
                {
                    var result = (OnePlaceAmount ?? 0m) * Amount;
                    if (IsAmountEqualsAmountInPacking())
                    {
                        orderedAmountInInternalMeasureUnit = result;
                    }
                    else
                    {
                        orderedAmountInInternalMeasureUnit = result / GetUnitFactorAsDivider(EdiAmountUnit);
                    }
                }
                return orderedAmountInInternalMeasureUnit;
            }
            set
            {
                orderedAmountInInternalMeasureUnit = value;
                if (OnePlaceAmount.GetValueOrDefault(0m) > 0m)
                {
                    orderedAmountInPacking = orderedAmountInInternalMeasureUnit / OnePlaceAmount.GetValueOrDefault(0m);
                }
                RecalculateAmountAndSum();
            }
        }

        public decimal? NullableConfirmedAmountInPacking
        {
            get
            {
                if (ConfirmedOnePlaceAmount == null)
                {
                    return null;
                }
                return NullableConfirmedAmountInInternalMeasureUnit / GetActualOnePlaceAmount(false);
            }
        }

        public decimal? NullableConfirmedAmountInInternalMeasureUnit
        {
            get
            {
                if (Product == null)
                {
                    return null;
                }

                decimal? result;
                if (IsKonturEdiSystem)
                {
                    result = ConfirmedOnePlaceAmount * ConfirmedAmount;
                }
                else
                {
                    result = ConfirmedAmountInPrimaryUnits;
                }
                if (IsAmountEqualsAmountInPacking(true))
                {
                    return result;
                }
                else
                {
                    return result / GetUnitFactorAsDivider(ConfirmedEdiAmountUnit);
                }
            }
        }

        /// <summary>
        /// Стоимость единицы товара с учетом НДС.
        /// </summary>
        public decimal OrderedPriceWithVat
        {
            get => orderedPriceWithVat ?? GetRecalculatedPrice(PriceWithVAT.GetValueOrDefault(0m));
            set
            {
                orderedPriceWithVat = value;
                RecalculatePriceEdi(true);
                SumWithVAT = GetSum(true);
                RecalculateAddictedOrderedPriceFields(true);
            }
        }

        /// <summary>
        /// Стоимость единицы товара без учета НДС.
        /// </summary>
        public decimal OrderedPriceWithoutVat
        {
            get => orderedPriceWithoutVat ?? GetRecalculatedPrice(Price.GetValueOrDefault(0m));
            set
            {
                orderedPriceWithoutVat = value;
                RecalculatePriceEdi(false);
                Sum = GetSum(false);
                RecalculateAddictedOrderedPriceFields(false);
            }
        }

        [UsedImplicitly]
        public decimal OrderedEdiPriceWithVat => OrderedAmountInPacking == 0m ? 0m : SumWithVAT.GetValueOrDefault(0m) / OrderedAmountInPacking;

        [UsedImplicitly]
        public decimal OrderedEdiPriceWithoutVat => OrderedAmountInPacking == 0m ? 0m : Sum.GetValueOrDefault(0m) / OrderedAmountInPacking;

        [UsedImplicitly]
        public decimal? NullableConfirmedPriceWithVat => ConfirmedPriceVAT == null
            ? (decimal?) null
            : GetRecalculatedPrice(ConfirmedPriceVAT.Value, false);

        [UsedImplicitly]
        public decimal? NullableConfirmedPriceWithoutVat => ConfirmedPrice == null ? (decimal?) null : GetRecalculatedPrice(ConfirmedPrice.Value, false);

        /// <summary>
        /// Поставщик
        /// </summary>
        /// <remarks>Автосвойства не работают: сериализация/десериализация проходят криво</remarks>
        public User Supplier
        {
            get { return supplier; }
            set { supplier = value; }
        }

        /// <summary>
        /// Заказанное количество отличается от подтвержденного/отгруженного
        /// </summary>
        public bool IsDifferentAmount
        {
            get { return status == EdiOrderItemStatus.CHANGED && OrderedAmountInInternalMeasureUnit != NullableConfirmedAmountInInternalMeasureUnit; }
        }

        /// <summary>
        /// Заказанная стоимость отличается от подтвержденной/отгруженной
        /// </summary>
        public bool IsDifferentPrice => status == EdiOrderItemStatus.CHANGED &&
                                        (OrderedPriceWithVat != ConfirmedPriceVAT ||
                                         SumWithVAT != ConfirmedSumVAT ||
                                         OrderedPriceWithoutVat != ConfirmedPrice ||
                                         Sum != ConfirmedSum);

        /// <summary>
        /// Заказанная фасовка отличается от подтвержденной/отгруженной
        /// </summary>
        public bool IsDifferentMeasureUnit
        {
            get { return status == EdiOrderItemStatus.CHANGED && !Equals(EdiAmountUnit, ConfirmedEdiAmountUnit); }
        }

        /// <summary>
 	    /// <c>true</c>, если документ является документом для Контура
 	    /// </summary>
        public bool IsKonturEdiSystem
        {
            get { return EdiSystem.IsKontur(Supplier); }
        }

        /// <summary>
        /// Текущий остаток товара на складе в базовых единицах измерения
        /// </summary>
        public decimal? StoreBalance
        {
            get { return storeBalance; }
            set { storeBalance = value; }
        }

        /// <summary>
        /// Проверяет совпадение размера фасовки, сохранённого в заказе (<see cref="OnePlaceAmount"/>),
        /// с фактическим размером фасовки. Они могут не совпадать, если сначала сохранили заказ,
        /// использующий какую-либо фасовку, а потом эту фасовку изменили в карточке номенклатуры.
        /// </summary>
        /// <returns>
        /// <c>true</c>, если размеры отличаются; <c>false</c>, если совпадают.
        /// </returns>
        public bool IsContainerCapacityIncorrect
        {
            get
            {
                return Container == null && OnePlaceAmount != null ||
                       Container != null && !Equals(OnePlaceAmount, Container.Count);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Обнуляет подтвержденные параметры
        /// </summary>
        public void ClearConfirmedFields()
        {
            confirmedAmount = null;
            confirmedComment = null;
            confirmedEdiAmountUnit = null;
            confirmedOnePlaceAmount = null;
            confirmedPrice = null;
            confirmedPriceVAT = null;
            confirmedSum = null;
            confirmedSumVAT = null;
        }

        /// <summary>
        /// Возвращает список возможных контейнеров (фасовок) для заданного продукта.
        /// </summary>        
        public IEnumerable<Container> GetContainers()
        {
            var containers = new List<Container>();
            var product = Product ?? SupplierProduct;

            if (product != null)
            {
                containers.Add(Container.GetMeasureUnitContainer(product.MainUnit));
                containers.AddRange(product.Containers);
            }

            return containers;
        }
        
        /// <summary>
        /// Обновляет продукт поставщика.
        /// </summary>
        /// <param name="supplierPrice">Прайс-лист поставщика</param>
        /// <param name="condition">Условие выборки</param>
        public void UpdateSupplierProduct(SupplierPriceList supplierPrice, Func<SupplierPriceListItem, bool> condition)
        {
            if (supplierPrice == null)
            {
                return;
            }

            // Множество строк прайс-листа, подходящих для данного продукта.
            var priceItems = CommonConfig.Instance.UseSupplierProducts
                ? supplierPrice.AllNotDeletedItems().Where(item => Equals(item.NativeProduct, Product) && condition(item)).ToArray()
                : supplierPrice.AllNotDeletedItems().Where(item => Equals(item.SupplierProduct, Product) && condition(item)).ToArray();

            // RMS-44016: По возможности стараемся не менять продукт постащика. Если текущий продукт поставщика
            // есть среди подходящих в прайс-листе, то оставляем его, иначе берём любой (первый по алфавиту) из подходящих.
            var priceItem = priceItems.FirstOrDefault(item => Equals(item.SupplierProduct, SupplierProduct))
                            ?? priceItems.OrderBy(item => item.SupplierProduct.NameLocal).FirstOrDefault();
            SupplierProduct = priceItem != null ? priceItem.SupplierProduct : null;
        }

        /// <summary>
        /// Устанавливает продукт номенклатуры по продукту поставщика
        /// </summary>
        /// <param name="supplierPrice">Прайс-лист поставщика</param>
        public void SetProductBySupplierProduct(SupplierPriceList supplierPrice)
        {
            var product = GetNewProductFromSupplierPriceList(supplierPrice, SupplierProduct);
            if (product != null)
            {
                Product = product;
            }
        }

        /// <summary>
        /// Возвращает продукт iiko, соответствующий продукту поставщика.
        /// </summary>
        /// <param name="supplierPrice">Прайс-лист поставщика</param>
        /// <param name="newSupplierProduct">Продукт поставщика</param>
        /// <returns>
        /// - если найдено соответствие <paramref name="newSupplierProduct"/> в прайс-листе поставщика, возвращает продукт из прайс-листа;
        /// - если не найдено соответствие в прайс-листе и <paramref name="newSupplierProduct"/> является внешним, возвращает <paramref name="newSupplierProduct"/>
        /// - иначе <c>null</c>.
        /// </returns>
        [CanBeNull]
        public Product GetNewProductFromSupplierPriceList(SupplierPriceList supplierPrice, Product newSupplierProduct)
        {
            if (Supplier == null || supplierPrice == null || newSupplierProduct == null)
            {
                return null;
            }

            var priceItem = supplierPrice.AllNotDeletedItems().FirstOrDefault(item => Equals(item.SupplierProduct, newSupplierProduct));

            if (priceItem != null)
            {
                return priceItem.NativeProduct;
            }

            if (!newSupplierProduct.IsOuter)
            {
                return newSupplierProduct;
            }

            return null;
        }

        /// <summary>
        /// Обновляет цены.
        /// </summary>
        /// <param name="supplierPrice">Прайс-лист поставщика</param>
        /// <param name="independentPrice">Внутренний прайс-лист</param>
        /// <param name="olnyZeroPrice">true: обновляем только нулевые цены</param>
        /// <remarks>
        /// Приоритет обновления:
        /// 1. Прайс-лист поставшика
        /// 2. Внутренний прайс-лист
        /// 3. Цена по последнему приходу
        /// </remarks>
        public void UpdatePriceByPriceList([CanBeNull] SupplierPriceList supplierPrice,
            [CanBeNull] IndependentPriceList independentPrice, bool olnyZeroPrice)
        {
            if (supplierPrice == null && independentPrice == null)
            {
                return;
            }

            var newPrice = GetNewPrice(supplierPrice, independentPrice);

            if (newPrice.HasValue && (OrderedPriceWithVat == 0m || !olnyZeroPrice))
            {
                var vatAccounting = CompanySetup.Corporation.VatAccounting;
                if (vatAccounting.Equals(VatAccounting.VAT_INCLUDED_IN_PRICE))
                {
                    OrderedPriceWithVat = newPrice.Value;
                }
                else if (vatAccounting.Equals(VatAccounting.VAT_NOT_INCLUDED_IN_PRICE))
                {
                    OrderedPriceWithoutVat = newPrice.Value;
                }
                else
                {
                    throw new UnsupportedEnumValueException<VatAccounting>(vatAccounting);
                }
            }
            else if (!olnyZeroPrice)
            {
                OrderedPriceWithVat = 0m;
                OrderedPriceWithoutVat = 0m;
            }
        }

        /// <summary>
        /// Возвращает новую цену из прайс-листов или, если отсутствуют, по последнему приходу.
        /// </summary>
        /// <param name="supplierPrice">Прайс-лист поставщика</param>
        /// <param name="independentPrice">Внутренний прайс-лист</param>
        private decimal? GetNewPrice(SupplierPriceList supplierPrice, IndependentPriceList independentPrice)
        {
            if (SupplierProduct != null)
            {
                var supplierPriceListItem = supplierPrice?.AllNotDeletedItems()
                    .FirstOrDefault(item => Equals(item.NativeProduct, Product) &&
                                            Equals(item.SupplierProduct, SupplierProduct) &&
                                            item.ContainerId == ContainerId.GetValueOrDefault());

                if (supplierPriceListItem != null)
                {
                    return supplierPriceListItem.CostPrice;
                }
            }

            var independentPriceListItem = independentPrice?.GetItemsByNativeProduct(Product)
                .FirstOrDefault(item => Equals(item.ContainerId, ContainerId.GetValueOrDefault()));

            if (independentPriceListItem != null)
            {
                return independentPriceListItem.CostPrice;
            }

            if (CommonConfig.Instance.NeedSubstituteSupplierPrice && SupplierProduct == null && Container != null)
            {
                var supplierInfo = SupplierInfo.GetSupplierInfoBy(Supplier, Product, Container.Id);
                if (supplierInfo != null)
                {
                    return supplierInfo.CostPrice.GetValueOrDefault();
                }
            }

            return null;
        }

        /// <summary>
        /// Пересчитывает значения количества и суммы отправляемых в EDI.
        /// </summary>
        public void RecalculateAmountAndSum()
        {
            orderedPriceWithVat = null;
            orderedPriceWithoutVat = null;
            orderedAmountInPacking = 0m;
            RecalculateAmountByEdiAmountUnit();
            SumWithVAT = GetSum(true);
            Sum = GetSum(false);
        }

        /// <summary>
        /// Пересчитывает общее количество в ед. измерения выбранных для отправки в EDI
        /// </summary>
        public void RecalculateAmountByEdiAmountUnit()
        {
            if (IsEmptyRecord)
            {
                return;
            }

            AmountInPrimaryUnits = OrderedAmountInInternalMeasureUnit * GetUnitFactor(EdiAmountUnit);
            Amount = OrderedAmountInPacking;
        }

        /// <summary>
        /// Возвращает сумму для позиции заказа в зависимости от <paramref name="forPriceWithVat"/>
        /// </summary>
        public decimal? GetSum(bool forPriceWithVat)
        {
            if (Product == null)
            {
                return null;
            }

            return (IsAmountEqualsAmountInPacking()
                       ? Amount
                       : AmountInPrimaryUnits) *
                   (forPriceWithVat
                       ? PriceWithVAT
                       : Price);
        }

        /// <summary>
        /// Пересчитывает цену с НДС или без НДС для данной записи в зависимости от <paramref name="forPriceWithVat"/>
        /// </summary>
        public void RecalculatePriceEdi(bool forPriceWithVat)
        {
            if (Product == null)
            {
                return;
            }

            if (forPriceWithVat)
            {
                PriceWithVAT = IsAmountEqualsAmountInPacking()
                    ? OrderedPriceWithVat
                    : decimal.Divide(OrderedPriceWithVat,
                        GetActualOnePlaceAmount() * GetUnitFactorAsDivider(EdiAmountUnit));
            }
            else
            {
                Price = IsAmountEqualsAmountInPacking()
                    ? OrderedPriceWithoutVat
                    : decimal.Divide(OrderedPriceWithoutVat,
                        GetActualOnePlaceAmount() * GetUnitFactorAsDivider(EdiAmountUnit));
            }
        }

        /// <summary>
        /// Пересчитывает зависимые поля цены с НДС или цены без НДС
        /// </summary>
        private void RecalculateAddictedOrderedPriceFields(bool forPriceWithVat)
        {
            if (Product == null)
            {
                return;
            }

            // для цены с НДС зависимое поле - цена без НДС, поэтому в условии отрицание
            if (!forPriceWithVat)
            {
                orderedPriceWithVat = orderedPriceWithoutVat * (1m + Product.VatPercent / 100m);
                RecalculatePriceEdi(true);
                SumWithVAT = GetSum(true);
            }
            else
            {
                orderedPriceWithoutVat = orderedPriceWithVat / (1m + Product.VatPercent / 100m);
                RecalculatePriceEdi(false);
                Sum = GetSum(false);
            }
        }

        /// <summary>
        /// Определяет равно ли общее количество в заказе EDI количеству в таре
        /// </summary>
        public bool IsAmountEqualsAmountInPacking(bool forConfirmed = false)
        {
            Contract.Requires(Product != null);

            if (!IsKonturEdiSystem)
            {
                return true;
            }

            var amountUnit = forConfirmed ? ConfirmedEdiAmountUnit : EdiAmountUnit;

            // Если в качестве ед. измерения EDI выбрана шт. а основная ед. измерения либо кг. либо л., то общее количество в EDI заказе должно быть равно количетву в таре.
            // Во всех остальных случаях общее количество в EDI заказе должно быть равно количеству в базовых единицах с учетом соответствующего коэфициента пересчета.
            return Equals(amountUnit, MeasureUnit.DefaultPieceUnit) && Product.MainUnit.In(MeasureUnit.DefaultKgUnit, MeasureUnit.DefaultLitreUnit);
        }

        /// <summary>
        /// Возвращает единицу измерения EDI принятую по умолчанию для данной записи
        /// </summary>
        /// <remarks>
        /// Реализация должна совпадать с серверной: resto.edi.documents.EdiOrderItem.getEdiAmountUnitByContainer
        /// </remarks>
        public MeasureUnit GetEdiAmountUnitByContainer()
        {
            if (Container != null && !Container.IsEmptyContainer)
            {
                return MeasureUnit.DefaultPieceUnit;
            }

            var product = Product ?? SupplierProduct;
            return Array.Exists(ValidEdiAmountUnits, element => Equals(element, product.MainUnit))
                ? product.MainUnit
                : MeasureUnit.DefaultPieceUnit;
        }

        /// <summary>
        /// Устанавливает <see cref="EdiAmountUnit"/> в соответствии с текущей фасовкой
        /// </summary>
        /// <remarks>
        /// Реализация должна совпадать с серверной: resto.edi.documents.EdiOrderItem.updateEdiAmountUnitByContainer
        /// </remarks>
        public void UpdateEdiAmountUnitByContainer()
        {
            EdiAmountUnit = GetEdiAmountUnitByContainer();
        }

        /// <summary>
        /// Возвращает коэфициент преобразования одних единиц измерения в другие
        /// </summary>
        private decimal GetUnitFactor(MeasureUnit measureUnit)
        {
            if (Product == null)
            {
                return 1m;
            }
            
            if (Equals(measureUnit, MeasureUnit.DefaultKgUnit))
            {
                return Product.UnitWeight;
            }
            if (Equals(measureUnit, MeasureUnit.DefaultLitreUnit))
            {
                return Product.UnitCapacityEffective;
            }

            return 1m;
        }

        /// <summary>
        /// Возвращает коэфициент преобразования одних единиц измерения в другие.
        /// В отличии от <see cref="GetUnitFactor"/> в случает если результат = 0, метод возвращает 1
        /// </summary>
        private decimal GetUnitFactorAsDivider(MeasureUnit measureUnit)
        {
            var divider = GetUnitFactor(measureUnit);
            return divider == 0m ? 1m : divider;
        }

        /// <summary>
        /// Обратный пересчет стоимости за единицу товара во внутриноменклатурных фасовках по переданной цене за единицу в EDI единицах измерения
        /// С НДС.
        /// </summary>
        /// <remarks>
        /// RMS-41779 На сервере в EDI API есть метод с симметричной логикой: resto.api.v1.edi.assemble.EdiApiOrderAssembler.getActualPrice.
        /// </remarks>
        private decimal GetRecalculatedPrice(decimal priceParameter, bool isRequested = true)
        {
            if (Product == null)
            {
                return 0m;
            }

            return IsAmountEqualsAmountInPacking() ? priceParameter : priceParameter * GetActualOnePlaceAmount(isRequested) * GetUnitFactor(EdiAmountUnit);
        }

        /// <summary>
        /// Возвращает актуальное значение емкости фасовки. Возвращаемое значение всегда больше 0.
        /// </summary>
        private decimal GetActualOnePlaceAmount(bool isRequested = true)
        {
            decimal? result;
            if (IsKonturEdiSystem)
            {
                result = isRequested ? OnePlaceAmount : ConfirmedOnePlaceAmount;
            }
            else
            {
                if (!isRequested)
                {
                    result = ConfirmedOnePlaceAmount;
                }
                else
                {
                    result = Container == null ? 1m : Container.Count;
                }
            }

            return result == null || result == 0m ? 1m : result.Value;
        }

        #endregion
    }
}
