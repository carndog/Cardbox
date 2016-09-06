using System.Collections.Generic;
using Cardbox.LexiconSearch;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            _anagram = new Anagram(new TrieSearcher(new LazyLoadingTrie(new LexiconRepository<TrieNode>(),
                new FileProcessor<TrieNode>(new ExecutingAssemblyFilePath(), new TrieNode(), new LineProcessor()))), new ResultsCache());

            _pattern = new Pattern(new TrieSearcher(new LazyLoadingTrie(new LexiconRepository<TrieNode>(),
                new FileProcessor<TrieNode>(new ExecutingAssemblyFilePath(), new TrieNode(), new LineProcessor()))), new ResultsCache());

            _build = new Build(new TrieSearcher(new LazyLoadingTrie(new LexiconRepository<TrieNode>(),
                new FileProcessor<TrieNode>(new ExecutingAssemblyFilePath(), new TrieNode(), new LineProcessor()))), new ResultsCache());
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
            Assert.AreEqual(4, results.Count);
            Assert.IsTrue(results.Contains("ZEBRASS"));
            Assert.IsTrue(results.Contains("ZEBRA"));
            Assert.IsTrue(results.Contains("ZOO"));
            Assert.IsTrue(results.Contains("CAT"));
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
