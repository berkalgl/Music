using MassTransit;
using MessagesAndEvents.Events;
using Microsoft.Extensions.Logging;
using Moq;
using User.API.Application.Commands;
using User.API.Application.Enums;
using User.Domain.AggregatesModel.UserProfileAggregate;

namespace User.UnitTests.Application.HandlerTests
{
    public class CreateUserProfileCommandHandlerTest
    {
        private readonly Mock<ILogger<CreateUserProfileCommandHandler>> _logger;
        private readonly Mock<IUserProfileRepository> _userProfileRepository;
        public CreateUserProfileCommandHandlerTest()
        {
            _logger = new Mock<ILogger<CreateUserProfileCommandHandler>>();
            _userProfileRepository = new Mock<IUserProfileRepository>();
        }
        [Fact]
        public async Task Create_user_profile_command_handled_success()
        {
            var fakeCreateUserProfileCommand =
                new CreateUserProfileCommand("FakeEmail", "FakePassword", UserRoleEnum.Player,
                new List<API.Application.Enums.BandRoleTypeEnum> { API.Application.Enums.BandRoleTypeEnum.Vocalist });
            var cltToken = default(CancellationToken);

            _userProfileRepository.Setup(userProfile => userProfile.GetByAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<UserProfile>(null));

            _userProfileRepository.Setup(userProfile => userProfile.AddAsync(It.IsAny<UserProfile>()))
                .Returns(Task.FromResult(fakeUserWithNoRole()));

            _userProfileRepository.Setup(userProfile => userProfile.UnitOfWork.SaveEntitiesAsync(cltToken))
                .Returns(Task.FromResult(true));

            //Act
            var handler = new CreateUserProfileCommandHandler(_userProfileRepository.Object, _logger.Object);
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