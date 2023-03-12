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
        public async Task<IEnumerable<JamResponse>> GetAsync(JamStatusEnum jamStatus)
        {
            var jams = await _jamRepository.GetByStatusAsync((int)jamStatus);
            
            return jams
                .Select(jam => 
                    new JamResponse(
                        jam.Id, 
                        jam.CreatedHostId, 
                        jam.RoleItems.Where(ri => ri.RegisteredUserId == null)
                        .Select(ri => (BandRoleTypeEnum)ri.RoleTypeId).ToList()
                        ))
                .ToList();
        }
        public async Task<JamResponse> GetAsync(int id)
        {
            var jam = await _jamRepository.GetAsync(id);
            return new JamResponse(
                jam.Id, 
                jam.CreatedHostId, 
                jam.RoleItems.Where(ri => ri.RegisteredUserId == null)
                .Select(ri => (BandRoleTypeEnum)ri.RoleTypeId).ToList()
                );
        }
    }
}
