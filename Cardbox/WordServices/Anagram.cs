using System.Collections.Generic;
using System.Linq;

namespace WordServices
{
    public class Anagram : ISearch
    {
        private readonly TrieSearcher _trieSearcher;

        public Anagram(TrieSearcher trieSearcher)
        {
            _trieSearcher = trieSearcher;
        }

        public IList<string> Query(string searchTerm)
        {
            return _trieSearcher.Query(searchTerm, (enumerable => enumerable.Where(x => x.Length == searchTerm.Length)))
                    .OrderByDescending(x => x.Length)
                    .ToList();
        }
    }
}