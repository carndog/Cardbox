using BonusAccumulator.WordServices.Helpers;
using BonusAccumulator.WordServices.TrieLoading;

namespace BonusAccumulator.WordServices.TrieSearching;

public class TrieSearcher : ITrieSearcher
{
    private readonly LazyLoadingTrie _lazyTrie;
    private readonly List<string> _resultsList;

    public TrieSearcher(LazyLoadingTrie lazyTrie)
    {
        _lazyTrie = lazyTrie;

        const int defaultCapacity = 20;
        _resultsList = new List<string>(defaultCapacity);
    }

    public IList<string> Query(string searchTerm, Func<IEnumerable<string>, IEnumerable<string>> wordFilter)
    {
        _resultsList.Clear();

        List<string> query = QueryLexicon(searchTerm, _lazyTrie.Lexicon, wordFilter)
            .OrderByDescending(x => x.Length).ToList();

        return query;
    }

    private IList<string> QueryLexicon(string search, TrieNode? current, Func<IEnumerable<string>, IEnumerable<string>> filter)
    {
        search = search.WildcardsFirst().ToUpper();

        int index = 0;
        foreach (char c in search)
        {
            if (c == '.' && index == 0)
            {
                if (current?.Edges != null)
                {
                    foreach (TrieNode? node in current.Edges)
                    {
                        if (node != null && search.IndexOf(node.Label) == -1)
                        {
                            if (node.Terminal)
                            {
                                _resultsList.AddRange(filter(node.AnagramsAtTerminal));
                            }

                            QueryLexicon(search.Remove(index, 1), node, filter);
                        }
                    }
                }
            }
            else if (search.IndexOf(c, 0, index) == -1)
            {
                TrieNode? node = current?.Edges.Find(edge => edge?.Label == c);
                if (node != null)
                {
                    if (node.Terminal)
                    {
                        _resultsList.AddRange(filter(node.AnagramsAtTerminal));
                    }
                    QueryLexicon(search.Remove(index, 1), node, filter);
                }
            }
            index++;
        }
        return _resultsList;
    }
}