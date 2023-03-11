using User.Domain.SeedWork;

namespace User.Domain.AggregatesModel.UserProfileAggregate
{
    public class UserProfile : Entity, IAggregateRoot
    {
        // DDD Patterns comment
        // Using private fields, allowed since EF Core 1.1, is a much better encapsulation
        // aligned with DDD Aggregates and Domain Entities (Instead of properties and property collections)
        public string Email { get; }
        public string Password { get; }
        public string UserType { get; private set; }
        private readonly List<UserBandRole> _bandRoles;
        public IReadOnlyCollection<UserBandRole> BandRoles => _bandRoles;

        private UserProfile()
        {
            _bandRoles = new List<UserBandRole>();
        }
        public UserProfile(string email, string password, string userType) : this()
        {
            Email = email;
            Password = password;
            UserType = userType;
        }
        public UserProfile AddBandRoleType(int bandRoleTypeId)
        {
            var existingBandRoleInUserProfile = _bandRoles.Where(b => b.RoleTypeId == bandRoleTypeId)?.SingleOrDefault();

            if (existingBandRoleInUserProfile == null)
            {
                _bandRoles.Add(new UserBandRole(bandRoleTypeId, Id));
            }

            return this;
        }
        public bool HasTheRole(int preferredRoleTypeId)
        {
            return _bandRoles.Any(br => br.RoleTypeId == preferredRoleTypeId);
        }

        public UserProfile UpdateUserType(string role)
        {
            UserType = role;
            return this;
        }
    }
}
