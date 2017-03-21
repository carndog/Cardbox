using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using WordServices;

namespace CardboxTests
{
    [TestClass]
    public class ServiceTests
    {
        private WordService _service;

        [TestInitialize]
        public void Init()
        {
            _service = new WordService(new TrieSearcher(
                new LazyLoadingTrie(new AnagramTrieBuilder(
                    new ExecutingAssemblyFilePath().GetPath(), new TrieNode()))));
        }

        [TestMethod]
        public void Anagram()
        {
            Answer answer = _service.Anagram("CAT");

            answer.Words.Count().Should().Be(2);
            answer.Words.Should().ContainInOrder("ACT", "CAT");
        }

        [TestMethod]
        public void Build()
        {
            Answer answer = _service.Build("TACT");

            answer.Words.Count().Should().Be(2);
            answer.Words.Should().ContainInOrder("ACT", "CAT");
        }
    }
}
