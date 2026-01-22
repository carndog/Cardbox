using System.Reflection;
using BonusAccumulator.WordServices.Factories;
using FluentAssertions;

namespace WordServicesTests;

[TestFixture]
public class WordServiceFactoryTests
{
    [Test]
    public void Create_MultipleSettingsProviderInstances()
    {
        Type factory = typeof(WordServiceFactory);
        MethodInfo? createMethod = factory.GetMethod("Create");
        
        object? service = createMethod?.Invoke(null, null);
        
        service.Should().NotBeNull();
    }
}
