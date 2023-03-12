using MediatR;
using User.API.Application.Enums;

namespace User.API.Application.Commands
{
    public record SendMailNotificationToUserCommand(int JamId, List<UserWithRoleItem> Users) : IRequest<bool>
    {
        public int JamId { get; } = JamId;
        public List<UserWithRoleItem> Users { get; } = Users;
    }
    public record UserWithRoleItem(int UserId, BandRoleTypeEnum AssignedRole);
}
