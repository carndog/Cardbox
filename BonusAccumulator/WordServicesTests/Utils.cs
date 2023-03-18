using System.Reflection;
using BonusAccumulator.WordServices;

namespace WordServicesTests;

public class Utils
{
    public AnagramTrieBuilder AnagramTrieBuilder { get; set; }

    public string FilePath { get; set; }

    public void CreateTrieLoader()
    {
        FilePath = AssemblyDirectory;

        AnagramTrieBuilder = new AnagramTrieBuilder(FilePath, new TrieNode());
    }
    
    public static string AssemblyDirectory
    {
        get
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
    }
}