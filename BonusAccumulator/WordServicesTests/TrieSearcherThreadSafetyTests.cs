using WordServices;
using WordServices.TrieLoading;
using WordServices.TrieSearching;
using FluentAssertions;
using static WordServicesTests.Utils;

namespace WordServicesTests;

[TestFixture]
public class TrieSearcherThreadSafetyTests
{
    [Test]
    public void Query_ConcurrentCalls_ShouldNotCorruptResults()
    {
        TrieSearcher sharedSearcher = new TrieSearcher(
            new LazyLoadingTrie(new AnagramTrieBuilder(TestFilePath, new TrieNode())));

        int iterations = 100;
        List<Task<IList<string>>> tasks = new();

        for (int i = 0; i < iterations; i++)
        {
            int iteration = i;
            Task<IList<string>> task = Task.Run(() =>
            {
                string searchTerm = iteration % 2 == 0 ? "CAT" : "DOG";
                return sharedSearcher.Query(searchTerm, words => words.Where(w => w.Length == searchTerm.Length));
            });
            tasks.Add(task);
        }

        Task.WaitAll(tasks.ToArray());

        List<IList<string>> allResults = tasks.Select(t => t.Result).ToList();

        List<IList<string>> catResults = allResults.Where((_, i) => i % 2 == 0).ToList();
        List<IList<string>> dogResults = allResults.Where((_, i) => i % 2 != 0).ToList();

        catResults.Should().AllSatisfy(result =>
        {
            result.Should().NotBeEmpty("CAT search should return results");
            result.Should().NotContain("DOG", "CAT search should not contain DOG results");
        });

        dogResults.Should().AllSatisfy(result =>
        {
            result.Should().NotBeEmpty("DOG search should return results");
            result.Should().NotContain("CAT", "DOG search should not contain CAT results");
            result.Should().NotContain("ACT", "DOG search should not contain ACT results");
        });
    }
}
