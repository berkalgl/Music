using MediatR;
using User.API.Application.Enums;
using User.API.Application.Models;

namespace User.API.Application.Commands
{
    // DDD and CQRS patterns command
    // In this case, its immutability is achieved by having all the setters as private
    // plus only being able to update the data just once, when creating the object through its constructor.
    public class CreateUserProfileCommand : IRequest<UserProfileResponse>
    {
        public static CreateUserProfileCommand FromRequest(CreateUserProfileRequest request)
        {
            return new CreateUserProfileCommand(request.Email, request.Password, request.Role,
                request.BandRoleTypes);
        }
        public string Email { get; }
        public string Password { get; }
        public UserRoleEnum UserType { get; }
        public IEnumerable<BandRoleTypeEnum> BandRoleTypes { get; }
        public CreateUserProfileCommand(string email, string password, UserRoleEnum userType, IEnumerable<BandRoleTypeEnum> bandRoleTypeEnums)
        {
            Email = email;
            Password = password;
            UserType = userType;
            BandRoleTypes = bandRoleTypeEnums;
        }
    }
}
