using System;
using System.IO;
using UserInterface.Handlers;
using UserInterface.DataStructures.Info;
using UserInterface.Handlers.FileHandlers;


namespace UserInterface.Menu.Options.LevelConverters.Options.ConvertFromPr2.Options
{
    internal class Pr2ToTmxOption : BaseMenuOption
    {

        private ConvertHandler _converter;
        private ConvertInfo _info;


        internal Pr2ToTmxOption()
        {
            _converter = new ConvertHandler();
            _info = new ConvertInfo();

            GetRequiredInfo();

            if (IsInputValid)
                Convert();
            else
                WriteLine(Environment.NewLine + "\tError: Invalid input!", ErrorColor);
        }


        private void Convert()
        {
            WriteLine(Environment.NewLine + "\tConverting...");
            var success = _converter.Pr2ToTmx(_info);

            if (success)
            {
                WriteLine(Environment.NewLine + "\tResult saved at: " + _info.FilePath);
                WriteLine(Environment.NewLine + "\tNote: This file can be opened with the program Tiled Editor.", NoteColor);
            }
        }

        private void GetRequiredInfo()
        {
            IsInputValid = true;

            _info.LevelID = ReadInteger("Level ID:  ", 0);

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Name = GetUsername();

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Token = GetToken();
        }

    }
}
