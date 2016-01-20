using System.Collections.Generic;
using System.Linq;
using HomerunLeague.ServiceInterface.Extensions;
using HomerunLeague.ServiceModel;
using HomerunLeague.ServiceModel.Types;
using ServiceStack;
using ServiceStack.FluentValidation;

namespace HomerunLeague.ServiceInterface.Validation
{
    public class TeamValidator : AbstractValidator<CreateTeam>
    {
        private readonly List<Division> _divisions;

        public TeamValidator(DivisionServices divService, AdminServices adminService)
        {
            var settings = adminService.Get(new GetSettings());

            _divisions = divService.GetAll(new GetDivisions {Year = settings.BaseballYear});

            RuleSet(ApplyTo.Post, () =>
            {
                RuleFor(t => t.Name).Length(3, 100);
                RuleFor(t => t.Email).EmailAddress();
                RuleFor(t => t.PlayerIds)
                    .Must(DivisionRequirmentsMet)
                    .WithMessage("Your team does not fulfill the Division requirements");
            });
        }

        public bool DivisionRequirmentsMet(List<int> playerIds)
        {
            // Check to see if they sent the exact number of players required across all divisions
            if (_divisions.Sum(d => d.PlayerRequirment) != playerIds.Count)
                return false;

            // Iterate the divisions and check to see if the player requirement for each is met
            foreach (var division in _divisions)
            {
                if (division.PlayerRequirment !=
                    division.Players.Select(p => p.Id).Intersect(playerIds).Count())
                    return false;
            }

            return true;
        }
    }
}
