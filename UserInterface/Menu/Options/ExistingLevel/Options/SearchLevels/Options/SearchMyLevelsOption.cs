using LevelModel.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UserInterface.DataStructures.Constants;
using UserInterface.Handlers;
using UserInterface.Menu.Options.ExistingLevel.Options.SearchPublicLevels.Formatters;

namespace UserInterface.Menu.Options.ExistingLevel.Options.SearchLevels.Options
{
    class SearchMyLevelsOption : BaseMenuOption
    {

        private AccessHandler _accessHandler;
        private ParseHandler _parseHandler;


        public SearchMyLevelsOption()
        {
            _accessHandler = new AccessHandler();
            _parseHandler  = new ParseHandler();

            LoadMyLevels();
        }

        private void LoadMyLevels()
        {
            string loadResult = _accessHandler.LoadMyLevels();

            if (loadResult.Length != 0)
            {
                var levels = _parseHandler.ParseLoadResult(loadResult);
                if (levels != null && levels.Count != 0)
                {
                    ShowResult(levels.Cast<BaseLevel>().ToList());
                }
            }
        }

        private void ShowResult(List<BaseLevel> levels)
        {
            SearchFormatter.ShowSearch(levels);
            ShowOptions();
            ParseOption(levels, ReadInput().ToLower(CultureInfo.InvariantCulture));
        }


        private void ShowOptions()
        {
            WriteLine();
            WriteLine("\t" + MenuOptions.QUIT       + "  -  Quit/Back" + Environment.NewLine);
            Write("Pick option:  ", UserInputColor);
        }

        private void ShowLevelOption(int id)
        {
            string levelData = _accessHandler.Download(id);

            if (levelData != AccessHandler.FAILED)
            {
                var level = _parseHandler.Parse(levelData);

                if (level != null)
                    SearchFormatter.ShowLevel(level);
            }
        }

        private void ParseOption(List<BaseLevel> levels, string option)
        {
            option = option.ToLower(CultureInfo.InvariantCulture);

            if (int.TryParse(option, NumberStyles.Any, CultureInfo.InvariantCulture, out int value) && value >= 0 && value <= levels.Count)
                ShowLevelOption(levels[value - 1].LevelID);
            else if (!option.Equals(MenuOptions.QUIT, StringComparison.InvariantCultureIgnoreCase))
                WriteLine(Environment.NewLine + "\tError, wrong input!", ErrorColor);
        }
    }
}
