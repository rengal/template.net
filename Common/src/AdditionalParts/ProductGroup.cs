using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common;
using Resto.Common.Extensions;
using Resto.Common.Properties;
using Resto.Framework.Common;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class ProductGroup : IComparable, IComparable<ProductGroup>
    {
        public static string rootGroupName = Resources.ProductGroupHighLevelGroup;

        public ProductGroup(Guid id, string name, string description, string num, ProductGroup parent, string code,
                            ProductGroupType type, long localId)
            : base(id, new LocalizableValue(name), description, num, parent, code)
        {
            this.type = type;
            LocalId = localId;
        }

        public string DisplayName
        {
            get
            {
                if (Code != null && Code != "" && !NameLocal.StartsWith(Code + " "))
                {
                    return Code + " " + NameLocal;
                }
                return NameLocal;
            }
        }

        public bool IncludeInReportExt
        {
            get { return Edition.CurrentEdition != BackVisualizationMode.IIKO_CHAIN || IncludeInReport; }
        }

        public override string ToString()
        {
            return NameLocal;
        }

        public void ApplyParentProperties()
        {
            Contract.Requires(Parent != null);

            TaxCategory = Parent.TaxCategory;
            MarkupSettings.Set(Parent.MarkupSettings);
            PriceMarkupPercent = Parent.PriceMarkupPercent.GetValueOrFakeDefault();
        }

        public override bool IsVisibleFor(DepartmentEntity department)
        {
            return Parent != null
                       ? base.IsVisibleFor(department)
                       : VisibilityFilter == null || VisibilityFilter.IsSatisfiedBy(department);
        }

        public bool IsVisibleForDepartments([CanBeNull] IEnumerable<DepartmentEntity> departments)
        {
            return departments == null || departments.Any(IsVisibleFor);
        }

        public IEnumerable<Product> GetNotDeletedChilds()
        {
            return EntityManager.INSTANCE.GetAllNotDeleted<Product>(p => Equals(p.Parent));
        }

        public IEnumerable<Product> GetNotDeletedChilds(ProductType productType)
        {
            return EntityManager.INSTANCE.GetAllNotDeleted<Product>(p => p.Type.Equals(productType) && Equals(p.Parent));
        }

        public static IEnumerable<ProductGroup> GetAllNotDeletedAndVisibleFor([CanBeNull] DepartmentEntity department)
        {
            if (department == null)
            {
                return EntityManager.INSTANCE.GetAllNotDeleted<ProductGroup>();
            }
            else
            {
                return EntityManager.INSTANCE.GetAllNotDeleted<ProductGroup>(g => g.IsVisibleFor(department));
            }
        }

        [NotNull]
        public static IEnumerable<ProductGroup> GetAllNotDeletedAndVisibleForDepartments([CanBeNull] IEnumerable<DepartmentEntity> departments)
        {
            return EntityManager.INSTANCE.GetAllNotDeleted<ProductGroup>(g => g.IsVisibleForDepartments(departments));
        }

        /// <summary>
        /// Возвращает список групп, в которых присутствуют продукты с типами <paramref name="availableTypes"/>
        /// и видимых для <paramref name="departments"/>
        /// </summary>
        public static IEnumerable<ProductGroup> GetAllNonEmptyGroups([CanBeNull] ProductType[] availableTypes,
            [CanBeNull] IReadOnlyCollection<DepartmentEntity> departments)
        {
            var groups = new HashSet<ProductGroup>();
            var products = EntityManager.INSTANCE.GetAllNotDeleted<Product>(product => availableTypes == null || product.Type.In(availableTypes.ToArray()));
            foreach (var product in products)
            {
                var group = product.Parent;
                while (group != null)
                {
                    if (group.IsVisibleForDepartments(departments))
                    {
                        groups.Add(group);
                    }

                    group = group.Parent;
                }
            }

            return groups;
        }

        /// <summary>
        /// Возвращает список всех дочерних групп для <paramref name="currentGroup"/>.
        /// <paramref name="currentGroup"/> тоже включена в список.
        /// </summary>
        /// <param name="currentGroup">Группа</param>
        public static IEnumerable<ProductGroup> GetAllChildGroups([NotNull] ProductGroup currentGroup)
        {
            return GetGroupsRecursively(GetAllNotDeletedAndVisibleFor(null).ToArray(), currentGroup);
        }

        private static IEnumerable<ProductGroup> GetGroupsRecursively([NotNull] IReadOnlyCollection<ProductGroup> allGroups,
            [NotNull] ProductGroup currentGroup)
        {
            var result = new HashSet<ProductGroup> { currentGroup };
            foreach (var productGroup in allGroups)
            {
                if (Equals(productGroup.Parent, currentGroup))
                {
                    result.AddRange(GetGroupsRecursively(allGroups, productGroup));
                }
            }

            return result;
        }

        #region IComparable implementation

        public int CompareTo(object obj)
        {
            return CompareTo((ProductGroup)obj);
        }

        #endregion

        #region IComparable<ProductGroup> implementation

        public int CompareTo(ProductGroup other)
        {
            if (other == null)
            {
                return -1;
            }

            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            int result = Comparer<string>.Default.Compare(NameLocal, other.NameLocal);

            if (result == 0)
            {
                result = Id.CompareTo(other.Id);
            }

            return result;
        }

        #endregion
    }
}