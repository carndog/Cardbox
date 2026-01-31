using BonusAccumulator.WordServices.Extensions;

namespace BonusAccumulator.WordServices.Output;

public class DefaultWordOutputService : IWordOutputService
{
    public string FormatWords(IList<string> words)
    {
        if (words.Count == 0)
            return string.Empty;

        IEnumerable<IGrouping<string, string>> anagramGroups = words
            .GroupBy(word => word.ToAlphagram())
            .OrderBy(group => group.Key);

        List<string> formattedWords = new List<string>();

        foreach (IGrouping<string, string> group in anagramGroups)
        {
            List<string> sortedWords = group.OrderBy(word => word).ToList();

            string formattedWord = sortedWords.Count == 1 ? sortedWords[0] : $"({string.Join(",", sortedWords)})";
            formattedWords.Add(formattedWord);
        }

        return string.Join(",", formattedWords);
    }
}
