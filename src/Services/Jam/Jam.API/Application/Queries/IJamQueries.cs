using Jam.API.Application.Enums;
using Jam.API.Application.Models;

namespace Jam.API.Application.Queries
{
    public interface IJamQueries
    {
        Task<IEnumerable<JamResponse>> GetAsync(JamStatusEnum jamStatus);
        Task<JamResponse> GetAsync(int id);
    }
}
