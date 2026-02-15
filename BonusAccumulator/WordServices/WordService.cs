using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using WordServices.Extensions;
using WordServices.Output;
using WordServices.TrieSearching;

namespace WordServices;

public class WordService(ITrieSearcher searcher, ISessionState sessionState, IWordOutputService wordOutputService)
    : IWordService
{
    private static readonly Random Random = Random.Shared;

    private readonly HashSet<string> _unasked = [];

    public Answer Anagram(string question)
    {
        IList<string> words = searcher.Query(question,
            wordsAtTerminal => wordsAtTerminal.Where(x => x.Length == question.Length));

        sessionState.Update(words);
        
        return new Answer
        {
            Words = new ReadOnlyCollection<string>(words)
        };
    }

    private Answer AnagramInternal(string question)
    {
        IList<string> words = searcher.Query(question,
            wordsAtTerminal => wordsAtTerminal.Where(x => x.Length == question.Length));
        
        return new Answer
        {
            Words = new ReadOnlyCollection<string>(words)
        };
    }

    public Answer Build(string question)
    {
        IList<string> words = searcher.Query(question, x => x);
        
        sessionState.Update(words);

        return new Answer
        {
            Words = new ReadOnlyCollection<string>(words)
        };
    }

    private Answer PatternInternal(string question)
    {
        Func<IEnumerable<string>, IEnumerable<string>> wordFilter =
            filter => filter.Where(x => x.Length == question.Length && Regex.IsMatch(x, question.ToUpper()));
        
        IList<string> words = searcher.Query(question, wordFilter);
        
        return new Answer
        {
            Words = new ReadOnlyCollection<string>(words)
        };
    }

    public Answer Pattern(string question)
    {
        Func<IEnumerable<string>, IEnumerable<string>> wordFilter =
            filter => filter.Where(x => x.Length == question.Length && Regex.IsMatch(x, question.ToUpper()));
        
        IList<string> words = searcher.Query(question, wordFilter);
        
        sessionState.Update(words);

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
        
        sessionState.Update(answer.Words);
        
        return answer;
    }

    public Answer AlphagramDistance(string? question)
    {
        if (question == null)
            return new Answer();

        Answer answer = RunWildCardCharacterQuestion(question.ToAlphagram, AnagramInternal);
        
        sessionState.Update(answer.Words);
        
        return answer;
    }

    public void AddWords(string[] words, Action<string> write)
    {
        List<string> filtered = words.Where(x => Pattern(x).Words.Count > 0).ToList();
        if (filtered.Count == 0)
            write("No valid words to add");
        else
            sessionState.Add(filtered);
    }
    
    public void AddLastWords(Action<string> write)
    {
        int wordsAdded = sessionState.LastResult.Count;
        if (wordsAdded == 0)
        {
            write("No words to add from last result.");
            return;
        }
        
        write($"Added {wordsAdded} words from last result:");
        write(wordOutputService.FormatWords(sessionState.LastResult));
        
        sessionState.AddedWords.UnionWith(sessionState.LastResult);
        sessionState.LastResult.Clear();
    }

    public string StoreAndClearAdded()
    {
        string filePath = sessionState.SaveAdded();
        sessionState.AddedWords.Clear();
        return filePath;
    }

    public void RunQuiz(
        QuizOptions options,
        string endQuizSessionCommand,
        Action<string> write,
        Func<string?> read)
    {
        HashSet<string> storedWords = options == QuizOptions.Session ? sessionState.SessionWords : sessionState.AddedWords;

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
            write($"Answers: {wordOutputService.FormatWords(sessionQuiz.Words)}");
            write(string.Empty);

            _unasked.Remove(answer);
        }

        write("Quiz over");
    }

    public void RunChainQuiz(string endCommand, Action<string> write, Func<string?> read)
    {
        write($"Chain Quiz Started - Type words to find chains, or '{endCommand}' to exit, or 'help' for help:");
        
        string? command = null;
        string? previousWord = null;
        
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
                continue;
            }
            
            if (previousWord != null && IsNewChain(command, previousWord))
            {
                write($"🔗 New chain starting from: {command.ToUpper()}");
            }
            else if (previousWord == null)
            {
                write($"🔗 Starting chain from: {command.ToUpper()}");
            }
            
            previousWord = command;
            
            Answer chainAnswer = AnagramInternal(command);
            bool noResults = chainAnswer.Words.Count == 0;
            if (noResults)
            {
                write($"'{command.ToUpper()}' is not a valid word or has no anagrams");
            }
            else
            {
                write(wordOutputService.FormatWords(chainAnswer.Words) + " - " +
                      string.Join(",", chainAnswer.Words.Select(x => x.ToAlphagram())));
            }
            if (noResults is false)
            {
                string currentCommand = command;
                Answer chainAlphagramDistanceAnswers = AlphagramDistance(currentCommand);
                string next = string.Join(" , ", chainAlphagramDistanceAnswers.Words.Select(x => x.ToAlphagram())
                    .Distinct().Select(x =>
                    {
                        foreach (char c in x)
                        {
                            if (currentCommand.Contains(c, StringComparison.OrdinalIgnoreCase) is false)
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
        writeLine("Enter alphagrams (one per line, empty line to finish):");
        
        List<string> allLines = [];

        while (readLine() is { } line && line.Trim() != string.Empty)
        {
            allLines.Add(line.Trim());
        }
        
        if (allLines.Count == 0)
        {
            writeLine("No input provided.");
            return;
        }
        List<string> validWords = [];
        List<string> alphagramInputs = [];
        
        foreach (string line in allLines.Where(line => line.Length > 0))
        {
            Answer anagramResult = AnagramInternal(line);
            if (anagramResult.Words.Count > 0)
            {
                validWords.Add(line.ToUpper());
                validWords.AddRange(anagramResult.Words);
            }
            else
            {
                alphagramInputs.Add(line);
            }
        }
        
        List<string> allValidWords = validWords.Distinct().ToList();
        List<string> allAlphagrams = alphagramInputs.Select(x => x.ToAlphagram()).Distinct().ToList();
        
        List<string> combinedItems = allValidWords.Concat(allAlphagrams).ToList();
        
        ILookup<string, string> lookup = combinedItems
            .ToLookup(x => x.ToAlphagram(), x => x);
            
        List<IGrouping<string, string>> sortedGroups = lookup
            .OrderByDescending(g => g.Key.Length)
            .ThenBy(g => g.Key)
            .ToList();
            
        foreach (IGrouping<string, string> group in sortedGroups)
        {
            writeLine($"{group.Key}-{group.Count()}");
        }

        writeLine("Answers");
        foreach (IGrouping<string, string> group in sortedGroups)
        {
            writeLine(group.Key + "- " + string.Join(", ", group.OrderBy(x => x)));
        }
    }

    private bool IsNewChain(string currentWord, string previousWord)
    {
        string previousAlphagram = previousWord.ToAlphagram();
        string currentAlphagram = currentWord.ToAlphagram();
        
        if (string.Equals(previousAlphagram, currentAlphagram, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }
        
        int previousLength = previousAlphagram.Length;
        int currentLength = currentAlphagram.Length;
        int lengthDifference = Math.Abs(previousLength - currentLength);
        
        if (lengthDifference > 1)
        {
            return true;
        }
        
        if (previousLength == currentLength)
        {
            return previousAlphagram.Except(currentAlphagram).Count() != 1;
        }
        
        string shorter = previousLength < currentLength ? previousAlphagram : currentAlphagram;
        string longer = previousLength < currentLength ? currentAlphagram : previousAlphagram;
        
        int shortIndex = 0, longIndex = 0, editCount = 0;
        while (shortIndex < shorter.Length && longIndex < longer.Length)
        {
            if (shorter[shortIndex] == longer[longIndex])
            {
                shortIndex++;
                longIndex++;
            }
            else
            {
                editCount++;
                if (editCount > 1)
                {
                    return true;
                }
                longIndex++;
            }
        }
        
        return editCount + (longer.Length - longIndex) != 1;
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
