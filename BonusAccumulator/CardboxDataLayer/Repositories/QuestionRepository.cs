using CardboxDataLayer.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CardboxDataLayer.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly CardboxDbContext _context;

    public QuestionRepository(CardboxDbContext context)
    {
        _context = context;
    }

    public async Task<Question?> GetByQuestionAsync(string question)
    {
        return await _context.Questions
            .FirstOrDefaultAsync(q => q.QuestionText == question);
    }

    public async Task<IEnumerable<Question>> GetAllAsync()
    {
        return await _context.Questions
            .OrderBy(q => q.QuestionText)
            .ToListAsync();
    }

    public async Task<IEnumerable<Question>> GetByCardboxAsync(int cardbox)
    {
        return await _context.Questions
            .Where(q => q.Cardbox == cardbox)
            .OrderBy(q => q.QuestionText)
            .ToListAsync();
    }

    public async Task<IEnumerable<Question>> GetByDifficultyAsync(int difficulty)
    {
        return await _context.Questions
            .Where(q => q.Difficulty == difficulty)
            .OrderBy(q => q.QuestionText)
            .ToListAsync();
    }

    public async Task<IEnumerable<Question>> GetByDifficultyRangeAsync(int minDifficulty, int maxDifficulty)
    {
        return await _context.Questions
            .Where(q => q.Difficulty >= minDifficulty && q.Difficulty <= maxDifficulty)
            .OrderBy(q => q.Difficulty)
            .ThenBy(q => q.QuestionText)
            .ToListAsync();
    }

    public async Task<IEnumerable<Question>> GetByStreakAsync(int minStreak)
    {
        return await _context.Questions
            .Where(q => q.Streak >= minStreak)
            .OrderByDescending(q => q.Streak)
            .ThenBy(q => q.QuestionText)
            .ToListAsync();
    }

    public async Task<IEnumerable<Question>> GetScheduledAsync(int maxNextScheduled)
    {
        return await _context.Questions
            .Where(q => q.NextScheduled <= maxNextScheduled)
            .OrderBy(q => q.NextScheduled)
            .ThenBy(q => q.QuestionText)
            .ToListAsync();
    }

    public async Task<IEnumerable<Question>> GetIncorrectAnswersAsync(int minIncorrect)
    {
        return await _context.Questions
            .Where(q => q.Incorrect >= minIncorrect)
            .OrderByDescending(q => q.Incorrect)
            .ThenBy(q => q.QuestionText)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Questions.CountAsync();
    }

    public async Task<int> GetCountByCardboxAsync(int cardbox)
    {
        return await _context.Questions
            .CountAsync(q => q.Cardbox == cardbox);
    }

    public async Task<double> GetAverageDifficultyAsync()
    {
        return await _context.Questions
            .Where(q => q.Difficulty.HasValue)
            .AverageAsync(q => q.Difficulty!.Value);
    }

    public async Task<IEnumerable<QuestionHistory>> GetQuestionHistoryAsync(string question)
    {
        try
        {
            return await _context.QuestionHistories
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
