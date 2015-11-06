using System;
using System.Collections.Generic;
using HomerunLeague.ServiceModel.Types;
using RestSharp;

namespace HomerunLeague.GameEngine.Bios
{
    public class MlbBioProvider : IBioData
    {
        public void Update(IEnumerable<Player> players)
        {
            var client = new RestClient("http://m.mlb.com/lookup/json/named.player.bam");


            foreach (var player in players)
            {
                var result = client.Get<MlbResponse>(new RestRequest().AddParameter("player_id", player.MlbId));
                
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var bio = result.Data.player.queryResults.row;

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
            }
        }

        public class PlayerInfomartion
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

        public class QueryResults
        {
            public string created { get; set; }
            public string totalSize { get; set; }
            public PlayerInfomartion row { get; set; }
        }

        public class MlbPlayer
        {
            public string copyRight { get; set; }
            public QueryResults queryResults { get; set; }
        }

        public class MlbResponse
        {
            public MlbPlayer player { get; set; }
        }
    }
}