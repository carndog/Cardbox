using WordServices.Extensions;
using FluentAssertions;

namespace WordServicesTests;

[TestFixture]
public class StringExtensionsTests
{
    [Test]
    public void ToWildCardCharacterQuestions_CaseInconsistency()
    {
        string input = "Cat";
        List<string> results = input.ToWildCardCharacterQuestions().ToList();
        
        results.Should().AllSatisfy(result => 
        {
            foreach (char c in result)
            {
                if (c != '.' && !char.IsUpper(c))
                {
                    throw new AssertionException($"Expected '{result}' to have all non-wildcard characters uppercase, but found '{c}'");
                }
            }
        }, "All generated questions should have non-wildcard characters uppercase");
    }
}
