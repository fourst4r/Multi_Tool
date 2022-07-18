using LevelModel.Models;

namespace Builders.DataStructures.DTO
{
    public class RemoveBlocksDTO
    {

        public int? BlockID { get; set; }

        public Level Level { get; set; }

        public bool RemoveAll { get; set; }
    }
}
