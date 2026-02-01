using CardboxDataLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CardboxDataLayer;

public static class DependencyInjection
{
    public static void AddCardboxDataLayer(this IServiceCollection services,
        IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("CardboxDatabase") 
                                  ?? configuration["CardboxDatabase:ConnectionString"]
                                  ?? configuration["ConnectionStrings:CardboxDatabase"];
        
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = "Data Source=C:\\AnagramsTestArea\\Anagrams.db";
        }

        services.AddDbContext<CardboxDbContext>(options =>
            options.UseSqlite(connectionString)
           .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        services.AddScoped<IQuestionRepository, QuestionRepository>();
    }
}
