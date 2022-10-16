using Jam.Domain.AggregatesModel.JamAggregate;
using MediatR;

namespace Jam.API.Application.Commands
{
    // Regular CommandHandler
    public class UpdateJamRoleItemStatusSuccessCommandHandler : IRequestHandler<UpdateJamRoleItemStatusSuccessCommand, bool>
    {
        private readonly IJamRepository _jamRepository;
        private readonly ILogger<UpdateJamRoleItemStatusSuccessCommandHandler> _logger;
        // Using DI to inject infrastructure persistence Repositories
        public UpdateJamRoleItemStatusSuccessCommandHandler(IJamRepository jamRepository, ILogger<UpdateJamRoleItemStatusSuccessCommandHandler> logger)
        {
            _jamRepository = jamRepository ?? throw new ArgumentNullException(nameof(jamRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        /// <summary>
        /// Handler which processes the command when
        /// event executes Update Jam Role Item Status Failed
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<bool> Handle(UpdateJamRoleItemStatusSuccessCommand request, CancellationToken cancellationToken)
        {
            var jam = await _jamRepository.GetAsync(request.JamId);
            if (jam != null)
            {
                jam.UpdateRoleItemSuccessStatus(request.UserId, (int)request.PreferredRole);

                _logger.LogInformation($"Update the jam's Role Item success with jam id {jam.Id} and userId {request.UserId} and role {request.PreferredRole}");
            }

            var saved = await _jamRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            //throw an event RegistrationFailed

            return saved;
        }
    }
}
