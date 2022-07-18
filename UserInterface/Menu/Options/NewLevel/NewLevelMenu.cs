using System;
using System.Globalization;
using UserInterface.DataStructures.Constants;
using UserInterface.Menu.Options.NewLevel.Options;
using UserInterface.Menu.Options.NewLevel.Options.BuildLevel;
using UserInterface.Menu.Options.LevelConverters.Options.ConvertFromTxt.Options;

namespace UserInterface.Menu.Options.NewLevel
{
    class NewLevelMenu : IOHandler
    {

        public NewLevelMenu()
        {
            ShowOptions();
            Write(Environment.NewLine + "Pick option:  ", UserInputColor);
            HandleOption(ReadInput());
        }


        private void ShowOptions()
        {
            WriteLine("\t" + MenuOptions.BUILD_LEVEL      + "  -  Build Level");
            WriteLine("\t" + MenuOptions.IMAGE_GENERATION + "  -  Image Generation");
            WriteLine("\t" + MenuOptions.TXT_LEVEL        + "  -  Upload Text File");

            WriteLine();
            WriteLine("\t" + MenuOptions.QUIT + "  -  Quit/Back");
        }

        private void HandleOption(string option)
        {
            switch (option.ToLower(CultureInfo.InvariantCulture))
            {
                case MenuOptions.BUILD_LEVEL:
                    new BuildLevelMenu();
                    break;
                case MenuOptions.IMAGE_GENERATION:
                    new ImageGenerationOption();
                    break;
                case MenuOptions.TXT_LEVEL:
                    new TxtToPr2Option();
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
