using Cardbox.LexiconSearch;

namespace CardboxTests
{
    public class Utils
    {
        public IFileProcessor<TrieNode> FileProcessor { get; set; }

        public ILineProcessor<TrieNode> LineProcessor { get; set; }

        public IFilePath FilePath { get; set; }

        public void CreateTrieLoader()
        {
            FilePath = new ExecutingAssemblyFilePath();

            LineProcessor = new LineProcessor();

            FileProcessor = new FileProcessor<TrieNode>(FilePath, new TrieNode(), LineProcessor);
        }
    }
}
