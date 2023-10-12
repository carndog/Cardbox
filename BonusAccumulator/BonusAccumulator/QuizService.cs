using BonusAccumulator.WordServices;
using BonusAccumulator.WordServices.Helpers;

namespace BonusAccumulator;

public class QuizService
{
    private readonly HashSet<string> _unasked = new();

    public void RunQuiz(
        HashSet<string> storedWords, 
        string endQuizSessionCommand, 
        Random random, 
        WordService wordService,
        Action<string?> write,
        Func<string?> read)
    {
        if (storedWords.Count != 0)
        {
            if (_unasked.Count == 0)
            {
                _unasked.UnionWith(storedWords);
            } 
            
            string? s = null;
            while (s != endQuizSessionCommand && _unasked.Any())
            {
                int next = random.Next(1, storedWords.Count + 1);
                string answer = storedWords.GetNthElement(next);
                Answer sessionQuiz = wordService.Anagram(answer);
                string question = answer.ToAlphagram();

                if (_unasked.Contains(answer))
                {
                    write($"{question} {sessionQuiz.Words.Count}");
                    s = read();
                    string[] answers = s.ToUpper().Split(" ", StringSplitOptions.RemoveEmptyEntries);
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
}