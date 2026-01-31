using BonusAccumulator.WordServices.Extensions;
using FluentAssertions;

namespace WordServicesTests;

[TestFixture]
public class HashSetExtensionsTests
{
    [Test]
    public void GetNthElement_ValidIndex_ReturnsCorrectElement()
    {
        HashSet<string> set = new HashSet<string> { "A", "B", "C", "D", "E" };

        string result = set.GetNthElement(3);

        result.Should().BeOneOf("A", "B", "C", "D", "E");
    }

    [Test]
    public void GetNthElement_FirstIndex_ReturnsFirstElement()
    {
        HashSet<int> set = new HashSet<int> { 10, 20, 30, 40, 50 };

        int result = set.GetNthElement(1);

        result.Should().BeOneOf(10, 20, 30, 40, 50);
    }

    [Test]
    public void GetNthElement_LastIndex_ReturnsLastElement()
    {
        HashSet<string> set = new HashSet<string> { "X", "Y", "Z" };

        string result = set.GetNthElement(3);

        result.Should().BeOneOf("X", "Y", "Z");
    }

    [Test]
    public void GetNthElement_ZeroIndex_ThrowsArgumentOutOfRangeException()
    {
        HashSet<string> set = new HashSet<string> { "A", "B", "C" };

        Action act = () => set.GetNthElement(0);

        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("n");
    }

    [Test]
    public void GetNthElement_NegativeIndex_ThrowsArgumentOutOfRangeException()
    {
        HashSet<int> set = new HashSet<int> { 1, 2, 3 };

        Action act = () => set.GetNthElement(-1);

        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("n");
    }

    [Test]
    public void GetNthElement_IndexGreaterThanCount_ThrowsArgumentOutOfRangeException()
    {
        HashSet<string> set = new HashSet<string> { "A", "B" };

        Action act = () => set.GetNthElement(5);

        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("n");
    }

    [Test]
    public void GetNthElement_IndexEqualsCount_ThrowsArgumentOutOfRangeException()
    {
        HashSet<int> set = new HashSet<int> { 1, 2, 3 };

        Action act = () => set.GetNthElement(4);

        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("n");
    }

    [Test]
    public void GetNthElement_SingleElementSet_ValidIndex_ReturnsElement()
    {
        HashSet<string> set = new HashSet<string> { "ONLY" };

        string result = set.GetNthElement(1);

        result.Should().Be("ONLY");
    }

    [Test]
    public void GetNthElement_SingleElementSet_InvalidIndex_ThrowsArgumentOutOfRangeException()
    {
        HashSet<string> set = new HashSet<string> { "ONLY" };

        Action act = () => set.GetNthElement(2);

        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("n");
    }

    [Test]
    public void GetNthElement_EmptySet_ThrowsArgumentOutOfRangeException()
    {
        HashSet<int> set = new HashSet<int>();

        Action act = () => set.GetNthElement(1);

        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("n");
    }
}
