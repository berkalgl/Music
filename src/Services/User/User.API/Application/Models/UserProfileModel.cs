using User.API.Application.Enums;

namespace User.API.Application.Models
{
    public record UserProfileResponse
    {
        public int Id { get; set; }
        public string Role { get; set; }
    }
    public record CreateUserProfileRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRoleEnum UserType { get; set; }
        public IEnumerable<BandRoleTypeEnum> BandRoleTypes { get; set; }
    }
}
