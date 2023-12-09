using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common.Search
{
    public sealed class SearchMatches<TObj>
    {
        private static readonly List<SearchMatch<TObj>> EmptyMatches =
            new List<SearchMatch<TObj>> { new SearchMatch<TObj>(null, null) };

        public TObj Object { get; }
        public List<SearchMatch<TObj>> Matches { get; private set; }

        [CanBeNull]
        public string SearchString { get; private set; }

        private string formattedMatchesStringView;
        public string FormattedMatchesStringView
        {
            get
            {
                return formattedMatchesStringView
                    ?? (formattedMatchesStringView = Matches == null
                            ? string.Empty
                            : Matches.Select(m => m.FormattedValue).Join(", "));
            }
        }

        private void InvalidateFormattedMatchesStringView()
        {
            formattedMatchesStringView = null;
        }

        public SearchMatches(TObj obj)
        {
            Object = obj;
        }

        public SearchMatches(TObj obj, string searchString,  bool emptyMatch)
        {
            SearchString = searchString;
            Object = obj;
            if (emptyMatch)
                Matches = EmptyMatches;
        }

        public SearchMatches(TObj obj, bool emptyMatch)
        {
            Object = obj;
            if (emptyMatch)
                Matches = EmptyMatches;
        }

        public void AddMatch(SearchMatch<TObj> match)
        {
            if (Matches == null)
                Matches = new List<SearchMatch<TObj>>();
            Matches.Add(match);

            InvalidateFormattedMatchesStringView();
        }

        internal void Unique()
        {
            if (Matches == null)
                return;

            if (Matches.Count > 1)
                Matches = Matches.Distinct().ToList();

            InvalidateFormattedMatchesStringView();
        }

        public bool IsMatched => Matches != null && Matches.Count > 0;

        public override bool Equals(object obj)
        {
            return obj is SearchMatches<TObj> matches && ReferenceEquals(matches.Object, Object);
        }

        public override int GetHashCode()
        {
            return ReferenceEquals(Object, null) ? 0 : Object.GetHashCode();
        }
    }
}
