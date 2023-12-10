using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using EnumerableExtensions;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common;
using Resto.Common.Extensions;
using Resto.Common.Properties;
using Resto.Framework.Common;
using Resto.Framework.Data;
using Resto.UI.Common;

namespace Resto.Data
{
    #region Product

    public static class ProductListExtension
    {
        /// <summary>
        /// Сортировка с учётом расположения товаров в группах.
        /// </summary>
        public static IEnumerable<Product> SortByGroupsHierarchy([NotNull] this IEnumerable<Product> products)
        {
            var tempProducts = products.ToList();

            // Товары которые не входят ни в одну группу
            List<Product> productsWithoutGroup = tempProducts.Where(p => p.Parent == null).OrderBy(p => p.NameLocal).ToList();

            List<Product> productValues = tempProducts.Where(p => p.Parent != null).OrderBy(p => p.GroupsHierarchyPath).ThenBy(p => p.NameLocal).ToList();

            productValues.AddRange(productsWithoutGroup);

            return productValues;
        }

        /// <summary>
        /// Фильтрует продукты с учётом видимости для ТП в РМС и в одноресторамнном чейне.
        /// </summary>
        public static IEnumerable<Product> FilterByCurrentDepartment([NotNull] this IEnumerable<Product> products)
        {
            Contract.Requires(products != null);
            Contract.Ensures(products != null);

            DepartmentEntity department = MultiDepartments.Instance.ChainOrRmsSingleDepartment;
            if (department != null)
            {
                products = products.Where(p => p.IsVisibleFor(department)).ToList();
            }
            return products;
        }

        /// <summary>
        /// Преобразует каждый элемент <paramref name="products"/> в <see cref="ProductSizeKey"/>
        /// </summary>
        /// <param name="products">Список продуктов</param>
        /// <param name="considerSizes"><c>true</c> - для каждого размера продукта свой ProductSizeKey,
        /// иначе - один Product = один ProductSize</param>
        public static IEnumerable<ProductSizeKey> ConvertToProductSizeKeys([NotNull] this IEnumerable<Product> products, bool considerSizes = true)
        {
            return products
                .SelectMany(product =>
                {
                    var allProducts = new List<ProductSizeKey>();
                    if (considerSizes)
                    {
                        allProducts.AddRange(product.ProductSizes.Select(size => new ProductSizeKey(product, size)));
                    }

                    return allProducts.IsEmpty()
                        ? new ProductSizeKey(product).AsSequence()
                        : allProducts;
                });
        }
    }

    /// <summary>
    /// Класс выполняет работу с элементом номенклатуры.
    /// Бэковское дополнение для работы с классом <see cref="Product"/>.
    /// </summary>
    public partial class Product : IComparable, IComparable<Product>
    {
        #region Static members

        public static ReadOnlyCollection<Product> GetAllNativeProducts()
        {
            return EntityManager.INSTANCE.GetAll<Product>(product => product.Type != ProductType.OUTER);
        }

        public static ReadOnlyCollection<Product> GetAllNotDeletedNativeProducts()
        {
            return EntityManager.INSTANCE.GetAllNotDeleted<Product>(product => product.Type != ProductType.OUTER);
        }

        /// <summary>
        /// Возвращает все неудалённые продукты с размерами
        /// </summary>
        /// <remarks>
        /// Для продуктов, которым не назначена шкала размеров возвращается
        /// пара "Продукт - null". Для продуктов с назначенной шкалой возвращается
        /// множество пар "Продукт - Размер" (по одной на каждый размер шкалы);
        /// при этом исключаются размеры, которые отключены (disabled) для
        /// данного продукта, а также удалённые размеры.
        /// </remarks>
        public static ReadOnlyCollection<ProductSizeKey> GetAllNotDeletedNativeProductsWithSizes()
        {
            return EntityManager.INSTANCE
                                .GetAllNotDeleted<Product>(product => product.Type != ProductType.OUTER)
                                .SelectMany(product => product.WithSizes())
                                .ToList()
                                .AsReadOnly();
        }

        public static ReadOnlyCollection<Product> GetAllNotDeletedAndVisibleNativeProductsFor(
            [CanBeNull] DepartmentEntity department)
        {
            return EntityManager.INSTANCE.GetAllNotDeleted<Product>(
                product => product.Type != ProductType.OUTER && (department == null || product.IsVisibleFor(department))
                );
        }

        [NotNull]
        [Framework.Attributes.JetBrains.Pure]
        public static ReadOnlyCollection<Product> GetAllNotDeletedAndVisibleNativeProductsForDepartments(
            [CanBeNull] ICollection<DepartmentEntity> departments)
        {
            return EntityManager.INSTANCE.GetAllNotDeleted<Product>(
                product => product.Type != ProductType.OUTER && product.IsVisibleForAnyDepartment(departments));
        }

        [NotNull]
        [Framework.Attributes.JetBrains.Pure]
        public static IReadOnlyCollection<Product> GetAllNotDeletedAndVisibleModifiersForDepartments(
            [CanBeNull] ICollection<DepartmentEntity> departments)
        {
            return EntityManager.INSTANCE.GetAllNotDeleted<Product>(
                product => product.Type == ProductType.MODIFIER && product.IsVisibleForAnyDepartment(departments));
        }

        public static Product GetProductByOuterCodeOrName(
            string supplierProductCode, string supplierProductName, decimal salePrice)
        {
            Product productByCode = null;
            if (!String.IsNullOrEmpty(supplierProductCode))
            {
                productByCode = EntityManager.INSTANCE.GetAllNotDeleted<Product>().FirstOrDefault(
                    prod => !string.IsNullOrEmpty(prod.Num) && prod.Type == ProductType.OUTER &&
                            prod.Num.Equals(supplierProductCode, StringComparison.CurrentCultureIgnoreCase));
            }
            if (productByCode != null)
            {
                return productByCode;
            }

            Product productByName =
                EntityManager.INSTANCE.GetAllNotDeleted<Product>()
                             .FirstOrDefault(prod => prod.NameLocal != null && prod.Type == ProductType.OUTER &&
                                                     prod.NameLocal.Equals(supplierProductName,
                                                                           StringComparison.CurrentCultureIgnoreCase));

            if (productByName != null)
            {
                return productByName;
            }
            return new Product
            {
                Id = Guid.NewGuid(),
                Num = supplierProductCode,
                Name = new LocalizableValue(supplierProductName),
                Type = ProductType.OUTER,
                EstimatedPurchasePrice = salePrice,
                UnitWeight = 1
            };
        }

        public static List<Product> GetProducts(string code, ProductType type)
        {
            return GetAllNotDeletedProducts().Where(p => p.Type == type && p.Num == code).ToList();
        }

        public static ReadOnlyCollection<Product> GetAllOuterProducts()
        {
            return EntityManager.INSTANCE.GetAll<Product>(product => product.Type == ProductType.OUTER);
        }

        public static ReadOnlyCollection<Product> GetAllProducts()
        {
            return EntityManager.INSTANCE.GetAll<Product>();
        }

        public static ReadOnlyCollection<Product> GetAllNotDeletedProducts()
        {
            return EntityManager.INSTANCE.GetAllNotDeleted<Product>();
        }

        public static ReadOnlyCollection<Product> GetAllNotDeletedNativeProducts(params ProductType[] types)
        {
            return
                EntityManager.INSTANCE.GetAllNotDeleted<Product>(
                    product => product.Type != ProductType.OUTER && types.Contains(product.Type));
        }

        #endregion Static members

        #region Constructor

        public Product(
            Guid id, string name, string description, string num, ProductGroup parent,
            CookingPlaceType cookingPlaceType, string code, string fullName, decimal? defaultSalePrice,
            bool defaultIncludedInMenu, ProductType type, ProductCategory category, MeasureUnit mainUnit,
            long localId)
            : base(id, new LocalizableValue(name), description, num, parent, code)
        {
            this.fullName = fullName;
            this.defaultSalePrice = defaultSalePrice;
            this.defaultIncludedInMenu = defaultIncludedInMenu;
            this.type = type;
            PlaceType = cookingPlaceType;
            MainUnit = mainUnit;
            LocalId = localId;
            Category = category;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Участвует ли услуга в движении по складу.
        /// </summary>
        public bool IsNotInStoreMovement
        {
            get
            {
                return type == ProductType.SERVICE && isNotInStoreMovement;
            }
            set
            {
                isNotInStoreMovement = type == ProductType.SERVICE && value;
            }
        }

        public decimal SalePrice
        {
            get { return NullableSalePrice ?? 0; }
        }

        public decimal? NullableSalePrice
        {
            get
            {
                decimal? result = null;

                var item = PriceListItem;
                if (item == null || DateTimeUtils.GetOperationalDay(null) >= item.DateTo)
                {
                    result = DefaultSalePrice;
                }
                else
                {
                    result = item.Price;
                }
                return result;
            }
        }

        [NotNull]
        public List<Container> NonDeletedContainers
        {
            get { return Containers.Where(container => !container.Deleted).ToList(); }
        }

        /// <summary>
        /// Все доступные для продукта фасовки
        /// </summary>
        [NotNull]
        public IEnumerable<Container> AvailableContainers => NonDeletedContainers.Concat(Container.GetMeasureUnitContainer(MainUnit).AsSequence());

        /// <summary>
        /// Возвращает список конечных модификаторов продукта
        /// </summary>
        public IEnumerable<Product> GetModifiersProducts()
        {
            return GetAllModifiersProducts().Distinct();
        }

        private IEnumerable<Product> GetAllModifiersProducts()
        {
            foreach (ChoiceBinding binding in ActualModifiers)
            {
                if (binding.IsSimpleModifier)
                {
                    yield return (Product)binding.Modifier;
                }
                else if (binding.IsGroupModifier)
                {
                    var productGroup = (ProductGroup)binding.Modifier;
                    foreach (Product child in productGroup.GetNotDeletedChilds(ProductType.MODIFIER))
                    {
                        yield return child;
                    }
                }
            }
        }

        /// <summary>
        /// Признак того, что блюдо содержит хотя бы один обязательный модификатор.
        /// </summary>
        public bool ContainsRequiredModifier
        {
            get
            {
                return NotNullableModifiers.Any(choiceBinding => choiceBinding.Required) ||
                       NotNullableModifiers.Where(grMod => grMod.ChildModifiers != null)
                                           .SelectMany(grMod => grMod.ChildModifiers)
                                           .Any(cb => cb.MinimumAmount > 0);
            }
        }

        /// <summary>
        /// Не использовать в RMSIntegration, см. RMS-45989.
        /// </summary>
        public bool IncludedInMenu
        {
            get
            {
                bool included = DefaultIncludedInMenu;
                var item = PriceListItem;
                if (item != null && DateTime.Today <= item.DateTo)
                {
                    included = item.Included;
                }
                return included;
            }
        }

        public PriceListItemDto PriceListItem
        {
            get
            {
                PriceListItemDto result = null;
                // // В RMS мы должны смотреть сначала на приказы об изменении прейскуранта.
                // if (Edition.CurrentEdition == BackVisualizationMode.IIKO_RMS)
                // {
                //     var priceListItemKey = new PriceListItemKey(MultiDepartments.Instance.ChainOrRmsSingleDepartment, this, DefaultSize, null);
                //     result = PriceListItemRepositoryInstance.INSTANCE.ItemsByKey.GetOrDefault(priceListItemKey);
                // } //todo debugnow

                return result;
            }
        }

        public Product OriginalProduct
        {
            get { return this; }
        }

        public CookingPlaceType PlaceTypeNonEmpty
        {
            get { return placeType; }
            set
            {
                if (value != null && value.Id != Guid.Empty)
                    placeType = value;
                else
                    placeType = null;
            }
        }

        public string DisplayName
        {
            get
            {
                if (Parent != null && !string.IsNullOrEmpty(Parent.Code) && NameLocal != null &&
                    !NameLocal.StartsWith(Parent.Code + " "))
                {
                    return Parent.Code + " " + NameLocal;
                }
                return NameLocal;
            }
        }

        /// <summary>
        /// Возвращает вес в кг одной единицы продукта (или коэффициент для пересчета кол-ва продукта в кг).
        /// В отличие от UnitWeight, гарантирует, что для продуктов с единицей измерения "кг" будет возвращено 1.
        /// На самом деле, сервер проверяет, что для всех таких продуктов значение UnitWeight установлено в 1.
        /// Поэтому, вызывать этот метод имеет смысл только для объектов, которые еще не сохранены на сервере.
        /// </summary>
        /// <returns>вес одной единицы продукта</returns>
        public decimal UnitWeightMultiplier
        {
            get { return MainUnit.MainUnit ? 1 : UnitWeight; }
        }

        /// <summary>
        /// Возвращает true, если элемент номенклатуры является товаром, блюдом или заготовкой.
        /// </summary>
        public bool IsNativeProduct
        {
            get { return Type == ProductType.DISH || type == ProductType.GOODS || type == ProductType.PREPARED; }
        }

        /// <summary>
        /// Возвращает true, если товар является внешним.
        /// </summary>
        public bool IsOuter
        {

            get { return Type == ProductType.OUTER; }
        }

        /// <summary>
        /// Возвращает true, если для элемента номенклатуры можно создать тех. карту.
        /// <remarks>Тех. карту можно создать для блюда, заготовки или модификатора.</remarks>
        /// </summary>
        public bool MayHaveAssemblyCharts
        {
            get { return Type == ProductType.DISH || Type == ProductType.PREPARED || Type == ProductType.MODIFIER; }
        }

        /// <summary>
        /// Возвращает true, если у элемента номенклатуры может быть настроен курс блюда по умолчанию.
        /// <remarks>Курс блюда можно устанавливать у товара, блюда или модификатора.</remarks>
        /// </summary>
        public bool MayHaveDefaultCourse
        {
            get { return Type == ProductType.GOODS || Type == ProductType.DISH || Type == ProductType.MODIFIER; }
        }

        /// <summary>
        /// Возвращает иерархию групп для данного элемента номенклатуры в виде строки.
        /// В качестве символа разделителя используется '\'.
        /// Ex: "Товары\Овощи\Зелень".
        /// </summary>
        [NotNull]
        public string GroupsHierarchyPath
        {
            get { return GroupsHierarchy.Select(g => g.NameLocal).Reverse().Join(@"\"); }
        }

        /// <summary>
        /// Возвращает иерархию групп для данного элемента номенклатуры в виде строки.
        /// В качестве символа разделителя используется '\'. Для самой верхней группы, возвращает <see cref="ProductGroup.rootGroupName"/>.
        /// Ex: "Товары\Овощи\Зелень".
        /// </summary>
        [NotNull]
        public string GroupsHierarchyWithRootGroupPath
        {
            get { return !string.IsNullOrEmpty(GroupsHierarchyPath) ? GroupsHierarchyPath : ProductGroup.rootGroupName; }
        }

        /// <summary>
        /// Возвращает иерархию групп для данного элемента номенклатуры.
        /// </summary>
        [NotNull]
        public List<ProductGroup> GroupsHierarchy
        {
            get
            {
                var list = new List<ProductGroup>();
                var parent = Parent;
                while (parent != null)
                {
                    list.Add(parent);
                    parent = parent.Parent;
                }
                return list;
            }
        }

        /// <summary>
        /// Возвращает единицу, если продукт измеряется в литрах, и <see cref="UnitCapacity"/> для всех остальных случаев.
        /// </summary>
        public decimal UnitCapacityEffective
        {
            get
            {
                return Equals(MainUnit, MeasureUnit.DefaultLitreUnit) ? decimal.One : UnitCapacity;
            }
        }

        /// <summary>
        /// Возвращает запрещенные размеры для блюда.
        /// </summary>
        [NotNull]
        public List<ProductSize> DisabledSizes
        {
            get
            {
                if (ScaleOfProductOrModifierSchema == null || DisabledProductSizes == null)
                    return new List<ProductSize>();

                return
                    EntityManager.INSTANCE.GetAll<ProductSize>(size => DisabledProductSizes.Contains(size.Id)).ToList();
            }
        }

        /// <summary>
        /// Шкала размеров
        /// </summary>
        /// <remarks>
        /// При наличии схемы модификаторов шкала берётся из неё, иначе - из самого продукта.
        /// </remarks>
        [CanBeNull]
        public ProductScale ScaleOfProductOrModifierSchema
        {
            get { return ModifierSchema != null ? ModifierSchema.ProductScale : productScale; }
        }

        [NotNull]
        [ItemCanBeNull]
        public IEnumerable<ProductSize> ProductSizes
        {
            get
            {
                return ModifierSchema != null
                    ? ModifierSchema.ProductScale != null ? ModifierSchema.ProductScale.GetSizes(this) : new ProductSize[] { null }
                    : productScale != null ? productScale.GetSizes(this) : new ProductSize[] { null };
            }
        }

        /// <summary>
        /// Первый штрихкод из списка штрихкодов продукта
        /// </summary>
        public string FirstBarCode
        {
            get
            {
                return (Barcodes != null && Barcodes.Count > 0)
                    ? Barcodes.First().Value.Barcode
                    : null;
            }
        }

        /// <summary>
        /// <c>true</c>, если продукту может быть назначена шкала размеров.
        /// </summary>
        public bool CanHaveSize
        {
            get { return type.CanHaveSize; }
        }

        /// <summary>
        /// Продукт без места приготовления
        /// </summary>
        public bool HasCookingPlace
        {
            get
            {
                return PlaceType != null || Type == ProductType.MODIFIER && CookWithMainDish;
            }
        }

        #endregion Properties

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return CompareTo((Product)obj);
        }

        #endregion IComparable Members



        public Product CopyWithEmptyLocalId(Guid id)
        {
            Product newProduct = Copy(id);
            newProduct.LocalId = -1;
            return newProduct;
        }

        public Product Copy(Guid id)
        {
            var newProduct = (Product)DeepClone(id);

            if (id != Id)
            {
                // если копирование осуществляется в новый продукт, обновляем идентификаторы фасовок
                foreach (var container in newProduct.Containers)
                {
                    container.Id = Guid.NewGuid();
                }
            }

            return newProduct;
        }

        public override string ToString()
        {
            return NameLocal;
        }

        /// <summary>
        /// Возвращает имя группы соответствующего уровня.
        /// Если группа не найдена, то возвращается имя "Группа самого высокого уровня" или пустая строка, 
        /// если группа предыдущего уровня была "Группой самого выского уровня".
        /// </summary>
        /// <param name="level">Уровень вложенности.</param>
        public string GetGroupNameByLevel(int level)
        {
            ProductGroup pg = GroupsHierarchy.ElementAtOrDefault(level);

            if (pg != null)
                return pg.NameLocal;

            if (level == 0)
                return ProductGroup.rootGroupName;

            pg = GroupsHierarchy.ElementAtOrDefault(level - 1);

            if (pg != null)
                return ProductGroup.rootGroupName;

            return string.Empty;
        }

        /// <summary>
        /// Возвращает true, если товар находится в одной из родительских групп.
        /// </summary>
        /// <param name="id">ID группы.</param>
        public bool ContainsInGroup(Guid id)
        {
            ProductGroup pg = Parent;
            while (pg != null && pg.Id != RootGroupId)
            {
                if (pg.Id == id)
                {
                    return true;
                }
                else
                {
                    pg = pg.Parent;
                }
            }
            return false;
        }

        public List<CertificateOfQuality> GetActualSertificates(User supplier)
        {
            if (Images.Count == 0)
                return null;
            List<CertificateOfQuality> actSertificate = new List<CertificateOfQuality>(); //Images[0];
            for (int i = 0; i < Images.Count; i++)
            {
                ProductImage curSertificate = Images[i];
                if (!(curSertificate is CertificateOfQuality))
                    continue;
                CertificateOfQuality certificate = (CertificateOfQuality)curSertificate;
                if (curSertificate.DateStart.GetValueOrFakeDefault() <= DateTime.Now && certificate.Supplier != null &&
                    certificate.Supplier.Equals(supplier))
                    actSertificate.Add((CertificateOfQuality)curSertificate);
            }

            return actSertificate;
        }

        [NotNull]
        public Container GetContainerById(Guid containerId)
        {
            return Containers.FirstOrDefault(container => container.Id == containerId)
                   ?? Container.GetMeasureUnitContainer(MainUnit);
        }

        public static Pair<Product, Container>? GetProductContainerByBarcode(string barcode)
        {
            var products =
                EntityManager.INSTANCE.GetAll<Product>(p => p.Barcodes != null && p.Barcodes.ContainsKey(barcode));
            if (products == null || products.Count == 0)
            {
                return null;
            }

            if (products.Count > 1)
            {
                throw new RestoException(string.Format(Resources.ProductProductMoreThanOneGoodWithTheBarcodeFound,
                                                       barcode));
            }

            return new Pair<Product, Container>(products[0], products[0].Barcodes[barcode].Container);
        }

        /// <summary>
        /// Возвращает продукт по артикулу.
        /// </summary>
        public static Product GetProductByNum(string num)
        {
            var products = EntityManager.INSTANCE.GetAll<Product>(p => p.Num == num);

            if (products == null || products.Count == 0)
            {
                return null;
            }

            if (products.Count > 1)
            {
                throw new RestoException(string.Format(Resources.ProductMoreThanOneGoodFoundWithTheArticle, num));
            }
            return products[0];
        }

        /// <summary>
        /// Возвращает штрихкод фасовки/контейнера.
        /// </summary>
        /// <param name="cont">Контейнер.</param>
        /// <returns>Штрихкод.</returns>
        public List<string> GetBarCodesByContainer([NotNull] Container cont)
        {
            List<string> codes = new List<string>();

            if (barcodes != null && barcodes.Any())
            {
                codes = barcodes.Where(b => (b.Value.Container == null && cont.IsEmptyContainer) ||
                                            (b.Value.Container != null && b.Value.Container.Equals(cont)))
                                .Select(r => r.Value.BarcodeString)
                                .ToList();
            }
            return codes;
        }

        /// <summary>
        /// Если <see cref="FullName"/> задано, возвращает его, иначе <see cref="LocalizableNamePersistedEntity.NameLocal"/>
        /// </summary>
        public string GetFullNameOrLocal()
        {
            return FullName.IsNullOrWhiteSpace() ? NameLocal : FullName;
        }

        #region IComparable<Product> Members

        public int CompareTo(Product other)
        {
            if (other == null)
            {
                // В нашем случае объект меньше значения null
                // (поведение взял из ранней реализации IComparable, которую затем переписал).
                return -1;
            }

            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            int result = Comparer<string>.Default.Compare(NameLocal, other.NameLocal);

            // Если имена одинаковые, сравниваем по идентификаторам (сами продукты могут быть разные).
            if (result == 0)
            {
                result = Id.CompareTo(other.Id);
            }

            return result;
        }

        #endregion

        /// <summary>
        /// Возвращает для продукта величину наценки для данного торгового предприятия.
        /// Наценка возвращается с учетом настроек продукта "Фиксированная цена".
        /// </summary>
        /// <param name="department">Торговое предприятие для определения наценки.</param>
        /// <returns>Наценка для продукта по торговому предприятию.</returns>
        public decimal GetMarkupForDepartment(DepartmentEntity department)
        {
            if (IsFixedPrice)
            {
                return 0m;
            }

            ProductMarkupSettings markup;
            if (MarkupSettings.TryGetValue(department, out markup))
            {
                return markup.MarkupPercent;
            }
            return PriceMarkupPercent.GetValueOrFakeDefault();
        }

        /// <summary>
        /// Если у продукта есть шкала размеров, возвращает размер по умолчанию
        /// из этой шкалы или первый из доступных, если размер по умолчанию для
        /// данного продукта недоступен (содержится в DisabledSizes).
        /// </summary>
        [CanBeNull]
        public ProductSize DefaultSize
        {
            get
            {
                var scale = ScaleOfProductOrModifierSchema;
                if (scale == null)
                {
                    return null;
                }

                var result = scale.DefaultProductSize;

                // Для шкалы не задан размер по умолчанию, либо размер
                // по умолчанию не доступен для данного продукта
                if (result == null || DisabledSizes.Contains(result))
                {
                    result = scale.GetSizes(this).FirstOrDefault();
                }

                return result;
            }
        }

        /// <summary>
        /// Коэффициент списания данного продукта для указанных размера и количества
        /// </summary>
        /// <remarks>
        /// см. Resto.CashServer.Services.Extensions.ProductExtensions.GetAmountFactor
        /// </remarks>
        public decimal GetAmountFactor(ProductSize productSize, decimal amount)
        {
            if (productSize == null || productSizeFactors == null || !CanHaveSize)
            {
                return ProductSizeServerConstants.INSTANCE.DefaultAmountFactor;
            }

            if (!productSizeFactors.Factors.TryGetValue(productSize, out var sizeFactors) || sizeFactors.IsEmpty())
            {
                return ProductSizeServerConstants.INSTANCE.DefaultAmountFactor;
            }

            foreach (var pair in sizeFactors.OrderByDescending(pair => pair.Key))
            {
                if (amount >= pair.Key)
                {
                    return pair.Value;
                }
            }

            return ProductSizeServerConstants.INSTANCE.DefaultAmountFactor;
        }

        public decimal? FatAmount => MainNutritionalValue.FatAmount;
        public decimal? FiberAmount => MainNutritionalValue.FiberAmount;
        public decimal? CarbohydrateAmount => MainNutritionalValue.CarbohydrateAmount;
        public decimal? EnergyAmount => MainNutritionalValue.EnergyAmount;

        /// <summary>
        /// Пищевая ценность для клиентов/интеграций, котороые не знают про деление КБЖУ по ТП и размерам.
        /// </summary>
        /// <para>
        /// Дубликат Product.java#getMainNutritionValue
        /// </para>
        [NotNull]
        private NutritionValue MainNutritionalValue
        {
            get
            {
                if (nutritionValues.IsEmpty())
                {
                    return new NutritionValue();
                }

                var general = GetNutritionValueNullable(null, null);
                if (general != null)
                {
                    return general;
                }

                //Не используется свойство DefaultSize для единства кода на беке и сервере
                var scale = ScaleOfProductOrModifierSchema;
                var defaultSize = scale?.DefaultProductSize;
                return defaultSize != null
                    ? GetNutritionValue(defaultSize, null)
                    : new NutritionValue();
            }
        }

        /// <summary>
        /// Возвращает пищевую ценность для департамента и размера.
        /// Если передать department null или productSize null, то вернёт общую пищевую ценность,
        /// а не список по всем департаментам и размерам.
        /// </summary>
        /// <param name="productSize">Размер. Null = Без размера</param>
        /// <param name="department">ТП. Null = Основное ТП</param>
        [Framework.Attributes.JetBrains.Pure]
        [NotNull]
        public NutritionValue GetNutritionValue([CanBeNull] ProductSize productSize, [CanBeNull] DepartmentEntity department)
        {
            return GetNutritionValueNullable(productSize, department) ?? new NutritionValue();
        }

        /// <summary>
        /// !!!! ВАЖНО !!!!!
        /// Метод должен точно повторять логику работы серверного метода ProductHelper.java#tryFindNutritionValue
        /// </summary>
        [CanBeNull]
        private NutritionValue GetNutritionValueNullable([CanBeNull] ProductSize productSize, [CanBeNull] DepartmentEntity department)
        {
            bool IsMatchingDepartment(NutritionValue value)
            {
                return department == null && value.Departments == null ||
                       value.Departments != null && value.Departments.Contains(department);
            }

            // 1. Сначала ищем точное совпадение размера и ТП
            var exact = NutritionValues.FirstOrDefault(value => Equals(productSize, value.ProductSize) &&
                                                                IsMatchingDepartment(value));
            if (exact != null)
            {
                return exact;
            }

            // 2. Точное совпадение размера и общее ТП
            // Если передали department == null, то эта проверка будет повторением проверки 1
            if (department != null)
            {
                var generalDepartment = NutritionValues.FirstOrDefault(value => Equals(productSize, value.ProductSize) &&
                                                                                value.Departments == null);
                if (generalDepartment != null)
                {
                    return generalDepartment;
                }
            }

            // 3. Точное совпадение ТП и Без размера
            // Если передали productSize == null, то эта проверка будет повторением проверки 1
            if (productSize != null)
            {
                var generalSize = NutritionValues.FirstOrDefault(value => value.ProductSize == null &&
                                                                          IsMatchingDepartment(value));
                if (generalSize != null)
                {
                    return generalSize;
                }
            }

            // 4. Общее ТП и Без размера
            // Если productSize == null или department == null, то эта проверка будет повторением проверки 1
            if (productSize != null && department != null)
            {
                var general = NutritionValues.FirstOrDefault(value => value.ProductSize == null &&
                                                                      value.Departments == null);
                if (general != null)
                {
                    return general;
                }
            }

            return null;
        }
    }

    #endregion Product
}
