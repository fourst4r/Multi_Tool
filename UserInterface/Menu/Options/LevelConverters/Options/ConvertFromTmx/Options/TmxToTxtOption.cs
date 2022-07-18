using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using UserInterface.Handlers;
using UserInterface.DataStructures.Info;
using UserInterface.Handlers.FileHandlers;

using static UserInterface.DataStructures.Constants.MyPaths;

namespace UserInterface.Menu.Options.LevelConverters.Options.ConvertFromTmx.Options
{
    class TmxToTxtOption : BaseMenuOption
    {

        private ConvertHandler _converter;
        private string _tmxFilepath;
        private string _txtFilepath;

        public static readonly string NO_TMX_FILES = "NO TMX FILES";


        internal TmxToTxtOption()
        {
            _converter   = new ConvertHandler();
            GetRequiredInfo();

            if (IsInputValid)
                Convert();
            else if (!_tmxFilepath.Equals(NO_TMX_FILES, StringComparison.InvariantCultureIgnoreCase))
                WriteLine(Environment.NewLine + "\tError: Invalid input!", ErrorColor);
        }


        private void Convert()
        {
            WriteLine(Environment.NewLine + "\tConverting...");
            var success = _converter.TmxToTxt(_tmxFilepath, _txtFilepath);

            if (success)
                WriteLine(Environment.NewLine + "\tResult saved at: " + _txtFilepath);
        }

        private void SetTxtFilepath(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                IsInputValid = false;
                return;
            }

            _txtFilepath = Path.IsPathRooted(filename) ? filename : Path.Combine(USER_LEVEL_FOLDER, filename);
            var ext = Path.GetExtension(_txtFilepath);

            if (string.IsNullOrWhiteSpace(ext))
                _txtFilepath += ".txt";
        }

        private void GetRequiredInfo()
        {
            IsInputValid = true;

            _tmxFilepath = GetFilePath();

            if (IsInputValid)
                SetTxtFilepath(ReadString("Save with filename:"));

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Name = GetUsername();

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Token = GetToken();
        }

        private string GetFilePath()
        {
            var files = new DirectoryInfo(USER_LEVEL_FOLDER).GetFiles("*.tmx");

            if (files != null && files.Length != 0)
            {
                ShowFiles(files);
                return HandleFileOption(files);
            }

            WriteLine("\tNo TMX files exist in " + USER_LEVEL_FOLDER);
            IsInputValid = false;
            return NO_TMX_FILES;
        }

        private void ShowFiles(FileInfo[] files)
        {
            for (int i = 1; i <= files.Length; i++)
            {
                WriteLine("\t" + i + "  -  " + files[i - 1].Name);
            }
        }

        private string HandleFileOption(FileInfo[] files)
        {
            Write(Environment.NewLine + "Pick file:  ", UserInputColor);
            string input = ReadInput();

            if (int.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out int value) && value > 0 && value <= files.Length)
                return files[value - 1].FullName;

            IsInputValid = false;
            return string.Empty;
        }

    }
}
