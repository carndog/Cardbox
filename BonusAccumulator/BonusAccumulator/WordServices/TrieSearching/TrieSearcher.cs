using BonusAccumulator.WordServices.Extensions;
using BonusAccumulator.WordServices.TrieLoading;
using static BonusAccumulator.WordServices.Constants;

namespace BonusAccumulator.WordServices.TrieSearching;

public class TrieSearcher : ITrieSearcher
{
    private readonly ILazyLoadingTrie _lazyTrie;
    private readonly List<string> _resultsList;

    public TrieSearcher(ILazyLoadingTrie lazyTrie)
    {
        _lazyTrie = lazyTrie;

        const int DefaultCapacity = 20;
        _resultsList = new(DefaultCapacity);
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
            if (c == Wildcard && index == 0)
            {
                if (current?.Edges != null)
                {
                    foreach (TrieNode? node in current.Edges)
                    {
                        if (search.IndexOf(node.Label) == -1)
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
                TrieNode? node = current?.Edges.FirstOrDefault(edge => edge.Label == c);
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