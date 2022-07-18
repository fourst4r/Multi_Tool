using System;
using System.Collections.Generic;
using System.Text;
using UserInterface.DataStructures.Constants;

namespace UserInterface.Menu.Options.Settings.Options.UserSettingsOptions.Options.AddUserOptions
{

    internal class AddUserMenu : BaseMenuOption
    {


        internal AddUserMenu()
        {
            ShowOptions();
            HandleOption(ReadInput());
        }

        private void ShowOptions()
        {
            WriteLine("What authentication method do you wish to use:" + Environment.NewLine);
            WriteLine("\t" + MenuOptions.ADD_USER_FROM_TOKEN + "  -  Token");
            WriteLine("\t" + MenuOptions.ADD_USER_FROM_PASSWORD + "  -  Password");

            WriteLine();
            WriteLine("\t" + MenuOptions.QUIT + "  -  Quit/Back");
            Write(Environment.NewLine + "Pick option:  ", UserInputColor);
        }

        private void HandleOption(string option)
        {
            if (option.Equals(MenuOptions.ADD_USER_FROM_TOKEN, StringComparison.InvariantCultureIgnoreCase))
                new AddUserOption(false);
            else if (option.Equals(MenuOptions.ADD_USER_FROM_PASSWORD, StringComparison.InvariantCultureIgnoreCase))
                new AddUserOption(true);
            else if (option.Equals(MenuOptions.QUIT, StringComparison.InvariantCultureIgnoreCase))
                return;
            else
                WriteLine("\tError: Invalid input.", ErrorColor);
        }

    }
}
