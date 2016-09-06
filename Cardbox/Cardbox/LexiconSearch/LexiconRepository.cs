using System.Collections.Generic;

namespace Cardbox.LexiconSearch
{
    public class LexiconRepository<T> : ILexiconRepository<T> where T : class
    {
        private readonly Dictionary<string, T> _lexicons;

        public LexiconRepository()
        {
            _lexicons = new Dictionary<string, T>();
        }

        public T this[IFilePath path] => _lexicons[path.GetPath()];

        public void InitialiseRepository(IFileProcessor<T>[] paths)
        {
            foreach (IFileProcessor<T> fileLinesProcessor in paths)
            {
                _lexicons[fileLinesProcessor.Path.GetPath()] = fileLinesProcessor.LoadLines();
            }
        }
    }
}
