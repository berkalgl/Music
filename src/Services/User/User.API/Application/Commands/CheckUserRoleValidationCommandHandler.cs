using MassTransit;
using MediatR;
using MessagesAndEvents.Events;
using User.Domain.AggregatesModel.UserProfileAggregate;

namespace User.API.Application.Commands
{
    // Regular CommandHandler
    public class CheckUserRoleValidationCommandHandler : IRequestHandler<CheckUserRoleValidationCommand, bool>
    {
        private readonly ILogger<CheckUserRoleValidationCommandHandler> _logger;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IPublishEndpoint _publishEndPoint;

        // Using DI to inject infrastructure persistence Repositories
        public CheckUserRoleValidationCommandHandler(
            ILogger<CheckUserRoleValidationCommandHandler> logger, 
            IUserProfileRepository userProfileRepository, 
            IPublishEndpoint publishEndpoint)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userProfileRepository = userProfileRepository ?? throw new ArgumentNullException(nameof(userProfileRepository));
            _publishEndPoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }
        /// <summary>
        /// Handler which processes the command when
        /// User check if there is any role they created
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<bool> Handle(CheckUserRoleValidationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Checking user with id {request.UserId} has the role...");

            var user = await _userProfileRepository.GetByAsync(request.UserId);

            if (user == null)
            {
                ThrowUserValidationRegisterJamFailedEvent(request, "User could not be found !");
                return false;
            }

            var doesUserHaveTheRole = user.HasTheRole((int)request.PreferredRoleType);

            if (!doesUserHaveTheRole)
            {
                ThrowUserValidationRegisterJamFailedEvent(request, "User does not have the Role !");
                return false;
            }

            ThrowUserValidationRegisterJamSuccessEvent(request);
            return true;

        }

        private async void ThrowUserValidationRegisterJamSuccessEvent(CheckUserRoleValidationCommand request)
        {
            _logger.LogInformation($"Throwing User Validation Success for Validation with user id {request.UserId} and jam id {request.JamId}");
            //throw an event of UserValidationRegisterJamSuccess
            var userValidationRegisterJamSuccessEvent = 
                new UserValidationRegisterJamSuccessEvent(request.JamId, request.UserId, (BandRoleTypeEnum)request.PreferredRoleType);
            await _publishEndPoint.Publish(userValidationRegisterJamSuccessEvent);
        }

        private async void ThrowUserValidationRegisterJamFailedEvent(CheckUserRoleValidationCommand request, string message)
        {
            _logger.LogInformation($"Throwing User Validation Failed for Validation with user id {request.UserId} and jam id {request.JamId}");
            //throw an event of UserValidationRegisterJamFailed
            var userValidationRegisterJamFailedEvent = 
                new UserValidationRegisterJamFailedEvent(request.JamId, request.UserId, (BandRoleTypeEnum)request.PreferredRoleType, message);
            await _publishEndPoint.Publish(userValidationRegisterJamFailedEvent);
        }
    }
}
