using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using BonusAccumulator.WordServices.TrieSearching;

namespace BonusAccumulator.WordServices;

public class WordService
{
    private readonly TrieSearcher _searcher;

    public WordService(TrieSearcher searcher)
    {
        _searcher = searcher;
    }

    public Answer Anagram(string question)
    {
        IList<string> words = _searcher.Query(question,
            wordsAtTerminal => wordsAtTerminal.Where(x => x.Length == question.Length));

        return new Answer
        {
            Words = new ReadOnlyCollection<string>(words)
        };
    }

    public Answer Build(string question)
    {
        IList<string> words = _searcher.Query(question, x => x);

        return new Answer
        {
            Words = new ReadOnlyCollection<string>(words)
        };
    }

    public Answer Pattern(string question)
    {
        Func<IEnumerable<string>, IEnumerable<string>> wordFilter =
            filter => filter.Where(x => x.Length == question.Length && new Regex(question.ToUpper()).IsMatch(x));
        
        IList<string> words = _searcher.Query(question, wordFilter);

        return new Answer
        {
            Words = new ReadOnlyCollection<string>(words)
        };
    }
}