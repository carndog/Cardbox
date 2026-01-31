using CardboxDataLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CardboxDataLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddCardboxDataLayer(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("CardboxDatabase") 
                                  ?? configuration["CardboxDatabase:ConnectionString"];
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("CardboxDatabase connection string not found in configuration.");
        }

        services.AddDbContext<CardboxDbContext>(options =>
            options.UseSqlite(connectionString)
           .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        services.AddScoped<IQuestionRepository, QuestionRepository>();

        return services;
    }
}
