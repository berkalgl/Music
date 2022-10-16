using Jam.API.Application.Commands;
using Jam.Domain.AggregatesModel.JamAggregate;
using Jam.Domain.Exceptions;
using MassTransit;
using MessagesAndEvents.Events;
using Microsoft.Extensions.Logging;
using Moq;
using BandRoleTypeEnum = Jam.API.Application.Enums.BandRoleTypeEnum;

namespace Jam.UnitTests.Application.HandlerTests
{
    public class RegisterToJamCommandHandlerTest
    {
        private readonly Mock<IJamRepository> _jamRepository;
        private readonly Mock<ILogger<RegisterToJamCommandHandler>> _logger;
        private readonly Mock<IPublishEndpoint> _publishEndPoint;
        public RegisterToJamCommandHandlerTest()
        {
            _jamRepository = new Mock<IJamRepository>();
            _logger = new Mock<ILogger<RegisterToJamCommandHandler>>();
            _publishEndPoint = new Mock<IPublishEndpoint>();
        }
        [Fact]
        public async Task Register_jam_command_handled_success()
        {
            var fakeRegisterJamCmd = new RegisterToJamCommand(It.IsAny<int>(), It.IsAny<int>(), BandRoleTypeEnum.Vocalist);

            _jamRepository.Setup(jamRepo => jamRepo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(fakeJam()));

            _jamRepository.Setup(jamRepo => jamRepo.UnitOfWork.SaveEntitiesAsync(default))
                .Returns(Task.FromResult(true));

            _publishEndPoint.Setup(publishEndPoint => publishEndPoint.Publish<UserRegisteredToJamEvent>(
                It.IsAny<object>(), It.IsAny<CancellationToken>()));

            //Act
            var handler = new RegisterToJamCommandHandler(_jamRepository.Object, _publishEndPoint.Object, _logger.Object);
            var cltToken = new CancellationToken();
            var result = await handler.Handle(fakeRegisterJamCmd, cltToken);

            //Assert
            Assert.True(result);
        }
        [Fact]
        public async Task Register_jam_command_handled_throws_could_not_find_jam()
        {
            var fakeRegisterJamCmd = new RegisterToJamCommand(It.IsAny<int>(), It.IsAny<int>(), BandRoleTypeEnum.Vocalist);

            _jamRepository.Setup(jamRepo => jamRepo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Jam.Domain.AggregatesModel.JamAggregate.Jam>(null));

            //Act
            var handler = new RegisterToJamCommandHandler(_jamRepository.Object, _publishEndPoint.Object, _logger.Object);
            var cltToken = new CancellationToken();

            //Assert
            await Assert.ThrowsAsync<JamDomainException>(async () => await handler.Handle(fakeRegisterJamCmd, cltToken));
        }

        private Jam.Domain.AggregatesModel.JamAggregate.Jam fakeJam()
        {
            return new Jam.Domain.AggregatesModel.JamAggregate.Jam(1, 2).AddRoleItem(1);
        }
    }
}
