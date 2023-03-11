using MediatR;
using User.API.Application.Models;
using User.Domain.AggregatesModel.UserProfileAggregate;

namespace User.API.Application.Commands
{
    // Regular CommandHandler
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, UserProfileResponse>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ILogger<UpdateUserProfileCommandHandler> _logger;

        // Using DI to inject infrastructure persistence Repositories
        public UpdateUserProfileCommandHandler(IUserProfileRepository userProfileRepository, ILogger<UpdateUserProfileCommandHandler> logger)
        {
            _userProfileRepository = userProfileRepository;
            _logger = logger;
        }

        /// <summary>
        /// Handler which processes the command when
        /// a user created their profile
        /// </summary>
        /// <returns></returns>
        public async Task<UserProfileResponse> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var userProfile = await _userProfileRepository.GetByAsync(request.Id);
            if (userProfile is null) return null;

            userProfile.UpdateUserType(request.Role.ToString());
            _userProfileRepository.Update(userProfile);
            await _userProfileRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            _logger.LogInformation($"{userProfile.Id} Updated !");

            return UserProfileResponse.FromUserProfile(userProfile);

        }
    }
}
