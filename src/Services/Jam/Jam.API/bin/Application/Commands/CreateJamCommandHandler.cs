using MediatR;

namespace Jam.API.Application.Commands
{
    public class CreateJamCommandHandler : IRequestHandler<CreateJamCommand, bool>
    {
        public Task<bool> Handle(CreateJamCommand request, CancellationToken cancellationToken)
        {

        }
    }
}
