using Microsoft.EntityFrameworkCore;
using User.Domain.AggregatesModel.UserProfileAggregate;
using User.Domain.SeedWork;

namespace User.Infrastructure
{
    public class UserDbContext : DbContext, IUnitOfWork
    {
        public const string DEFAULT_SCHEMA = "UserDbSchema";
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserBandRole> UserBandRoles { get; set; }
        public DbSet<BandRoleType> BandRoleTypes { get; set; }
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

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
