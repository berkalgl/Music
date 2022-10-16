using Jam.API.Application.Enums;
using Jam.API.Application.Models;
using Jam.Domain.AggregatesModel.JamAggregate;

namespace Jam.API.Application.Queries
{
    public class JamQueries : IJamQueries
    {
        private readonly IJamRepository _jamRepository;
        public JamQueries(IJamRepository jamRepository) 
        {
            _jamRepository = jamRepository;
        }
        public async Task<IEnumerable<JamResponse>> GetJamsByStatus(JamStatusEnum jamStatus)
        {
            var jams = await _jamRepository.GetJamsByStatus((int)jamStatus);
            return MapJams(jams);
        }
        private IEnumerable<JamResponse> MapJams(IEnumerable<Domain.AggregatesModel.JamAggregate.Jam> jams)
        {
            return jams
                .Select(jam => new JamResponse() 
                { 
                    Id = jam.Id,
                    AvailableRoles = jam.RoleItems.Where(ri => ri.RegisteredUserId == null).Select(ri => (BandRoleTypeEnum)ri.RoleTypeId).ToList(),
                    CreatedHostId = jam.CreatedHostId
                })
                .ToList();
        }
    }
}
