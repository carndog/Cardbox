namespace Cardbox.LexiconSearch
{
    public interface IFileProcessor<out T> where T : class
    {
        IFilePath Path { get; }

        T LoadLines();
    }
}