using System;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using UserInterface.DataStructures.Constants;
using UserInterface.Handlers.FileHandlers;

namespace UserInterface.Menu.Options.Settings.Options.UserSettingsOptions.Options
{
    internal class SwitchUserOption : BaseMenuOption
    {

        public SwitchUserOption() {
            ShowUsers();
            HandleOption(ReadInput());
        }


        private void ShowUsers() {
            WriteLine("Available Users:" + Environment.NewLine);

            for (int i = 0; i < UserSettingsHandler.Users.Count; i++)
                WriteLine("\t" + (i + 1) + "  -  " + UserSettingsHandler.Users[i].Name);

            WriteLine();
            WriteLine("\t" + MenuOptions.QUIT + "  -  Quit/Back");
            Write(Environment.NewLine + "Pick option:  ", UserInputColor);
        }

        private void HandleOption(string option) {
            if (int.TryParse(option, NumberStyles.Any, CultureInfo.InvariantCulture, out int value) && value > 0 && value < (UserSettingsHandler.Users.Count + 1))
                UserSettingsHandler.SetCurrentUser(UserSettingsHandler.Users[value - 1].Name, UserSettingsHandler.Users[value - 1].Token);
            else if (option.Equals(MenuOptions.QUIT, StringComparison.InvariantCultureIgnoreCase))
                return;
            else
                WriteLine("\tError: Invalid input.", ErrorColor);
        }

    }
}
