namespace Cardbox.LexiconSearch
{
    public interface ILineProcessor<in T>
    {
        void LoadLine(T item, string line);
    }
}