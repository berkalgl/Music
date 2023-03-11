using User.API.Application.Enums;
using User.Domain.AggregatesModel.UserProfileAggregate;

namespace User.API.Application.Models
{
    public record UserProfileResponse
    {
        public static UserProfileResponse FromUserProfile(UserProfile userProfile)
        {
            if (userProfile == null)
                return null;

            return new UserProfileResponse()
            {
                Id = userProfile.Id,
                Role = userProfile.UserType,
                Email = userProfile.Email,
                BandRoleTypes = userProfile.BandRoles.Select(up => (BandRoleTypeEnum)up.RoleTypeId).ToList()
            };
        }
        public int Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public IEnumerable<BandRoleTypeEnum> BandRoleTypes { get; set; }
    }
    public record CreateUserProfileRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRoleEnum Role { get; set; }
        public IEnumerable<BandRoleTypeEnum> BandRoleTypes { get; set; }
    }
    public record UpdateUserProfileRequest
    {
        public UserRoleEnum Role { get; set; }
    }
}
