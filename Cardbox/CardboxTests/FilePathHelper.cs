using System;
using System.IO;
using System.Linq;

namespace CardboxTests
{
    public static class FilePathHelper
    {
        public static string ShortenPath(Predicate<string> predicate, string path)
        {
            return ShortenPath(new Func<string, bool>(predicate), path);
        }

        private static string ShortenPath(Func<string, bool> predicate, string path)
        {
            string[] tokens = path.Split(Path.DirectorySeparatorChar);
            tokens = tokens.TakeWhile(predicate).ToArray();
            path = string.Join(Path.DirectorySeparatorChar.ToString(), tokens);
            return path;
        }

        public static string AddElementsToPath(string path, params string[] elements)
        {
            string[] elementsWithDir = new string[elements.Length + 1];
            elementsWithDir[0] = path;
            elements.CopyTo(elementsWithDir, 1);
            return Path.Combine(elementsWithDir);
        }
    }
}
