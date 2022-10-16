using MassTransit;
using MediatR;
using MessagesAndEvents.Events;
using User.API.Application.Commands;
using BandRoleTypeEnum = User.API.Application.Enums.BandRoleTypeEnum;

namespace User.API.Application.IntegrationEvents.Consumers
{
    public class UserRegisteredToJamEventConsumer : IConsumer<UserRegisteredToJamEvent>
    {
        private readonly ILogger<UserRegisteredToJamEventConsumer> _logger;
        private readonly IMediator _mediator;
        public UserRegisteredToJamEventConsumer(ILogger<UserRegisteredToJamEventConsumer> logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        /// <summary>
        /// Event handler which confirms that the user has the specific band role
        /// Therefore, the registiration process continues for validation. 
        /// </summary>
        /// <param name="event">       
        /// </param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<UserRegisteredToJamEvent> context)
        {
            _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", context.RequestId, Program.AppName, nameof(context.Message));

            var command = new CheckUserRoleValidationCommand(context.Message.JamId, context.Message.UserId, (BandRoleTypeEnum)context.Message.PreferredRoleType);

            _logger.LogInformation(
                "----- Sending command: {CommandName} ({@Command})",
                command.GetType(),
                command);

            await _mediator.Send(command);
        }
    }
}
