using Cardbox.LexiconSearch;
using System.Collections.Generic;
using System.Linq;

namespace Cardbox
{
    public class WordService
    {
        private readonly TrieSearcher _searcher;

        public WordService(TrieSearcher searcher)
        {
            _searcher = searcher;
        }

        public AnswerDto Anagram(string question)
        {
            IList<string> words = _searcher.Query(question,
                wordsAtTerminal => wordsAtTerminal.Where(x => x.Length == question.Length));

            return new AnswerDto
            {
                Words = words
            };
        }

        //public AnswerDto Build(string question)
        //{

        //}

        //public AnswerDto Pattern(string question)
        //{

        //}
    }
}
