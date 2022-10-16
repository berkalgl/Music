using MediatR;
using Microsoft.AspNetCore.Mvc;
using User.API.Application.Commands;
using User.API.Application.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace User.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserProfileController> _logger;

        public UserProfileController(IMediator mediator, ILogger<UserProfileController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Route("Create")]
        [HttpPost]
        public async Task<IActionResult> CreateUserProfileAsync([FromBody] CreateUserProfileRequest request)
        {
            var createUserProfileCommand = 
                new CreateUserProfileCommand(request.Email, request.Password, request.UserType, request.BandRoleTypes);

            _logger.LogInformation(
                "----- Sending command: {CommandName} ({@Command})",
                createUserProfileCommand.GetType(),
                createUserProfileCommand);

            var commandResult = await _mediator.Send(createUserProfileCommand);

            if (!commandResult)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
