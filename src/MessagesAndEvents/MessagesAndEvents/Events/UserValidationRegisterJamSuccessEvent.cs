namespace MessagesAndEvents.Events
{
    public record UserValidationRegisterJamSuccessEvent
    {
        public int JamId { get; }
        public int UserId { get; }
        public BandRoleTypeEnum PreferredRoleType { get; }
        public UserValidationRegisterJamSuccessEvent(int jamId, int userId, BandRoleTypeEnum preferredRoleType)
        {
            JamId = jamId;
            UserId = userId;
            PreferredRoleType = preferredRoleType;
        }
    }
}
