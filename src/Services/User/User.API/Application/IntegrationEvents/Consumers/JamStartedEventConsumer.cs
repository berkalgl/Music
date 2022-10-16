using MassTransit;
using MediatR;
using MessagesAndEvents.Events;
using User.API.Application.Commands;

namespace User.API.Application.IntegrationEvents.Consumers
{
    public class JamStartedEventConsumer : IConsumer<JamStartedEvent>
    {
        private readonly ILogger<JamStartedEventConsumer> _logger;
        private readonly IMediator _mediator;
        public JamStartedEventConsumer(ILogger<JamStartedEventConsumer> logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        /// <summary>
        /// Event handler which handles the jam start process
        /// Therefore, the registiration success continues for notification. 
        /// </summary>
        /// <param name="event">       
        /// </param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<JamStartedEvent> context)
        {
            _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", context.RequestId, Program.AppName, nameof(context.Message));

            var command = new SendMailNotificationToUserCommand(context.Message.JamId, 
                context.Message.Users.Select(u => new Commands.UserWithRoleItem(u.UserId, (Enums.BandRoleTypeEnum)u.AssignedRole)).ToList());

            _logger.LogInformation(
                "----- Sending command: {CommandName} ({@Command})",
                command.GetType(),
                command);

            await _mediator.Send(command);
        }
    }
}
