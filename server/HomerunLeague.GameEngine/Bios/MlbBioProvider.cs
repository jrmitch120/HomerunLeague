using System;
using System.Collections.Generic;
using HomerunLeague.ServiceModel.Types;
using RestSharp;
using ServiceStack;

namespace HomerunLeague.GameEngine.Bios
{
    public class MlbBioProvider : IBioData
    {
        public void UpdatePlayerBios(IEnumerable<Player> players)
        {
            //http://m.mlb.com/lookup/json/named.player_teams.bam?player_id=488862
            var bioClient = new RestClient("http://m.mlb.com/lookup/json/named.player.bam");
            var teamsClient = new RestClient("http://m.mlb.com/lookup/json/named.player_teams.bam");

            foreach (var player in players)
            {
                // Basic player bio information
                var bioResult =
                    bioClient.Get<MlbPlayerBioResponse>(new RestRequest().AddParameter("player_id", player.MlbId));

                if (bioResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var bio = bioResult.Data.player.queryResults.row;

                    player.Active = bio.active_sw == "Y";
                    player.Bats = bio.bats;
                    player.BirthDate = bio.birth_date;
                    player.FirstName = bio.name_first;
                    player.HeightFeet = bio.height_feet;
                    player.HeightInches = bio.height_inches;
                    player.JerseyNumber = bio.jersey_number;
                    player.LastName = bio.name_last;
                    player.PrimaryPosition = bio.primary_position_txt;
                    player.Weight = bio.weight;
                }

                // Current team is not on the player bio.  We have to lookup a player's team history to figure that out.
                var teamsResult =
                    teamsClient.Get<MlbPlayerTeamResponse>(new RestRequest().AddParameter("player_id", player.MlbId));

                if (teamsResult.StatusCode == System.Net.HttpStatusCode.OK)
                    player.TeamName = teamsResult.Data.player_teams.queryResults.row[0].team;
            }
        }
    }

    #region MlbBioResponse

    public class MlbPlayerBioData
    {
        public string name_first_last { get; set; }
        public string name_prefix { get; set; }
        public string birth_country { get; set; }
        public int weight { get; set; }
        public string birth_state { get; set; }
        public string draft_round { get; set; }
        public string player { get; set; }
        public string draft_year { get; set; }
        public string last_played { get; set; }
        public string college { get; set; }
        public int height_inches { get; set; }
        public string name_middle { get; set; }
        public string name_last_first_html { get; set; }
        public string death_country { get; set; }
        public int jersey_number { get; set; }
        public string name_pronunciation { get; set; }
        public string death_state { get; set; }
        public string bats { get; set; }
        public string name_first { get; set; }
        public string age { get; set; }
        public int height_feet { get; set; }
        public string gender { get; set; }
        public string birth_city { get; set; }
        public string pro_debut_date { get; set; }
        public string name_roster_html { get; set; }
        public string name_nick { get; set; }
        public string draft_team_abbrev { get; set; }
        public string death_date { get; set; }
        public string primary_position { get; set; }
        public string name_matrilineal { get; set; }
        public DateTime birth_date { get; set; }
        public string throws { get; set; }
        public string death_city { get; set; }
        public string name_first_last_html { get; set; }
        public string name_roster { get; set; }
        public string primary_position_txt { get; set; }
        public string twitter_id { get; set; }
        public string high_school { get; set; }
        public string name_use { get; set; }
        public string name_title { get; set; }
        public string player_id { get; set; }
        public string name_last { get; set; }
        public string primary_stat_type { get; set; }
        public string active_sw { get; set; }
        public string primary_sport_code { get; set; }
    }

    public class MlbPlayerBioQueryResults
    {
        public string created { get; set; }
        public string totalSize { get; set; }
        public MlbPlayerBioData row { get; set; }
    }

    public class MlbPlayerBio
    {
        public string copyRight { get; set; }
        public MlbPlayerBioQueryResults queryResults { get; set; }
    }

    public class MlbPlayerBioResponse
    {
        public MlbPlayerBio player { get; set; }
    }

    #endregion

    #region MlbPlayerTeamsResponse 
    public class MlbPlayerTeamData
    {
        public string season_state { get; set; }
        public string hitting_season { get; set; }
        public string sport_full { get; set; }
        public string org { get; set; }
        public string sport_code { get; set; }
        public string org_short { get; set; }
        public string jersey_number { get; set; }
        public string end_date { get; set; }
        public string team_brief { get; set; }
        public string forty_man_sw { get; set; }
        public string sport_id { get; set; }
        public string league_short { get; set; }
        public string org_full { get; set; }
        public string status_code { get; set; }
        public string league_full { get; set; }
        public string primary_position { get; set; }
        public string team_abbrev { get; set; }
        public string status { get; set; }
        public string org_abbrev { get; set; }
        public string league_id { get; set; }
        public string @class { get; set; }
        public string sport { get; set; }
        public string team_short { get; set; }
        public string team { get; set; }
        public string league { get; set; }
        public string fielding_season { get; set; }
        public string org_id { get; set; }
        public string class_id { get; set; }
        public string league_season { get; set; }
        public string pitching_season { get; set; }
        public string sport_short { get; set; }
        public string status_date { get; set; }
        public string player_id { get; set; }
        public string current_sw { get; set; }
        public string primary_stat_type { get; set; }
        public string team_id { get; set; }
        public string start_date { get; set; }
    }

    public class MlbPlayerTeamQueryResults
    {
        public string created { get; set; }
        public string totalSize { get; set; }
        public List<MlbPlayerTeamData> row { get; set; }
    }

    public class MlbPlayerTeam
    {
        public string copyRight { get; set; }
        public MlbPlayerTeamQueryResults queryResults { get; set; }
    }

    public class MlbPlayerTeamResponse
    {
        public MlbPlayerTeam player_teams { get; set; }
    }
    #endregion
}