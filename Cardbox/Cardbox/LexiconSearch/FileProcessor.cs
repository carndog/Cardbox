using System.IO;

namespace Cardbox.LexiconSearch
{
    public class FileProcessor<T> : IFileProcessor<T> where T : class
    {
        private readonly T _item;
        private readonly ILineProcessor<T> _loader;

        public FileProcessor(IFilePath filePath, T item, ILineProcessor<T> loader)
        {
            Path = filePath;
            _item = item;
            _loader = loader;
        }

        public IFilePath Path { get; }

        public T LoadLines()
        {
            if (!File.Exists(Path.GetPath()))
            {
                throw new FileNotFoundException(Path.GetPath());
            }

            using (var reader = new StreamReader(Path.GetPath()))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    _loader.LoadLine(_item, line);
                }
            }
            return _item;
        }
    }
}
