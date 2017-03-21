using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Leitner.Persistence
{
    internal class LeitnerContext : DbContext
    {
        public DbSet<Question> Questions { get; set; }

        public DbSet<Cardbox> Cardboxes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("word");

            EntityTypeConfiguration<Question> cardboxConfiguration = modelBuilder.Entity<Question>();
            cardboxConfiguration.HasKey(x => x.Id);
            cardboxConfiguration.HasRequired(x => x.Cardbox);
            cardboxConfiguration.Property(x => x.Text).IsRequired().HasColumnType("nvarchar").HasMaxLength(15);
            cardboxConfiguration.Property(x => x.QuestionType).IsRequired();
            cardboxConfiguration.Property(x => x.DateAddedValue).IsRequired().HasColumnType("date");
            cardboxConfiguration.Ignore(x => x.DateAdded);

            EntityTypeConfiguration<Cardbox> cardboxDefinitionConfiguration = modelBuilder.Entity<Cardbox>();
            cardboxDefinitionConfiguration.HasKey(x => x.Id);
            cardboxDefinitionConfiguration.Property(x => x.Duration).IsRequired();
            cardboxDefinitionConfiguration.Property(x => x.Number).IsRequired();
        }
    }
}
