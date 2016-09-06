using System;
using System.Linq;

namespace Cardbox.LexiconSearch
{
    public static class StringExtensions
    {
        public static string WildcardsFirst(this string word)
        {
            return CopyTo(word, true);
        }

        public static string WildcardsLast(this string word)
        {
            return CopyTo(word, false);
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

        public static string ReplaceAt(this string input, int index, char newChar)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            char[] chars = input.ToCharArray();
            chars[index] = newChar;
            return new string(chars);
        }
    }
}
