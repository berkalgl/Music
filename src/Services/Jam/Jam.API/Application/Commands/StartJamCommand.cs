using MediatR;

namespace Jam.API.Application.Commands
{
    // DDD and CQRS patterns command
    // In this case, its immutability is achieved by having all the setters as private
    // plus only being able to update the data just once, when creating the object through its constructor.
    public class StartJamCommand : IRequest<bool>
    {
        public int JamId { get; private set; }
        public int UserId { get; private set; }
        public StartJamCommand(int jamId, int userId)
        {
            JamId = jamId;
            UserId = userId;
        }
    }
}
