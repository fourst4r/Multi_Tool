namespace Builders.DataStructures.DTO
{
    public class BuildDTO
    {


        public ImageDTO ImageInfo { get; set; }

        public string Title { get; set; }

        public BuildType Type { get; set; }

        public int Difficulty { get; set; }

        public enum BuildType { Simple = 1, SmallLabyrinth, LargeLabyrinth, ShortTraps, Trapwork, MaxBlockLimit, Image };

        public enum TrapDifficulty { Easy = 1, Medium, Hard }; 


        public BuildDTO(BuildType type) : this()
        {
            Type = type;
        }

        public BuildDTO()
        {
            ImageInfo = new ImageDTO();
        }


    }
}
