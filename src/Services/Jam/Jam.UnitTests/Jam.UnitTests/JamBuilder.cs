using Jam.Domain.AggregatesModel.JamAggregate;

namespace Jam.UnitTests
{
    public class JamBuilder
    {
        private readonly Jam.Domain.AggregatesModel.JamAggregate.Jam jam;

        public JamBuilder(int createdHostId, int jamTypeId)
        {
            jam = new Jam.Domain.AggregatesModel.JamAggregate.Jam(createdHostId, jamTypeId);
        }
        public JamBuilder AddRoleItem(int roleId)
        {
            jam.AddRoleItem(roleId);
            return this;
        }
        public JamBuilder RegisterPreferredRole(int userId, int preferredRoleId)
        {
            jam.RegisterPreferredRole(userId, preferredRoleId); 
            return this;
        }
        public JamBuilder UpdateRoleItemSuccessStatus(int userId, int preferredRoleId)
        {
            jam.UpdateRoleItemSuccessStatus(userId, preferredRoleId);
            return this;
        }
        public Jam.Domain.AggregatesModel.JamAggregate.Jam Build()
        {
            return jam;
        }
    }
}
