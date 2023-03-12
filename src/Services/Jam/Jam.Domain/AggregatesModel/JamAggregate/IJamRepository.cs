using Jam.Domain.SeedWork;

namespace Jam.Domain.AggregatesModel.JamAggregate
{
    public interface IJamRepository : IRepository<Jam>
    {
        Task<Jam> AddAsync(Jam jam);
        Task<Jam> GetAsync(int id);
        Task<IEnumerable<Jam>> GetByStatusAsync(int statusId);
    }
}
