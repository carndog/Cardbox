using System.Collections.Generic;

namespace Cardbox.LexiconSearch
{
    public class TrieNode
    {
        public List<TrieNode> Edges { get; set; }

        public char Label { get; set; }

        public bool Terminal { get; set; }

        public List<string> AnagramssAtTerminal { get; set; }
    }
}
