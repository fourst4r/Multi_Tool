using LevelModel.Models;
using static LevelModel.Models.Level;

namespace Builders.DataStructures.DTO
{
    public class AddArtBlocksDTO
    {


        public ArtType ArtType { get; set; }

        public Level Level { get; set; }

        public int BlockId { get; set; }

        public int ArtBlockId { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
    }
}
