using MediatR;
using User.API.Application.Enums;

namespace User.API.Application.Commands
{
    public class CheckUserRoleValidationCommand : IRequest<bool>
    {
        public int JamId { get; set; }
        public int UserId { get; set; }
        public BandRoleTypeEnum PreferredRoleType { get; set; }

        public CheckUserRoleValidationCommand(int jamId, int userId, BandRoleTypeEnum preferredRoleType)
        {
            JamId = jamId;
            UserId = userId;
            PreferredRoleType = preferredRoleType;
        }
    }
}
