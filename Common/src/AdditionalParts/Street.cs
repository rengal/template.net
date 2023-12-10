using Resto.Framework.Common;
using System;
using System.Collections.Generic;

namespace Resto.Data
{
    public partial class Street : IComparable<Street>, IComparable, IWithDependents
    {
        public override string ToString()
        {
            return Name;
        }

        #region IComparable<Street> Members

        public int CompareTo(Street other)
        {
            if (other == null)
            {
                return 1;
            }
            if (ReferenceEquals(this, other))
            {
                return 0;
            }
            int result = Comparer<string>.Default.Compare(Name, other.Name);
            if (result == 0)
            {
                result = Comparer<Guid>.Default.Compare(Id, other.Id);
            }
            return result;
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return CompareTo(obj as Street);
        }

        #endregion

        public IDictionary<Guid, int> GetDependents()
        {
            return new Dictionary<Guid, int>(1) { { City.Id, City.Revision } };
        }

        public bool IsEmpty => Id == PredefinedGuids.EMPTY_STREET_ID.Id;
        public static bool IsEmptyStreetId(Guid id) => id == PredefinedGuids.EMPTY_STREET_ID.Id;
    }
}