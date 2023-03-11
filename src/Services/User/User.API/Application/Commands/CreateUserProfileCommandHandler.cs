using MediatR;
using User.API.Application.Enums;
using User.API.Application.Models;
using User.Domain.AggregatesModel.UserProfileAggregate;

namespace User.API.Application.Commands
{
    // Regular CommandHandler
    public class CreateUserProfileCommandHandler : IRequestHandler<CreateUserProfileCommand, UserProfileResponse>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ILogger<CreateUserProfileCommandHandler> _logger;

        // Using DI to inject infrastructure persistence Repositories
        public CreateUserProfileCommandHandler(IUserProfileRepository userProfileRepository, ILogger<CreateUserProfileCommandHandler> logger)
        {
            _userProfileRepository = userProfileRepository;
            _logger = logger;
        }

        /// <summary>
        /// Handler which processes the command when
        /// a user created their profile
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<UserProfileResponse> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Creating new user profile one... with the email {request.Email}");
            var userProfile = new UserProfile(request.Email, request.Password, request.UserType.ToString());

            if (userProfile.UserType.Equals(UserRoleEnum.Player.ToString()) && request.BandRoleTypes != null)
            {
                foreach (var roleType in request.BandRoleTypes)
                {
                    userProfile.AddBandRoleType((int)roleType);
                }
            }

            await _userProfileRepository.AddAsync(userProfile);
            await _userProfileRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            _logger.LogInformation($"{userProfile.Id} created !");

            return UserProfileResponse.FromUserProfile(userProfile);
        }
    }
}
