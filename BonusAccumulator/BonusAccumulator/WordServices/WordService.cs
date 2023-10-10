using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using BonusAccumulator.WordServices.Helpers;
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
            filter => filter.Where(x => x.Length == question.Length && Regex.IsMatch(x, question.ToUpper()));
        
        IList<string> words = _searcher.Query(question, wordFilter);

        return new Answer
        {
            Words = new ReadOnlyCollection<string>(words)
        };
    }

    public Answer Distance(string? question)
    {
        if (question == null)
            return new Answer();

        return RunWildCardCharacterQuestion(() => question, Pattern);
    }
    
    public Answer AlphagramDistance(string? question)
    {
        if (question == null)
            return new Answer();

        return RunWildCardCharacterQuestion(question.ToAlphagram, Anagram);
    }

    private Answer RunWildCardCharacterQuestion(Func<string> getQuestion, Func<string, Answer> getAnswer)
    {
        HashSet<string> words = new HashSet<string>();

        foreach (string modifiedQuestion in getQuestion().ToWildCardCharacterQuestions())
        {
            Answer anagram = getAnswer(modifiedQuestion);
            words.UnionWith(anagram.Words);
        }

        return new Answer
        {
            Words = words.ToList()
        };
    }
}