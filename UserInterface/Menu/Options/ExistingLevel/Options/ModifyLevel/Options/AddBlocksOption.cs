using System;
using System.Globalization;
using UserInterface.DataStructures.Info;
using UserInterface.Handlers;
using UserInterface.Handlers.FileHandlers;

using static Builders.DataStructures.DTO.ExtendDTO;

namespace UserInterface.Menu.Options.ExistingLevel.Options.ModifyLevel.Options
{
    internal class AddBlocksOption : BaseMenuOption
    {

        private BuildHandler _builder;
        private ExtendInfo _info;


        internal AddBlocksOption()
        {
            _builder = new BuildHandler();
            _info = new ExtendInfo();

            Init();
            GetRequiredInfo();

            if (IsInputValid)
                Extend();
            else
                WriteLine(Environment.NewLine + "\tError: Invalid input!", ErrorColor);
        }


        private void Init()
        {
            _info.Type = ExtendType.Blocks;
        }

        private void GetRequiredInfo()
        {
            IsInputValid = true;

            _info.LevelID = ReadInteger("Level ID:  ", 0);

            if (IsInputValid)
                _info.InputBlocks = GetBlockData();

            if (IsInputValid)
                _info.SetPaddingX(ReadInteger("Block Padding, X - axis:  "));

            if (IsInputValid)
                _info.SetPaddingY(ReadInteger("Block Padding, Y - axis:  "));

            if (IsInputValid)
                _info.Multiplier = GetMultiplier();

            if (IsInputValid)
                _info.Title = ReadString("Title:  ");

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Name = GetUsername();

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Token = GetToken();
        }

        private void Extend()
        {
            WriteLine(Environment.NewLine + "\tAdding blocks...");

            if (IsInputValid)
                _builder.ExtendLevel(_info);
        }

        protected string GetBlockData()
        {
            Write("Block Data:  ", UserInputColor);
            string input = ReadInput();

            if (input.Length != 0)
                return RemoveStartAndEndSymbols(input);

            IsInputValid = false;
            return String.Empty;
        }

        private string RemoveStartAndEndSymbols(string input)
        {
            if (input == null || input.Length == 0)
                return String.Empty;

            if (input[0] == '`' || input[0] == ',')
                return RemoveStartAndEndSymbols(input.Substring(1, input.Length - 1));

            if (input[input.Length - 1] == '`' || input[input.Length - 1] == ',')
                return RemoveStartAndEndSymbols(input.Substring(0, input.Length - 1));

            return input;
        }

        private int GetMultiplier()
        {
            Write("Multiplier:  ", UserInputColor);
            string input = ReadInput();

            if (int.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out int value) && value > 0)
                return value;

            IsInputValid = false;
            return ERROR;
        }

    }
}

