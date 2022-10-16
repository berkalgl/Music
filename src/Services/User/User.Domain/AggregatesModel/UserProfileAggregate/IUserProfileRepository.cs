using User.Domain.SeedWork;

namespace User.Domain.AggregatesModel.UserProfileAggregate
{
    public interface IUserProfileRepository : IRepository<UserProfile>
    {
        UserProfile Add(UserProfile userProfile);
        UserProfile Update(UserProfile userProfile);
        Task<UserProfile> FindBy(string email);
        Task<UserProfile> FindBy(string email, string password);
        Task<UserProfile> GetAsync(int id);
        Task<IEnumerable<UserProfile>> GetEmailsAsync(IEnumerable<int> userIds);
    }
}
