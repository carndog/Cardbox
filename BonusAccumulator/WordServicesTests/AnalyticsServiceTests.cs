using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using CardboxDataLayer;
using WordServices.Analytics;

namespace WordServicesTests;

[TestFixture]
public class AnalyticsServiceTests
{
    [Test]
    public void AnalyticsService_ShouldBeCreatable()
    {
        ServiceCollection services = new ServiceCollection();
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:CardboxDatabase"] = "Data Source=C:\\AnagramsTestArea\\Anagrams.db"
            })
            .Build();

        services.AddSingleton(configuration);
        services.AddCardboxDataLayer(configuration);
        ServiceProvider serviceProvider = services.BuildServiceProvider();
        
        IAnalyticsService analyticsService = serviceProvider.GetRequiredService<IAnalyticsService>();
        Assert.That(analyticsService, Is.Not.Null);
        Assert.That(analyticsService, Is.InstanceOf<CardboxDataLayer.Analytics.AnalyticsService>());
    }
}
