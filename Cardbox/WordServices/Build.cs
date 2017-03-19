using System.Collections.Generic;
using System.Linq;

namespace WordServices
{
    public class Build : ISearch
    {
        private readonly TrieSearcher _trieSearcher;

        public Build(TrieSearcher trieSearcher)
        {
            _trieSearcher = trieSearcher;
        }

        public IList<string> Query(string searchTerm)
        {
            return _trieSearcher.Query(searchTerm, enumerable => enumerable)
                .OrderByDescending(x => x.Length)
                .ToList();
        }
    }
}