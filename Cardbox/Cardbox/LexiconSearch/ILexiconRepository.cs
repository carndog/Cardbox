namespace Cardbox.LexiconSearch
{
    public interface ILexiconRepository<T> where T : class
    {
        T this[IFilePath path] { get; }

        void InitialiseRepository(IFileProcessor<T>[] paths);
    }
}