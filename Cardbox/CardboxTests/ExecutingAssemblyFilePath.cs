using Cardbox.LexiconSearch;
using System.Reflection;

namespace CardboxTests
{
    public class ExecutingAssemblyFilePath
    {
        public string GetPath()
        {
            string path = FilePathHelper.ShortenPath(x => !x.Contains("bin"), Assembly.GetExecutingAssembly().Location);
            path = FilePathHelper.AddElementsToPath(path, "Resources", "TestDictionary.txt");
            return path;
        }
    }
}
