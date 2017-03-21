using System.Collections.Generic;
using System.Linq;

namespace WordServices
{
    public class WordService
    {
        private readonly TrieSearcher _searcher;

        public WordService(TrieSearcher searcher)
        {
            _searcher = searcher;
        }

        public Answer Anagram(string question)
        {
            IList<string> words = _searcher.Query(question,
                wordsAtTerminal => wordsAtTerminal.Where(x => x.Length == question.Length));

            return new Answer
            {
                Words = words
            };
        }

        public Answer Build(string question)
        {
            IList<string> words = _searcher.Query(question, x => x);

            return new Answer
            {
                Words = words
            };
        }

        public Answer Pattern(string question)
        {
            return null;
        }
    }
}
