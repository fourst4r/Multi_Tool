using System;
using System.Diagnostics;
using System.Globalization;
using System.Collections.Generic;
using LevelModel.Models;
using LevelModel.Models.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LevelModel.DTO;

namespace Parsers.Parsers
{
    internal class LoadResultParser : BaseParser
    {


        public List<LoadResultLevel> Levels { get; }

        private  Dictionary<string, Action<string, LoadResultLevel>> _parser;

        private JObject _jsonInput;


        public LoadResultParser(string input)
        {
            Levels     = new List<LoadResultLevel>();
            _jsonInput = (JObject)JsonConvert.DeserializeObject(input);
            _parser    = new Dictionary<string, Action<string, LoadResultLevel>>();

            buildParser();

            if(IsDataValid())
            {
                Parse(_jsonInput.Last);
            }
        }


        private void Parse(dynamic jsonLevels)
        {
            foreach(JArray array in jsonLevels)
            {
                foreach (JObject level in array.Children<JObject>())
                {
                    ParseLevel(level);
                }
            }
        }

        private void ParseLevel(JObject jsonLevel)
        {
            var levelToAdd = new LoadResultLevel();

            foreach (JProperty field in jsonLevel.Properties())
            {
                if (_parser.TryGetValue(field.Name.ToLower(CultureInfo.InvariantCulture), out Action<string, LoadResultLevel> action))
                    action.Invoke(field.Value.ToString(), levelToAdd);
            }

            Levels.Add(levelToAdd);
        }

        protected void buildParser()
        {
            _parser.Add("level_id",     (x, l) => l.LevelID   = ParseInt(x));
            _parser.Add("live",         (x, l) => l.Published = ParseBool(x));
            _parser.Add("min_level",    (x, l) => l.RankLimit = ParseInt(x));
            _parser.Add("name",         (x, l) => l.UserName  = ParsePr2Text(x));
            _parser.Add("note",         (x, l) => l.Note      = ParsePr2Text(x));
            _parser.Add("play_count",   (x, l) => l.PlayCount = ParseInt(x));
            _parser.Add("power",        (x, l) => l.Power     = ParseInt(x));
            _parser.Add("rating",       (x, l) => l.Rating    = ParseDouble(x));
            _parser.Add("title",        (x, l) => l.Title     = ParsePr2Text(x));
            _parser.Add("type",         (x, l) => l.GameMode  = new GameMode(x));
            _parser.Add("user_id",      (x, l) => l.UserID    = ParseInt(x));
            _parser.Add("version",      (x, l) => l.Version   = ParseInt(x));
            _parser.Add("time",         (x, l) => l.Time      = ParseInt(x));
            _parser.Add("trial_mod",    (x, l) => l.TrialMode = ParseBool(x)); 
            _parser.Add("bad_hats",      (x, l) => {     /* ignore*/       } ); 
        }


        private bool IsDataValid()
        {
            if (_jsonInput?.GetValue("success")?.ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase) ?? false)
                return true;

            string errorMessage = _jsonInput?.GetValue("error")?.ToString() ?? "Input data is invalid, parsing canceled.";

            throw new InvalidDataException(errorMessage);
        }


    }
}
