using System;
using System.Linq;
using Resto.Framework.Common;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class Conception : IComparable<Conception>, IComparable
    {
        /// <summary>
        /// Концепция "Без концепции", её нельзя удалить и редактировать.
        /// </summary>
        public static Conception NoConception
        {
            get { return EntityManager.INSTANCE.GetAllNotDeleted<Conception>().Single(c => c.Id == PredefinedGuids.NO_CONCEPTION.Id); }
        }

        /// <summary>
        /// Возвращает True, если данная концепция "Без концепции".
        /// </summary>
        public bool IsNoConception => Id.Equals(PredefinedGuids.NO_CONCEPTION.Id);

        public string GroupList
        {
            get
            {
                return ProductGroups
                    .Select(group => group != null ? group.NameLocal : ProductGroup.rootGroupName)
                    .Join(", ");
            }
        }

        public override string ToString()
        {
            return NameLocal;
        }

        #region IComparable<Conception> Members

        public int CompareTo(Conception other)
        {
            return other == null ? 1 : string.Compare(NameLocal, other.NameLocal);
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return (obj as Conception) == null ? 1 : CompareTo((Conception)obj);
        }

        #endregion
    }
}
