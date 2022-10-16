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

        public async Task<UserProfileResponse> ValidateUserAsync(string email, string password)
        {
            return MapUserProfile(await _userProfileRepository.FindBy(email, password));
        }
        private static UserProfileResponse MapUserProfile(UserProfile userProfile)
        {
            if (userProfile == null)
                return new UserProfileResponse();

            return new UserProfileResponse()
            {
                Id = userProfile.Id,
                Role = userProfile.UserType
            };
        }
    }
}
