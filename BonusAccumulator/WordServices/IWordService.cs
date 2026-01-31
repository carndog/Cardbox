namespace WordServices;

public interface IWordService
{
    Answer Anagram(string letters);

    Answer Build(string letters);

    Answer Pattern(string pattern);

    Answer Distance(string word);

    Answer AlphagramDistance(string word);

    void RunQuiz(QuizOptions options, string endCommand, Action<string> output, Func<string?> input);

    void RunChainQuiz(string endCommand, Action<string> output, Func<string?> input);

    void AddWords(string[] words, Action<string> output);

    void AddLastWords();

    string StoreAndClearAdded();

    void ConvertToAlphagrams(Action<string> output, Func<string?> input);
}
