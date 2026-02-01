using CardboxDataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace CardboxDataLayer;

public class CardboxDbContext : DbContext
{
    public CardboxDbContext(DbContextOptions<CardboxDbContext> options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public DbSet<Question> Questions { get; set; }
    
    public DbSet<QuestionHistory> QuestionHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Question>(entity =>
        {
            entity.ToTable("questions");
            entity.HasKey(e => e.QuestionText);
            
            entity.Property(e => e.QuestionText)
                .HasColumnName("question")
                .HasColumnType("varchar(16)")
                .HasMaxLength(16);
                
            entity.Property(e => e.Correct)
                .HasColumnName("correct");
                
            entity.Property(e => e.Incorrect)
                .HasColumnName("incorrect");
                
            entity.Property(e => e.Streak)
                .HasColumnName("streak");
                
            entity.Property(e => e.LastCorrect)
                .HasColumnName("last_correct");
                
            entity.Property(e => e.Difficulty)
                .HasColumnName("difficulty");
                
            entity.Property(e => e.Cardbox)
                .HasColumnName("cardbox");
                
            entity.Property(e => e.NextScheduled)
                .HasColumnName("next_scheduled");
        });

        modelBuilder.Entity<QuestionHistory>(entity =>
        {
            entity.ToTable("next_Added");
            entity.HasKey(e => new { Question = e.QuestionText, e.TimeStamp });
            
            entity.Property(e => e.QuestionText)
                .HasColumnName("question")
                .HasColumnType("varchar(16)")
                .HasMaxLength(16);
                
            entity.Property(e => e.TimeStamp)
                .HasColumnName("timeStamp");
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException("Cardbox database is read-only. SaveChangesAsync is not allowed.");
    }

    public override int SaveChanges()
    {
        throw new InvalidOperationException("Cardbox database is read-only. SaveChanges is not allowed.");
    }
}
