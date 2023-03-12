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
    public class LoginWebApiTests
    {
        private readonly Mock<IUserProfileQueries> _userProfileQueriesMock;
        public LoginWebApiTests()
        {
            _userProfileQueriesMock = new Mock<IUserProfileQueries>();
        }
        [Fact]
        public async Task Login_success()
        {
            //Arrange
            var fakeDynamicResult = new UserProfileResponse { Id = 1, Role = "Player" };

            _userProfileQueriesMock.Setup(x => x.ValidateUserAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(fakeDynamicResult));
            //Act
            var loginController = new LoginController(_userProfileQueriesMock.Object);
            var actionResult = await loginController
                .Login(new LoginRequest { Email = It.IsAny<string>(), Password = It.IsAny<string>() }) as OkObjectResult;

            //Assert
            Assert.Equal(actionResult.StatusCode, (int)HttpStatusCode.OK);

        }
        [Fact]
        public async Task Login_bad_request()
        {
            //Arrange
            var fakeDynamicResult = new UserProfileResponse { Id = 0 };

            _userProfileQueriesMock.Setup(x => x.ValidateUserAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(fakeDynamicResult));
            //Act
            var loginController = new LoginController(_userProfileQueriesMock.Object);
            var actionResult = await loginController
                .Login(new LoginRequest { Email = It.IsAny<string>(), Password = It.IsAny<string>() }) as BadRequestObjectResult;

            //Assert
            Assert.Equal(actionResult.StatusCode, (int)HttpStatusCode.BadRequest);

        }
    }
}