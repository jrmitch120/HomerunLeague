using System;

namespace HomerunLeague.ServiceModel.Types
{
    public class GameLog
    {
        public string Id { get { return PlayerId + "" +
                                        ":" + GameId; } }

        public string GameId { get; set; }

        public int PlayerId { get; set; }

        public DateTime GameDate { get; set; }

        // Stats
        public int Ab { get; set; }

        public decimal Avg { get; set; }

        public int Bb { get; set; }

        public int Ibb { get; set; } // intensional base on balls

        public int Cs { get; set; } // caught stealing

        public int D { get; set; } // doubles

        public int H { get; set; }

        public int Hbp { get; set; }

        public int Hr { get; set; }

        public int R { get; set; } // runs

        public int Rbi { get; set; }

        public decimal Slg { get; set; }

        public int Sb { get; set; } // sac bunt

        public int Sf { get; set; } // sav fly

        public int So { get; set; }

        public int T { get; set; } // tripples

        public int Tb { get; set; }

        // Supporting info
        public string Team { get; set; }

        public string Opponent { get; set; }

        public string HomeAway { get; set; }

        public int TeamScore { get; set; }

        public int OppnentScore { get; set; }

        public string Result { get; set; }
    }
}
