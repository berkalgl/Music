using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Jam.API.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private IHttpContextAccessor _context;
        public IdentityService(IHttpContextAccessor context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public int GetUserId()
        {
            var _userId = 0;
            var identity = _context.HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                int.TryParse(identity.FindFirst(JwtRegisteredClaimNames.Name).Value, out _userId);
            }
            return _userId;
        }
    }
}
