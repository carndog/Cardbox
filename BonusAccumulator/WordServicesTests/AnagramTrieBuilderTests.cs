using BonusAccumulator.WordServices;
using BonusAccumulator.WordServices.TrieLoading;
using FluentAssertions;

namespace WordServicesTests;

[TestFixture]
public class AnagramTrieBuilderTests
{
    [Test]
    public void FileNotFound_IncorrectException()
    {
        AnagramTrieBuilder builder = new AnagramTrieBuilder("nonexistent.txt", new TrieNode());
        
        Action act = () => builder.LoadLines();
        
        act.Should().Throw<FileNotFoundException>()
           .WithMessage("*Dictionary file not found.*");
    }
}
