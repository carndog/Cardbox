using CardboxDataLayer.Entities;

namespace CardboxDataLayer.Repositories;

public interface IQuestionRepository
{
    Task<Question?> GetByQuestionAsync(string question);

    Task<IEnumerable<Question>> GetAllAsync();

    Task<IEnumerable<Question>> GetByCardboxAsync(int cardbox);

    Task<IEnumerable<Question>> GetByDifficultyAsync(int difficulty);

    Task<IEnumerable<Question>> GetByDifficultyRangeAsync(int minDifficulty, int maxDifficulty);

    Task<IEnumerable<Question>> GetByStreakAsync(int minStreak);

    Task<IEnumerable<Question>> GetScheduledAsync(int maxNextScheduled);

    Task<IEnumerable<Question>> GetIncorrectAnswersAsync(int minIncorrect);

    Task<int> GetTotalCountAsync();

    Task<int> GetCountByCardboxAsync(int cardbox);

    Task<double> GetAverageDifficultyAsync();

    Task<IEnumerable<QuestionHistory>> GetQuestionHistoryAsync(string question);
}
