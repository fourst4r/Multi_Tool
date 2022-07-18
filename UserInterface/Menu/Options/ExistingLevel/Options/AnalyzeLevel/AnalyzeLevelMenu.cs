using System;
using System.Globalization;
using UserInterface.DataStructures.Constants;
using UserInterface.Menu.Options.ExistingLevel.Options;
using UserInterface.Menu.Options.ExistingLevel.Options.AnalyzeLevel.Options;
using UserInterface.Menu.Options.ExistingLevel.Options.ModifyLevel;
using UserInterface.Menu.Options.ExistingLevel.Options.SearchLevels;

namespace UserInterface.Menu.Options.ExistingLevel.Options.AnalyzeLevel
{
    class AnalyzeLevelMenu : IOHandler
    {


        public AnalyzeLevelMenu()
        {
            ShowOptions();
            Write(Environment.NewLine + "Pick option:  ", UserInputColor);
            HandleOption(ReadInput());
        }

        private void ShowOptions()
        {
            WriteLine("\t" + MenuOptions.GENERAL_INFO   + "  -  General Info");
            WriteLine("\t" + MenuOptions.BLOCK_COUNT    + "  -  Block Count");
            WriteLine("\t" + MenuOptions.ART_SIZE       + "  -  Art Size");

            WriteLine();
            WriteLine("\t" + MenuOptions.QUIT + "  -  Quit/Back");
        }

        private void HandleOption(string option)
        {
            switch (option.ToLower(CultureInfo.InvariantCulture))
            {
                case MenuOptions.GENERAL_INFO:
                    new GeneralInfoOption();
                    break;
                case MenuOptions.BLOCK_COUNT:
                    new BlockCountOption();
                    break;
                case MenuOptions.ART_SIZE:
                    new ArtSizeOption();
                    break;

                case MenuOptions.QUIT:
                    break;
                default:
                    WriteLine("\tError: Invalid input.", ErrorColor);
                    break;
            }
        }


    }
}
