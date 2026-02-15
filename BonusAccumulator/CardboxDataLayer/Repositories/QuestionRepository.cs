using CardboxDataLayer.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CardboxDataLayer.Repositories;

public class QuestionRepository(CardboxDbContext context) : IQuestionRepository
{
    public async Task<Question?> GetByQuestionAsync(string question)
    {
        return await context.Questions
            .FirstOrDefaultAsync(q => q.QuestionText == question);
    }

    public async Task<IEnumerable<Question>> GetAllAsync()
    {
        return await context.Questions
            .OrderBy(q => q.QuestionText)
            .ToListAsync();
    }

    public async Task<IEnumerable<Question>> GetByCardboxAsync(int cardbox)
    {
        return await context.Questions
            .Where(q => q.Cardbox == cardbox)
            .OrderBy(q => q.QuestionText)
            .ToListAsync();
    }

    public async Task<IEnumerable<Question>> GetByDifficultyAsync(int difficulty)
    {
        return await context.Questions
            .Where(q => q.Difficulty == difficulty)
            .OrderBy(q => q.QuestionText)
            .ToListAsync();
    }

    public async Task<IEnumerable<Question>> GetByDifficultyRangeAsync(int minDifficulty, int maxDifficulty)
    {
        return await context.Questions
            .Where(q => q.Difficulty >= minDifficulty && q.Difficulty <= maxDifficulty)
            .OrderBy(q => q.Difficulty)
            .ThenBy(q => q.QuestionText)
            .ToListAsync();
    }

    public async Task<IEnumerable<Question>> GetByStreakAsync(int minStreak)
    {
        return await context.Questions
            .Where(q => q.Streak >= minStreak)
            .OrderByDescending(q => q.Streak)
            .ThenBy(q => q.QuestionText)
            .ToListAsync();
    }

    public async Task<IEnumerable<Question>> GetScheduledAsync(int maxNextScheduled)
    {
        return await context.Questions
            .Where(q => q.NextScheduled <= maxNextScheduled)
            .OrderBy(q => q.NextScheduled)
            .ThenBy(q => q.QuestionText)
            .ToListAsync();
    }

    public async Task<IEnumerable<Question>> GetIncorrectAnswersAsync(int minIncorrect)
    {
        return await context.Questions
            .Where(q => q.Incorrect >= minIncorrect)
            .OrderByDescending(q => q.Incorrect)
            .ThenBy(q => q.QuestionText)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await context.Questions.CountAsync();
    }

    public async Task<int> GetCountByCardboxAsync(int cardbox)
    {
        return await context.Questions
            .CountAsync(q => q.Cardbox == cardbox);
    }

    public async Task<double> GetAverageDifficultyAsync()
    {
        return await context.Questions
            .Where(q => q.Difficulty.HasValue)
            .AverageAsync(q => q.Difficulty!.Value);
    }

    public async Task<IEnumerable<QuestionHistory>> GetQuestionHistoryAsync(string question)
    {
        try
        {
            return await context.QuestionHistories
                .Where(qh => qh.QuestionText == question)
                .OrderBy(qh => qh.TimeStamp)
                .ToListAsync();
        }
        catch (SqliteException)
        {
            return [];
        }
    }
}
