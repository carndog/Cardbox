using CardboxDataLayer.Entities;
using CardboxDataLayer;
using Microsoft.EntityFrameworkCore;

namespace CardboxDataLayerTests;

public class TestDbContext : CardboxDbContext
{
    public TestDbContext(DbContextOptions<CardboxDbContext> options) : base(options)
    {
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        return base.SaveChanges();
    }
}
