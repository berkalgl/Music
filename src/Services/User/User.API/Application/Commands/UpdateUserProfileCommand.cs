using MediatR;
using User.API.Application.Enums;
using User.API.Application.Models;

namespace User.API.Application.Commands
{
    // DDD and CQRS patterns command
    // In this case, its immutability is achieved by having all the setters as private
    // plus only being able to update the data just once, when creating the object through its constructor.
    public class UpdateUserProfileCommand : IRequest<UserProfileResponse>
    {
        public static UpdateUserProfileCommand FromRequest(int id, UpdateUserProfileRequest request)
        {
            return new UpdateUserProfileCommand(id, request.Role);
        }
        public int Id { get; }
        public UserRoleEnum Role { get; }
        public UpdateUserProfileCommand(int id, UserRoleEnum role)
        {
            Id = id;
            Role = role;
        }
    }
}
