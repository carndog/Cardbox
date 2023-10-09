// See https://aka.ms/new-console-template for more information
using BonusAccumulator.WordServices;
using BonusAccumulator.WordServices.Factories;
using BonusAccumulator.WordServices.TrieSearching;

WordService wordService = WordServiceFactory.Create();

string command = "";

const string ExitCommand = "x";
const string AnagramCommand = "a";
const string BuildCommand = "b";
const string PatternCommand = "p";
const string DistanceCommand = "d";
const string CommandsText = $"Commands: Anagram: {AnagramCommand}, Build: {BuildCommand}, Pattern: {PatternCommand}, Distance: {DistanceCommand}, Exit: {ExitCommand}:   ";


while (!command.Equals(ExitCommand, StringComparison.CurrentCultureIgnoreCase))
{
    Console.Write(CommandsText);
    command = Console.ReadLine();

    switch (command.ToLower())
    {
        case ExitCommand:
            break;
        case AnagramCommand:
            Console.Write($"Enter the characters: ");
            command = Console.ReadLine();
            Answer anagram = wordService.Anagram(command);
            Console.WriteLine(string.Join(",", anagram.Words));
            break;
        case BuildCommand:
            Console.Write($"Enter the characters: ");
            command = Console.ReadLine();
            Answer build = wordService.Build(command);
            Console.WriteLine(string.Join(",", build.Words));
            break;
        case PatternCommand:
            Console.Write($"Enter the characters: ");
            command = Console.ReadLine();
            Answer pattern = wordService.Pattern(command);
            Console.WriteLine(string.Join(",", pattern.Words));
            break;
        case DistanceCommand:
            Console.Write($"Enter the characters: ");
            command = Console.ReadLine();
            Answer distance = wordService.Distance(command);
            Console.WriteLine(string.Join(",", distance.Words));
            break;
    }
}

Console.WriteLine("Goodbye!");