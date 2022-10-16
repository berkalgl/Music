using Mail.BackgroundTasks.Consumers;
using MassTransit;

namespace Mail.BackgroundTasks
{
    public class Program
    {
        public static async Task Main()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("rabbitmq", "/", host =>
                {
                    host.Username("guest");
                    host.Password("guest");
                });

                cfg.ReceiveEndpoint("SendMailNotificationEvent", e =>
                {
                    e.Consumer<SendMailNotificationEventConsumer>();
                });
                Console.WriteLine("RabbitMQ Configuration done!");
            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            try
            {
                Console.WriteLine("Press enter to exit");

                await Task.Run(() => Console.ReadLine());
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}