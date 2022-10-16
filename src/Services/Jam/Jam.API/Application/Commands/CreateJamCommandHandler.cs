using Jam.API.Application.IntegrationEvents.Consumers;
using Jam.Domain.AggregatesModel.JamAggregate;
using MediatR;

namespace Jam.API.Application.Commands
{
    // Regular CommandHandler
    public class CreateJamCommandHandler : IRequestHandler<CreateJamCommand, bool>
    {
        private readonly IJamRepository _jamRepository;
        private readonly ILogger<CreateJamCommandHandler> _logger;
        // Using DI to inject infrastructure persistence Repositories
        public CreateJamCommandHandler(IJamRepository jamRepository, ILogger<CreateJamCommandHandler> logger)
        {
            _jamRepository = jamRepository ?? throw new ArgumentNullException(nameof(jamRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        /// <summary>
        /// Handler which processes the command when
        /// a host executes Create Jam command
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<bool> Handle(CreateJamCommand request, CancellationToken cancellationToken)
        {
            // Add the JAM AggregateRoot
            var jam = new Domain.AggregatesModel.JamAggregate.Jam(request.HostId, (int)request.JamType);

            if (request.Roles != null)
            {
                foreach (var roleType in request.Roles)
                {
                    jam.AddRoleItem((int)roleType);
                }
            }
            _jamRepository.Add(jam);

            _logger.LogInformation($"Creating Jam with the Id {jam.Id}");

            var result = await _jamRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return result;
        }
    }
}
