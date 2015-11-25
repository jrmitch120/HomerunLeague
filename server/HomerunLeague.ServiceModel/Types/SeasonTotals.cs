using ServiceStack.DataAnnotations;

namespace HomerunLeague.ServiceModel.Types
{
    [CompositeIndex("Year", "PlayerId")]
    public class SeasonTotals
    {
        [PrimaryKey]
        public string Id { get { return PlayerId + "/" + Year; } }

        public int Year { get; set; }

        public int PlayerId { get; set; }

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
    }
}