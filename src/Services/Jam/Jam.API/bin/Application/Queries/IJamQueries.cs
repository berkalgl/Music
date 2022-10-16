namespace Jam.API.Application.Queries
{
    public interface IJamQueries
    {
        Task<IEnumerable<JamDTO>> GetJamsAsync();
    }
}
