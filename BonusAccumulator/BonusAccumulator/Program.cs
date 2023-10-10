// See https://aka.ms/new-console-template for more information
using BonusAccumulator.WordServices;
using BonusAccumulator.WordServices.Factories;

WordService wordService = WordServiceFactory.Create();

string? command = "";

const string ExitCommand = "x";
const string AnagramCommand = "a";
const string BuildCommand = "b";
const string PatternCommand = "p";
const string DistanceCommand = "d";
const string AlphagramDistanceCommand = "ad";
const string HelpCommand = "help";
const string CommandsText = $"Commands: Anagram: {AnagramCommand}, Build: {BuildCommand}, Pattern: {PatternCommand}, " +
                            $"Distance: {DistanceCommand}, Alphagram Distance: {AlphagramDistanceCommand}, Exit: {ExitCommand}:   ";

Console.Write(CommandsText);

while (command == null || !command.Equals(ExitCommand, StringComparison.CurrentCultureIgnoreCase))
{
    command = Console.ReadLine();
    
    if(command == null) 
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
            Console.WriteLine(string.Join(",", pattern.Words));
            break;
        case DistanceCommand:
            command = Console.ReadLine();
            Answer distance = wordService.Distance(command);
            Console.WriteLine(string.Join(",", distance.Words));
            break;
        case AlphagramDistanceCommand:
            command = Console.ReadLine();
            Answer alphagramDistance = wordService.AlphagramDistance(command);
            Console.WriteLine(string.Join(",", alphagramDistance.Words));
            break;
    }
}

Console.WriteLine("Goodbye!");