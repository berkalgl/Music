using Jam.Domain.AggregatesModel.JamAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jam.Infrastructure
{
    public static class SeedData
    {
        public static void Initialize(JamDbContext jamDbContext)
        {
            SeedJams(jamDbContext);
        }

        private static void SeedJams(JamDbContext jamDbContext)
        {
            if (!jamDbContext.Jams.Any())
            {
                var jams = new List<Domain.AggregatesModel.JamAggregate.Jam>
                {
                    new Domain.AggregatesModel.JamAggregate.Jam(1, 1).AddRoleItem(1).AddRoleItem(2).AddRoleItem(3),
                    new Domain.AggregatesModel.JamAggregate.Jam(1, 2).AddRoleItem(4).AddRoleItem(5).AddRoleItem(6),
                    new Domain.AggregatesModel.JamAggregate.Jam(2, 2).AddRoleItem(1).AddRoleItem(2).AddRoleItem(3),
                };

                jamDbContext.Jams.AddRange(jams);
                jamDbContext.SaveChanges();
            }
        }
    }
}
