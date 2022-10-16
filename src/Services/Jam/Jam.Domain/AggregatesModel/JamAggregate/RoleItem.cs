using Jam.Domain.SeedWork;

namespace Jam.Domain.AggregatesModel.JamAggregate
{
    public class RoleItem : Entity
    {
        public int JamId { get; private set; }
        public Jam Jam { get; private set; }
        public int RoleTypeId { get; private set; }
        public int? RegisteredUserId { get; private set; }
        public int RoleItemStatusId { get; private set; }
        protected RoleItem() { }
        public RoleItem(int roleId, int jamId) : this()
        {
            JamId = jamId;
            RoleTypeId = roleId;
            RoleItemStatusId = RoleItemStatus.Created.Id;
        }
        internal void SetRegisteredUserId(int userId)
        {
            if (RoleItemStatusId.Equals(RoleItemStatus.Created.Id) && RegisteredUserId == null)
            {
                RegisteredUserId = userId;
                RoleItemStatusId = RoleItemStatus.Pending.Id;
            }
        }
        internal void SetStatusCompleted(int userId)
        {
            if (RoleItemStatusId.Equals(RoleItemStatus.Pending.Id) && RegisteredUserId == userId)
            {
                RoleItemStatusId = RoleItemStatus.Completed.Id;
            }
        }
        internal void SetStatusCreated(int userId)
        {
            if (RoleItemStatusId.Equals(RoleItemStatus.Pending.Id) && RegisteredUserId == userId)
            {
                RoleItemStatusId = RoleItemStatus.Created.Id;
                RegisteredUserId = null;
            }
        }
    }
}
