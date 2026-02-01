using WordServices.Output;
using FluentAssertions;

namespace WordServicesTests;

[TestFixture]
public class DefaultWordOutputServiceTests
{
    private DefaultWordOutputService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _service = new DefaultWordOutputService();
    }

    [Test]
    public void FormatWords_EmptyList_ReturnsEmptyString()
    {
        List<string> words = [];

        string result = _service.FormatWords(words);

        result.Should().BeEmpty();
    }

    [Test]
    public void FormatWords_SingleWord_ReturnsWord()
    {
        List<string> words = ["CAT"];

        string result = _service.FormatWords(words);

        result.Should().Be("CAT");
    }

    [Test]
    public void FormatWords_MultipleNonAnagrams_ReturnsAlphabeticalWords()
    {
        List<string> words = ["DOG", "CAT", "BIRD"];

        string result = _service.FormatWords(words);

        result.Should().Be("CAT,BIRD,DOG");
    }

    [Test]
    public void FormatWords_AnagramGroup_ReturnsBracedGroup()
    {
        List<string> words = ["CAT", "ACT", "TAC"];

        string result = _service.FormatWords(words);

        result.Should().Be("(ACT,CAT,TAC)");
    }

    [Test]
    public void FormatWords_MixedWordsAndAnagrams_ReturnsCorrectFormat()
    {
        List<string> words = ["DOG", "CAT", "ACT", "BIRD", "GOD"];

        string result = _service.FormatWords(words);

        result.Should().Be("(ACT,CAT),BIRD,(DOG,GOD)");
    }

    [Test]
    public void FormatWords_MultipleAnagramGroups_ReturnsCorrectFormat()
    {
        List<string> words = ["CAT", "ACT", "DOG", "GOD", "TAC", "MAN", "NAM"];

        string result = _service.FormatWords(words);

        result.Should().Be("(ACT,CAT,TAC),(MAN,NAM),(DOG,GOD)");
    }

    [Test]
    public void FormatWords_AnagramGroupWithSingleWord_ReturnsUnbracedWord()
    {
        List<string> words = ["CAT", "DOG", "ACT"];

        string result = _service.FormatWords(words);

        result.Should().Be("(ACT,CAT),DOG");
    }

    [Test]
    public void FormatWords_PreservesAlphagramOrdering()
    {
        List<string> words = ["MAN", "CAT", "DOG", "ACT", "NAM", "GOD"];

        string result = _service.FormatWords(words);

        result.Should().Be("(ACT,CAT),(MAN,NAM),(DOG,GOD)");
    }
}
