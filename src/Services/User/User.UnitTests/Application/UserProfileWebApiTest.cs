using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using User.API.Application.Commands;
using User.API.Application.Enums;
using User.API.Application.Models;
using User.API.Application.Queries;
using User.API.Controllers;

namespace User.UnitTests.Application
{
    public class UserProfileWebApiTest
    {
        private readonly Mock<IUserProfileQueries> _userProfileQueriesMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger<UserProfileController>> _loggerMock;
        public UserProfileWebApiTest()
        {
            _userProfileQueriesMock = new Mock<IUserProfileQueries>();
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<UserProfileController>>();
        }
        [Fact]
        public async Task Create_user_profile_success()
        {
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateUserProfileCommand>(), default))
                .Returns(Task.FromResult(new UserProfileResponse()));
            //Act
            var userProfileController = new UserProfileController(_mediatorMock.Object, _loggerMock.Object, _userProfileQueriesMock.Object);
            var actionResult = await userProfileController
                .Add(new CreateUserProfileRequest { 
                    Email = It.IsAny<string>(), 
                    Password = It.IsAny<string>(), 
                    Role = It.IsAny<UserRoleEnum>(), 
                    BandRoleTypes = new List<BandRoleTypeEnum> { BandRoleTypeEnum.Vocalist } }
                ) as OkResult;

            //Assert
            Assert.NotNull(actionResult);
            Assert.Equal((int)HttpStatusCode.OK, actionResult.StatusCode);

        }
        [Fact]
        public async Task Create_user_profile_bad_request()
        {
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateUserProfileCommand>(), default))
                .Returns(Task.FromResult(new UserProfileResponse()));
            //Act
            var userProfileController = new UserProfileController(_mediatorMock.Object, _loggerMock.Object, _userProfileQueriesMock.Object);
            var actionResult = await userProfileController
                .Add(new CreateUserProfileRequest
                {
                    Email = It.IsAny<string>(),
                    Password = It.IsAny<string>(),
                    Role = It.IsAny<UserRoleEnum>(),
                    BandRoleTypes = new List<BandRoleTypeEnum> { BandRoleTypeEnum.Vocalist }
                }) as BadRequestResult;

            //Assert
            Assert.NotNull(actionResult);
            Assert.Equal((int)HttpStatusCode.BadRequest, actionResult.StatusCode);

        }
    }
}