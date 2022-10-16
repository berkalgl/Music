using Microsoft.EntityFrameworkCore;
using System;
using User.Domain.AggregatesModel.UserProfileAggregate;
using User.Domain.SeedWork;

namespace User.Infrastructure.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly UserDbContext _context;
        public IUnitOfWork UnitOfWork => _context;
        public UserProfileRepository(UserDbContext context)
        {
            _context = context;
        }
        public UserProfile Add(UserProfile userProfile)
        {
            if(userProfile.IsTransient())
                return _context.UserProfiles.Add(userProfile).Entity;

            return userProfile;
        }
        public UserProfile Update(UserProfile userProfile)
        {
            return _context.UserProfiles
                    .Update(userProfile)
                    .Entity;
        }
        public async Task<UserProfile> FindBy(string email)
        {
            return await _context.UserProfiles.Include(up => up.BandRoles).SingleOrDefaultAsync(up => up.Email.Equals(email));
        }
        public async Task<UserProfile> FindBy(string email, string password)
        {
            return await _context.UserProfiles.SingleOrDefaultAsync(up => up.Email.Equals(email) && up.Password.Equals(password));
        }
        public async Task<UserProfile> GetAsync(int id)
        {
            var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(o => o.Id == id);
            userProfile ??= _context.UserProfiles.Local.FirstOrDefault(o => o.Id == id);

            if (userProfile != null)
            {
                await _context.Entry(userProfile)
                    .Collection(i => i.BandRoles).LoadAsync();
            }

            return userProfile;
        }

        public async Task<IEnumerable<UserProfile>> GetEmailsAsync(IEnumerable<int> userIds)
        {
            return await _context.UserProfiles.Where(u => userIds.Contains(u.Id)).ToListAsync();
        }
    }
}
