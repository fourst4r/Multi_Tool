using System;
using System.Globalization;
using UserInterface.DataStructures.Constants;
using UserInterface.Menu.Options.Settings.Options;
using UserInterface.Menu.Options.Settings.Options.UserSettingsOptions;

namespace UserInterface.Menu.Options.Settings
{
    class SettingsMenu : IOHandler
    {

        public SettingsMenu() 
        {
            ShowOptions();
            Write(Environment.NewLine + "Pick option:  ", UserInputColor);
            HandleOption(ReadInput());
        }


        private void ShowOptions()
        {
            WriteLine("\t" + MenuOptions.USER_SETTINGS  + "  -  User Settings");
            WriteLine("\t" + MenuOptions.VIEW_BLOCKS    + "  -  Block IDs");
            WriteLine("\t" + MenuOptions.NEW_UPDATE     + "  -  Check for Updates");

            WriteLine("\t" + MenuOptions.INFO           + "  -  Info");
            WriteLine();
            WriteLine("\t" + MenuOptions.QUIT + "  -  Quit/Back");
        }

        private void HandleOption(string option)
        {
            switch (option.ToLower(CultureInfo.InvariantCulture))
            {
                case MenuOptions.USER_SETTINGS:
                    new UserSettingsMenu();
                    break;
                case MenuOptions.VIEW_BLOCKS:
                    new ViewBlocksOption();
                    break;
                case MenuOptions.NEW_UPDATE:
                    new NewUpdateOption();
                    break;
                case MenuOptions.INFO:
                    new InfoOption();
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
