using System;
using HomerunLeague.ServiceModel;
using HomerunLeague.ServiceModel.Operations;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace HomerunLeague.ServiceInterface.Validation
{
    public class DivisionsValidator : AbstractValidator<CreateDivision>
    {
        public DivisionsValidator()
        {
            RuleSet(ApplyTo.Post, () =>
            {
                RuleFor(d => d.Name).Length(3, 100);
                RuleFor(d => d.Order).GreaterThan(-1);
                RuleFor(d => d.PlayerRequirement).GreaterThan(0);
                RuleFor(d => d.PlayerIds)
                    .Must((division, playerIds) => playerIds.Count >= division.PlayerRequirement)
                    .WithMessage("You must assign at least as many players to a division as it requires.");
            });
        }
    }
}
