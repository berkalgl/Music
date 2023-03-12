﻿using Jam.API.Application.Commands;
using MassTransit;
using MediatR;
using MessagesAndEvents.Events;

namespace Jam.API.Application.IntegrationEvents.Consumers
{
    public class UserValidationRegisterJamSuccessEventConsumer : IConsumer<UserValidationRegisterJamSuccessEvent>
    {
        private readonly ILogger<UserValidationRegisterJamSuccessEventConsumer> _logger;
        private readonly IMediator _mediator;

        public UserValidationRegisterJamSuccessEventConsumer
            (ILogger<UserValidationRegisterJamSuccessEventConsumer> logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Event handler which handles that the user registiration failed event
        /// Therefore, the registiration will be updated. 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<UserValidationRegisterJamSuccessEvent> context)
        {
            _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", context.RequestId, Program.AppName, nameof(context.Message));

            var command = new UpdateJamRoleItemStatusSuccessCommand(context.Message.JamId, context.Message.UserId, (Enums.BandRoleTypeEnum)context.Message.PreferredRoleType);

            _logger.LogInformation(
                "----- Sending command: {CommandName} ({@Command})",
                command.GetType(),
                command);

            await _mediator.Send(command);
        }
    }
}
