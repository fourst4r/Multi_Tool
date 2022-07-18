using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using LevelModel.Models;
using UserInterface.Handlers;
using UserInterface.DataStructures.Info;
using UserInterface.Handlers.FileHandlers;

namespace UserInterface.Menu.Options.ExistingLevel.Options.ModifyLevel.Options
{
    class MoveBlocksOption : BaseMenuOption
    {

        private BuildHandler _builder;
        private MoveBlocksInfo _info;
        private bool _quit;

        internal MoveBlocksOption(bool moveArt)
        {
            _builder = new BuildHandler();
            _info    = new MoveBlocksInfo(moveArt);
            _quit    = false;

            GetRequiredInfo();

            if (IsInputValid)
                _builder.MoveBlocks(_info);
            else if(!_quit)
                WriteLine(Environment.NewLine + "\tError: Invalid input!", ErrorColor);
        }

        private void ShowStartPosition()
        {
            _info.Level = _builder.DownloadLevel(_info.LevelID);

            if(_info.Level?.Blocks == null)
            {
                if (_info.Level?.Blocks?.Count == 0)
                    WriteLine("The level has no blocks..." + Environment.NewLine);

                IsInputValid = false;
                _quit = true;
                return;
            }

            WriteLine("Current start position:");
            WriteLine("\tX = " + _info.Level.Blocks.First().X);
            WriteLine("\tY = " + _info.Level.Blocks.First().Y);
            WriteLine();
        }

        private void GetRequiredInfo()
        {
            IsInputValid = true;

            if (IsInputValid)
                _info.LevelID = ReadInteger("Level ID:  ", 0);

            if (IsInputValid)
                ShowStartPosition();

            if (IsInputValid)
                _info.X = ReadInteger("Move Distance, X-axis:  ");

            if (IsInputValid)
                _info.Y = ReadInteger("Move Distance, Y-axis:  ");

            if (IsInputValid)
                _info.Title = ReadString("Title:  ");

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Name = GetUsername();

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Token = GetToken();
        }

    }
}
