namespace MessagesAndEvents.Events
{
    public record UserValidationRegisterJamFailedEvent
    {
        public int JamId { get; }
        public int UserId { get; }
        public BandRoleTypeEnum PreferredRoleType { get; }
        public string Message { get; }
        public UserValidationRegisterJamFailedEvent(int jamId, int userId, BandRoleTypeEnum preferredRoleType, string message) 
        {
            JamId = jamId;
            UserId = userId;
            PreferredRoleType = preferredRoleType;
            Message = message;
        }
    }
}
