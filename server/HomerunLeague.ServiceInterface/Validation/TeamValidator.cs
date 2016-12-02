using System.Collections.Generic;
using System.Linq;
using HomerunLeague.ServiceInterface.Extensions;
using HomerunLeague.ServiceModel.Operations;
using HomerunLeague.ServiceModel.Types;
using ServiceStack;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;

namespace HomerunLeague.ServiceInterface.Validation
{


    public class TeamValidator : AbstractValidator<CreateTeam>
    {
        private readonly List<Division> _divisions;

        public TeamValidator(DivisionServices divService, AdminServices adminService)
        {
            var settings = adminService.Get(new GetSettings());
            var registrationPassword = adminService.Get(new GetLeaguePasswordHash());

            _divisions = divService.GetAll(new GetDivisions {Year = settings.BaseballYear});

            RuleSet(ApplyTo.Post, () =>
            {
                RuleFor(t => t.Name).Length(3, 100);
                RuleFor(t => t.Email).EmailAddress();
                RuleFor(t => t.Year)
                    .Equal(settings.BaseballYear)
                    .WithMessage($"You can only create a team for {settings.BaseballYear}.");

                RuleFor(t => t.PlayerIds)
                    .Must(DivisionRequirmentsMet)
                    .WithMessage("Your team does not fulfill the Division requirements.");

                // Check password if required
                if (settings.PasswordProtected)
                {

                    RuleFor(t => t.Password)
                        .NotNull()
                        .Must(p => p.ToSha256String() == registrationPassword)
                        .WithMessage("Your registration password is incorrect");
                }
            });
        }

        private bool DivisionRequirmentsMet(List<int> playerIds)
        {
            // Check to see if they sent the exact number of players required across all divisions
            if (playerIds == null || _divisions.Sum(d => d.PlayerRequirement) != playerIds.Count)
                return false;

            // Iterate the divisions and check to see if the player requirement for each is met
            foreach (var division in _divisions)
            {
                if (division.PlayerRequirement !=
                    division.Players.Select(p => p.Id).Intersect(playerIds).Count())
                    return false;
            }

            return true;
        }
    }
}
