using Jam.Domain.SeedWork;

namespace Jam.Domain.AggregatesModel.JamAggregate
{
    public interface IJamRepository : IRepository<Jam>
    {
        Jam Add(Jam jam);
        Task<Jam> GetAsync(int jamId);
        Task<IEnumerable<Jam>> GetJamsByStatus(int statusId);
    }
}
