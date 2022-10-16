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
    public class UpdateJamRoleItemStatusSuccessCommandHandlerTest
    {
        private readonly Mock<IJamRepository> _jamRepository;
        private readonly Mock<ILogger<UpdateJamRoleItemStatusSuccessCommandHandler>> _logger;
        public UpdateJamRoleItemStatusSuccessCommandHandlerTest()
        {
            _jamRepository = new Mock<IJamRepository>();
            _logger = new Mock<ILogger<UpdateJamRoleItemStatusSuccessCommandHandler>>();
        }
        [Fact]
        public async Task Update_jam_role_item_status_failed_command_handled_success()
        {
            var fakeUpdateCommand = new UpdateJamRoleItemStatusSuccessCommand(It.IsAny<int>(), It.IsAny<int>(), BandRoleTypeEnum.Vocalist);

            _jamRepository.Setup(jamRepo => jamRepo.GetAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(fakeJam()));

            _jamRepository.Setup(jamRepo => jamRepo.UnitOfWork.SaveEntitiesAsync(default))
                .Returns(Task.FromResult(true));

            //Act
            var handler = new UpdateJamRoleItemStatusSuccessCommandHandler(_jamRepository.Object, _logger.Object);
            var cltToken = new CancellationToken();
            var result = await handler.Handle(fakeUpdateCommand, cltToken);

            //Assert
            Assert.True(result);
        }

        private Jam.Domain.AggregatesModel.JamAggregate.Jam fakeJam()
        {
            var jam = new JamBuilder(1, 2)
                .AddRoleItem(1)
                .RegisterPreferredRole(3, 1)
                .UpdateRoleItemSuccessStatus(3, 1)
                .Build();
            return jam;
        }
    }
}
