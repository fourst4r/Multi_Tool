using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UserInterface.DataStructures.Constants;
using UserInterface.DataStructures.Info;
using UserInterface.Handlers;
using UserInterface.Handlers.FileHandlers;
using LevelModel.Models.Components;

namespace UserInterface.Menu.Options.ExistingLevel.Options.ModifyLevel.Options
{
    class RemoveBlocksOption : BaseMenuOption
    {

        private BuildHandler _builder;
        private RemoveBlocksInfo _info;
        private bool _quit;

        internal RemoveBlocksOption()
        {
            _builder = new BuildHandler();
            _info    = new RemoveBlocksInfo();
            _quit    = false;

            GetRequiredInfo();

            if (IsInputValid)
                _builder.RemoveBlocks(_info);
            else if(!_quit)
                WriteLine(Environment.NewLine + "\tError: Invalid input!", ErrorColor);
        }


        private void ShowBlockOptions()
        {
            WriteLine("\t" + MenuOptions.REMOVE_ALL_BLOCKS      + "  -  All Blocks");
            WriteLine("\t" + MenuOptions.REMOVE_SPECIFIC_BLOCK  + "  -  Specific Block");
            WriteLine();
            WriteLine("\t" + MenuOptions.QUIT + "  -  Quit/Back");

            Write(Environment.NewLine + "Pick Option:  ", UserInputColor);
        }

        private void HandleBlockOption(string option)
        {
            switch (option.ToLower(CultureInfo.InvariantCulture))
            {
                case MenuOptions.ALL_BLOCKS:
                    _info.DTO.RemoveAll = true;
                    break;

                case MenuOptions.SPECIFIC_BLOCK:
                     ShowBlockIdPath(0);
                    _info.DTO.BlockID = ReadInteger("Block ID:  ", Block.BASIC_WHITE, Block.MaxBlockId);
                    break;

                case MenuOptions.QUIT:
                    IsInputValid = false;
                    _quit = true;
                    break;

                default:
                    IsInputValid = false;
                    break;
            }
        }

        private void SetBlockType()
        {
            ShowBlockOptions();
            HandleBlockOption(ReadInput());
        }

        private void GetRequiredInfo()
        {
            IsInputValid = true;

            if (IsInputValid)
                SetBlockType();

            if (IsInputValid)
                _info.LevelID = ReadInteger("Level ID:  ", 0);

            if (IsInputValid)
                _info.Title = ReadString("Title:  ");

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Name = GetUsername();

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Token = GetToken();
        }

    }
}
