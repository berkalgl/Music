using Jam.Domain.Exceptions;
using Jam.Domain.SeedWork;
using System.Linq;

namespace Jam.Domain.AggregatesModel.JamAggregate
{
    public class Jam : Entity, IAggregateRoot
    {
        // DDD Patterns comment
        // aligned with DDD Aggregates and Domain Entities (Instead of properties and property collections)
        public int CreatedHostId { get; private set; }
        // JamStatus is a Value Object pattern example persisted
        public int JamStatusId { get; private set; }
        public Status JamStatus { get; private set; }
        public int JamTypeId { get; private set; }
        public JamType JamType { get; private set; }
        private readonly List<RoleItem> _roleItems;
        public IReadOnlyCollection<RoleItem> RoleItems => _roleItems;
        protected Jam() 
        {
            _roleItems = new List<RoleItem>();
        }
        public Jam(int createdHostId, int jamTypeId) : this()
        {
            CreatedHostId = createdHostId;
            JamStatusId = Status.Pending.Id;
            JamTypeId = jamTypeId;
        }
        // DDD Patterns comment
        // This Jam AggregateRoot's method "AddRoleItem()" should be the only way to add Roles to the Jam,
        // so any behavior and validations are controlled by the AggregateRoot 
        // in order to maintain consistency between the whole Aggregate. 
        public Jam AddRoleItem(int roleId)
        {
            var existingRole = RoleItems.Where(r => r.Id == roleId).SingleOrDefault();

            if (existingRole == null)
            {
                _roleItems.Add(new RoleItem(roleId, Id));
            }

            return this;
        }
        public void StartJamStatus(int userId)
        {
            // Check if there userId match with host id
            if (userId != CreatedHostId)
                throw new JamDomainException("You do not have the permission the start this Jam");

            // Check if there is at least one unassigned role
            if (_roleItems.Where(ri => !ri.RoleItemStatusId.Equals(RoleItemStatus.Completed.Id) || ri.RegisteredUserId == null).Any())
                throw new JamDomainException("There is still unregistered band role");

            JamStatusId = Status.Started.Id;
        }
        public void RegisterPreferredRole(int userId, int preferredRole)
        {
            if(!JamTypeId.Equals(JamType.Public.Id))
                throw new JamDomainException("You can only register for public jams");

            var checkAnUserRegisterForTheRole = _roleItems.Where(ri => ri.RoleTypeId.Equals(preferredRole) && ri.RegisteredUserId != null);

            if(checkAnUserRegisterForTheRole.Any())
            {
                var pendingRegisteredRole =
                    checkAnUserRegisterForTheRole.Where(ri => ri.RoleItemStatusId.Equals(RoleItemStatus.Pending.Id)).Any();

                if (pendingRegisteredRole)
                    throw new JamDomainException("There is another user registered for the role but it is pending, try your luck for another time !");

                throw new JamDomainException("Another user has been already registered for the role, sorry...");

            }

            var checkRegisteredRole = _roleItems.Where(ri => ri.RegisteredUserId == userId);

            if (checkRegisteredRole.Any())
            {
                var pendingRegisteredRole = 
                    checkRegisteredRole.Where(ri => ri.RoleItemStatusId.Equals(RoleItemStatus.Pending.Id)).Any();

                if (pendingRegisteredRole)
                    throw new JamDomainException("You have still unresolved application for this jam");

                throw new JamDomainException("You have already role in the jam");
            }

            GetRoleItemByStatus(preferredRole).SetRegisteredUserId(userId);
        }
        public void UpdateRoleItemSuccessStatus(int userId, int preferredRole)
        {
            GetRoleItemByStatus(preferredRole).SetStatusCompleted(userId);
        }
        public void UpdateRoleItemFailedStatus(int userId, int preferredRole)
        {
            GetRoleItemByStatus(preferredRole).SetStatusCreated(userId);
        }
        private RoleItem GetRoleItemByStatus(int preferredRole)
        {
            var role = _roleItems.FirstOrDefault(ri => ri.RoleTypeId == preferredRole);

            if (role == null)
                throw new JamDomainException("Jam does not have an available band role");

            return role;
        }
    }
}
