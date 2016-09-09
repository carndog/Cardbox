using Cardbox;
using Cardbox.LexiconSearch;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CardboxTests
{
    [TestClass]
    public class AnagrammerTests
    {
        private static WordService _anagrammer;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            _anagrammer = new WordService(new TrieSearcher(
                new LazyLoadingTrie(new AnagramTrieBuilder(
                    new ExecutingAssemblyFilePath().GetPath(), new TrieNode()))));
        }

        [TestMethod]
        public void Anagram()
        {
            AnswerDto answerDto = _anagrammer.Anagram("CAT");

            answerDto.Words.Count().Should().Be(2);
            answerDto.Words.Should().ContainInOrder("ACT", "CAT");
        }
    }
}
