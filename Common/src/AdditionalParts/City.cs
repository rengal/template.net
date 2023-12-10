using System;
using System.Collections.Generic;

namespace Resto.Data
{

    public partial class City : IComparable<City>, IComparable
    {
        public override string ToString()
        {
            return Name;
        }

        #region IComparable<City> Members

        public int CompareTo(City other)
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
            return CompareTo(obj as City);
        }

        #endregion

        /// <summary>
        /// Признак того, что у города задан телефонный код, и притом только один.
        /// </summary>
        public bool HasSinglePhoneCode
        {
            get { return !string.IsNullOrWhiteSpace(PhoneCodes) && PhoneCodes.Split(';').Length == 1; }
        }
    }
}