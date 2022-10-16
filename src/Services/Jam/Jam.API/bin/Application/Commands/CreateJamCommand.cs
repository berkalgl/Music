using MediatR;

namespace Jam.API.Application.Commands
{
    public class CreateJamCommand : IRequest<bool>
    {
        public int CreatedHostId { get; set; }
        public IEnumerable<BandRoleTypeEnum>? JamRoleTypes { get; set; }
        public CreateJamCommand(int createdHostId, IEnumerable<BandRoleTypeEnum> jamRoleTypes)
        {
            CreatedHostId = createdHostId;
            JamRoleTypes = jamRoleTypes;
        }
        public CreateJamCommand() { }
    }
    public enum BandRoleTypeEnum
    {
        Vocalist,
        LeadGuitarist,
        RhythmGuitarist,
        BassGuitarist,
        Drummer,
        KeyboardPlayer
    }
}
