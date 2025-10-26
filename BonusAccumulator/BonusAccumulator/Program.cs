using BonusAccumulator.WordServices;
using BonusAccumulator.WordServices.Factories;
using static System.Console;

WordService wordService = WordServiceFactory.Create();

string? command = string.Empty;

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
const string AlphagramConversionCommand = "ac";
const string EndChainsCommand = "xch";
const string HelpCommand = "help";
const string CommandsText = $"""
    Available Commands:
    -------------------
    Word Search:
      {AnagramCommand}   - Anagram: Find anagrams of letters
      {BuildCommand}   - Build: Build words from letters
      {PatternCommand}   - Pattern: Find words matching a pattern
      {DistanceCommand}   - Distance: Find words by edit distance
      {AlphagramDistanceCommand}  - Alphagram Distance: Find words by alphagram distance
    
    Quiz & Practice:
      {QuizSessionCommand}  - Quiz Session: Start a quiz session
      {QuizWordsCommand}  - Quiz Words: Quiz on added words
      {EndQuizSessionCommand} - End Quiz Session
      {ChainsCommand}  - Chains: Run chain quiz
      {EndChainsCommand} - End Chains
    
    Word Management:
      {AddWordCommand} - Add Words: Add words to your list
      {AddLastWordsCommand} - Add Last Words: Add previous results
      {StoreAndClearAdded} - Store and Clear Added: Save and clear your word list
    
    Utilities:
      {AlphagramConversionCommand}  - Alphagram Conversion
      {HelpCommand} - Help: Show this menu
      {ExitCommand}   - Exit: Quit the program
    -------------------
    """;

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
        case AnagramCommand when ReadLine() is { } input:
            Answer anagram = wordService.Anagram(input);
            WriteLine(string.Join(",", anagram.Words));
            break;
        case BuildCommand when ReadLine() is { } input:
            Answer build = wordService.Build(input);
            WriteLine(string.Join(",", build.Words));
            break;
        case PatternCommand when ReadLine() is { } input:
            Answer pattern = wordService.Pattern(input);
            WriteLine(string.Join(",", pattern.Words));
            break;
        case DistanceCommand when ReadLine() is { } input:
            Answer distance = wordService.Distance(input);
            WriteLine(string.Join(",", distance.Words));
            break;
        case AlphagramDistanceCommand when ReadLine() is { } input:
            Answer alphagramDistance = wordService.AlphagramDistance(input);
            WriteLine(string.Join(",", alphagramDistance.Words));
            break;
        case QuizSessionCommand:
            wordService.RunQuiz(QuizOptions.Session, EndQuizSessionCommand, WriteLine, ReadLine);
            break;
        case QuizWordsCommand:
            wordService.RunQuiz(QuizOptions.Added, EndQuizSessionCommand, WriteLine, ReadLine);
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
            wordService.RunChainQuiz(EndChainsCommand, WriteLine, ReadLine);
            break;
        case AlphagramConversionCommand:
            WordService.ConvertToAlphagrams(WriteLine, ReadLine);
            break;
        case AddLastWordsCommand:
            wordService.AddLastWords();
            break;
    }
}

WriteLine("Goodbye!");