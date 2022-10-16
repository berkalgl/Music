namespace MessagesAndEvents.Events
{
    public record SendMailNotificationEvent
    {
        public List<MailItem> Mails { get; }
        public SendMailNotificationEvent(List<MailItem> mails)
        {
            Mails = mails;
        }
    }
    public record MailItem
    {
        public string Email { get; }
        public string Message { get; }
        public MailItem(string email, string message)
        {
            Email = email;
            Message = message;
        }
    }
}
