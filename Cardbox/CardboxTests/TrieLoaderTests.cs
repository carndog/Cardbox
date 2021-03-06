﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using WordServices;

namespace CardboxTests
{
    [TestClass]
    public class TrieLoaderTests
    {
        private readonly Utils _utils = new Utils();

        [TestInitialize]
        public void Setup()
        {
            _utils.CreateTrieLoader();
        }

        [TestMethod]
        public void LoadTrie_IndividualWordsOnSeparateLinesInAFile_ProducesAnagramNodeStructure()
        {
            TrieNode rootNode = _utils.AnagramTrieBuilder.LoadLines();

            Assert.IsNotNull(rootNode);
            Assert.AreEqual(7, rootNode.Edges.Count);
            Assert.IsFalse(rootNode.Edges[1].Terminal);
            Assert.AreEqual('A', rootNode.Edges[0].Label);
            Assert.IsTrue(rootNode.Edges[0].Edges[0].Edges[0].Terminal);
            Assert.AreEqual(2, rootNode.Edges[0].Edges[0].Edges[0].AnagramssAtTerminal.Count);
            Assert.IsTrue(rootNode.Edges[0].Edges[0].Edges[0].AnagramssAtTerminal.Contains("CAT"));
        }
    }
}
