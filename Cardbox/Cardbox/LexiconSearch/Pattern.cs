using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Cardbox.LexiconSearch
{
    public class Pattern : ISearch
    {
        private readonly TrieSearcher _trieSearcher;

        public Pattern(TrieSearcher trieSearcher)
        {
            _trieSearcher = trieSearcher;
        }

        public IList<string> Query(string searchTerm)
        {
            Func<IEnumerable<string>, IEnumerable<string>> wordFilter =
                filter => filter.Where(x => x.Length == searchTerm.Length && new Regex(searchTerm).IsMatch(x));

            return _trieSearcher
                .Query(searchTerm, wordFilter)
                .OrderByDescending(x => x.Length)
                .ToList();
        }
    }
}
