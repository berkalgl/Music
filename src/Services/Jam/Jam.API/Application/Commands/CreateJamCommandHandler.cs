using Jam.API.Application.Enums;
using Jam.API.Application.Models;
using Jam.Domain.AggregatesModel.JamAggregate;
using MediatR;

namespace Jam.API.Application.Commands
{
    // Regular CommandHandler
    public class CreateJamCommandHandler : IRequestHandler<CreateJamCommand, JamResponse>
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
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<JamResponse> Handle(CreateJamCommand request, CancellationToken cancellationToken)
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
            await _jamRepository.AddAsync(jam);
            await _jamRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            _logger.LogInformation($"Creating Jam with the Id {jam.Id}");

            return new JamResponse(
                jam.Id, 
                jam.CreatedHostId, 
                jam.RoleItems.Where(ri => ri.RegisteredUserId == null)
                    .Select(ri => (BandRoleTypeEnum)ri.RoleTypeId).ToList());
        }
    }
}
