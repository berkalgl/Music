using MassTransit;
using MediatR;
using MessagesAndEvents.Events;
using User.Domain.AggregatesModel.UserProfileAggregate;

namespace User.API.Application.Commands
{
    // Regular CommandHandler
    public class SendMailNotificationToUserCommandHandler : IRequestHandler<SendMailNotificationToUserCommand, bool>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IPublishEndpoint _publishEndPoint;
        private readonly ILogger<SendMailNotificationToUserCommandHandler> _logger;

        // Using DI to inject infrastructure persistence Repositories
        public SendMailNotificationToUserCommandHandler(IUserProfileRepository userProfileRepository, IPublishEndpoint publishEndpoint, ILogger<SendMailNotificationToUserCommandHandler> logger)
        {
            _userProfileRepository = userProfileRepository ?? throw new ArgumentNullException(nameof(userProfileRepository));
            _publishEndPoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        /// <summary>
        /// Handler which processes the command when
        /// a Jam started and executes notification for user
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<bool> Handle(SendMailNotificationToUserCommand request, CancellationToken cancellationToken)
        {
            // Update the UserProfile AggregateRoot
            var userProfiles = await _userProfileRepository.GetEmailsAsync(request.Users.Select(u => u.UserId));

            if (!userProfiles.Any())
                return false;

            _logger.LogInformation($"Publishing SendMailNotificationEvent with the jam Id {request.JamId}");
            var mailsBeSent = request.Users.Select(
                u => new MailItem(
                    userProfiles.FirstOrDefault(us => us.Id == u.UserId).Email, 
                    $"Jam with the id {request.JamId} has been started for you to attend, your role is {u.AssignedRole}")
                ).ToList();

            var sendMailNotificationEvent = new SendMailNotificationEvent(mailsBeSent);
            await _publishEndPoint.Publish(sendMailNotificationEvent);

            return true;
        }
    }
}
