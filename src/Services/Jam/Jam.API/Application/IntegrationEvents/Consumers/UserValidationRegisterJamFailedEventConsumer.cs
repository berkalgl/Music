using Jam.API.Application.Commands;
using MassTransit;
using MediatR;
using MessagesAndEvents.Events;

namespace Jam.API.Application.IntegrationEvents.Consumers
{
    public class UserValidationRegisterJamFailedEventConsumer : IConsumer<UserValidationRegisterJamFailedEvent>
    {
        private readonly ILogger<UserValidationRegisterJamFailedEventConsumer> _logger;
        private readonly IMediator _mediator;

        public UserValidationRegisterJamFailedEventConsumer
            (ILogger<UserValidationRegisterJamFailedEventConsumer> logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        /// <summary>
        /// Event handler which handles that the user registiration failed event
        /// Therefore, the registiration will be updated. 
        /// </summary>
        /// <param name="event">       
        /// </param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<UserValidationRegisterJamFailedEvent> context)
        {
            _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", context.RequestId, Program.AppName, nameof(context.Message));

            var command = new UpdateJamRoleItemStatusFailedCommand(context.Message.JamId, context.Message.UserId, (Enums.BandRoleTypeEnum)context.Message.PreferredRoleType);

            _logger.LogInformation(
                "----- Sending command: {CommandName} ({@Command})",
                command.GetType(),
                command);

            await _mediator.Send(command);
        }
    }
}
