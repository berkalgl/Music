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
    public class StartJamCommandHandlerTest
    {
        private readonly Mock<IJamRepository> _jamRepository;
        private readonly Mock<ILogger<StartJamCommandHandler>> _logger;
        private readonly Mock<IPublishEndpoint> _publishEndPoint;
        public StartJamCommandHandlerTest()
        {
            _jamRepository = new Mock<IJamRepository>();
            _logger = new Mock<ILogger<StartJamCommandHandler>>();
            _publishEndPoint = new Mock<IPublishEndpoint>();
        }
        [Fact]
        public async Task Start_jam_command_handled_success()
        {
            var fakeStartJamCmd = new StartJamCommand(1,1);

            _jamRepository.Setup(jamRepo => jamRepo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(fakeJam()));

            _jamRepository.Setup(jamRepo => jamRepo.UnitOfWork.SaveEntitiesAsync(default))
                .Returns(Task.FromResult(true));

            _publishEndPoint.Setup(publishEndPoint => publishEndPoint.Publish<JamStartedEvent>(
                It.IsAny<object>(), It.IsAny<CancellationToken>()));

            //Act
            var handler = new StartJamCommandHandler(_jamRepository.Object, _publishEndPoint.Object, _logger.Object);
            var cltToken = new CancellationToken();
            var result = await handler.Handle(fakeStartJamCmd, cltToken);

            //Assert
            Assert.True(result);
        }
        [Fact]
        public async Task Start_jam_command_handled_throws_could_not_find_jam()
        {
            var fakeStartJamCmd = new StartJamCommand(It.IsAny<int>(), It.IsAny<int>());

            _jamRepository.Setup(jamRepo => jamRepo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Jam.Domain.AggregatesModel.JamAggregate.Jam>(null));

            //Act
            var handler = new StartJamCommandHandler(_jamRepository.Object, _publishEndPoint.Object, _logger.Object);
            var cltToken = new CancellationToken();

            //Assert
            await Assert.ThrowsAsync<JamDomainException>(async () => await handler.Handle(fakeStartJamCmd, cltToken));
        }

        private Jam.Domain.AggregatesModel.JamAggregate.Jam fakeJam()
        {
            var jam = new JamBuilder(1,2)
                .AddRoleItem(1)
                .RegisterPreferredRole(3,1)
                .UpdateRoleItemSuccessStatus(3,1)
                .Build();
            return jam;
        }
    }
}
