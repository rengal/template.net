using System;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Data;

namespace Resto.Data
{
    /// <summary>
    /// Одна строка составляющей в технологической карте.
    /// </summary>
    public partial class AssemblyChartItem : ISupportTreeSorting<Func<AssemblyChartItem, object>>,
                                             ISupportTreeSorting<Guid, Func<AssemblyChartItem, object>>
    {
        private const int WeightRoundoffDigits = 4;

        [Transient]
        private bool fake;

        [Transient]
        private Container packageAC;

        [Transient]
        private decimal coldLossPercent;

        [Transient]
        private decimal hotLossPercent;

        [Transient]
        private AssemblyChartItem parentItem;

        [Transient]
        private ProductNumCompletionData productNum;

        /// <summary>
        /// Уровень вложенности (нужен для отображения в виде дерева fake-айтемов).
        /// </summary>
        [Transient]
        private int level = 0;

        public AssemblyChartItem(Guid id, double sortWeight, Product product, decimal amountIn, decimal amountMiddle,
                                 decimal amountOut, decimal amountIn1,
                                 decimal amountOut1, decimal amountIn2, decimal amountOut2, decimal amountIn3,
                                 decimal amountOut3, AssemblyChart assemblyChart)
            : base(id, sortWeight, product, amountIn, amountMiddle, amountOut)
        {
            this.amountIn1 = amountIn1;
            this.amountOut1 = amountOut1;
            this.amountIn2 = amountIn2;
            this.amountOut2 = amountOut2;
            this.amountIn3 = amountIn3;
            this.amountOut3 = amountOut3;
            this.assemblyChart = assemblyChart;
        }

        /// <summary>
        /// Продукт, который может быть равен <c>null</c>
        /// (например, в случае если это строка для добавления новой записи в редакторе ТК).
        /// </summary>
        [CanBeNull]
        public Product ProductNullable
        {
            get { return Product; }
        }

        /// <summary>
        /// Возвращает базовое количество единиц товара, используемых для приготовления одной базовой единицы блюда.
        /// Вычисляется как отношение брутто и нормы закладки.
        /// </summary>
        public decimal BaseAmount
        {
            get
            {
                return assemblyChart.AssembledAmount > 0 ? AmountIn / assemblyChart.AssembledAmount : 0;
            }

        }

        public decimal CalculatedColdLoss
        {
            get { return AmountInMU != 0 ? 100 * (1 - AmountMiddleMU / AmountInMU) : 0; }
            set { AmountMiddleMU = AmountInMU * (1 - value / 100); }

        }

        public decimal CalculatedHotLoss
        {
            get { return AmountMiddleMU != 0 ? 100 * (1 - AmountOutMU / AmountMiddleMU) : 0; }
            set { AmountOutMU = AmountMiddleMU * (1 - value / 100); }

        }

        /// <summary>
        /// Потери, при холодной обработке, %.
        /// </summary>
        public decimal ColdLossPercent
        {
            get { return coldLossPercent; }
            set { coldLossPercent = value; }
        }

        /// <summary>
        /// Потери, при горячей обработке, %.
        /// </summary>
        public decimal HotLossPercent
        {
            get { return hotLossPercent; }
            set { hotLossPercent = value; }
        }

        /// <summary>
        /// Брутто, ед. изм.
        /// </summary>
        public decimal AmountInAC
        {
            get { return AmountIn; }
            set
            {
                AmountIn = value;
                if (AmountMiddleMU == 0)
                {
                    AmountMiddleMU = AmountInMU;
                }
            }
        }

        /// <summary>
        /// Брутто, кг.
        /// </summary>
        public decimal AmountInMU
        {
            get
            {
                return Product != null ? AmountIn * Product.UnitWeightMultiplier : 0;
            }
            set
            {
                if (Product != null)
                {
                    if (Product.UnitWeightMultiplier == 0)
                        CheckForZeroWeight(value);
                    else
                        AmountIn = Math.Round(value / Product.UnitWeightMultiplier, WeightRoundoffDigits, MidpointRounding.AwayFromZero);

                    packageCount = PackageTypeAC.GetContainerCountByWeigth(value);
                }
                else
                    AmountIn = 0;
            }
        }

        /// <summary>
        /// Нетто, кг.
        /// </summary>
        public decimal AmountMiddleMU
        {
            get
            {
                return Product != null ? AmountMiddle * Product.UnitWeightMultiplier : 0;
            }
            set
            {
                if (Product != null)
                {
                    if (Product.UnitWeightMultiplier == 0)
                        CheckForZeroWeight(value);
                    else
                        AmountMiddle = Math.Round(value / Product.UnitWeightMultiplier, WeightRoundoffDigits, MidpointRounding.AwayFromZero);
                }
                else
                    AmountMiddle = 0;
            }
        }

        /// <summary>
        /// Выход готового продукта, кг.
        /// </summary>
        public decimal AmountOutMU
        {
            get { return Product != null ? AmountOut * Product.UnitWeightMultiplier : 0; }
            set
            {
                if (Product != null)
                {
                    if (Product.UnitWeightMultiplier == 0)
                        CheckForZeroWeight(value);
                    else
                        AmountOut = value / Product.UnitWeightMultiplier;
                }
                else
                    AmountOut = 0;
            }
        }

        public decimal AmountMiddleOutMU
        {
            get { return Product != null ? AmountOut * Product.UnitWeightMultiplier : 0; }
            set
            {
                if (Product != null)
                {
                    if (Product.UnitWeightMultiplier == 0)
                        CheckForZeroWeight(value);
                    else
                    {
                        AmountMiddleMU = value;
                        AmountOutMU = value;
                    }
                }
                else
                {
                    AmountOut = 0;
                    AmountMiddle = 0;
                }
            }
        }

        public decimal? PackageCountAC
        {
            get
            {
                return PackageTypeAC.GetContainerCountByWeigth(AmountIn);
            }
            set
            {
                if (value != null)
                    AmountIn = PackageTypeAC.GetWeigthByContainerCount(value.Value);
            }
        }

        public Container PackageTypeAC
        {
            get
            {
                if (packageAC == null)
                {
                    if (packageTypeId == null || packageTypeId.Value.Equals(Guid.Empty))
                        packageAC = MakeContainer();
                    else
                    {
                        packageAC = Product.Containers.Find(container => container.Id.Equals(packageTypeId)) ?? MakeContainer();
                    }
                }
                return packageAC;
            }
            set
            {
                packageAC = value;
                if (value != null)
                    packageTypeId = value.Id;
            }
        }

        private Container MakeContainer()
        {
            return Product != null ? Container.GetMeasureUnitContainer(Product.MainUnit) : Container.GetEmptyContainer();
        }

        public decimal FullLosesPercent
        {
            get { return AmountIn != 0 ? 100 - ((AmountOut * 100) / AmountIn) : 0; }
        }

        public decimal FullLoses
        {
            get { return AmountOut != 0 ? AmountIn / AmountOut : 0; }
        }

        public bool Fake
        {
            get { return fake; }
            set { fake = value; }
        }

        /// <summary>
        /// Уровень вложенности (нужен для отображения в виде дерева fake-айтемов).
        /// </summary>
        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        /// <summary>
        /// Возвращает или устанавливает родительский ингредиент.
        /// </summary>
        /// <remarks>Это свойство нужно для реализации древовидного отображения карты ингредиентов.</remarks>
        public AssemblyChartItem ParentItem
        {
            get { return parentItem; }
            set { parentItem = value; }
        }

        /// <summary>
        /// Nullable обёртка над свойством <see cref="Product"/> 
        /// </summary>
        /// <remarks>
        /// Нужно чтобы избавиться от лишних предупреждений решарпера.
        /// Product отмечен как [NotNull], но в некоторых случаях он
        /// всё-таки может быть null (пустая строка в редакторе тех.карт).
        /// </remarks>
        public Product NullableProduct
        {
            get { return Product; }
        }

        /// <summary>
        /// Артикул
        /// </summary>
        public ProductNumCompletionData Num
        {
            get { return productNum = ProductNumCompletionData.SameOrNew(productNum, Product); }
            set
            {
                if (value != null && value.Product != null)
                {
                    Product = value.Product;
                }
            }
        }

        /// <summary>
        /// Возвращает <c>true</c>, если для элемента техкарты задано хоть одно отрицательное значение мер и весов.
        /// Иначе - <c>false</c>
        /// </summary>
        public bool HasNegativeAmountValues => PackageCountAC < 0m || AmountMiddleMU < 0m || AmountInMU < 0m || AmountOutMU < 0m;

        private void CheckForZeroWeight(decimal value)
        {
            if (value != 0 && OnZeroWeight != null)
                OnZeroWeight(Product);
        }

        public AssemblyChartItem Copy()
        {
            var newItem = (AssemblyChartItem)DeepClone(Guid.NewGuid());
            newItem.ColdLossPercent = coldLossPercent;
            newItem.HotLossPercent = hotLossPercent;
            newItem.Fake = fake;
            return newItem;
        }

        public static event AssemblyChartItemUnitWeightIsZeroDelegate OnZeroWeight;

        #region ISupportTreeSorting<Guid, Func<AssemblyChartItem, object>> Members

        public TreeSortingValue<Guid, V> ExtractTreeSortingValue<V>(Func<AssemblyChartItem, object> valueSelector)
            where V : IComparable, IComparable<V>
        {
            // Если элемент новый (например только что создан в режиме редактирования)
            if (Product == null)
            {
                // Возвращаем значение null, которое всегда самое весомое (независимо от направления сортировки).
                return null;
            }

            SimpleTreeSortingValue<Guid, V> parentSortingValue = null;
            // При текущем подходе конечно есть оверхэд по производительности,
            // так как для дочерних элементов родительские будут строиться снова и снова.
            // Но пока выбран именно такой компромисс, так как код в тех. картах представляет собой бардак
            // и поддерживать сортировку на этом уровне опасно...
            // Может потом стоит кэширование прикрутить...
            if (ParentItem != null)
            {
                parentSortingValue = (SimpleTreeSortingValue<Guid, V>)
                                     ParentItem.ExtractTreeSortingValue<V>(valueSelector);
            }

            return new SimpleTreeSortingValue<Guid, V>(Id, (V)valueSelector(this), parentSortingValue);
        }

        #endregion ISupportTreeSorting<Guid, Func<AssemblyChartItem, object>> Members

        #region ISupportTreeSorting<Func<AssemblyChartItem, object>> Members

        public TreeSortingValue ExtractTreeSortingValue(Func<AssemblyChartItem, object> valueSelector)
        {
            // Если элемент новый (например только что создан в режиме редактирования)
            if (Product == null)
            {
                // Возвращаем значение null, которое всегда самое весомое (независимо от направления сортировки).
                return null;
            }

            TreeSortingValue parentSortingValue = null;
            if (ParentItem != null)
            {
                parentSortingValue = ParentItem.ExtractTreeSortingValue(valueSelector);
            }

            return new SimpleTreeSortingValue(Id, (IComparable)valueSelector(this), parentSortingValue);
        }

        #endregion ISupportTreeSorting<Func<AssemblyChartItem, object>> Members
    }
}