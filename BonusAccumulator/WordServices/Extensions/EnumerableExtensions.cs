namespace WordServices.Extensions;

public static class EnumerableExtensions
{
    public static List<T> Shuffled<T>(this IEnumerable<T> source, Random random)
    {
        List<T> list = source.ToList();

        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }

        return list;
    }
}
