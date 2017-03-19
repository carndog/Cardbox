using System.Data.Entity;

namespace CardboxDefinition
{
    internal class CardboxDefinitionContext : DbContext
    {
        public DbSet<CardboxDefinition> Definitions { get; set; }
    }
}
