namespace MessagesAndEvents.Events
{
    public record UserRegisteredToJamEvent
    {
        public int JamId { get; }
        public int UserId { get; }
        public BandRoleTypeEnum PreferredRoleType { get; }
        public UserRegisteredToJamEvent(int jamId, int userId, BandRoleTypeEnum preferredRoleType)
        {
            JamId = jamId;
            UserId = userId;
            PreferredRoleType = preferredRoleType;
        }
    }
    public enum BandRoleTypeEnum
    {
        Vocalist = 1,
        LeadGuitarist = 2,
        RhythmGuitarist = 3,
        BassGuitarist = 4,
        Drummer = 5,
        KeyboardPlayer = 6
    }
}
