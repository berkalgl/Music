using User.API.Application.Enums;
using User.API.Application.Models;
using User.Domain.AggregatesModel.UserProfileAggregate;

namespace User.API.Application.Queries
{
    public class UserProfileQueries : IUserProfileQueries
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public UserProfileQueries(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public async Task<UserProfileResponse> GetUserProfileAsync(int id)
        {
            return UserProfileResponse.FromUserProfile(await _userProfileRepository.GetByAsync(id));
        }

        public async Task<UserProfileResponse> ValidateUserAsync(string email, string password)
        {
            return UserProfileResponse.FromUserProfile(await _userProfileRepository.GetByAsync(email, password));
        }
    }
}
