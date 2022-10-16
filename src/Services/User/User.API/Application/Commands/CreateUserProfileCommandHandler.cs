using MediatR;
using User.API.Application.Enums;
using User.Domain.AggregatesModel.UserProfileAggregate;

namespace User.API.Application.Commands
{
    // Regular CommandHandler
    public class CreateUserProfileCommandHandler : IRequestHandler<CreateUserProfileCommand, bool>
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
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<bool> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var userProfile = await _userProfileRepository.FindBy(request.Email);
            bool userProfileExisted = userProfile != null;


            if(!userProfileExisted)
            {
                _logger.LogInformation($"Could not find the existing User, creating new one... with the email {request.Email}");
                userProfile = new UserProfile(request.Email, request.Password, request.UserType.ToString());
            }

            if (userProfile.UserType.Equals(UserRoleEnum.Player.ToString()) && request.BandRoleTypes != null)
            {
                foreach (var roleType in request.BandRoleTypes)
                {
                    userProfile.AddBandRoleType((int)roleType);
                }
            }

            var addOrUptadeUserProfile = userProfileExisted ?
                _userProfileRepository.Update(userProfile) : _userProfileRepository.Add(userProfile);

            _logger.LogInformation($"{addOrUptadeUserProfile.Email} {(userProfileExisted ? "updated" : "created" )} !");

            return await _userProfileRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        }
    }
}
