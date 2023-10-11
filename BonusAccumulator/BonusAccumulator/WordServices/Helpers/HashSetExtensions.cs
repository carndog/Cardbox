namespace BonusAccumulator.WordServices.Helpers;

public static class HashSetExtensions
{
    public static T GetNthElement<T>(this HashSet<T> set, int n)
    {
        if (n <= 0 || n > set.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Value must be between 1 and the number of elements in the HashSet.");
        }

        int count = 1;
        foreach (T item in set)
        {
            if (count == n)
            {
                return item;
            }
            count++;
        }

        throw new Exception("Unexpected error.");
    }
}