﻿using System.Collections.Generic;
using System.Linq;

namespace Cardbox.LexiconSearch
{
    public class LineProcessor : ILineProcessor<TrieNode>
    {
        public void LoadLine(TrieNode root, string line)
        {
            TrieNode current = root;
            string word = line.Trim();
            char[] chars = word.ToAlphagram().ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                char c = chars[i];

                if (current.Edges == null)
                {
                    current.Edges = new List<TrieNode>();
                }

                TrieNode next = AddCurrentCharacterToTrie(current, c);

                bool isTerminal = i == chars.Length - 1;
                if (isTerminal)
                {
                    AssignWordToNode(next, word);
                }
                current = next;
            }
        }

        private static void AssignWordToNode(TrieNode next, string word)
        {
            next.Terminal = true;
            CollectAnagrams(next, word);
        }

        private static void CollectAnagrams(TrieNode next, string word)
        {
            if (next.AnagramssAtTerminal == null)
            {
                next.AnagramssAtTerminal = new List<string>();
            }
            next.AnagramssAtTerminal.Add(word);
        }

        private static TrieNode AddCurrentCharacterToTrie(TrieNode current, char c)
        {
            TrieNode next;
            if (current.Edges.All(edge => edge.Label != c))
            {
                next = new TrieNode() { Label = c, Edges = new List<TrieNode>() };
                current.Edges.Add(next);
            }
            else
            {
                next = current.Edges.First(edge => edge.Label == c);
            }

            return next;
        }
    }
}
