using MediatR;
using User.API.Application.Enums;

namespace User.API.Application.Commands
{
    // DDD and CQRS patterns command
    // In this case, its immutability is achieved by having all the setters as private
    // plus only being able to update the data just once, when creating the object through its constructor.
    public class CreateUserProfileCommand : IRequest<bool>
    {
        public string Email { get; private set; }
        public string Password { get; private set; }
        public UserRoleEnum UserType { get; private set; }
        public IEnumerable<BandRoleTypeEnum> BandRoleTypes { get; private set; }
        public CreateUserProfileCommand(string email, string password, UserRoleEnum userType, IEnumerable<BandRoleTypeEnum> bandRoleTypeEnums)
        {
            Email = email;
            Password = password;
            UserType = userType;
            BandRoleTypes = bandRoleTypeEnums;
        }
    }
}
