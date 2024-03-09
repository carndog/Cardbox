using System.Text;

namespace BonusAccumulator.WordServices.Extensions;

public static class StringExtensions
{
    public static string WildcardsFirst(this string? word)
    {
        if (word == null)
            return string.Empty;

        return CopyTo(word, true);
    }

    private static string CopyTo(string word, bool blanksFirst)
    {
        char[] wildcards = word.Where(c => c == '.').ToArray();
        char[] chars = word.Where(c => c != '.').ToArray();
        Array.Sort(chars);
        char[] result = new char[word.Length];

        if (blanksFirst)
        {
            wildcards.CopyTo(result, 0);
            chars.CopyTo(result, wildcards.Length);
        }
        else
        {
            chars.CopyTo(result, 0);
            wildcards.CopyTo(result, chars.Length);
        }

        return new string(result);
    }

    public static string ToAlphagram(this string? word)
    {
        if (word == null)
            return string.Empty;

        char[] chars = word.ToCharArray();
        Array.Sort(chars);

        return new string(chars);
    }

    public static IEnumerable<string> ToWildCardCharacterQuestions(this string? word)
    {
        if(word == null) 
            yield break;
        
        const char wildcard = '.';
        StringBuilder modifiedQuestion = new StringBuilder(word.ToUpper());
        
        for (int i = 0; i < word.Length; i++)
        {
            char c = word[i];
            modifiedQuestion[i] = wildcard;
            yield return modifiedQuestion.ToString();
            modifiedQuestion[i] = c;
        }
    }
}