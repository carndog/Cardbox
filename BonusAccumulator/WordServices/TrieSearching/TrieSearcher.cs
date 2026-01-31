using WordServices.Extensions;
using WordServices.TrieLoading;
using static WordServices.Constants;

namespace WordServices.TrieSearching;

public class TrieSearcher : ITrieSearcher
{
    private const int DefaultResultsCapacity = 20;
    private readonly ILazyLoadingTrie _lazyTrie;

    public TrieSearcher(ILazyLoadingTrie lazyTrie)
    {
        _lazyTrie = lazyTrie;
    }

    public IList<string> Query(string searchTerm, Func<IEnumerable<string>, IEnumerable<string>> wordFilter)
    {
        List<string> resultsList = new(DefaultResultsCapacity);

        QueryLexicon(searchTerm, _lazyTrie.Lexicon, wordFilter, resultsList);

        return resultsList.OrderByDescending(x => x.Length).ToList();
    }

    private void QueryLexicon(string search, TrieNode? current, Func<IEnumerable<string>, IEnumerable<string>> filter, List<string> resultsList)
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
                                resultsList.AddRange(filter(node.AnagramsAtTerminal));
                            }

                            QueryLexicon(search.Remove(index, 1), node, filter, resultsList);
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
                        resultsList.AddRange(filter(node.AnagramsAtTerminal));
                    }
                    QueryLexicon(search.Remove(index, 1), node, filter, resultsList);
                }
            }
            index++;
        }
    }
}