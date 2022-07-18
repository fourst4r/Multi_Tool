namespace LevelModel.Models
{
    public class LoadResultLevel : BaseLevel
    {

        public int PlayCount { get; set; }

        public double Rating { get; set; }

        public int UserID { get; set; }

        public string UserName { get; set; }

        public int Power { get; set; }

        public int Time { get; set; }

        public bool TrialMode { get; set; }

    }
}
