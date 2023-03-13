using Jam.API.Application.Commands;
using Jam.API.Application.Enums;
using Jam.API.Application.Models;
using Jam.API.Application.Queries;
using Jam.API.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Jam.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class JamController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<JamController> _logger;
        private readonly IIdentityService _identityService;
        private readonly IJamQueries _jamQueries;

        public JamController(IMediator mediator, ILogger<JamController> logger, IIdentityService identityService, IJamQueries jamQueries)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _jamQueries = jamQueries ?? throw new ArgumentNullException(nameof(jamQueries));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateJamRequest createJamRequest)
        {
            var createJamCommand = new CreateJamCommand(_identityService.GetUserId(), createJamRequest.JamType,
                createJamRequest.Roles);

            _logger.LogInformation(
                "----- Sending command: {CommandName} ({@Command})",
                createJamCommand.GetType(),
                createJamCommand);

            var commandResult = await _mediator.Send(createJamCommand);

            return CreatedAtAction(nameof(Get), new {id = commandResult.Id}, commandResult);
        }

        [Route("{id:int}")]
        [HttpGet]
        [Authorize(Roles = "Host")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation(
                "----- Execution Query GetJamAsync");
            
            var jam = await _jamQueries.GetAsync(id);

            if (jam is null) return NotFound();

            return Ok(jam);
        }

        [Route("{jamStatus}")]
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<JamResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<JamResponse>>> GetByStatus(JamStatusEnum jamStatus = JamStatusEnum.Pending)
        {
            var jams = await _jamQueries.GetAsync(jamStatus);

            return Ok(jams);
        }

        [Route("start")]
        [HttpPut]
        [Authorize(Roles = "Host")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Start([FromBody] StartJamRequest request)
        {
            var command = new StartJamCommand(request.JamId, _identityService.GetUserId());

            _logger.LogInformation(
                "----- Sending command: {CommandName} ({@Command})",
                command.GetType(),
                command);

            var commandResult = await _mediator.Send(command);

            if (!commandResult)
            {
                return BadRequest();
            }

            return Ok();
        }

        [Route("register")]
        [HttpPut]
        [Authorize(Roles = "Player")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterToJamRequest request)
        {
            var command = new RegisterToJamCommand(request.JamId, _identityService.GetUserId(), request.PreferedRole);

            _logger.LogInformation(
                "----- Sending command: {CommandName} ({@Command})",
                command.GetType(),
                command);

            var commandResult = await _mediator.Send(command);

            if (!commandResult)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
