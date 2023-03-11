using Microsoft.Extensions.Logging;
using Moq;
using User.API.Application.Commands;
using User.API.Application.Enums;
using User.Domain.AggregatesModel.UserProfileAggregate;

namespace User.UnitTests.Application.HandlerTests
{
    public class UpdateUserProfileCommandHandlerTest
    {
        private readonly Mock<ILogger<UpdateUserProfileCommandHandler>> _logger;
        private readonly Mock<IUserProfileRepository> _userProfileRepository;
    
        public UpdateUserProfileCommandHandlerTest()
        {
            _logger = new Mock<ILogger<UpdateUserProfileCommandHandler>>();
            _userProfileRepository = new Mock<IUserProfileRepository>();
        }
        
        [Fact]
        public async Task Create_user_profile_command_handled_update_success()
        {
            var fakeCreateUserProfileCommand =
                new UpdateUserProfileCommand(1, UserRoleEnum.Player);
            var cltToken = default(CancellationToken);

            _userProfileRepository.Setup(userProfile => userProfile.GetByAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(fakeUserWithNoRole()));

            _userProfileRepository.Setup(userProfile => userProfile.Update(It.IsAny<UserProfile>()))
                .Returns(await Task.FromResult(fakeUserWithNoRole()));

            _userProfileRepository.Setup(userProfile => userProfile.UnitOfWork.SaveEntitiesAsync(cltToken))
                .Returns(Task.FromResult(true));

            //Act
            var handler = new UpdateUserProfileCommandHandler(_userProfileRepository.Object, _logger.Object);
            var result = await handler.Handle(fakeCreateUserProfileCommand, cltToken);

            //Assert
            Assert.NotNull(result);
        }
        private UserProfile fakeUserWithNoRole()
        {
            return new UserProfile("fakeEmail", "fakePassword", "Player");
        }
    }
}