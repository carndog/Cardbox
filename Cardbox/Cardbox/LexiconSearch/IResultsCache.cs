namespace Cardbox.LexiconSearch
{
    public interface IResultsCache<in T, TData>
    {
        TData Get(T key);

        void Add(T key, TData data);
    }
}
