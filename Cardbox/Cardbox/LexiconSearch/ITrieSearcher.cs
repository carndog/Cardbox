using System;
using System.Collections.Generic;

namespace Cardbox.LexiconSearch
{
    public interface ITrieSearcher
    {
        List<string> Query(string searchTerm, Func<IEnumerable<string>, IEnumerable<string>> wordFilter);
    }
}