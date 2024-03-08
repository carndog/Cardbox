// See https://aka.ms/new-console-template for more information

using BonusAccumulator.WordServices;
using BonusAccumulator.WordServices.Factories;
using BonusAccumulator.WordServices.Helpers;
using static System.Console;

WordService wordService = WordServiceFactory.Create();

string? command = "";

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
const string AddLastWordsCommand = "addlast";
const string StoreAndClearAdded = "sca";
const string ChainsCommand = "ch";
const string EndChainsCommand = "xch";
const string HelpCommand = "help";
const string CommandsText = $"Commands: Anagram: {AnagramCommand}, Build: {BuildCommand}, Pattern: {PatternCommand}, " +
                            $"Distance: {DistanceCommand}, Alphagram Distance: {AlphagramDistanceCommand}, " +
                            $"Quiz Session: {QuizSessionCommand}, Quiz Words: {QuizWordsCommand}, End Quiz: {EndQuizSessionCommand}, " +
                            $"Add Words: {AddWordCommand}, Add Last Words: {AddLastWordsCommand} " +
                            $"Store and clear added: {StoreAndClearAdded}, " +
                            $"End Exit: {ExitCommand}:";

Write(CommandsText);

while (command == null || !command.Equals(ExitCommand, StringComparison.CurrentCultureIgnoreCase))
{
    command = ReadLine();

    if (command == null)
        continue;

    switch (command.ToLower())
    {
        case ExitCommand:
            break;
        case HelpCommand:
            Write(CommandsText);
            break;
        case AnagramCommand:
            command = ReadLine();
            Answer anagram = wordService.Anagram(command);
            WriteLine(string.Join(",", anagram.Words));
            break;
        case BuildCommand:
            command = ReadLine();
            Answer build = wordService.Build(command);
            WriteLine(string.Join(",", build.Words));
            break;
        case PatternCommand:
            command = ReadLine();
            Answer pattern = wordService.Pattern(command);
            WriteLine(string.Join(",", pattern.Words));
            break;
        case DistanceCommand:
            command = ReadLine();
            Answer distance = wordService.Distance(command);
            WriteLine(string.Join(",", distance.Words));
            break;
        case AlphagramDistanceCommand:
            command = ReadLine();
            Answer alphagramDistance = wordService.AlphagramDistance(command);
            WriteLine(string.Join(",", alphagramDistance.Words));
            break;
        case QuizSessionCommand:
            wordService.RunQuiz(WordService.Options.Session, EndQuizSessionCommand, WriteLine, ReadLine);
            break;
        case QuizWordsCommand:
            wordService.RunQuiz(WordService.Options.Added, EndQuizSessionCommand, WriteLine, ReadLine);
            break;
        case StoreAndClearAdded:
            string name = wordService.StoreAndClearAdded();
            WriteLine($"saved at {name}");
            break;
        case AddWordCommand:
            string readLine = ReadLine() ?? string.Empty;
            string[] words = readLine.ToUpper().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            wordService.AddWords(words, WriteLine);
            break;
        case ChainsCommand:
            while (command != EndChainsCommand)
            {
                command = ReadLine();
                if(command == null)
                    continue;
                Answer chainAnswer = wordService.Anagram(command);
                bool noResults = chainAnswer.Words.Count == 0;
                WriteLine(noResults ? "No chains found" : string.Join(",", chainAnswer.Words) + " - " + string.Join(",", chainAnswer.Words.Select(x => x.ToAlphagram())));
                if (noResults is false)
                {
                    Answer chainAlphagramDistanceAnswers = wordService.AlphagramDistance(command);
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
                    WriteLine(next);
                }
            }
            break;
        case AddLastWordsCommand:
            wordService.AddLastWords();
            break;
    }
}

WriteLine("Goodbye!");