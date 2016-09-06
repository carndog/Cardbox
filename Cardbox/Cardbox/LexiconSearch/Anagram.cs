using System.Collections.Generic;
using System.Linq;

namespace Cardbox.LexiconSearch
{
    public class Anagram : ISearch
    {
        private readonly ITrieSearcher _trieSearcher;

        private readonly IResultsCache<string, IList<string>> _resultsCache;

        public Anagram(ITrieSearcher trieSearcher, IResultsCache<string, IList<string>> resultsCache)
        {
            _trieSearcher = trieSearcher;
            _resultsCache = resultsCache;
        }


        public IList<string> Query(string searchTerm)
        {
            IList<string> results = _resultsCache.Get(searchTerm);

            if (results == null)
            {

                results = _trieSearcher.Query(searchTerm, (enumerable => enumerable.Where(x => x.Length == searchTerm.Length)))
                        .OrderByDescending(x => x.Length)
                        .ToList();

                _resultsCache.Add(searchTerm, results);
            }

            return results;
        }
    }
}