using System.Xml;
using LevelModel.DTO;

namespace Converters.DataStructures.DTO
{
    public class ToLevelDTO
    {

        public XmlDocument Level { get; set; }

        public Messages Messages { get; set; }

        public ToLevelDTO() {
            Messages = new Messages();
        }
    }
}
