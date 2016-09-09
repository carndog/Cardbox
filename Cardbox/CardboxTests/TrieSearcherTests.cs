using Cardbox.LexiconSearch;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace CardboxTests
{
    [TestClass]
    public class TrieSearcherTests
    {
        private Anagram _anagram;
        private Pattern _pattern;
        private Build _build;

        [TestInitialize]
        public void Setup()
        {
            _anagram = new Anagram(new TrieSearcher(new LazyLoadingTrie(new AnagramTrieBuilder(new ExecutingAssemblyFilePath().GetPath(), new TrieNode()))));

            _pattern = new Pattern(new TrieSearcher(new LazyLoadingTrie(new AnagramTrieBuilder(new ExecutingAssemblyFilePath().GetPath(), new TrieNode()))));

            _build = new Build(new TrieSearcher(new LazyLoadingTrie(new AnagramTrieBuilder(new ExecutingAssemblyFilePath().GetPath(), new TrieNode()))));
        }

        [TestMethod]
        public void RunAnagramSearch()
        {
            IList<string> results = _anagram.Query("DGO");
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("DOG", results[0]);
        }

        [TestMethod]
        public void RunPatternSearch()
        {
            IList<string> results = _pattern.Query("DOGGY");
            Assert.AreEqual("DOGGY", results[0]);
        }

        [TestMethod]
        public void RunBuildSearch()
        {
            IList<string> results = _build.Query("DOGGY");
            Assert.AreEqual(2, results.Count);
            Assert.IsTrue(results.Contains("DOG"));
            Assert.IsTrue(results.Contains("DOGGY"));
        }

        [TestMethod]
        public void RunBuildWithWildcardSearch()
        {
            IList<string> results = _build.Query("ZEBR.AS.");
            Assert.AreEqual(5, results.Count);
            Assert.IsTrue(results.Contains("ZEBRASS"));
            Assert.IsTrue(results.Contains("ZEBRA"));
            Assert.IsTrue(results.Contains("ZOO"));
            Assert.IsTrue(results.Contains("CAT"));
            Assert.IsTrue(results.Contains("ACT"));
        }


        [TestMethod]
        public void RunPatternWithWildcardSearch()
        {
            IList<string> results = _pattern.Query("ZEB.AS.");
            Assert.AreEqual(1, results.Count);
            Assert.IsTrue(results.Contains("ZEBRASS"));
        }


        [TestMethod]
        public void RunAnagramWithWildcardSearch()
        {
            IList<string> results = _anagram.Query("ZEBRA..");
            Assert.AreEqual(1, results.Count);
            Assert.IsTrue(results.Contains("ZEBRASS"));
        }
    }
}
