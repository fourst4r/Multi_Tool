using LevelModel.Models;

namespace Builders.DataStructures.DTO
{
    public class MergeDTO
    {
        public Level Level1 { get; set; }

        public Level Level2 { get; set; }

        public int PaddingX { get; set; }

        public int PaddingY { get; set; }

        public string Title { get; set; }

        public enum KeepSettings { First = 1, Second }

        public KeepSettings Settings { get; set; }

    }

}
