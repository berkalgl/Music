using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using User.API.Application.Commands;
using User.API.Application.Models;
using User.API.Application.Queries;

namespace User.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileQueries _userProfileQueries;
        private readonly IMediator _mediator;
        private readonly ILogger<UserProfileController> _logger;

        public UserProfileController(IMediator mediator, ILogger<UserProfileController> logger,
            IUserProfileQueries userProfileQueries)
        {
            _userProfileQueries = userProfileQueries ?? throw new ArgumentNullException(nameof(userProfileQueries));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(typeof(UserProfileResponse), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation(
                "----- Execution Query GetUserProfileAsync");

            var result = await _userProfileQueries.GetUserProfileAsync(id);
            if (result == null) return NotFound();
            
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserProfileResponse), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> Add([FromBody] CreateUserProfileRequest request)
        {
            var createUserProfileCommand = new CreateUserProfileCommand(request.Email, request.Password, request.Role, 
                request.BandRoleTypes);

            _logger.LogInformation(
                "----- Sending command: {CommandName} ({@Command})",
                createUserProfileCommand.GetType(),
                createUserProfileCommand);

            var userProfileResponse = await _mediator.Send(createUserProfileCommand);

            return CreatedAtAction(nameof(Get), new { id = userProfileResponse.Id }, userProfileResponse);
        }
        
        [Route("{id}")]
        [HttpPut]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserProfileRequest request)
        {
            var updateUserProfileCommand = UpdateUserProfileCommand.FromRequest(id, request);

            _logger.LogInformation(
                "----- Sending command: {CommandName} ({@Command})",
                updateUserProfileCommand.GetType(),
                updateUserProfileCommand);

            var userProfileResponse = await _mediator.Send(updateUserProfileCommand);

            if (userProfileResponse is null) return NotFound();

            return Ok(userProfileResponse);
        }
    }
}
