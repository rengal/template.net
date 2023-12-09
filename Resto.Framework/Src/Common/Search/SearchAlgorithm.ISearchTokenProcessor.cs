using System;

namespace Resto.Framework.Common.Search
{
    public partial class SearchAlgorithm<TObj>
    {
        private interface ISearchTokenProcessor
        {
            Func<SearchToken, bool> IsActiveOnToken { get; }
            bool Check(TObj objToCheck, SearchToken searchToken);
            bool Match(TObj objectToMatch, SearchMatches<TObj> matches, SearchToken searchToken);
            void Freeze(SearchAlgorithm<TObj> algorithm);
        }
    }
}
