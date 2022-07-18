using System;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using DataAccess.DataStructures;
using UserInterface.DataStructures.Constants;
using LevelModel.Models;
using Converters.DataStructures.DTO;
using UserInterface.Handlers.FileHandlers;

namespace UserInterface.Handlers
{
    class OnLevelExistHandler : IOHandler
    {

        private LevelExistArg _arg;
        private Level _level;

        public OnLevelExistHandler(Level level)
        {
            _level = level;
        }

        public void OnLevelExist(LevelExistArg arg)
        {
            _arg = arg;

            WriteLine(Environment.NewLine + "\tWarning: Level already exist.", WarningColor);

            ShowOptions();
            HandleOption(ReadInput());
        }


        private void ShowOptions()
        {
            WriteLine();
            WriteLine("\t" + MenuOptions.OVERWRITE     + "  -  Overwrite Level");
            WriteLine("\t" + MenuOptions.CHANGE_TITLE  + "  -  Change Title");

            WriteLine();
            WriteLine("\t" + MenuOptions.QUIT + "  -  Quit/Back");

            Write(Environment.NewLine + "Pick option:  ", UserInputColor);
        }

        private void Overwrite()
        {
            WriteLine("\tOverwriting...");
            CreateNewLevelData(true);
        }

        private void CreateNewLevelData(bool overwrite)
        {
            var info = new ToPr2DTO
            {
                Level          = _level,
                Username       = UserSettingsHandler.CurrentUser.Name,
                Token          = UserSettingsHandler.CurrentUser.Token,
                EnableEncoding = true,
                OverWrite      = overwrite
            };

            _arg.NewLevelData = new ConvertHandler().LevelToPr2(info);
            _arg.TryAgain     = true;
        }

        private string GetTitle()
        {
            Write("New Title: ", UserInputColor);
            return ReadInput();
        }
        private void ChangeTitle()
        {
            var title = GetTitle();

            if(string.IsNullOrWhiteSpace(title))
            {
                WriteLine("\tError: Invalid input.", ErrorColor);
                return;
            }

            WriteLine("\tChanging Title...");
            _level.Title  = title;
            CreateNewLevelData(false);
        }

        private void HandleOption(string option)
        {
            switch (option.ToLower(CultureInfo.InvariantCulture))
            {

                case MenuOptions.OVERWRITE:
                    Overwrite();
                    break;

                case MenuOptions.CHANGE_TITLE:
                    ChangeTitle();
                    break;

                case MenuOptions.QUIT:
                    break;

                default:
                    WriteLine("\tError: Invalid input.", ErrorColor);
                    break;
            }
        }

    }
}
