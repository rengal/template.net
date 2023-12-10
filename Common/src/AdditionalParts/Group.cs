using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Data;

namespace Resto.Data
{
    partial class Group : IComparable, IComparable<Group>, ICorporatedEntityProps
    {
        public override string ToString()
        {
            return NameLocal;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return CompareTo((Group)obj);
        }

        #endregion IComparable Members

        /// <summary>
        /// Возвращает список всех концепций заданной группы
        /// </summary>
        public IEnumerable<Conception> GetGroupConceptions()
        {
            return NotDeletedPointsOfSales.Select(ps => ps.Conception).Where(conc => conc != null).Distinct();
        }

        public IEnumerable<PointOfSale> NotDeletedPointsOfSales => PointsOfSale?.Where(i => !i.Deleted) ?? Enumerable.Empty<PointOfSale>();

        #region IComparable<Group> Members

        public int CompareTo(Group other)
        {
            if (other == null)
            {
                return 1;
            }
            if (ReferenceEquals(this, other))
            {
                return 0;
            }
            int result = Comparer<string>.Default.Compare(NameLocal, other.NameLocal);
            if (result == 0)
            {
                result = Comparer<Guid>.Default.Compare(Id, other.Id);
            }
            return result;
        }

        #endregion

        #region ICorporatedEntityProps implementation

        public string CEDescription
        {
            get { return string.Empty; }
            set { /* do nothing */ }
        }

        public PersistedEntity CEParent
        {
            get { return Department; }
            set
            {
                var departmentEntity = value as DepartmentEntity;
                if (departmentEntity == null)
                {
                    throw new ArgumentException("CEParent must be DepartmentEntity");
                }
                Department = departmentEntity;
            }
        }

        public CorporatedEntityType CEType
        {
            get { return CorporatedEntityType.GROUP; }
        }

        #endregion
    }
}