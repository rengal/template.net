using System;
using System.Globalization;

namespace Resto.Framework.Common.Search
{
    public sealed class SearchMatch<TObj> : IEquatable<SearchMatch<TObj>>
    {
        private PropertySearchInfo<TObj> Property { get; set; }
        public object MatchedValue { get; private set; }
        public string SearchPropertyKey { get { return Property != null ? Property.SearchPropertyKey : null; } }

        internal SearchMatch(PropertySearchInfo<TObj> property, object matchedValue)
        {
            Property = property;
            MatchedValue = matchedValue;
        }

        public string FormattedValue
        {
            get
            {
                return Property == null
                    ? string.Empty
                    : Property.Format == null
                        ? string.Format(CultureInfo.CurrentUICulture, "{0}", MatchedValue)
                        : string.Format(CultureInfo.CurrentUICulture, Property.Format, MatchedValue);
            }
        }

        bool IEquatable<SearchMatch<TObj>>.Equals(SearchMatch<TObj> other)
        {
            return MatchedValue == other.MatchedValue;
        }

        public override int GetHashCode()
        {
            return MatchedValue != null ? MatchedValue.GetHashCode() : 0;
        }
    }
}
