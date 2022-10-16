using Microsoft.EntityFrameworkCore;
using Jam.Domain.AggregatesModel.JamAggregate;
using Jam.Domain.SeedWork;
using System.Reflection.Metadata;

namespace Jam.Infrastructure
{
    public class JamDbContext : DbContext, IUnitOfWork
    {
        public const string DEFAULT_SCHEMA = "JamDbSchema";
        public DbSet<Domain.AggregatesModel.JamAggregate.Jam> Jams { get; set; }
        public DbSet<RoleItem> RoleItems { get; set; }
        public DbSet<RoleItemStatus> RoleItemStatuses { get; set; }
        public DbSet<RoleType> RoleTypes { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<JamType> JamTypes { get; set; }
        public JamDbContext(DbContextOptions<JamDbContext> options) : base(options) { }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await base.SaveChangesAsync(cancellationToken);
            return true;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

    }
}
