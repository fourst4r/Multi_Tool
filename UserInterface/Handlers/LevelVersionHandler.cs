using System;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using LevelModel.Models;
using UserInterface.DataStructures.Constants;

namespace UserInterface.Handlers
{
    class LevelVersionHandler : IOHandler
    {

        private readonly Level Level;

        public bool Cancel { get; set; }

        public LevelVersionHandler(Level level, bool askToUpdate)
        {
            Level = level;

            if(!IsUpdateNeeded())
                return;

            if(askToUpdate)
            {
                Update();
            }
            else
            {
                ShowOptions();
                HandleOption(ReadInput());
            }
        }

        private bool IsUpdateNeeded()
        {
            if(Level == null)
                return false;

            return !string.Equals(LevelModel.Constants.DATA_VERSION, Level.DataVersion, StringComparison.InvariantCulture);
        }


        private void ShowOptions()
        {
            WriteLine(Environment.NewLine + $"\tWarning: The level is missing the block version number '{LevelModel.Constants.DATA_VERSION}'", WarningColor);
            WriteLine(Environment.NewLine + "\tDo you wish to add it?");
            WriteLine();
            WriteLine("\t" + MenuOptions.YES   + "  -  Yes  (Recommended)");
            WriteLine("\t" + MenuOptions.NO    + "  -  No ");
            WriteLine();
            WriteLine("\t" + MenuOptions.QUIT + "  -  Quit/Back");

            Write(Environment.NewLine + "Pick option:  ", UserInputColor);
        }

        private void Update()
        {
            Level.DataVersion = LevelModel.Constants.DATA_VERSION;
        }

        private void HandleOption(string option)
        {
            switch (option.ToLower(CultureInfo.InvariantCulture))
            {

                case MenuOptions.YES:
                    Update();
                    break;

                case MenuOptions.NO:
                    break;

                case MenuOptions.QUIT:
                    Cancel = true;
                    break;

                default:
                    WriteLine("\tError: Invalid input.", ErrorColor);
                    Cancel = true;
                    break;
            }
        }

    }
}
