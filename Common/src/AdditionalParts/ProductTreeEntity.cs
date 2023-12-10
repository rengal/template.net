using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Common.Extensions;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class ProductTreeEntity
    {
        /// <summary>
        /// Группа самого высокого уровня
        /// </summary>
        public static readonly ProductGroup RootGroup = new ProductGroup(RootGroupId,
            new LocalizableValue(ProductGroup.rootGroupName), string.Empty, ProductGroupType.PRODUCTS);

        /// <summary>
        /// Значение НДС, взятое из налоговой категории.
        /// Напрямую НДС больше не редактируется.
        /// </summary>
        public decimal VatPercent
        {
            get { return TaxCategory != null ? TaxCategory.VatPercent : 0m; }
        }

        /// <summary>
        /// Группа самого высокого уровня.
        /// </summary>
        public static Guid RootGroupId
        {
            get { return Guid.Empty; }
        }

        public Guid ParentId
        {
            get { return parent != null ? parent.Id : RootGroupId; }
        }

        /// <summary>
        /// Родительская группа первого уровня или null если родителей нет
        /// </summary>
        private ProductGroup RootParent
        {
            get
            {
                if (Parent == null)
                {
                    return null;
                }

                var traversedIds = new HashSet<Guid> { Id };

                ProductGroup currentParent = Parent;
                while (currentParent.Parent != null)
                {
                    if (!traversedIds.Add(currentParent.Id))
                    {
                        // Детектирована циклическая зависимость, формируем сообщение об ошибке.
                        var traversedProducts = new List<ProductTreeEntity>();
                        var traversedProduct = this;
                        traversedProducts.Add(traversedProduct);
                        do
                        {
                            traversedProduct = traversedProduct.Parent;
                            traversedProducts.Add(traversedProduct);
                        } while (!Equals(traversedProduct, currentParent));

                        throw new RestoException(string.Format(
                            "Cyclic reference detected: {0}.",
                            string.Join(" -> ", traversedProducts.Select(p => p.NameLocal))));
                    }
                    currentParent = currentParent.Parent;
                }
                return currentParent;
            }
        }

        /// <summary>
        /// Проверяет видимость эл-та для подразделения.
        /// </summary>
        public virtual bool IsVisibleFor(DepartmentEntity department)
        {
            ProductGroup rootParent = RootParent;
            return rootParent == null || rootParent.VisibilityFilter == null || rootParent.VisibilityFilter.IsSatisfiedBy(department);
        }

        /// <summary>
        /// Проверяет, что элемент видим в хотя бы одном ТП.
        /// Если <see cref="departments"/> равен null - считает, что элемент видим.
        /// </summary>
        /// <param name="departments">Торговые предприятия, видимость в которых проверяется.</param>
        public bool IsVisibleForAnyDepartment([CanBeNull] [InstantHandle] IEnumerable<DepartmentEntity> departments)
        {
            return departments == null || departments.Any(IsVisibleFor);
        }

        /// <summary>
        /// Проверяет явлется ли данный элемент номенклатуры дочерним к переданной группе
        /// </summary>
        public bool IsChildOf(ProductGroup productGroup)
        {
            ProductGroup currentParent = parent;
            while (currentParent != null)
            {
                if (currentParent.Equals(productGroup))
                {
                    return true;
                }
                currentParent = currentParent.Parent;
            }
            return false;
        }

        public void SetMinimalStoreBalanceLevel(Store store, decimal? value)
        {
            if (MinimalStoreBalanceLevels.ContainsKey(store) && MinimalStoreBalanceLevels[store].Value.GetValueOrFakeDefault() == value) return;

            if (MinimalStoreBalanceLevels.ContainsKey(store))
            {
                MinimalStoreBalanceLevels[store].Value = value;
                MinimalStoreBalanceLevels[store].ValueAssigned = true;
            }
            else
            {
                MinimalStoreBalanceLevels.Add(store, new StoreLevelValue(value, true));
            }
        }

        public void SetMaximalStoreBalanceLevel(Store store, decimal? value)
        {
            if (MaximumStoreBalanceLevels.ContainsKey(store) && MaximumStoreBalanceLevels[store].Value.GetValueOrFakeDefault() == value) return;

            if (MaximumStoreBalanceLevels.ContainsKey(store))
            {
                MaximumStoreBalanceLevels[store].Value = value;
                MaximumStoreBalanceLevels[store].ValueAssigned = true;
            }
            else
            {
                MaximumStoreBalanceLevels.Add(store, new StoreLevelValue(value, true));
            }
        }

        public decimal? MinimumValueForStore(Store store)
        {
            return MinimalStoreBalanceLevels.ContainsKey(store) && MinimalStoreBalanceLevels[store].ValueAssigned
                       ? MinimalStoreBalanceLevels[store].Value
                       : DefaultMinimumStoreBalanceLevel;
        }

        public decimal? MaximumValueForStore(Store store)
        {
            return MaximumStoreBalanceLevels.ContainsKey(store) && MaximumStoreBalanceLevels[store].ValueAssigned
                       ? MaximumStoreBalanceLevels[store].Value
                       : DefaultMaximumStoreBalanceLevel;
        }

        /// <summary>
        /// Безопасная работа со списком модификаторов.
        /// </summary>
        public List<ChoiceBinding> NotNullableModifiers
        {
            get { return Modifiers ?? (Modifiers = new List<ChoiceBinding>()); }
            set { Modifiers = value; }
        }

        /// <summary>
        /// Список модификаторов с учётом схемы модификаторов (если назначена схема,
        /// то возвращаются модификаторы из схемы).
        /// </summary>
        public IEnumerable<ChoiceBinding> ActualModifiers
        {
            get { return ModifierSchema == null ? NotNullableModifiers : ModifierSchema.Modifiers; }
        }

        /// <summary>
        /// Безопасная работа со списком производителей/импортеров
        /// </summary>
        public List<User> NotNullOrderedProducers
        {
            get { return Producers != null ? Producers.OrderBy(user => user.Name).ToList() : new List<User>(); }
            set { Producers = value.OrderBy(user => user.Name).ToHashSet(); }
        }
    }
}