using System;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using UserInterface.DataStructures.Constants;
using UserInterface.Menu.Options.LevelConverters.Options.ConvertFromPr2.Options;

namespace UserInterface.Menu.Options.LevelConverters.Options.ConvertFromPr2
{
    class ConvertFromPr2Menu : BaseMenuOption
    {

        public ConvertFromPr2Menu()
        {
            ShowOptions();
            Write(Environment.NewLine + "Pick option:  ", UserInputColor);
            HandleOption(ReadInput());
        }


        private void ShowOptions()
        {
            WriteLine("Convert To:");
            WriteLine("\t" + MenuOptions.PR2_TO_TMX + "  -  Tiled");
            WriteLine("\t" + MenuOptions.PR2_TO_TXT + "  -  Text File");

            WriteLine();
            WriteLine("\t" + MenuOptions.QUIT + "  -  Quit/Back");
        }

        private void HandleOption(string option)
        {
            switch (option.ToLower(CultureInfo.InvariantCulture))
            {

                case MenuOptions.PR2_TO_TMX:
                    new Pr2ToTmxOption();
                    break;

                case MenuOptions.PR2_TO_TXT:
                    new Pr2ToTxtOption();
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
