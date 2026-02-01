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
            .FirstOrDefaultAsync(q => q.question == question);
    }

    public async Task<IEnumerable<Question>> GetAllAsync()
    {
        return await _context.Questions
            .OrderBy(q => q.question)
            .ToListAsync();
    }

    public async Task<IEnumerable<Question>> GetByCardboxAsync(int cardbox)
    {
        return await _context.Questions
            .Where(q => q.cardbox == cardbox)
            .OrderBy(q => q.question)
            .ToListAsync();
    }

    public async Task<IEnumerable<Question>> GetByDifficultyAsync(int difficulty)
    {
        return await _context.Questions
            .Where(q => q.difficulty == difficulty)
            .OrderBy(q => q.question)
            .ToListAsync();
    }

    public async Task<IEnumerable<Question>> GetByDifficultyRangeAsync(int minDifficulty, int maxDifficulty)
    {
        return await _context.Questions
            .Where(q => q.difficulty >= minDifficulty && q.difficulty <= maxDifficulty)
            .OrderBy(q => q.difficulty)
            .ThenBy(q => q.question)
            .ToListAsync();
    }

    public async Task<IEnumerable<Question>> GetByStreakAsync(int minStreak)
    {
        return await _context.Questions
            .Where(q => q.streak >= minStreak)
            .OrderByDescending(q => q.streak)
            .ThenBy(q => q.question)
            .ToListAsync();
    }

    public async Task<IEnumerable<Question>> GetScheduledAsync(int maxNextScheduled)
    {
        return await _context.Questions
            .Where(q => q.next_scheduled <= maxNextScheduled)
            .OrderBy(q => q.next_scheduled)
            .ThenBy(q => q.question)
            .ToListAsync();
    }

    public async Task<IEnumerable<Question>> GetIncorrectAnswersAsync(int minIncorrect)
    {
        return await _context.Questions
            .Where(q => q.incorrect >= minIncorrect)
            .OrderByDescending(q => q.incorrect)
            .ThenBy(q => q.question)
            .ToListAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Questions.CountAsync();
    }

    public async Task<int> GetCountByCardboxAsync(int cardbox)
    {
        return await _context.Questions
            .CountAsync(q => q.cardbox == cardbox);
    }

    public async Task<double> GetAverageDifficultyAsync()
    {
        return await _context.Questions
            .Where(q => q.difficulty.HasValue)
            .AverageAsync(q => q.difficulty.Value);
    }

    public async Task<IEnumerable<QuestionHistory>> GetQuestionHistoryAsync(string question)
    {
        try
        {
            return await _context.QuestionHistories
                .Where(qh => qh.question == question)
                .OrderBy(qh => qh.timeStamp)
                .ToListAsync();
        }
        catch (SqliteException)
        {
            return [];
        }
    }
}
