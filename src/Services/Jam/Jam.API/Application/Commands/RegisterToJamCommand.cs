using Jam.API.Application.Enums;
using MediatR;

namespace Jam.API.Application.Commands
{
    // DDD and CQRS patterns command
    // In this case, its immutability is achieved by having all the setters as private
    // plus only being able to update the data just once, when creating the object through its constructor.
    public class RegisterToJamCommand : IRequest<bool>
    {
        public int JamId { get; }
        public int UserId { get; }
        public BandRoleTypeEnum PreferredRole { get; }
        public RegisterToJamCommand(int jamId, int userId, BandRoleTypeEnum preferredRole) 
        {
            JamId = jamId;
            UserId = userId;
            PreferredRole = preferredRole;
        }
    }
}
