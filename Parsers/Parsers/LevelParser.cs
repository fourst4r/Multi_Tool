using System;
using System.Diagnostics;
using System.Globalization;
using System.Collections.Generic;
using Parsers.DTO;
using LevelModel;
using LevelModel.Models.Components;
using static LevelModel.DTO.Message;

namespace Parsers.Parsers
{
    internal class LevelParser : AttributeParser
    {


        internal LevelDTO Result { get; private set; }

        // value indicate that the map has a password
        private const string HAS_PASSWORD = "1";


        internal LevelParser(string data)
        {
            Result = new LevelDTO();

            BuildAttributeParser();
            ParseAttributes(data, true);
        }


        protected override void ParseAttribute(string name, string value)
        {
            if (name != null && value != null && attributeParser.TryGetValue(name.ToLower(CultureInfo.InvariantCulture), out Action<string> action))
            {
                action?.Invoke(value);
            }
        }

        protected void BuildAttributeParser()
        {
            attributeParser.Add("level_id", x     => Result.Level.LevelID      = ParseInt(x));
            attributeParser.Add("version", x      => Result.Level.Version      = ParseInt(x));
            attributeParser.Add("user_id", x      => Result.Level.UserID       = ParseDouble(x));
            attributeParser.Add("credits", x      => Result.Level.Credits      = ParsePr2Text(x));
            attributeParser.Add("cowboychance", x => Result.Level.CowboyChance = ParseInt(x));
            attributeParser.Add("title", x        => Result.Level.Title        = ParsePr2Text(x));
            attributeParser.Add("time", x         => Result.Level.Time         = ParseDouble(x));
            attributeParser.Add("note", x         => Result.Level.Note         = ParsePr2Text(x));
            attributeParser.Add("min_level", x    => Result.Level.RankLimit    = ParseInt(x));
            attributeParser.Add("song", x         => Result.Level.Song         = ParseSong(x));
            attributeParser.Add("gravity", x      => Result.Level.Gravity      = ParseDouble(x));
            attributeParser.Add("max_time", x     => Result.Level.MaxTime      = ParseDouble(x));
            attributeParser.Add("has_pass", x     => Result.Level.HasPassword  = ParseHasPassword(x));
            attributeParser.Add("live", x         => Result.Level.Published    = ParseBool(x));
            attributeParser.Add("items", x        => Result.Level.Items        = ParseItems(x));
            attributeParser.Add("badhats", x      => Result.Level.BadHats      = ParseBadHats(x));
            attributeParser.Add("gamemode", x     => Result.Level.GameMode     = new GameMode(x));
            attributeParser.Add("data", x         => ParseData(x));
        }

        private bool ParseHasPassword(string s) => s.Equals(HAS_PASSWORD, StringComparison.InvariantCultureIgnoreCase);  

        private List<Item> ParseItems(string s)
        {
            List<Item> myReturn = new List<Item>();

            foreach (string itemStr in s.Split('`'))
            {
                if (int.TryParse(itemStr, NumberStyles.Any, CultureInfo.InvariantCulture, out int value))
                    myReturn.Add(new Item(value));
                else
                {
                    var item = new Item(itemStr);
                    if (item.ID != Item.NONE)
                        myReturn.Add(item);
                }
            }
            return myReturn;
        }

        private List<int> ParseBadHats(string s)
        {
            List<int> myReturn = new List<int>();

            if(string.IsNullOrWhiteSpace(s))
                return myReturn;

            foreach (string itemStr in s.Split(','))
            {
                if (int.TryParse(itemStr, NumberStyles.Any, CultureInfo.InvariantCulture, out int value))
                    myReturn.Add(value);
            }

            return myReturn;
        }


        private Song ParseSong(string s)
        {
            if (s == null || s.Length == 0)
                return new Song(Song.NONE);

            if(s.Equals("random", StringComparison.InvariantCultureIgnoreCase))
                return new Song(Song.RANDOM);

            return new Song(ParseInt(s));
        }

        private void ParseData(string dataInput)
        {
            string[] data = dataInput.Split("`");

            Result.Level.DataVersion     = GetElement(data, 0);
            Result.Level.BackgroundColor = GetElement(data, 1);
            Result.Level.Blocks          = new BlockParser(GetElement(data, 2)).Result;
            Result.Level.TextArt1        = new TextArtParser(GetElement(data, 3), Result.Messages, "Text Art 1").Result;
            Result.Level.TextArt2        = new TextArtParser(GetElement(data, 4), Result.Messages, "Text Art 2").Result;
            Result.Level.TextArt3        = new TextArtParser(GetElement(data, 5), Result.Messages, "Text Art 3").Result;
            Result.Level.DrawArt1        = new DrawArtParser(GetElement(data, 6), Result.Messages, "Draw Art 1" ).Result;
            Result.Level.DrawArt2        = new DrawArtParser(GetElement(data, 7), Result.Messages, "Draw Art 2").Result;
            Result.Level.DrawArt3        = new DrawArtParser(GetElement(data, 8), Result.Messages, "Draw Art 3").Result;
             
            ParseExtendedData(data);
        }

        private void ParseExtendedData(string[] data) {
            if (data.Length == 14) {
                Result.Level.BackgroundImage = GetElement(data, 9);
                Result.Level.TextArt0        = new TextArtParser(GetElement(data, 10),  Result.Messages, "Text Art 0").Result;
                Result.Level.TextArt00       = new TextArtParser(GetElement(data, 11),  Result.Messages, "Text Art 00").Result;
                Result.Level.DrawArt0        = new DrawArtParser(GetElement(data, 12),  Result.Messages, "Draw Art 0").Result;
                Result.Level.DrawArt00       = new DrawArtParser(GetElement(data, 13),  Result.Messages, "Draw Art 00").Result;
            }
            else if(data.Length == 10) { 
                Result.Level.BackgroundImage = GetElement(data, 9);
            }
            else {
                Result.Messages.Add("Invalid amount of art elements with the attribute data");
            }
        }

        private string GetElement(string[] data, int index)
        {
            if (data != null && data.Length > index) 
                return data[index];

            return string.Empty;
        }


    }
}
