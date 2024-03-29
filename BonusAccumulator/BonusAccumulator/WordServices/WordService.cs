﻿using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using BonusAccumulator.WordServices.Extensions;
using BonusAccumulator.WordServices.TrieSearching;

namespace BonusAccumulator.WordServices;

public class WordService
{
    public enum Options 
    {
        Session,
        Added
    }
    
    private static readonly Random Random = new((int)DateTime.Now.Ticks);
    
    private readonly ITrieSearcher _searcher;
    
    private readonly ISessionState _sessionState;

    private readonly HashSet<string> _unasked = new();

    public WordService(ITrieSearcher searcher, ISessionState sessionState)
    {
        _searcher = searcher;
        _sessionState = sessionState;
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

        Answer answer = RunWildCardCharacterQuestion(() => question, Pattern);
        
        _sessionState.Update(answer.Words);
        
        return answer;
    }

    public Answer AlphagramDistance(string? question)
    {
        if (question == null)
            return new Answer();

        Answer answer = RunWildCardCharacterQuestion(question.ToAlphagram, Anagram);
        
        _sessionState.Update(answer.Words);
        
        return answer;
    }

    public void AddWords(string[] words, Action<string?> write)
    {
        List<string> filtered = words.Where(x => Pattern(x).Words.Count > 0).ToList();
        if (filtered.Count == 0)
            write("No valid words to add");
        else
            _sessionState.Add(filtered);
    }
    
    public void AddLastWords()
    {
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
        Options options,
        string endQuizSessionCommand,
        Action<string?> write,
        Func<string?> read)
    {
        HashSet<string> storedWords = options == Options.Session ? _sessionState.SessionWords : _sessionState.AddedWords;

        if (storedWords.Count != 0)
        {
            if (_unasked.Count == 0)
            {
                _unasked.UnionWith(storedWords);
            }

            string? s = null;
            while (s != endQuizSessionCommand && _unasked.Any())
            {
                int next = Random.Next(1, storedWords.Count + 1);
                string answer = storedWords.GetNthElement(next);
                Answer sessionQuiz = Anagram(answer);
                string question = answer.ToAlphagram();

                if (_unasked.Contains(answer))
                {
                    write($"{question} {sessionQuiz.Words.Count}");
                    string[] answers = read()?.ToUpper().Split(" ", StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
                    List<string> list = answers.Union(sessionQuiz.Words).ToList();
                    write(string.Empty);
                    write(list.Count == sessionQuiz.Words.Count ? "Correct" : "Wrong");
                    write(string.Join(",", sessionQuiz.Words));
                    write(string.Empty);
                    _unasked.Remove(answer);
                }
            }

            Console.WriteLine("Quiz over");
        }
    }

    public void RunChainQuiz(string endCommand, Action<string?> write, Func<string?> read)
    {
        string? command = null;
        while (command != endCommand)
        {
            command = read();
            if (command == null)
                continue;
            Answer chainAnswer = Anagram(command);
            bool noResults = chainAnswer.Words.Count == 0;
            write(noResults
                ? "No chains found"
                : string.Join(",", chainAnswer.Words) + " - " +
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

    public static void ConvertToAlphagrams(Action<string> writeLine, Func<string?> readLine)
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