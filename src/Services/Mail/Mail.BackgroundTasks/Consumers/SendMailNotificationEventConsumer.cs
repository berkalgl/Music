using MassTransit;
using MessagesAndEvents.Events;

namespace Mail.BackgroundTasks.Consumers
{
    public class SendMailNotificationEventConsumer : IConsumer<SendMailNotificationEvent>
    {
        public async Task Consume(ConsumeContext<SendMailNotificationEvent> context)
        {
            foreach(var mailItem in context.Message.Mails)
            {
                Console.WriteLine($"Sending Mail to ... {mailItem.Email} with the message ' {mailItem.Message}.. '");
            }
        }
    }
}
