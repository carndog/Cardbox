// See https://aka.ms/new-console-template for more information

using BonusAccumulator.WordServices;
using BonusAccumulator.WordServices.Factories;
using BonusAccumulator.WordServices.Helpers;

WordService wordService = WordServiceFactory.Create();

string? command = "";
HashSet<string> sessionWords = new HashSet<string>();
HashSet<string> addedWords = new HashSet<string>();
Random random = new Random((int)DateTime.Now.Ticks);
string? reply = null;

const string ExitCommand = "x";
const string AnagramCommand = "a";
const string BuildCommand = "b";
const string PatternCommand = "p";
const string DistanceCommand = "d";
const string AlphagramDistanceCommand = "ad";
const string QuizSessionCommand = "qs";
const string QuizWordsCommand = "qw";
const string EndQuizSessionCommand = "eqs";
const string AddWordCommand = "add";
const string HelpCommand = "help";
const string CommandsText = $"Commands: Anagram: {AnagramCommand}, Build: {BuildCommand}, Pattern: {PatternCommand}, " +
                            $"Distance: {DistanceCommand}, Alphagram Distance: {AlphagramDistanceCommand}," +
                            $"Quiz Session: {QuizSessionCommand}, Quiz Words: {QuizWordsCommand}, End Quiz: {EndQuizSessionCommand}, " +
                            $"Add Word: {AddWordCommand}, End Exit: {ExitCommand}:   ";

Console.Write(CommandsText);

while (command == null || !command.Equals(ExitCommand, StringComparison.CurrentCultureIgnoreCase))
{
    command = Console.ReadLine();

    if (command == null)
        continue;

    switch (command.ToLower())
    {
        case ExitCommand:
            break;
        case HelpCommand:
            Console.Write(CommandsText);
            break;
        case AnagramCommand:
            command = Console.ReadLine();
            Answer anagram = wordService.Anagram(command);
            sessionWords.UnionWith(anagram.Words);
            Console.WriteLine(string.Join(",", anagram.Words));
            break;
        case BuildCommand:
            command = Console.ReadLine();
            Answer build = wordService.Build(command);
            Console.WriteLine(string.Join(",", build.Words));
            break;
        case PatternCommand:
            command = Console.ReadLine();
            Answer pattern = wordService.Pattern(command);
            sessionWords.UnionWith(pattern.Words);
            Console.WriteLine(string.Join(",", pattern.Words));
            break;
        case DistanceCommand:
            command = Console.ReadLine();
            Answer distance = wordService.Distance(command);
            sessionWords.UnionWith(distance.Words);
            Console.WriteLine(string.Join(",", distance.Words));
            break;
        case AlphagramDistanceCommand:
            command = Console.ReadLine();
            Answer alphagramDistance = wordService.AlphagramDistance(command);
            sessionWords.UnionWith(alphagramDistance.Words);
            Console.WriteLine(string.Join(",", alphagramDistance.Words));
            break;
        case QuizSessionCommand:
            if (sessionWords.Count != 0)
            {
                while (reply != EndQuizSessionCommand)
                {
                    int next = random.Next(1, sessionWords.Count);
                    string answer = sessionWords.GetNthElement(next);
                    Answer sessionQuiz = wordService.Anagram(answer);
                    Console.WriteLine($"{answer.ToAlphagram()} {sessionQuiz.Words.Count}");
                    reply = Console.ReadLine();
                    string[] answers = reply.ToUpper().Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    List<string> list = answers.Union(sessionQuiz.Words).ToList();
                    Console.WriteLine();
                    Console.WriteLine(list.Count == sessionQuiz.Words.Count ? "Correct" : "Wrong");
                    Console.WriteLine(string.Join(",", sessionQuiz.Words));
                    Console.WriteLine();
                }
                Console.WriteLine("Quiz over");
            }
            break;
        case QuizWordsCommand:
            if (addedWords.Count != 0)
            {
                reply = null;
                while (reply != EndQuizSessionCommand)
                {
                    int next = random.Next(1, addedWords.Count);
                    string answer = addedWords.GetNthElement(next);
                    Answer sessionQuiz = wordService.Anagram(answer);
                    Console.WriteLine($"{answer.ToAlphagram()} {sessionQuiz.Words.Count}");
                    reply = Console.ReadLine();
                    string[] answers = reply.ToUpper().Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    List<string> list = answers.Union(sessionQuiz.Words).ToList();
                    Console.WriteLine();
                    Console.WriteLine(list.Count == sessionQuiz.Words.Count ? "Correct" : "Wrong");
                    Console.WriteLine(string.Join(",", sessionQuiz.Words));
                    Console.WriteLine();
                }
                Console.WriteLine("Quiz over");
            }
            break;
        case AddWordCommand:
            string readLine = Console.ReadLine() ?? string.Empty;
            Answer addedWord = wordService.Pattern(readLine.ToUpper());
            switch (addedWord.Words.Count)
            {
                case 0:
                    Console.WriteLine("Added word invalid");
                    break;
                default:
                    addedWords.UnionWith(new[] { readLine.ToUpper() });
                    break;
            }
            break;
    }
}

Console.WriteLine("Goodbye!");