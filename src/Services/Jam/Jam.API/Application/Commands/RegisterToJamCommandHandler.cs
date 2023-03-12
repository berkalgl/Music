using Jam.Domain.AggregatesModel.JamAggregate;
using Jam.Domain.Exceptions;
using MassTransit;
using MediatR;
using MessagesAndEvents.Events;

namespace Jam.API.Application.Commands
{
    // Regular CommandHandler
    public class RegisterToJamCommandHandler : IRequestHandler<RegisterToJamCommand, bool>
    {
        private readonly IJamRepository _jamRepository;
        private readonly IPublishEndpoint _publishEndPoint;
        private readonly ILogger<RegisterToJamCommandHandler> _logger;
        // Using DI to inject infrastructure persistence Repositories
        public RegisterToJamCommandHandler(IJamRepository jamRepository, IPublishEndpoint publishEndpoint, ILogger<RegisterToJamCommandHandler> logger)
        {
            _jamRepository = jamRepository ?? throw new ArgumentNullException(nameof(jamRepository));
            _publishEndPoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Handler which processes the command when
        /// a player executes Register To Jam command
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(RegisterToJamCommand request, CancellationToken cancellationToken)
        {
            var jamToRegister = await _jamRepository.GetAsync(request.JamId);

            if(jamToRegister == null)
            {
                throw new JamDomainException("Could not find the jam to register");
            }

            jamToRegister.RegisterPreferredRole(request.UserId, (int)request.PreferredRole);

            var saved = await _jamRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            _logger.LogInformation($"Registered to role {request.PreferredRole} in the jam id {jamToRegister.Id}");

            if (saved)
            {
                _logger.LogInformation($"Publishing UserRegisteredToJamEvent to user {request.UserId}");
                var userRegisteredToJamEvent = new UserRegisteredToJamEvent(request.JamId, request.UserId, (BandRoleTypeEnum) request.PreferredRole);
                await _publishEndPoint.Publish(userRegisteredToJamEvent);
            }

            return saved;
        }
    }
}
