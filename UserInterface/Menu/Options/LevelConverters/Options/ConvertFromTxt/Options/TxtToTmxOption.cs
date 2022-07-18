using System;
using System.IO;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using UserInterface.Handlers;
using UserInterface.DataStructures.Info;
using LevelModel.Models;

using static UserInterface.DataStructures.Constants.MyPaths;
using UserInterface.Handlers.FileHandlers;

namespace UserInterface.Menu.Options.LevelConverters.Options.ConvertFromTxt.Options
{
    class TxtToTmxOption : BaseMenuOption
    {

        private ConvertHandler _converter;
        private ConvertInfo _info;
        private (string msg, ConsoleColor color) _message;


        public TxtToTmxOption()
        {
            _converter    = new ConvertHandler();
            _info         = new ConvertInfo();
            _message      = default;
            IsInputValid  = true;
            var levelData = GetData();

            ShowMessage();

            if(IsInputValid)
                ConvertToTmx(levelData);
        }


        private void ConvertToTmx(string levelData)
        {
            WriteLine(Environment.NewLine + "\tConverting...");
            var success = _converter.TxtToTmx(_info, levelData);

            if (success) 
                WriteLine(Environment.NewLine + "\tResult saved at: " + _info.FilePath);
        }

        private void ShowMessage()
        {
            if (string.IsNullOrWhiteSpace(_message.msg) == false)
                WriteLine("\tError: " + _message.msg, _message.color);
        }

        private string GetFilePath()
        {
            var files = new DirectoryInfo(USER_LEVEL_FOLDER).GetFiles("*.txt");

            if (files != null && files.Length != 0)
            {
                ShowFiles(files);
                return HandleFileOption(files);
            }

            IsInputValid = false;
            _message = ("No Text files exist in " + USER_LEVEL_FOLDER, DefaultColor);
            return null;
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
            _message = ("Invalid input.", ErrorColor);
            return null;
        }

        private string GetData()
        {
            var filepath = GetFilePath();

            if (!IsInputValid || string.IsNullOrWhiteSpace(filepath))
                return string.Empty;


            _info.FilePath = Path.GetFileNameWithoutExtension(filepath) + ".tmx";
            return TxtFileHandler.Read(filepath);
        }

    }
}
