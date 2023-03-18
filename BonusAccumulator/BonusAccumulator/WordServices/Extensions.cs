namespace BonusAccumulator.WordServices;

public static class StringExtensions
{
    public static string WildcardsFirst(this string word)
    {
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

    public static string ToAlphagram(this string word)
    {
        char[] chars = word.ToCharArray();
        Array.Sort(chars);

        return new string(chars);
    }
}