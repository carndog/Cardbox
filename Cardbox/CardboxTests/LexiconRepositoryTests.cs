using Cardbox.LexiconSearch;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CardboxTests
{
    [TestClass]
    public class LexiconRepositoryTests
    {
        private readonly Utils _utils = new Utils();

        private ILexiconRepository<TrieNode> _lexiconRepository;

        [TestInitialize]
        public void Init()
        {
            _utils.CreateTrieLoader();
        }

        [TestMethod]
        public void ConstructRepository()
        {
            _lexiconRepository = new LexiconRepository<TrieNode>();
            var fileProcessor = new FileProcessor<TrieNode>(new ExecutingAssemblyFilePath(), new TrieNode(), new LineProcessor());
            _lexiconRepository.InitialiseRepository(new[] { fileProcessor });

            Assert.IsInstanceOfType(_lexiconRepository[fileProcessor.Path], typeof(TrieNode));
        }
    }
}
