using FluentValidation;
using Jam.API.Application.Models;

namespace Jam.API.Application.Validators;

public class JamValidator : AbstractValidator<CreateJamRequest>
{
    public JamValidator()
    {
        // Jam Type must not null
        RuleFor(x => x.JamType)
            .IsInEnum()
            .WithMessage("Jam type must not null");
        
        // Roles must not null
        RuleFor(x => x.Roles)
            .NotNull()
            .Must(x => x.Count != 0)
            .WithMessage("Roles must not be null")
            .ForEach(x => x.IsInEnum())
            .WithMessage("Roles must be declared");
    }
}