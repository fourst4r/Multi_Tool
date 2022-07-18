using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using LevelModel.Models;
using UserInterface.Handlers;
using UserInterface.Handlers.FileHandlers;
using UserInterface.DataStructures.Constants;
using UserInterface.Menu.Options.ExistingLevel.Options.SearchPublicLevels.Formatters;

namespace UserInterface.Menu.Options.ExistingLevel.Options.SearchPublicLevels
{
    internal class SearchPublicLevelsOption : BaseMenuOption
    {

        private string _userToSearch;
        private int    _currentPage = 1;
        private bool _showSearchResult;

        private AccessHandler _accessHandler;
        private ParseHandler  _parseHandler;


        internal SearchPublicLevelsOption()
        {
            _parseHandler  = new ParseHandler();
            _accessHandler = new AccessHandler();

            StartSearch();
        }


        private void GetRequiredInfo(bool getUsername = true)
        {
            IsInputValid = true;

            if (getUsername)
                _userToSearch = GetUsernameToSearch();

            if (IsInputValid)
                _currentPage = GetPage();
        }

        private void StartSearch(bool getUsername = true)
        {
            GetRequiredInfo(getUsername);

            if (!IsInputValid)
            {
                WriteLine(Environment.NewLine + "\tError, wrong input!", ErrorColor);
                return;
            }

            SearchUser();
        }

        private void SearchUser()
        {
            string searchResult = _accessHandler.Search(_userToSearch, _currentPage);
            _showSearchResult = true;

            if (searchResult.Length != 0)
            {
                var levels = _parseHandler.ParseSearchResult(searchResult);
                if (levels != null && levels.Count != 0)
                    ShowResult(levels.Cast<BaseLevel>().ToList());
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
            if(_showSearchResult)
                WriteLine("\t" + MenuOptions.CHANGE_PAGE + "  -  Change page");

            WriteLine("\t" + MenuOptions.NEW_SEARCH + "  -  New search");
            WriteLine("\t" + MenuOptions.QUIT       + "  -  Quit/Back" + Environment.NewLine);

            Write("Pick option:  ", UserInputColor);
        }

        private int GetPage()
        {
            Write("Page:  ", UserInputColor);
            string page = ReadInput();

            if (int.TryParse(page, NumberStyles.Any, CultureInfo.InvariantCulture, out int value))
                return value;

            IsInputValid = false;
            return ERROR;
        }

        private void ParseOption(List<BaseLevel> levels, string option)
        {
            option = option.ToLower(CultureInfo.InvariantCulture);

            if (int.TryParse(option, NumberStyles.Any, CultureInfo.InvariantCulture, out int value) && value >= 0 && value <= levels.Count)
                ShowLevelOption(levels[value - 1].LevelID);
            else if (option.Equals(MenuOptions.NEW_SEARCH, StringComparison.InvariantCultureIgnoreCase))
                StartSearch();
            else if (option.Equals(MenuOptions.CHANGE_PAGE, StringComparison.InvariantCultureIgnoreCase) && _showSearchResult)
                StartSearch(false);
            else if (!option.Equals(MenuOptions.QUIT, StringComparison.InvariantCultureIgnoreCase))
                WriteLine(Environment.NewLine + "\tError, wrong input!", ErrorColor);
        }

        private void ShowLevelOption(int id)
        {
            string levelData = _accessHandler.Download(id);

            if (levelData != AccessHandler.FAILED)
            {
                var level = _parseHandler.Parse(levelData);

                if(level != null)
                    SearchFormatter.ShowLevel(level);
            }
        }

        private string GetUsernameToSearch()
        {
            Write("Username:  ", UserInputColor);
            string username = ReadInput();

            if (!string.IsNullOrWhiteSpace(username))
                return username;

            IsInputValid = false;
            return string.Empty;
        }


    }
}
