using BonusAccumulator.WordServices;
using FluentAssertions;
using static WordServicesTests.Utils;

namespace WordServicesTests;

[TestFixture]
public class ServiceTests
{
    private WordService _service;

    [SetUp]
    public void SetUp()
    {
        _service = new WordService(new TrieSearcher(
            new LazyLoadingTrie(new AnagramTrieBuilder(
                TestFilePath, new TrieNode()))));
    }

    [Test]
    public void Anagram()
    {
        Answer answer = _service.Anagram("CAT");

        answer.Words.Count().Should().Be(2);
        answer.Words.Should().ContainInOrder("ACT", "CAT");
    }

    [Test]
    public void Build()
    {
        Answer answer = _service.Build("TACT");

        answer.Words.Count().Should().Be(2);
        answer.Words.Should().ContainInOrder("ACT", "CAT");
    }
}