using User.API.Application.Models;

namespace User.API.Application.Queries
{
    public interface IUserProfileQueries
    {
        Task<UserProfileResponse> GetUserProfileAsync(int id);
        Task<UserProfileResponse> ValidateUserAsync(string email, string password);
    }
}
