using Jam.API.Application.Commands;
using Jam.API.Application.Enums;

namespace Jam.API.Application.Models
{
    public record JamResponse
    {
        public int Id { get; init; }
        public int CreatedHostId { get; init; }
        public List<BandRoleTypeEnum> AvailableRoles { get; set; }
    }
    public record StartJamRequest
    {
        public int JamId { get; init; }
    }
    public record CreateJamRequest
    {
        public JamTypeEnum JamType { get; set; }
        public List<BandRoleTypeEnum> Roles { get; set; }
    }
    public record RegisterToJamRequest
    {
        public int JamId { get; init; }
        public BandRoleTypeEnum PreferedRole { get; set; }

    }
}
