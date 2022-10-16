using MediatR;
using User.API.Application.Enums;

namespace User.API.Application.Commands
{
    public record SendMailNotificationToUserCommand : IRequest<bool>
    {
        public int JamId { get; private set; }
        public List<UserWithRoleItem> Users { get; private set; }
        public SendMailNotificationToUserCommand(int jamId, List<UserWithRoleItem> users)
        {
            JamId = jamId;
            Users = users;
        }
    }
    public record UserWithRoleItem
    {
        public int UserId { get; }
        public BandRoleTypeEnum AssignedRole { get; }
        public UserWithRoleItem(int userId, BandRoleTypeEnum assignedRole)
        {
            UserId = userId;
            AssignedRole = assignedRole;
        }
    }
}
