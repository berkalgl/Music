using User.Domain.AggregatesModel.UserProfileAggregate;

namespace User.Infrastructure
{
    public static class SeedData
    {
        public static void Initialize(UserDbContext userDbContext)
        {
            SeedUsers(userDbContext);
        }

        private static void SeedUsers(UserDbContext userDbContext)
        {
            if (!userDbContext.UserProfiles.Any())
            {
                var users = new List<UserProfile>
                {
                    new UserProfile("berkHost@mail.com", "berkHost", "Host"),
                    new UserProfile("berkHost2@mail.com", "berkHost2", "Host"),
                    new UserProfile("berkUser@mail.com", "berkUser", "Player").AddBandRoleType(1),
                    new UserProfile("berkUser2@mail.com", "berkUser2", "Player").AddBandRoleType(1).AddBandRoleType(3),
                    new UserProfile("berkUser3@mail.com", "berkUser3", "Player").AddBandRoleType(4)
                };

                userDbContext.UserProfiles.AddRange(users);
                userDbContext.SaveChanges();
            }
        }
    }
}
