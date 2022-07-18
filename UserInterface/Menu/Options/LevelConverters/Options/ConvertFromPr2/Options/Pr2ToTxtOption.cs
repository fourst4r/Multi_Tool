using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UserInterface.Handlers;
using UserInterface.DataStructures.Info;
using UserInterface.Handlers.FileHandlers;

using static UserInterface.DataStructures.Constants.MyPaths;

namespace UserInterface.Menu.Options.LevelConverters.Options.ConvertFromPr2.Options
{
    class Pr2ToTxtOption : BaseMenuOption
    {

        private ConvertHandler _converter;
        private string _filepath;
        private int _levelID;


        internal Pr2ToTxtOption()
        {
            _converter = new ConvertHandler();

            GetRequiredInfo();

            if (IsInputValid)
                Convert();
            else
                WriteLine(Environment.NewLine + "\tError: Invalid input!", ErrorColor);
        }


        private void Convert()
        {
            WriteLine(Environment.NewLine + "\tConverting...");
            var success = _converter.Pr2ToTxt(_filepath, _levelID);

            if (success)
                WriteLine(Environment.NewLine + "\tResult saved at: " + _filepath);
        }

        private void SetFilepath(string filename)
        {
            if(string.IsNullOrWhiteSpace(filename))
            {
                IsInputValid = false;
                return;
            }

            _filepath = Path.IsPathRooted(filename) ? filename : Path.Combine(USER_LEVEL_FOLDER, filename);
            var ext   = Path.GetExtension(_filepath);

            if(string.IsNullOrWhiteSpace(ext))
                _filepath += ".txt";
        }

        private void GetRequiredInfo()
        {
            IsInputValid = true;

            _levelID = ReadInteger("Level ID:  ", 0);

            if(IsInputValid)
                SetFilepath(ReadString("Save with filename: "));

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Name = GetUsername();

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Token = GetToken();
        }

    }
}
