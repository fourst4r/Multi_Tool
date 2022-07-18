using LevelModel.DTO;
using LevelModel.Models;

namespace Parsers.DTO
{
    public class LevelDTO
    {

        public Level Level { get; set; }

         public Messages Messages { get; set; }


        public LevelDTO() {
            Messages = new Messages();
            Level = new Level();
        }
    }
}
