using User.Domain.SeedWork;

namespace User.Domain.AggregatesModel.UserProfileAggregate
{
    public class UserBandRole : Entity
    {
        public int UserId { get; private set; }
        public UserProfile UserProfile { get; private set; }
        public int RoleTypeId { get; private set; }
        public BandRoleType RoleType { get; private set; }
        protected UserBandRole() { }
        public UserBandRole(int bandRoleTypeId, int userId)
        {
            RoleTypeId = bandRoleTypeId;
            UserId = userId;
        }
    }
}
