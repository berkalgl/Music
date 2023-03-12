using Jam.API.Application.Enums;
using Jam.API.Application.Models;
using MediatR;

namespace Jam.API.Application.Commands
{
    // DDD and CQRS patterns command
    // In this case, its immutability is achieved by having all the setters as private
    // plus only being able to update the data just once, when creating the object through its constructor.
    public class CreateJamCommand : IRequest<JamResponse>
    {
        public int HostId { get; }
        public JamTypeEnum JamType { get; }
        public List<BandRoleTypeEnum> Roles { get; }
        public CreateJamCommand(int hostId, JamTypeEnum jamType, List<BandRoleTypeEnum> roles)
        {
            HostId = hostId;
            JamType = jamType;
            Roles = roles;
        }
    }
}
