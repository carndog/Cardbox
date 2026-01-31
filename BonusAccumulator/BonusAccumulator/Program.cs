using BonusAccumulator.WordServices;
using BonusAccumulator.WordServices.Factories;
using BonusAccumulator.WordServices.Output;
using static System.Console;

WordService wordService = WordServiceFactory.Create();
IWordOutputService wordOutputService = WordOutputServiceFactory.Create();

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
        case AnagramCommand:
            Write("Enter letters for anagram: ");
            string? anagramInput = ReadLine();
            if (anagramInput != null)
            {
                Answer anagram = wordService.Anagram(anagramInput);
                WriteLine(wordOutputService.FormatWords(anagram.Words));
            }
            break;
        case BuildCommand:
            Write("Enter letters to build from: ");
            string? buildInput = ReadLine();
            if (buildInput != null)
            {
                Answer build = wordService.Build(buildInput);
                WriteLine(wordOutputService.FormatWords(build.Words));
            }
            break;
        case PatternCommand:
            Write("Enter pattern: ");
            string? patternInput = ReadLine();
            if (patternInput != null)
            {
                Answer pattern = wordService.Pattern(patternInput);
                WriteLine(wordOutputService.FormatWords(pattern.Words));
            }
            break;
        case DistanceCommand:
            Write("Enter word for distance search: ");
            string? distanceInput = ReadLine();
            if (distanceInput != null)
            {
                Answer distance = wordService.Distance(distanceInput);
                WriteLine(wordOutputService.FormatWords(distance.Words));
            }
            break;
        case AlphagramDistanceCommand:
            Write("Enter word for alphagram distance: ");
            string? alphagramDistanceInput = ReadLine();
            if (alphagramDistanceInput != null)
            {
                Answer alphagramDistance = wordService.AlphagramDistance(alphagramDistanceInput);
                WriteLine(wordOutputService.FormatWords(alphagramDistance.Words));
            }
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
            Write("Enter words to add (space separated): ");
            string? addWordsInput = ReadLine();
            if (addWordsInput != null)
            {
                string[] words = addWordsInput.ToUpper().Split(" ", StringSplitOptions.RemoveEmptyEntries);
                wordService.AddWords(words, WriteLine);
            }
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