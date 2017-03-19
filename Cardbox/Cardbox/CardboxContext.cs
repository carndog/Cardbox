using System.Data.Entity;

namespace Cardbox
{
    internal class CardboxContext : DbContext
    {
        public DbSet<Cardbox> Definitions { get; set; }
    }
}
