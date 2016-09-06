using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Cardbox.LexiconSearch
{
    public class Pattern : ISearch
    {
        private readonly ITrieSearcher _trieSearcher;

        private readonly IResultsCache<string, IList<string>> _resultsCache;

        public Pattern(ITrieSearcher trieSearcher, IResultsCache<string, IList<string>> resultsCache)
        {
            _trieSearcher = trieSearcher;
            _resultsCache = resultsCache;
        }

        public IList<string> Query(string searchTerm)
        {
            IList<string> results = _resultsCache.Get(searchTerm);

            if (results == null)
            {
                Func<IEnumerable<string>, IEnumerable<string>> wordFilter =
                    filter => filter.Where(x => x.Length == searchTerm.Length && new Regex(searchTerm).IsMatch(x));

                results = _trieSearcher
                    .Query(searchTerm, wordFilter)
                    .OrderByDescending(x => x.Length)
                    .ToList();

                _resultsCache.Add(searchTerm, results);
            }

            return results;
        }
    }
}
