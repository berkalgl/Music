using Jam.Domain.AggregatesModel.JamAggregate;
using Jam.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Jam.Infrastructure.Repositories
{
    public class JamRepository : IJamRepository
    {
        private readonly JamDbContext _context;
        public IUnitOfWork UnitOfWork => _context;
        public JamRepository(JamDbContext context)
        {
            _context = context;
        }
        public Domain.AggregatesModel.JamAggregate.Jam Add(Domain.AggregatesModel.JamAggregate.Jam jam)
        {
            if (jam.IsTransient())
                return _context.Jams.Add(jam).Entity;

            return jam;
        }

        public async Task<IEnumerable<Domain.AggregatesModel.JamAggregate.Jam>> GetJamsByStatus(int statusId)
        {
            var list = await _context.Jams.Where(j => j.JamStatusId.Equals(statusId))
                .Include(j => j.RoleItems)
                .ToListAsync();
            return list;
        }
        public async Task<Domain.AggregatesModel.JamAggregate.Jam> GetAsync(int jamId)
        {
            var jam = await _context.Jams.FirstOrDefaultAsync(o => o.Id == jamId);
            if (jam == null)
            {
                jam = _context.Jams.Local.FirstOrDefault(o => o.Id == jamId);
            }

            if (jam != null)
            {
                await _context.Entry(jam)
                    .Collection(i => i.RoleItems).LoadAsync();
                await _context.Entry(jam)
                    .Reference(i => i.JamStatus).LoadAsync();
                await _context.Entry(jam)
                    .Reference(i => i.JamType).LoadAsync();
            }

            return jam;
        }
    }
}
