using System;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using System.Collections.Generic;
using LevelModel.Models;
using LevelModel.Models.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Parsers.Parsers
{
    internal class SearchResultParser : BaseParser
    {

        public List<SearchResultLevel> Levels { get; }

        private Dictionary<string, Action<string, SearchResultLevel>> _parser;

        private JObject _jsonInput;


        public SearchResultParser(string input)
        {
            Levels     = new List<SearchResultLevel>();
            _jsonInput = JsonConvert.DeserializeObject(input) as JObject;
            _parser    = new Dictionary<string, Action<string, SearchResultLevel>>();

            BuildParser();

            if (IsDataValid())
            {
                Parse(_jsonInput.First);
            }
        }


        private void Parse(dynamic jsonLevels)
        {
            foreach (JArray array in jsonLevels)
            {
                foreach (JObject level in array.Children<JObject>())
                {
                    ParseLevel(level);
                }
            }
        }

        private void ParseLevel(JObject jsonLevel)
        {
            var currentLevel = new SearchResultLevel();
           
            foreach (JProperty field in jsonLevel.Properties())
            {
                if (_parser.TryGetValue(field.Name.ToLower(CultureInfo.InvariantCulture), out Action<string, SearchResultLevel> action))
                    action.Invoke(field.Value.ToString(), currentLevel);
            }

            Levels.Add(currentLevel);
        }

        protected void BuildParser()
        {
            _parser.Add("level_id",   (x, l) => l.LevelID   = ParseInt(x));
            _parser.Add("version",    (x, l) => l.Version   = ParseInt(x));
            _parser.Add("title",      (x, l) => l.Title     = ParsePr2Text(x));
            _parser.Add("rating",     (x, l) => l.Rating    = ParseDouble(x));
            _parser.Add("play_count", (x, l) => l.PlayCount = ParseInt(x));
            _parser.Add("min_level",  (x, l) => l.RankLimit = ParseInt(x));
            _parser.Add("note",       (x, l) => l.Note      = ParsePr2Text(x));
            _parser.Add("live",       (x, l) => l.Published = ParseBool(x));
            _parser.Add("pass",       (x, l) => l.Password  = ParsePr2Text(x));
            _parser.Add("type",       (x, l) => l.GameMode  = new GameMode(x));
            _parser.Add("time",       (x, l) => l.Time      = ParseInt(x));
            _parser.Add("user_name",  (x, l) => l.UserName  = ParsePr2Text(x));
            _parser.Add("user_group", (x, l) => l.Group     = ParsePr2Text(x));
            _parser.Add("bad_hats",   (x, l) => {    /*  ignore  */         });
        }

        private bool IsDataValid()
        {
            if (_jsonInput != null && _jsonInput.Count >= 2)
                return true;

            string errorMessage = _jsonInput?.GetValue("error")?.ToString() ?? "Input data is invalid, parsing canceled";

            throw new InvalidDataException(errorMessage);
        }

    }
}
