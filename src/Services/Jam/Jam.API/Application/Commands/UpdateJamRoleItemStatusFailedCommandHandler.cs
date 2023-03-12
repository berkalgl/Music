using Jam.Domain.AggregatesModel.JamAggregate;
using MediatR;

namespace Jam.API.Application.Commands
{
    // Regular CommandHandler
    public class UpdateJamRoleItemStatusFailedCommandHandler : IRequestHandler<UpdateJamRoleItemStatusFailedCommand, bool>
    {
        private readonly IJamRepository _jamRepository;
        private readonly ILogger<UpdateJamRoleItemStatusFailedCommandHandler> _logger;
        // Using DI to inject infrastructure persistence Repositories
        public UpdateJamRoleItemStatusFailedCommandHandler(IJamRepository jamRepository, ILogger<UpdateJamRoleItemStatusFailedCommandHandler> logger)
        {
            _jamRepository = jamRepository ?? throw new ArgumentNullException(nameof(jamRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Handler which processes the command when
        /// event executes Update Jam Role Item Status Success
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(UpdateJamRoleItemStatusFailedCommand request, CancellationToken cancellationToken)
        {
            var jam = await _jamRepository.GetAsync(request.JamId);
            if (jam != null)
            {
                jam.UpdateRoleItemFailedStatus(request.UserId, (int)request.PreferredRole);

                _logger.LogInformation($"Update the jam's Role Item failed with jam id {jam.Id} and userId {request.UserId} and role {request.PreferredRole}");
            }

            var saved = await _jamRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            //TO-DO throw an event RegistrationCompleted

            return saved;
        }
    }
}
