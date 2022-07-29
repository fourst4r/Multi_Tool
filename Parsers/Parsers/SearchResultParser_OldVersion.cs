using System;
using System.Diagnostics;
using System.Globalization;
using System.Collections.Generic;
using LevelModel.Models;
using LevelModel.Models.Components;

namespace Parsers.Parsers
{

    /// <summary>
    /// Parse the response after making a search for levels to pr2 server
    /// </summary>
    internal class SearchResultParser_OldVersion : AttributeParser
    {


        private SearchResultLevel currentLevel;
        internal List<SearchResultLevel> Levels { get; set; }

        internal static SearchResultParser_OldVersion Empty = new SearchResultParser_OldVersion();

        internal SearchResultParser_OldVersion(string result)
        {
            Levels = new List<SearchResultLevel>();
            buildAttributeParser();
            ParseAttributes(result, false);
        }

        private SearchResultParser_OldVersion() {
            Levels = new List<SearchResultLevel>();
        }


        protected void buildAttributeParser()
        {
            attributeParser.Add("levelid",   x => createNewLevel(x)); ;
            attributeParser.Add("version",   x => currentLevel.Version   = ParseInt(x));
            attributeParser.Add("title",     x => currentLevel.Title     = ParsePr2Text(x));
            attributeParser.Add("rating",    x => currentLevel.Rating    = ParseDouble(x));
            attributeParser.Add("playcount", x => currentLevel.PlayCount = ParseInt(x));
            attributeParser.Add("minlevel",  x => currentLevel.RankLimit = ParseInt(x));
            attributeParser.Add("note",      x => currentLevel.Note      = ParsePr2Text(x));
            attributeParser.Add("username",  x => currentLevel.UserName  = ParsePr2Text(x));
            attributeParser.Add("group",     x => currentLevel.Group     = ParsePr2Text(x));
            attributeParser.Add("live",      x => currentLevel.Published = ParseBool(x));
            attributeParser.Add("pass",      x => currentLevel.Password  = ParsePr2Text(x));
            attributeParser.Add("type",      x => currentLevel.GameMode  = new GameMode(x));
            attributeParser.Add("time",      x => currentLevel.Time      = ParseInt(x));
        }

        private void createNewLevel(string id)
        {
            currentLevel = new SearchResultLevel() { LevelID = ParseInt(id) };
            Levels.Add(currentLevel);
        }


        protected override void ParseAttribute(string name, string value)
        {
            if (name == null || name.Equals("error", StringComparison.InvariantCultureIgnoreCase))
                return;

            name = ParseAttributeName(name);

            if (name != null && value != null && attributeParser.TryGetValue(name.ToLower(CultureInfo.InvariantCulture), out Action<string> action))
            {
                action?.Invoke(value);
            }
        }

        //All attributes are extended with an ID in searchResult, this removes the ID
        private string ParseAttributeName(string input)
        {
            if (input.Equals("hash", StringComparison.InvariantCultureIgnoreCase))
                return input;

            string lastSymbol = input.Substring(input.Length - 1, 1);

            if (ParseInt(lastSymbol) == ERROR)
                return input;

            return input.Substring(0, input.Length - 1);
        }


    }
}
