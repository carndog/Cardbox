using Cardbox.LexiconSearch;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CardboxTests
{
    [TestClass]
    public class LexiconRepositoryTests
    {
        private readonly Utils _utils = new Utils();

        private TrieNode _trie;

        [TestInitialize]
        public void Init()
        {
            _utils.CreateTrieLoader();
        }

        [TestMethod]
        public void ConstructRepository()
        {
            var fileProcessor = new AnagramTrieBuilder(new ExecutingAssemblyFilePath().GetPath(), new TrieNode());
            _trie = fileProcessor.LoadLines();

            _trie.Should().NotBeNull();
        }
    }
}
