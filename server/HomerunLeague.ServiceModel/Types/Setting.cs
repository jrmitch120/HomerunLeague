using ServiceStack.DataAnnotations;

namespace HomerunLeague.ServiceModel.Types
{
    public class Setting
    {
        [AutoIncrement]
        public int Id { get; set; }

        [Index(Unique = true)]
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
