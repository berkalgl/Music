using Jam.API.Application.Enums;
using MediatR;

namespace Jam.API.Application.Commands
{
    // DDD and CQRS patterns command
    // In this case, its immutability is achieved by having all the setters as private
    // plus only being able to update the data just once, when creating the object through its constructor.
    public class CreateJamCommand : IRequest<bool>
    {
        public int HostId { get; private set; }
        public JamTypeEnum JamType { get; private set; }
        public List<BandRoleTypeEnum> Roles { get; private set; }
        public CreateJamCommand(int hostId, JamTypeEnum jamType, List<BandRoleTypeEnum> roles)
        {
            HostId = hostId;
            JamType = jamType;
            Roles = roles;
        }
    }
}
