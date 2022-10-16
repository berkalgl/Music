using MassTransit;
using MessagesAndEvents.Events;
using Microsoft.Extensions.Logging;
using Moq;
using User.API.Application.Commands;
using User.API.Application.Enums;
using User.Domain.AggregatesModel.UserProfileAggregate;

namespace User.UnitTests.Application
{
    public class CheckUserRoleValidationCommandHandlerTest
    {
        private readonly Mock<ILogger<CheckUserRoleValidationCommandHandler>> _logger;
        private readonly Mock<IUserProfileRepository> _userProfileRepository;
        private readonly Mock<IPublishEndpoint> _publishEndPoint;
        public CheckUserRoleValidationCommandHandlerTest()
        {
            _logger = new Mock<ILogger<CheckUserRoleValidationCommandHandler>>();
            _userProfileRepository = new Mock<IUserProfileRepository>();
            _publishEndPoint = new Mock<IPublishEndpoint>();
        }
        [Fact]
        public async Task Check_user_role_validation_command_handled_success()
        {
            var fakeCheckUserRoleValidationCommand = new CheckUserRoleValidationCommand(1, 1, API.Application.Enums.BandRoleTypeEnum.Vocalist);
            var cltToken = default(CancellationToken);

            _userProfileRepository.Setup(userProfile => userProfile.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(fakeUserWithRole()));

            _publishEndPoint.Setup(publishEndPoint => publishEndPoint.Publish<UserValidationRegisterJamSuccessEvent>(
                It.IsAny<object>(), It.IsAny<CancellationToken>()));

            //Act
            var handler = new CheckUserRoleValidationCommandHandler(_logger.Object, _userProfileRepository.Object, _publishEndPoint.Object);
            var result = await handler.Handle(fakeCheckUserRoleValidationCommand, cltToken);

            //Assert
            Assert.True(result);
        }
        [Fact]
        public async Task Check_user_role_validation_command_handled_failed_user_not_found()
        {
            var fakeCheckUserRoleValidationCommand = new CheckUserRoleValidationCommand(1, 1, API.Application.Enums.BandRoleTypeEnum.Vocalist);
            var cltToken = default(CancellationToken);

            _userProfileRepository.Setup(userProfile => userProfile.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<UserProfile>(null));

            _publishEndPoint.Setup(publishEndPoint => publishEndPoint.Publish<UserValidationRegisterJamFailedEvent>(
                It.IsAny<object>(), It.IsAny<CancellationToken>()));

            //Act
            var handler = new CheckUserRoleValidationCommandHandler(_logger.Object, _userProfileRepository.Object, _publishEndPoint.Object);
            var result = await handler.Handle(fakeCheckUserRoleValidationCommand, cltToken);

            //Assert
            Assert.False(result);
        }
        [Fact]
        public async Task Check_user_role_validation_command_handled_failed_user_has_no_role()
        {
            var fakeCheckUserRoleValidationCommand = new CheckUserRoleValidationCommand(1, 1, API.Application.Enums.BandRoleTypeEnum.Vocalist);
            var cltToken = default(CancellationToken);

            _userProfileRepository.Setup(userProfile => userProfile.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<UserProfile>(fakeUserWithNoRole()));

            _publishEndPoint.Setup(publishEndPoint => publishEndPoint.Publish<UserValidationRegisterJamFailedEvent>(
                It.IsAny<object>(), It.IsAny<CancellationToken>()));

            //Act
            var handler = new CheckUserRoleValidationCommandHandler(_logger.Object, _userProfileRepository.Object, _publishEndPoint.Object);
            var result = await handler.Handle(fakeCheckUserRoleValidationCommand, cltToken);

            //Assert
            Assert.False(result);
        }

        private UserProfile fakeUserWithNoRole()
        {
            return new UserProfile("fakeEmail", "fakePassword", "Player");
        }
        private UserProfile fakeUserWithRole()
        {
            return new UserProfile("fakeEmail", "fakePassword", "Player").AddBandRoleType(1);
        }
    }
}