using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using User.API.Application.Commands;
using User.API.Application.Enums;
using User.API.Application.Models;
using User.API.Controllers;

namespace User.UnitTests.Application
{
    public class UserProfileWebApiTest
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger<UserProfileController>> _loggerMock;
        public UserProfileWebApiTest()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<UserProfileController>>();
        }
        [Fact]
        public async Task Create_user_profile_success()
        {
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateUserProfileCommand>(), default))
                .Returns(Task.FromResult(true));
            //Act
            var userProfileController = new UserProfileController(_mediatorMock.Object, _loggerMock.Object);
            var actionResult = await userProfileController
                .CreateUserProfileAsync(new CreateUserProfileRequest { 
                    Email = It.IsAny<string>(), 
                    Password = It.IsAny<string>(), 
                    UserType = It.IsAny<UserRoleEnum>(), 
                    BandRoleTypes = new List<BandRoleTypeEnum> { BandRoleTypeEnum.Vocalist } }
                ) as OkResult;

            //Assert
            Assert.Equal(actionResult.StatusCode, (int)HttpStatusCode.OK);

        }
        [Fact]
        public async Task Create_user_profile_bad_request()
        {
            //Arrange
            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateUserProfileCommand>(), default))
                .Returns(Task.FromResult(false));
            //Act
            var userProfileController = new UserProfileController(_mediatorMock.Object, _loggerMock.Object);
            var actionResult = await userProfileController
                .CreateUserProfileAsync(new CreateUserProfileRequest
                {
                    Email = It.IsAny<string>(),
                    Password = It.IsAny<string>(),
                    UserType = It.IsAny<UserRoleEnum>(),
                    BandRoleTypes = new List<BandRoleTypeEnum> { BandRoleTypeEnum.Vocalist }
                }) as BadRequestResult;

            //Assert
            Assert.Equal(actionResult.StatusCode, (int)HttpStatusCode.BadRequest);

        }
    }
}