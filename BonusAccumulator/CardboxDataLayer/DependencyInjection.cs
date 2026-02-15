using CardboxDataLayer.Repositories;
using CardboxDataLayer.Analytics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WordServices.Analytics;

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
            connectionString = "Data Source=C:\\Users\\jason\\Dropbox\\Apps\\Zyzzyva\\quiz\\data\\CSW24\\Anagrams.db";
        }

        services.AddDbContext<CardboxDbContext>(options =>
            options.UseSqlite(connectionString)
           .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        services.AddScoped<IQuestionRepository, QuestionRepository>();
        
        services.AddScoped<IGetDeckStatsByCardbox, GetDeckStatsByCardbox>();
        services.AddScoped<IGetDeckStatsByWordLength, GetDeckStatsByWordLength>();
        services.AddScoped<IGetDueNow, GetDueNow>();
        services.AddScoped<IGetDueSoon, GetDueSoon>();
        services.AddScoped<IGetHighestErrorRate, GetHighestErrorRate>();
        services.AddScoped<IGetMostWrong, GetMostWrong>();
        services.AddScoped<IGetPainPerRecentMemory, GetPainPerRecentMemory>();
        services.AddScoped<IGetRegressions, GetRegressions>();
        services.AddScoped<IGetNotSeenForAges, GetNotSeenForAges>();
        services.AddScoped<IGetIntervalStats, GetIntervalStats>();
        services.AddScoped<IGetForgettingCurveStats, GetForgettingCurveStats>();
        services.AddScoped<IGetBlindSpots, GetBlindSpots>();
        services.AddScoped<IGetPriorityItems, GetPriorityItems>();
        
        services.AddScoped<IAnalyticsService, AnalyticsService>();
    }
}
