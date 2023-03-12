using Jam.API.Application.Commands;
using Jam.API.Application.Enums;
using Jam.Domain.AggregatesModel.JamAggregate;
using Microsoft.Extensions.Logging;
using Moq;

namespace Jam.UnitTests.Application.HandlerTests
{
    public class CreateJamCommandHandlerTest
    {
        private readonly Mock<IJamRepository> _jamRepository;
        private readonly Mock<ILogger<CreateJamCommandHandler>> _logger;
        public CreateJamCommandHandlerTest()
        {
            _jamRepository = new Mock<IJamRepository>();
            _logger = new Mock<ILogger<CreateJamCommandHandler>>();
        }
        [Fact]
        public async Task Create_jam_command_handled_success()
        {
            var fakeCreateJamCmd = new CreateJamCommand(It.IsAny<int>(), JamTypeEnum.Public, new List<BandRoleTypeEnum> { BandRoleTypeEnum.Vocalist });
            var cltToken = default(CancellationToken);

            _jamRepository.Setup(jamRepo => jamRepo.AddAsync(It.IsAny<Jam.Domain.AggregatesModel.JamAggregate.Jam>()))
                .Returns(Task.FromResult(FakeJam()));

            _jamRepository.Setup(jamRepo => jamRepo.UnitOfWork.SaveEntitiesAsync(cltToken))
                .Returns(Task.FromResult(true));

            //Act
            var handler = new CreateJamCommandHandler(_jamRepository.Object, _logger.Object);
            var result = await handler.Handle(fakeCreateJamCmd, cltToken);

            //Assert
            Assert.NotNull(result);
        }

        private static Jam.Domain.AggregatesModel.JamAggregate.Jam FakeJam()
        {
            return new Jam.Domain.AggregatesModel.JamAggregate.Jam(1, 2);
        }
    }
}
