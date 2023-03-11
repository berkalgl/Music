using User.Domain.SeedWork;

namespace User.Domain.AggregatesModel.UserProfileAggregate
{
    public interface IUserProfileRepository : IRepository<UserProfile>
    {
        Task<UserProfile> AddAsync(UserProfile userProfile);
        UserProfile Update(UserProfile userProfile);
        Task<UserProfile> GetByAsync(string email);
        Task<UserProfile> GetByAsync(string email, string password);
        Task<UserProfile> GetByAsync(int id);
        Task<IEnumerable<UserProfile>> GetEmailsAsync(IEnumerable<int> userIds);
    }
}
