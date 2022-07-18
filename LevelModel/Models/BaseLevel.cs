using LevelModel.Models.Components;

namespace LevelModel.Models
{
    public abstract class BaseLevel
    {

        public int LevelID { get; set; }

        public int Version { get; set; }

        public string Title { get; set; }

        public string Note { get; set; }

        public int RankLimit { get; set; }

        public bool Published { get; set; }

        public GameMode GameMode { get; set; }

    }
}
