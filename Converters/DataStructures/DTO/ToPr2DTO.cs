using LevelModel.Models;

namespace Converters.DataStructures.DTO
{
    public class ToPr2DTO
    {

        public Level Level { get; set; }

        public string Username { get; set; }

        public string Token { get; set; }

        public bool EnableEncoding { get; set; }

        public bool OverWrite { get; set; }

        public bool Newest { get; set; }


    }
}
