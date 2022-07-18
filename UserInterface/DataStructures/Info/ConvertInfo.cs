using Converters.DataStructures.DTO;
using LevelModel.Models;

namespace UserInterface.DataStructures.Info
{
    internal class ConvertInfo
    {

        public int LevelID { get; set; }

        private Level level;
        public Level Level
        {
            get => level;
            set
            {
                level = value;
                ToTmxDTO.Level = level;
            }
        }

        public string Title { get; set; }

        public string FilePath { get; set; }

        public ToTmxDTO ToTmxDTO { get; set; } = new ToTmxDTO();

        public ToLevelDTO ToLevelDTO { get; set; } = new ToLevelDTO();

    }
}
