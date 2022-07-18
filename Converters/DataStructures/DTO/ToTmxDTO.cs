using LevelModel.DTO;
using LevelModel.Models;

namespace Converters.DataStructures.DTO
{
    public class ToTmxDTO
    {

        public Level Level { get; set; }

        public string LevelData { get; set; }

        public string TilesetPath { get; set; }

        public Messages Messages { get; set; }

        public ToTmxDTO() {
            Messages = new Messages();
        }
    }
}
