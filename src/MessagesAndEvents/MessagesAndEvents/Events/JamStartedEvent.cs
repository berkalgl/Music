namespace MessagesAndEvents.Events
{
    public record JamStartedEvent
    {
        public int JamId { get; set; }
        public List<UserWithRoleItem> Users { get; }
        public JamStartedEvent(int jamId, List<UserWithRoleItem> users)
        {
            JamId = jamId;
            Users = users;
        }
    }
    public record UserWithRoleItem
    {
        public int UserId { get; }
        public BandRoleTypeEnum AssignedRole { get; }
        public UserWithRoleItem(int userId, BandRoleTypeEnum assignedRole)
        {
            UserId = userId;
            AssignedRole = assignedRole;
        }
    }
}
