using Jam.Domain.AggregatesModel.JamAggregate;
using Jam.Domain.Exceptions;
using MassTransit;
using MediatR;
using MessagesAndEvents.Events;

namespace Jam.API.Application.Commands
{
    // Regular CommandHandler
    public class StartJamCommandHandler : IRequestHandler<StartJamCommand, bool>
    {
        private readonly IJamRepository _jamRepository;
        private readonly IPublishEndpoint _publishEndPoint;
        private readonly ILogger<StartJamCommandHandler> _logger;
        // Using DI to inject infrastructure persistence Repositories
        public StartJamCommandHandler(IJamRepository jamRepository, IPublishEndpoint publishEndpoint, ILogger<StartJamCommandHandler> logger)
        {
            _jamRepository = jamRepository ?? throw new ArgumentNullException(nameof(jamRepository));
            _publishEndPoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Handler which processes the command when
        /// a host executes Start Jam command
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(StartJamCommand request, CancellationToken cancellationToken)
        {
            // Update the JAM AggregateRoot
            var jamToUpdate = await _jamRepository.GetAsync(request.JamId);
            if (jamToUpdate == null)
            {
                throw new JamDomainException("Could not find the jam");
            }

            jamToUpdate.StartJamStatus(request.UserId);

            var saved = await _jamRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            _logger.LogInformation($"Starting the jam id {jamToUpdate.Id}");

            if (!saved) return false;
            
            _logger.LogInformation($"Publishing JamStartedEvent jam id {jamToUpdate.Id}");
            var jamStartedEvent =
                new JamStartedEvent(
                    jamToUpdate.Id, 
                    jamToUpdate.RoleItems.Select(ri => new UserWithRoleItem((int)ri.RegisteredUserId, (BandRoleTypeEnum)ri.RoleTypeId)).ToList()
                );
            await _publishEndPoint.Publish(jamStartedEvent, cancellationToken);

            return true;
        }
    }
}
