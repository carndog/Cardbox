using System.Reflection;
using BonusAccumulator.WordServices;
using BonusAccumulator.WordServices.TrieLoading;

namespace WordServicesTests;

public class Utils
{
    public AnagramTrieBuilder? AnagramTrieBuilder { get; private set; }

    public void CreateTrieLoader()
    {
        AnagramTrieBuilder = new AnagramTrieBuilder(TestFilePath, new TrieNode());
    }
    
    public static string? TestFilePath
    {
        get
        {
            string location = Assembly.GetExecutingAssembly().Location;
            string[]? parts = Path.GetDirectoryName(Path.GetDirectoryName(location))?.Split(Path.DirectorySeparatorChar).TakeWhile(x => x != "bin").ToArray();
            string[] relativePath = { "Resources", "TestDictionary.txt" };
            if (parts != null)
            {
                string[] allParts = new string[parts.Length + relativePath.Length];
                parts.CopyTo(allParts, 0);
                relativePath.CopyTo(allParts, parts.Length);
                string? fullPath = Path.Combine(allParts);
                return fullPath;
            }
            return null;
        }
    }
}