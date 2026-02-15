using BonusAccumulator;
using WordServices;
using WordServices.Analytics;
using WordServices.Output;
using CardboxDataLayer;
using CardboxDataLayer.Entities;
using CardboxDataLayer.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static System.Console;

IHost host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddWordServices();
        services.AddCardboxDataLayer(context.Configuration);
    })
    .Build();

IWordService wordService = host.Services.GetRequiredService<IWordService>();
IWordOutputService wordOutputService = host.Services.GetRequiredService<IWordOutputService>();
IQuestionRepository questionRepository = host.Services.GetRequiredService<IQuestionRepository>();
IAnalyticsService analyticsService = host.Services.GetRequiredService<IAnalyticsService>();

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
const string CardboxAnalysisCommand = "ca";
const string AnalyticsCommand = "an";
const string AnalyticsShowCommand = "an show";
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
      {QuizSessionCommand}  - Quiz Session: Start a quiz session (type 'eqs' or 'help' during quiz)
      {QuizWordsCommand}  - Quiz Words: Quiz on added words (type 'eqs' or 'help' during quiz)
      {EndQuizSessionCommand} - End Quiz Session
      {ChainsCommand}  - Chains: Run chain quiz (type 'xch' to exit)
      {EndChainsCommand} - End Chains (used during chains quiz)
    
    Word Management:
      {AddWordCommand} - Add Words: Add words to your list
      {AddLastWordsCommand} - Add Last Words: Add previous results
      {StoreAndClearAdded} - Store and Clear Added: Save and clear your word list
    
    Cardbox Analysis:
      {CardboxAnalysisCommand} - Cardbox Analysis: Analyze cardbox data
      {AnalyticsCommand} - Analytics: Advanced analytics queries
      {AnalyticsShowCommand} - Analytics with Anagrams: Show analytics with anagram solutions
    
    Utilities:
      {AlphagramConversionCommand}  - Alphagram Conversion: Convert words to alphagrams
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
            wordService.ConvertToAlphagrams(WriteLine, ReadLine);
            break;
        case AddLastWordsCommand:
            wordService.AddLastWords(WriteLine);
            break;
        case CardboxAnalysisCommand:
            await RunCardboxAnalysis(questionRepository);
            break;
        case AnalyticsCommand:
            await RunAnalytics(analyticsService, false);
            break;
        case AnalyticsShowCommand:
            await RunAnalytics(analyticsService, true);
            break;
    }
}

async Task RunAnalytics(IAnalyticsService analytics, bool showAnagrams)
{
    WriteLine("Cardbox Analytics Options:");
    WriteLine("");
    WriteLine("Word-based Queries (shows specific words needing attention):");
    WriteLine("1. Due Now - Words scheduled for review right now");
    WriteLine("2. Priority Items - Words calculated as high-priority based on error rate and cardbox");
    WriteLine("3. Highest Error Rate - Words you get wrong most often (by percentage)");
    WriteLine("4. Most Wrong - Words with the most incorrect answer attempts");
    WriteLine("5. Pain per Recent Memory - Words causing trouble relative to how recently learned");
    WriteLine("6. Regressions - Words that dropped to a lower cardbox (moved backward)");
    WriteLine("7. Not Seen for Ages - Words overdue for review by the longest time");
    WriteLine("");
    WriteLine("Stats-based Queries (shows aggregate statistics):");
    WriteLine("8. Deck Stats by Cardbox - Breakdown of items and accuracy per cardbox level");
    WriteLine("9. Deck Stats by Word Length - Breakdown of items and accuracy by word length");
    WriteLine("10. Interval Stats - Average learning intervals for each cardbox level");
    WriteLine("11. Forgetting Curve Stats - Accuracy trends over time since last review");
    WriteLine("12. Blind Spots - Difficulty/length combinations with lowest accuracy (30+ reviews)");
    WriteLine("Enter your choice (1-12): ");
    
    string? choice = ReadLine();
    if (choice == null) return;
    
    switch (choice)
    {
        case "1":
            IReadOnlyList<DueItem> dueNow = (await ((IGetDueNow)analytics).ExecuteAsync()).ToList();
            WriteLine($"Questions due now: {dueNow.Count}");
            foreach (DueItem item in dueNow.Take(20))
            {
                DisplayWordWithAnagrams(item.Question, $"Cardbox: {item.Cardbox}, Difficulty: {item.Difficulty}, Due: {item.DueAt:yyyy-MM-dd HH:mm}", showAnagrams);
            }
            if (dueNow.Count > 20)
            {
                WriteLine($"  ... and {dueNow.Count - 20} more");
            }
            PromptQuizOnWords(dueNow.Select(w => w.Question).ToList());
            break;
            
        case "2":
            IReadOnlyList<PriorityItem> priorityItems = (await ((IGetPriorityItems)analytics).ExecuteAsync()).ToList();
            WriteLine($"Priority items: {priorityItems.Count}");
            foreach (PriorityItem item in priorityItems.Take(20))
            {
                DisplayWordWithAnagrams(item.Question, $"Priority: {item.Priority:F2}, Cardbox: {item.Cardbox}, Difficulty: {item.Difficulty}", showAnagrams);
            }
            if (priorityItems.Count > 20)
            {
                WriteLine($"  ... and {priorityItems.Count - 20} more");
            }
            PromptQuizOnWords(priorityItems.Select(w => w.Question).ToList());
            break;
            
        case "3":
            IReadOnlyList<ErrorRateStats> highestErrorRate = (await ((IGetHighestErrorRate)analytics).ExecuteAsync()).ToList();
            WriteLine($"Highest error rate questions: {highestErrorRate.Count}");
            foreach (ErrorRateStats item in highestErrorRate.Take(20))
            {
                DisplayWordWithAnagrams(item.Question, $"Error Rate: {item.ErrorRate:P2}, Attempts: {item.Attempts}, Cardbox: {item.Cardbox}", showAnagrams);
            }
            if (highestErrorRate.Count > 20)
            {
                WriteLine($"  ... and {highestErrorRate.Count - 20} more");
            }
            PromptQuizOnWords(highestErrorRate.Select(w => w.Question).ToList());
            break;
            
        case "4":
            IReadOnlyList<MostWrongStats> mostWrong = (await ((IGetMostWrong)analytics).ExecuteAsync()).ToList();
            WriteLine($"Most wrong questions: {mostWrong.Count}");
            foreach (MostWrongStats item in mostWrong.Take(20))
            {
                DisplayWordWithAnagrams(item.Question, $"Incorrect: {item.Incorrect}, Correct: {item.Correct}, Cardbox: {item.Cardbox}", showAnagrams);
            }
            if (mostWrong.Count > 20)
            {
                WriteLine($"  ... and {mostWrong.Count - 20} more");
            }
            PromptQuizOnWords(mostWrong.Select(w => w.Question).ToList());
            break;
            
        case "5":
            IReadOnlyList<PainStats> painPerRecentMemory = (await ((IGetPainPerRecentMemory)analytics).ExecuteAsync()).ToList();
            WriteLine($"Painful questions from recent memory: {painPerRecentMemory.Count}");
            foreach (PainStats item in painPerRecentMemory.Take(20))
            {
                DisplayWordWithAnagrams(item.Question, $"Incorrect: {item.Incorrect}, Correct: {item.Correct}, Cardbox: {item.Cardbox}", showAnagrams);
            }
            if (painPerRecentMemory.Count > 20)
            {
                WriteLine($"  ... and {painPerRecentMemory.Count - 20} more");
            }
            PromptQuizOnWords(painPerRecentMemory.Select(w => w.Question).ToList());
            break;
            
        case "6":
            IReadOnlyList<RegressionStats> regressions = (await ((IGetRegressions)analytics).ExecuteAsync()).ToList();
            WriteLine($"Regressed questions: {regressions.Count}");
            foreach (RegressionStats item in regressions.Take(20))
            {
                DisplayWordWithAnagrams(item.Question, $"Last Correct: {item.LastCorrectAt:yyyy-MM-dd}, Cardbox: {item.Cardbox}", showAnagrams);
            }
            if (regressions.Count > 20)
            {
                WriteLine($"  ... and {regressions.Count - 20} more");
            }
            PromptQuizOnWords(regressions.Select(w => w.Question).ToList());
            break;
            
        case "7":
            IReadOnlyList<NotSeenForAgesStats> notSeenForAges = (await ((IGetNotSeenForAges)analytics).ExecuteAsync()).ToList();
            WriteLine($"Questions not seen for ages: {notSeenForAges.Count}");
            foreach (NotSeenForAgesStats item in notSeenForAges.Take(20))
            {
                DisplayWordWithAnagrams(item.Question, $"Days since last correct: {item.DaysSinceLastCorrect:F0}, Cardbox: {item.Cardbox}", showAnagrams);
            }
            if (notSeenForAges.Count > 20)
            {
                WriteLine($"  ... and {notSeenForAges.Count - 20} more");
            }
            PromptQuizOnWords(notSeenForAges.Select(w => w.Question).ToList());
            break;
            
        case "8":
            IReadOnlyList<CardboxStats> deckStatsByCardbox = (await ((IGetDeckStatsByCardbox)analytics).ExecuteAsync()).ToList();
            WriteLine($"Deck stats by cardbox:");
            foreach (CardboxStats stat in deckStatsByCardbox)
            {
                WriteLine($"  Cardbox {stat.Cardbox}: {stat.Items} items, {stat.PercentCorrect:P1} correct");
            }
            break;
            
        case "9":
            IReadOnlyList<WordLengthStats> deckStatsByWordLength = (await ((IGetDeckStatsByWordLength)analytics).ExecuteAsync()).ToList();
            WriteLine($"Deck stats by word length:");
            foreach (WordLengthStats stat in deckStatsByWordLength.OrderBy(s => s.Length))
            {
                WriteLine($"  Length {stat.Length}: {stat.Items} items, {stat.PercentCorrect:P1} correct");
            }
            break;
            
        case "10":
            IReadOnlyList<IntervalStats> intervalStats = (await ((IGetIntervalStats)analytics).ExecuteAsync()).ToList();
            WriteLine($"Learning interval statistics:");
            foreach (IntervalStats stat in intervalStats)
            {
                WriteLine($"  Cardbox {stat.Cardbox}: {stat.Items} items, Avg Interval: {stat.AverageIntervalDays:F1} days");
            }
            break;
            
        case "11":
            IReadOnlyList<ForgettingCurveStats> forgettingCurveStats = (await ((IGetForgettingCurveStats)analytics).ExecuteAsync()).ToList();
            WriteLine($"Forgetting curve statistics:");
            foreach (ForgettingCurveStats stat in forgettingCurveStats)
            {
                WriteLine($"  {stat.AgeBucket}: {stat.Items} items, {stat.PercentCorrect:P1} correct");
            }
            break;
            
        case "12":
            IReadOnlyList<BlindSpotStats> blindSpots = (await ((IGetBlindSpots)analytics).ExecuteAsync()).ToList();
            WriteLine($"Blind spots (areas of weakness):");
            foreach (BlindSpotStats spot in blindSpots.OrderBy(s => s.Difficulty).ThenBy(s => s.Length))
            {
                WriteLine($"  Difficulty {spot.Difficulty}, Length {spot.Length}: {spot.Items} items, {spot.PercentCorrect:P1} correct, {spot.Reviews} reviews");
            }
            break;
            
        default:
            WriteLine("Invalid choice.");
            break;
    }
}

WriteLine("Goodbye!");

async Task RunCardboxAnalysis(IQuestionRepository repo)
{
    WriteLine("Cardbox Analysis Options:");
    WriteLine("1. Total questions count");
    WriteLine("2. Questions by cardbox");
    WriteLine("3. Average difficulty");
    WriteLine("4. Questions with incorrect answers");
    WriteLine("5. Questions by difficulty range");
    WriteLine("6. Recent additions (next_Added table)");
    WriteLine("Enter your choice (1-6): ");
    
    string? choice = ReadLine();
    if (choice == null) return;
    
    switch (choice)
    {
        case "1":
            int totalCount = await repo.GetTotalCountAsync();
            WriteLine($"Total questions: {totalCount}");
            break;
        case "2":
            WriteLine("Enter cardbox number: ");
            string? cardboxInput = ReadLine();
            if (int.TryParse(cardboxInput, out int cardbox))
            {
                IReadOnlyList<Question> questions = (await repo.GetByCardboxAsync(cardbox)).ToList();
                WriteLine($"Questions in cardbox {cardbox}: {questions.Count}");
                foreach (Question question in questions.Take(10))
                {
                    WriteLine($"  {question.QuestionText} - Correct: {question.Correct ?? 0}, Incorrect: {question.Incorrect ?? 0}, Streak: {question.Streak ?? 0}");
                }
                if (questions.Count > 10)
                {
                    WriteLine($"  ... and {questions.Count - 10} more");
                }
            }
            break;
        case "3":
            double avgDifficulty = await repo.GetAverageDifficultyAsync();
            WriteLine($"Average difficulty: {avgDifficulty:F2}");
            break;
        case "4":
            WriteLine("Enter minimum incorrect answers: ");
            string? minIncorrectInput = ReadLine();
            if (int.TryParse(minIncorrectInput, out int minIncorrect))
            {
                IReadOnlyList<Question> questions = (await repo.GetIncorrectAnswersAsync(minIncorrect)).ToList();
                WriteLine($"Questions with {minIncorrect}+ incorrect answers: {questions.Count}");
                foreach (Question question in questions.Take(10))
                {
                    WriteLine($"  {question.QuestionText} - Incorrect: {question.Incorrect}");
                }
                if (questions.Count > 10)
                {
                    WriteLine($"  ... and {questions.Count - 10} more");
                }
            }
            break;
        case "5":
            WriteLine("Enter minimum difficulty: ");
            string? minDiffInput = ReadLine();
            WriteLine("Enter maximum difficulty: ");
            string? maxDiffInput = ReadLine();
            if (int.TryParse(minDiffInput, out int minDiff) && int.TryParse(maxDiffInput, out int maxDiff))
            {
                IReadOnlyList<Question> questions = (await repo.GetByDifficultyRangeAsync(minDiff, maxDiff)).ToList();
                WriteLine($"Questions with difficulty {minDiff}-{maxDiff}: {questions.Count}");
                foreach (Question question in questions.Take(10))
                {
                    WriteLine($"  {question.QuestionText} - Difficulty: {question.Difficulty}");
                }
                if (questions.Count > 10)
                {
                    WriteLine($"  ... and {questions.Count - 10} more");
                }
            }
            break;
        case "6":
            WriteLine("Enter question to check additions (or leave empty for all): ");
            string? questionInput = ReadLine();
            if (!string.IsNullOrEmpty(questionInput))
            {
                IReadOnlyList<QuestionHistory> history = (await repo.GetQuestionHistoryAsync(questionInput)).ToList();
                WriteLine($"Addition history for {questionInput}: {history.Count} entries");
                foreach (QuestionHistory entry in history)
                {
                    WriteLine($"  Added at: {entry.TimeStamp}");
                }
            }
            else
            {
                WriteLine("Enter a specific question to see its addition history.");
            }
            break;
        default:
            WriteLine("Invalid choice.");
            break;
    }
}

void DisplayWordWithAnagrams(string word, string additionalInfo, bool showAnagrams)
{
    WriteLine($"  {word} - {additionalInfo}");
    if (showAnagrams)
    {
        Answer anagram = wordService.Anagram(word);
        if (anagram.Words.Count > 0)
        {
            WriteLine($"    {wordOutputService.FormatWords(anagram.Words)}");
        }
    }
}

void PromptQuizOnWords(List<string> quizWords)
{
    if (quizWords.Count == 0)
    {
        return;
    }
    
    WriteLine("");
    Write($"Quiz on these {quizWords.Count} words? (y/n): ");
    string? quizResponse = ReadLine();
    
    if (quizResponse?.Equals("y", StringComparison.OrdinalIgnoreCase) == true)
    {
        ISessionState sessionState = host.Services.GetRequiredService<ISessionState>();
        sessionState.AddedWords.UnionWith(quizWords);
        
        WriteLine($"Added {quizWords.Count} words to quiz pool. Starting quiz...");
        WriteLine("");
        
        wordService.RunQuiz(QuizOptions.Added, EndQuizSessionCommand, WriteLine, ReadLine);
        
        WriteLine("");
        Write($"Clear these {quizWords.Count} words from your added words list? (y/n): ");
        string? clearResponse = ReadLine();
        
        if (clearResponse?.Equals("y", StringComparison.OrdinalIgnoreCase) == true)
        {
            sessionState.AddedWords.ExceptWith(quizWords);
            WriteLine($"Cleared {quizWords.Count} words from added words list.");
        }
        else
        {
            WriteLine($"Kept {quizWords.Count} words in added words list. Use 'qw' to quiz again or 'sca' to save.");
        }
    }
}
