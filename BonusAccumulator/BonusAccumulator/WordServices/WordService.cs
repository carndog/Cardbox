using System.Collections.ObjectModel;
using System.Text;
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

    public Answer Distance(string question)
    {
        const char wildcard = '.';

        StringBuilder modifiedQuestion = new StringBuilder(question.ToUpper());
        HashSet<string> words = new HashSet<string>();

        for (int i = 0; i < question.Length; i++)
        {
            char c = question[i];
            modifiedQuestion[i] = wildcard;
            Answer pattern = Pattern(modifiedQuestion.ToString());
            modifiedQuestion[i] = c;
            words.UnionWith(pattern.Words);
        }

        return new Answer
        {
            Words = words.ToList()
        };
    }
}