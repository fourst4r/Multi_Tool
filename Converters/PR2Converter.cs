using System.Xml;
using LevelModel.Models;
using Converters.Converters;
using Converters.DataStructures.DTO;

namespace Converters
{
    public class PR2Converter
    {

        public static XmlDocument LevelToTMX(ToTmxDTO info) => new LevelToTmx(info).Result;

        public static Level TmxToLevel(ToLevelDTO info) => new TmxToLevel(info).Result;

        public static string LevelToPr2(ToPr2DTO info) => new LevelToPr2(info).Result;

    }
}