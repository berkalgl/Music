using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using User.API.Application.Models;
using User.API.Application.Queries;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private const string _issuer = "dummy.com.tr";
        private const string _audience = "dummy.com.tr";
        private const string _secret = "a-secret-sentence";
        private readonly IUserProfileQueries _userProfileQueries;
        public LoginController(IUserProfileQueries userProfileQueries)
        {
            _userProfileQueries = userProfileQueries ?? throw new ArgumentNullException(nameof(userProfileQueries));
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            try
            {
                var user = await _userProfileQueries.ValidateUserAsync(loginRequest.Email, loginRequest.Password);

                if (user.Id == 0)
                {
                    return BadRequest("could not find the user!");
                }

                var claims = new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role),
                };

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: _issuer,
                    audience: _audience,
                    claims: claims,
                    expires: DateTime.Now.AddHours(4),
                    signingCredentials: signinCredentials
                );

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken) });
            }
            catch
            {
                return BadRequest("An error occurred in generating the token");
            }
        }
    }
}
