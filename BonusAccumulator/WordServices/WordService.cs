using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using WordServices.Extensions;
using WordServices.Output;
using WordServices.TrieSearching;

namespace WordServices;

public class WordService : IWordService
{
    private static readonly Random Random = Random.Shared;
    
    private readonly ITrieSearcher _searcher;
    
    private readonly ISessionState _sessionState;

    private readonly IWordOutputService _wordOutputService;

    private readonly HashSet<string> _unasked = [];

    public WordService(ITrieSearcher searcher, ISessionState sessionState, IWordOutputService wordOutputService)
    {
        _searcher = searcher;
        _sessionState = sessionState;
        _wordOutputService = wordOutputService;
    }

    public Answer Anagram(string question)
    {
        IList<string> words = _searcher.Query(question,
            wordsAtTerminal => wordsAtTerminal.Where(x => x.Length == question.Length));

        _sessionState.Update(words);
        
        return new Answer
        {
            Words = new ReadOnlyCollection<string>(words)
        };
    }

    private Answer AnagramInternal(string question)
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
        
        _sessionState.Update(words);

        return new Answer
        {
            Words = new ReadOnlyCollection<string>(words)
        };
    }

    private Answer PatternInternal(string question)
    {
        Func<IEnumerable<string>, IEnumerable<string>> wordFilter =
            filter => filter.Where(x => x.Length == question.Length && Regex.IsMatch(x, question.ToUpper()));
        
        IList<string> words = _searcher.Query(question, wordFilter);
        
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
        
        _sessionState.Update(words);

        return new Answer
        {
            Words = new ReadOnlyCollection<string>(words)
        };
    }

    public Answer Distance(string? question)
    {
        if (question == null)
            return new Answer();

        Answer answer = RunWildCardCharacterQuestion(() => question, PatternInternal);
        
        _sessionState.Update(answer.Words);
        
        return answer;
    }

    public Answer AlphagramDistance(string? question)
    {
        if (question == null)
            return new Answer();

        Answer answer = RunWildCardCharacterQuestion(question.ToAlphagram, AnagramInternal);
        
        _sessionState.Update(answer.Words);
        
        return answer;
    }

    public void AddWords(string[] words, Action<string> write)
    {
        List<string> filtered = words.Where(x => Pattern(x).Words.Count > 0).ToList();
        if (filtered.Count == 0)
            write("No valid words to add");
        else
            _sessionState.Add(filtered);
    }
    
    public void AddLastWords(Action<string> write)
    {
        int wordsAdded = _sessionState.LastResult.Count;
        if (wordsAdded == 0)
        {
            write("No words to add from last result.");
            return;
        }
        
        write($"Added {wordsAdded} words from last result:");
        write(_wordOutputService.FormatWords(_sessionState.LastResult));
        
        _sessionState.AddedWords.UnionWith(_sessionState.LastResult);
        _sessionState.LastResult.Clear();
    }

    public string StoreAndClearAdded()
    {
        string filePath = _sessionState.SaveAdded();
        _sessionState.AddedWords.Clear();
        return filePath;
    }

    public void RunQuiz(
        QuizOptions options,
        string endQuizSessionCommand,
        Action<string> write,
        Func<string?> read)
    {
        HashSet<string> storedWords = options == QuizOptions.Session ? _sessionState.SessionWords : _sessionState.AddedWords;

        if (storedWords.Count == 0)
        {
            write("No words available for quiz");
            return;
        }

        _unasked.Clear();
        _unasked.UnionWith(storedWords);

        foreach (string answer in _unasked.Shuffled(Random))
        {
            Answer sessionQuiz = AnagramInternal(answer);
            string question = answer.ToAlphagram();

            write($"Question: {question} ({sessionQuiz.Words.Count} answers)");
            write($"Type your answers separated by spaces, or '{endQuizSessionCommand}' to exit quiz, or 'help' for help:");

            string? input = read()?.Trim();
            
            if (input?.Equals(endQuizSessionCommand, StringComparison.OrdinalIgnoreCase) == true)
            {
                write("Quiz ended by user.");
                break;
            }
            
            if (input?.Equals("help", StringComparison.OrdinalIgnoreCase) == true)
            {
                write($"Quiz Help:");
                write($"  - Type all answers separated by spaces (e.g., 'CAT ACT')");
                write($"  - Type '{endQuizSessionCommand}' to exit the quiz");
                write($"  - Current question: {question} ({sessionQuiz.Words.Count} answers)");
                write("Try again or type a command:");
                continue;
            }

            string[] answers = input?.ToUpper()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? [];

            string[] typed = answers.Distinct().ToArray();
            
            bool isCorrect = typed.Length == sessionQuiz.Words.Count
                           && typed.All(a => sessionQuiz.Words.Contains(a));

            write(string.Empty);
            write(isCorrect ? "✓ Correct!" : "✗ Wrong");
            write($"Answers: {_wordOutputService.FormatWords(sessionQuiz.Words)}");
            write(string.Empty);

            _unasked.Remove(answer);
        }

        write("Quiz over");
    }

    public void RunChainQuiz(string endCommand, Action<string?> write, Func<string?> read)
    {
        write($"Chain Quiz Started - Type words to find chains, or '{endCommand}' to exit, or 'help' for help:");
        
        string? command = null;
        while (command != endCommand)
        {
            write("Enter a word (or command):");
            command = read()?.Trim();
            
            if (command?.Equals(endCommand, StringComparison.OrdinalIgnoreCase) == true)
            {
                write("Chain quiz ended.");
                break;
            }
            
            if (command?.Equals("help", StringComparison.OrdinalIgnoreCase) == true)
            {
                write("Chain Quiz Help:");
                write("  - Type any word to find its anagram chains");
                write($"  - Type '{endCommand}' to exit the quiz");
                write("  - The quiz will show all words that can be made from your letters");
                write("  - Then shows possible next words using unused letters");
                continue;
            }
            
            if (string.IsNullOrEmpty(command))
            {
                write("Please enter a word or command.");
                continue;
            }
            
            Answer chainAnswer = AnagramInternal(command);
            bool noResults = chainAnswer.Words.Count == 0;
            write(noResults
                ? "No chains found"
                : _wordOutputService.FormatWords(chainAnswer.Words) + " - " +
                  string.Join(",", chainAnswer.Words.Select(x => x.ToAlphagram())));
            if (noResults is false)
            {
                Answer chainAlphagramDistanceAnswers = AlphagramDistance(command);
                string next = string.Join(" , ", chainAlphagramDistanceAnswers.Words.Select(x => x.ToAlphagram())
                    .Distinct().Select(x =>
                    {
                        foreach (char c in x)
                        {
                            if (command.Contains(c, StringComparison.OrdinalIgnoreCase) is false)
                            {
                                return c.ToString();
                            }
                        }

                        return string.Empty;
                    }).Where(x => string.IsNullOrEmpty(x) is false).OrderBy(x => x).Distinct());
                write(next);
            }
        }
    }

    public void ConvertToAlphagrams(Action<string> writeLine, Func<string?> readLine)
    {
        string? line = readLine();
        if (line != null)
        {
            ILookup<string, string> lookup = line.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .ToLookup(x => x.ToAlphagram(), x => x);
            foreach (IGrouping<string, string> group in lookup)
            {
                writeLine(group.Count() != 1 ? $"{group.Key}-{group.Count()}" : $"{group.Key}");
            }

            writeLine("Answers");
            foreach (IGrouping<string, string> group in lookup)
            {
                writeLine(group.Key + "- " + string.Join(", ", group));
            }
        }
    }

    private Answer RunWildCardCharacterQuestion(Func<string> getQuestion, Func<string, Answer> getAnswer)
    {
        HashSet<string> words = [];

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
