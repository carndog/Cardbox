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
            entity.HasKey(e => e.question);
            
            entity.Property(e => e.question)
                .HasColumnName("question")
                .HasColumnType("varchar(16)")
                .HasMaxLength(16);
                
            entity.Property(e => e.correct)
                .HasColumnName("correct");
                
            entity.Property(e => e.incorrect)
                .HasColumnName("incorrect");
                
            entity.Property(e => e.streak)
                .HasColumnName("streak");
                
            entity.Property(e => e.last_correct)
                .HasColumnName("last_correct");
                
            entity.Property(e => e.difficulty)
                .HasColumnName("difficulty");
                
            entity.Property(e => e.cardbox)
                .HasColumnName("cardbox");
                
            entity.Property(e => e.next_scheduled)
                .HasColumnName("next_scheduled");
        });

        modelBuilder.Entity<QuestionHistory>(entity =>
        {
            entity.ToTable("next_Added");
            entity.HasKey(e => new { e.question, e.timeStamp });
            
            entity.Property(e => e.question)
                .HasColumnName("question")
                .HasColumnType("varchar(16)")
                .HasMaxLength(16);
                
            entity.Property(e => e.timeStamp)
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
