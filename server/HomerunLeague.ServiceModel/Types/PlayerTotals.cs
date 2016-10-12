using ServiceStack.DataAnnotations;

namespace HomerunLeague.ServiceModel.Types
{
    [CompositeIndex("Year", "PlayerId", Unique=true)]
    public class PlayerTotals
    {
        public string Id { get { return PlayerId + "/" + Year; } }

        public int Year { get; set; }

        public int PlayerId { get; set; }

        // Stats
        public int G { get; set; }

        public int Ab { get; set; }

        public decimal Avg { get; set; }

        public int Bb { get; set; }

        public int Ibb { get; set; } // intensional base on balls

        public int Cs { get; set; } // caught stealing

        public int D { get; set; } // doubles

        public int H { get; set; }

        public int Hbp { get; set; }

        public int Hr { get; set; }

        public int Hr7 { get; set; }

        public int Hr14 { get; set; }

        public int Hr30 { get; set; }

        public int R { get; set; } // runs

        public int Rbi { get; set; }

        public decimal Slg { get; set; }

        public int Sb { get; set; } // sac bunt

        public int Sf { get; set; } // sav fly

        public int So { get; set; }

        public int T { get; set; } // tripples

        public int Tb { get; set; }
    }
}