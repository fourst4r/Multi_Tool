using System;
using System.IO;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using Parsers.DTO;
using LevelModel.Models;
using UserInterface.Handlers;
using UserInterface.Handlers.FileHandlers;

using static UserInterface.DataStructures.Constants.MyPaths;
using LevelModel;

namespace UserInterface.Menu.Options.LevelConverters.Options.ConvertFromTxt.Options
{
    class TxtToPr2Option : BaseMenuOption
    {

        private (string msg, ConsoleColor color) _message;
        private ConvertHandler _converter;
        private string _levelData;


        public TxtToPr2Option()
        {
            _converter    = new ConvertHandler();
            _message      = default;
        
            GetRequiredInfo();
            ShowMessage();
            ToPr2();
        }


        private void ShowMessage()
        {
            if(string.IsNullOrWhiteSpace(_message.msg) == false)
                WriteLine("\tError: " + _message.msg, _message.color);
        }

        private void ToPr2()
        {
            _converter.TxtToPr2(_levelData);
        }

        private void GetRequiredInfo()
        {
            IsInputValid = true;

            _levelData = GetData();

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Name = GetUsername();

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Token = GetToken();
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

            if(!IsInputValid)
                return string.Empty;

            return TxtFileHandler.Read(filepath);
        }

    }
}
