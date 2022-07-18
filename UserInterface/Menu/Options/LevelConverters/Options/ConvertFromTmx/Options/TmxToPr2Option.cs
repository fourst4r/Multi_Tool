using System;
using System.IO;
using System.Globalization;
using UserInterface.DataStructures.Info;
using UserInterface.Handlers;
using UserInterface.Handlers.FileHandlers;

using static UserInterface.DataStructures.Constants.MyPaths;

namespace UserInterface.Menu.Options.LevelConverters.Options.ConvertFromTmx.Options
{
    internal class TmxToPr2Option : BaseMenuOption
    {

        private ConvertHandler _converter;
        private ConvertInfo _info;

        public static readonly string NO_TMX_FILES = "NO TMX FILES";


        internal TmxToPr2Option()
        {
            _converter = new ConvertHandler();
            _info      = new ConvertInfo();

            GetRequiredInfo();

            if (IsInputValid)
                Convert();
            else if(!_info.FilePath.Equals(NO_TMX_FILES, StringComparison.InvariantCultureIgnoreCase))
                WriteLine(Environment.NewLine + "\tError: Invalid input!", ErrorColor);
        }


        private void Convert()
        {
            WriteLine(Environment.NewLine + "\tConverting...");
            _converter.TmxToPr2(_info);
        }

        private void GetRequiredInfo()
        {
            IsInputValid = true;

            _info.FilePath = GetFilePath();

            if (IsInputValid)
                _info.Title = ReadString("Title:  ");

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
                WriteLine("\t" + i + "  -  " + files[i- 1].Name);
            }
        }

        private string HandleFileOption(FileInfo[] files)
        {
            Write(Environment.NewLine + "Pick file:  ", UserInputColor);
            string input = ReadInput();

            if (int.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out int value) && value > 0 && value <= files.Length)
                return files[value - 1].FullName;

            IsInputValid = false;
            return String.Empty;
        }

    }
}
