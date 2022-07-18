using System;
using System.Globalization;
using UserInterface.Handlers;
using UserInterface.DataStructures.Info;
using UserInterface.Handlers.FileHandlers;

using static Builders.DataStructures.DTO.MergeDTO;

namespace UserInterface.Menu.Options.ExistingLevel.Options
{
    internal class MergeLevelsOption : BaseMenuOption
    {


        private BuildHandler _builder;
        private MergeInfo _info;


        internal MergeLevelsOption()
        {
            _builder = new BuildHandler();
            _info    = new MergeInfo();

            GetRequiredInfo();

            if (IsInputValid)
                MergeLevels();
            else
                WriteLine(Environment.NewLine + "\tError, invalid input!", ErrorColor);
        }


        private void GetRequiredInfo()
        {
            IsInputValid = true;

            if (IsInputValid)
                _info.LevelID1 = ReadInteger("Level 1 ID:  ", 0);

            if (IsInputValid)
                _info.LevelID2 = ReadInteger("Level 2 ID:  ", 0);

            if (IsInputValid)
                _info.Settings = GetSettings();

            if (IsInputValid)
                _info.PaddingX = ReadInteger("Block Padding, X-axis:  ");

            if (IsInputValid)
                _info.PaddingY = ReadInteger("Block Padding, Y-axis:  ");

            if (IsInputValid)
                _info.Title = ReadString("Title:  ");

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Name = GetUsername();

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Token = GetToken();
        }

        private KeepSettings GetSettings()
        {
            WriteLine("Keep art and settings of:  ");
            WriteLine("\t" + (int)KeepSettings.First  + "  -  First  level");
            WriteLine("\t" + (int)KeepSettings.Second + "  -  Second level");
            Write(Environment.NewLine + "Pick option:  ", UserInputColor);

            string input = ReadInput();

            if (int.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out int value) && Enum.IsDefined(typeof(KeepSettings), value))
                return (KeepSettings)value;

            IsInputValid = false;
            return KeepSettings.First;
        }


        private void MergeLevels()
        {
            WriteLine(Environment.NewLine + "\tMerging levels...");
            _builder.BuildMerge(_info);
        }


    }
}
