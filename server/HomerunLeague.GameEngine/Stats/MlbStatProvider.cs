using System;
using System.Collections.Generic;
using System.Net;
using HomerunLeague.ServiceModel.Types;
using RestSharp;

namespace HomerunLeague.GameEngine.Stats
{
    public class MlbStatProvider : IStatData
    {
        public StatPull FetchStats(Player player, int year)
        {
            var playerStats = new StatPull();

            var client =
                new RestClient(
                    "http://m.mlb.com/lookup/json/named.sport_hitting_game_log_composed.bam?game_type='R'&league_list_id='mlb'&sit_code='1'&sit_code='2'&sit_code='3'&sit_code='4'&sit_code='5'&sit_code='6'&sit_code='7'&sit_code='8'&sit_code='9'&sit_code='10'&sit_code='11'&sit_code='12'");

            var result = client.Get<MlbStatsResponse>(new RestRequest().AddParameter("player_id", player.MlbId).AddParameter("season", year));

            if (result.StatusCode == HttpStatusCode.OK &&
                result.Data.sport_hitting_game_log_composed.SportHittingGameLog.queryResults.totalSize > 0) // any data?
            {
                foreach (
                    var stat in result.Data.sport_hitting_game_log_composed.SportHittingGameLog.queryResults.row)
                    playerStats.GameLogs.Add(new GameLog
                    {
                        PlayerId = player.Id,
                        GameId = stat.game_id,
                        Ab = stat.ab,
                        Avg = stat.avg.Equals(string.Empty) ? decimal.Zero : Convert.ToDecimal(stat.avg),
                        Bb = stat.bb,
                        Cs = stat.cs,
                        D = stat.d,
                        GameDate = stat.game_date,
                        H = stat.h,
                        Hbp = stat.hbp,
                        HomeAway = stat.home_away,
                        Hr = stat.hr,
                        Ibb = stat.ibb,
                        OppnentScore = stat.opp_score,
                        Opponent = stat.opponent,
                        R = stat.r,
                        Rbi = stat.rbi,
                        Result = stat.team_result,
                        Sb = stat.sb,
                        Sf = stat.sf,
                        Slg = stat.slg.Equals(string.Empty) ? decimal.Zero : Convert.ToDecimal(stat.slg),
                        So = stat.so,
                        T = stat.t,
                        Tb = stat.tb,
                        Team = stat.team,
                        TeamScore = stat.team_score
                    });

                var season = result.Data.sport_hitting_game_log_composed.sport_hitting.queryResults.row;

                playerStats.Totals = new PlayerTotals
                {
                    Year = season.season,
                    PlayerId = player.Id,
                    Ab = season.ab,
                    Avg = season.avg.Equals(string.Empty) ? decimal.Zero : Convert.ToDecimal(season.avg),
                    Bb = season.bb,
                    Cs = season.cs,
                    D = season.d,
                    H = season.h,
                    Hbp = season.hbp,
                    Hr = season.hr,
                    Ibb = season.ibb,
                    R = season.r,
                    Rbi = season.rbi,
                    Sb = season.sb,
                    Sf = season.sf,
                    Slg = season.slg.Equals(string.Empty) ? decimal.Zero : Convert.ToDecimal(season.slg),
                    So = season.so,
                    T = season.t,
                    Tb = season.tb,
                };
            }

            return playerStats;
        }

        public class GameLogStat
        {
            public int hr { get; set; }
            public string game_type { get; set; }
            public string game_nbr { get; set; }
            public string sac { get; set; }
            public string game_day { get; set; }
            public int rbi { get; set; }
            public string lob { get; set; }
            public string opponent { get; set; }
            public string opponent_short { get; set; }
            public int tb { get; set; }
            public string game_id { get; set; }
            public int bb { get; set; }
            public string avg { get; set; }
            public string slg { get; set; }
            public int opp_score { get; set; }
            public string ops { get; set; }
            public int hbp { get; set; }
            public int d { get; set; }
            public string team_abbrev { get; set; }
            public int so { get; set; }
            public DateTime game_date { get; set; }
            public string sport { get; set; }
            public int sf { get; set; }
            public string game_pk { get; set; }
            public string team { get; set; }
            public string league { get; set; }
            public int h { get; set; }
            public int cs { get; set; }
            public string obp { get; set; }
            public int t { get; set; }
            public string ao { get; set; }
            public int r { get; set; }
            public string go_ao { get; set; }
            public int sb { get; set; }
            public string opponent_abbrev { get; set; }
            public string opponent_league { get; set; }
            public string player_id { get; set; }
            public int ibb { get; set; }
            public int ab { get; set; }
            public string team_result { get; set; }
            public string opponent_id { get; set; }
            public string team_id { get; set; }
            public string home_away { get; set; }
            public int team_score { get; set; }
            public string go { get; set; }
        }

        public class GameLogResults
        {
            public string created { get; set; }
            public int totalSize { get; set; }
            public List<GameLogStat> row { get; set; }
        }

        public class GameLogData
        {
            public GameLogResults queryResults { get; set; }
        }

        public class Hitting
        {
            public int hr { get; set; }
            public string gidp { get; set; }
            public string sac { get; set; }
            public string team_count { get; set; }
            public string sport_code { get; set; }
            public int rbi { get; set; }
            public int tb { get; set; }
            public string sport_id { get; set; }
            public int bb { get; set; }
            public string avg { get; set; }
            public string slg { get; set; }
            public string ops { get; set; }
            public int hbp { get; set; }
            public string g { get; set; }
            public int d { get; set; }
            public int so { get; set; }
            public string sport { get; set; }
            public int sf { get; set; }
            public int h { get; set; }
            public int cs { get; set; }
            public string obp { get; set; }
            public int t { get; set; }
            public string ao { get; set; }
            public int season { get; set; }
            public int r { get; set; }
            public string go_ao { get; set; }
            public int sb { get; set; }
            public string player_id { get; set; }
            public int ab { get; set; }
            public int ibb { get; set; }
            public string go { get; set; }
        }

        public class SportHittingResults
        {
            public string created { get; set; }
            public string totalSize { get; set; }
            public Hitting row { get; set; }
        }

        public class SportHitting
        {
            public SportHittingResults queryResults { get; set; }
        }

        public class SportHittingGameLogComposed
        {
            public string copyRight { get; set; }
            public GameLogData SportHittingGameLog { get; set; }
            public SportHitting sport_hitting { get; set; }
        }

        public class MlbStatsResponse
        {
            public SportHittingGameLogComposed sport_hitting_game_log_composed { get; set; }
        }
    }
}