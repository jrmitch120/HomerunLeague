using System.Collections.Generic;
using HomerunLeague.ServiceModel.Types;
using HomerunLeague.ServiceModel.ViewModels;
using ServiceStack;

namespace HomerunLeague.ServiceInterface.Extensions
{
    public static class MappingExtensions
    {
        public static TeamView ToViewModel(this Team team)
        {
            return team.ConvertTo<TeamView>();
        }

        public static List<TeamListView> ToViewModel(this List<Team> teams)
        {
            return teams.ConvertAll(team => team.ConvertTo<TeamListView>());
        }
    }
}
