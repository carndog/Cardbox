using WordServices;

namespace CardboxTests
{
    public class Utils
    {
        public AnagramTrieBuilder AnagramTrieBuilder { get; set; }

        public string FilePath { get; set; }

        public void CreateTrieLoader()
        {
            FilePath = new ExecutingAssemblyFilePath().GetPath();

            AnagramTrieBuilder = new AnagramTrieBuilder(FilePath, new TrieNode());
        }
    }
}
