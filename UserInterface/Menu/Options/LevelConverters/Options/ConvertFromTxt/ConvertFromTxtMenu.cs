using System;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using UserInterface.DataStructures.Constants;
using UserInterface.Menu.Options.LevelConverters.Options.ConvertFromTxt.Options;

namespace UserInterface.Menu.Options.LevelConverters.Options.ConvertFromTxt
{
    class ConvertFromTxtMenu : BaseMenuOption
    {

        public ConvertFromTxtMenu()
        {
            ShowOptions();
            Write(Environment.NewLine + "Pick option:  ", UserInputColor);
            HandleOption(ReadInput());
        }


        private void ShowOptions()
        {
            WriteLine("Convert To:");
            WriteLine("\t" + MenuOptions.TXT_TO_PR2 + "  -  Pr2 Level");
            WriteLine("\t" + MenuOptions.TXT_TO_TMX + "  -  Tiled");

            WriteLine();
            WriteLine("\t" + MenuOptions.QUIT + "  -  Quit/Back");
        }

        private void HandleOption(string option)
        {
            switch (option.ToLower(CultureInfo.InvariantCulture))
            {

                case MenuOptions.TXT_TO_PR2:
                    new TxtToPr2Option();
                    break;

                case MenuOptions.TXT_TO_TMX:
                    new TxtToTmxOption();
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
