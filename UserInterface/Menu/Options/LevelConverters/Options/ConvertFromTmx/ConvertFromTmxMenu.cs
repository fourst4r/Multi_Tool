using System;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using UserInterface.DataStructures.Constants;
using UserInterface.Menu.Options.LevelConverters.Options.ConvertFromTmx.Options;

namespace UserInterface.Menu.Options.LevelConverters.Options.ConvertFromTmx
{
    class ConvertFromTmxMenu : BaseMenuOption
    {

        public ConvertFromTmxMenu()
        {
            ShowOptions();
            Write(Environment.NewLine + "Pick option:  ", UserInputColor);
            HandleOption(ReadInput());
        }


        private void ShowOptions()
        {
            WriteLine("Convert To:");
            WriteLine("\t" + MenuOptions.TMX_TO_PR2 + "  -  Pr2 Level");
            WriteLine("\t" + MenuOptions.TMX_TO_TXT + "  -  Text File");

            WriteLine();
            WriteLine("\t" + MenuOptions.QUIT + "  -  Quit/Back");
        }

        private void HandleOption(string option)
        {
            switch (option.ToLower(CultureInfo.InvariantCulture))
            {

                case MenuOptions.TMX_TO_PR2:
                    new TmxToPr2Option();
                    break;

                case MenuOptions.TMX_TO_TXT:
                    new TmxToTxtOption();
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
