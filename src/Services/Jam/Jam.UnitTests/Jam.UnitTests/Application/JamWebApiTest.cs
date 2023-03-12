using Jam.API.Application.Commands;
using Jam.API.Application.Enums;
using Jam.API.Application.Models;
using Jam.API.Application.Queries;
using Jam.API.Controllers;
using Jam.API.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace Jam.UnitTests.Application
{
    public class JamWebApiTest
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IJamQueries> _jamQueriesMock;
        private readonly Mock<IIdentityService> _identityServiceMock;
        private readonly Mock<ILogger<JamController>> _loggerMock;
        public JamWebApiTest()
        {
            _mediatorMock = new Mock<IMediator>();
            _jamQueriesMock = new Mock<IJamQueries>();
            _identityServiceMock = new Mock<IIdentityService>();
            _loggerMock = new Mock<ILogger<JamController>>();
        }
        [Fact]
        public async Task Get_jams_success()
        {
            //Arrange
            var fakeDynamicResult = Enumerable.Empty<JamResponse>();

            _identityServiceMock.Setup(x => x.GetUserId())
                .Returns(1);

            _jamQueriesMock.Setup(x => x.GetAsync(JamStatusEnum.Pending))
                .Returns(Task.FromResult(fakeDynamicResult));

            //Act
            var jamController = new JamController(_mediatorMock.Object, _loggerMock.Object, _identityServiceMock.Object, _jamQueriesMock.Object);
            var actionResult = await jamController.GetByStatus();

            //Assert
            Assert.Equal((actionResult.Result as OkObjectResult)?.StatusCode, (int)HttpStatusCode.OK);

        }
        [Fact]
        public async Task Create_jam_success()
        {
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateJamCommand>(), default))
                .Returns(Task.FromResult(FakeJam()));
            //Act
            var jamController = new JamController(_mediatorMock.Object, _loggerMock.Object, _identityServiceMock.Object, _jamQueriesMock.Object);
            var actionResult = await jamController
                .Create(new CreateJamRequest() { JamType = JamTypeEnum.Public, Roles = new List<BandRoleTypeEnum> { BandRoleTypeEnum.Vocalist} }) as OkResult;

            //Assert
            Assert.NotNull(actionResult);
            Assert.Equal((int)HttpStatusCode.OK, actionResult.StatusCode);

        }
        [Fact]
        public async Task Create_jam_bad_request()
        {
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateJamCommand>(), default))
                .Returns(Task.FromResult(FakeJam()));

            //Act
            var jamController = new JamController(_mediatorMock.Object, _loggerMock.Object, _identityServiceMock.Object, _jamQueriesMock.Object);
            var actionResult = await jamController
                .Create(new CreateJamRequest() { JamType = JamTypeEnum.Public, Roles = new List<BandRoleTypeEnum> { BandRoleTypeEnum.Vocalist } }) as BadRequestResult;

            //Assert
            Assert.NotNull(actionResult);
            Assert.Equal((int)HttpStatusCode.BadRequest, actionResult.StatusCode);

        }
        [Fact]
        public async Task Start_jam_success()
        {
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<StartJamCommand>(), default))
                .Returns(Task.FromResult(true));
            //Act
            var jamController = new JamController(_mediatorMock.Object, _loggerMock.Object, _identityServiceMock.Object, _jamQueriesMock.Object);
            var actionResult = await jamController
                .Start(new StartJamRequest { JamId = It.IsAny<int>()}) as OkResult;

            //Assert
            Assert.NotNull(actionResult);
            Assert.Equal((int)HttpStatusCode.OK, actionResult.StatusCode);

        }
        [Fact]
        public async Task Start_jam_bad_request()
        {
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<StartJamCommand>(), default))
                .Returns(Task.FromResult(false));
            //Act
            var jamController = new JamController(_mediatorMock.Object, _loggerMock.Object, _identityServiceMock.Object, _jamQueriesMock.Object);
            var actionResult = await jamController
                .Start(new StartJamRequest { JamId = It.IsAny<int>() }) as BadRequestResult;

            //Assert
            Assert.NotNull(actionResult);
            Assert.Equal((int)HttpStatusCode.BadRequest, actionResult.StatusCode);

        }
        [Fact]
        public async Task Register_jam_success()
        {
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<RegisterToJamCommand>(), default))
                .Returns(Task.FromResult(true));
            //Act
            var jamController = new JamController(_mediatorMock.Object, _loggerMock.Object, _identityServiceMock.Object, _jamQueriesMock.Object);
            var actionResult = await jamController
                .Register(new RegisterToJamRequest { JamId = It.IsAny<int>(), PreferedRole = It.IsAny<BandRoleTypeEnum>()}) as OkResult;

            //Assert
            Assert.NotNull(actionResult);
            Assert.Equal((int)HttpStatusCode.OK, actionResult.StatusCode);

        }
        [Fact]
        public async Task Register_jam_bad_request()
        {
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<RegisterToJamCommand>(), default))
                .Returns(Task.FromResult(false));
            //Act
            var jamController = new JamController(_mediatorMock.Object, _loggerMock.Object, _identityServiceMock.Object, _jamQueriesMock.Object);
            var actionResult = await jamController
                .Register(new RegisterToJamRequest { JamId = It.IsAny<int>(), PreferedRole = It.IsAny<BandRoleTypeEnum>() }) as BadRequestResult;

            //Assert
            Assert.NotNull(actionResult);
            Assert.Equal((int)HttpStatusCode.BadRequest, actionResult.StatusCode);

        }

        private static JamResponse FakeJam()
        {
            return new JamResponse(1, 1, new List<BandRoleTypeEnum>() { BandRoleTypeEnum.Drummer });
        }
    }
}