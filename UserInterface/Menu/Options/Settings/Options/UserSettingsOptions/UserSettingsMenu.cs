using System;
using System.Globalization;
using UserInterface.DataStructures.Constants;
using UserInterface.Handlers.FileHandlers;
using UserInterface.Menu.Options.Settings.Options.UserSettingsOptions.Options;
using UserInterface.Menu.Options.Settings.Options.UserSettingsOptions.Options.AddUserOptions;

namespace UserInterface.Menu.Options.Settings.Options.UserSettingsOptions
{
    internal class UserSettingsMenu : BaseMenuOption
    {

        private bool _isLoggedIn;
        private bool _usersExists;


        internal UserSettingsMenu()
        {
            _isLoggedIn  = UserSettingsHandler.CurrentUser.Name != null && UserSettingsHandler.CurrentUser.Name.Length != 0;
            _usersExists = UserSettingsHandler.Users.Count > 0;

            ShowUserData();
            ShowOptions();
            HandleOption(ReadInput());
        }


        private void ShowUserData()
        {
            if (_isLoggedIn) {
                WriteLine("You are currently logged in as:");
                WriteLine("\tUser  = " + UserSettingsHandler.CurrentUser.Name);
                WriteLine("\tToken = " + UserSettingsHandler.CurrentUser.Token + Environment.NewLine);
            }
            else {
                WriteLine("\t You are currently not logged in." + Environment.NewLine);
            }
        }

        private void ShowOptions()
        {
            if (_isLoggedIn)
                ShowLoggedInOptions();
            else
                ShowNewUserOptions();

            WriteLine();
            WriteLine("\t" + MenuOptions.QUIT + "  -  Quit/Back");
            Write(Environment.NewLine + "Pick option:  ", UserInputColor);
        }

        private void ShowLoggedInOptions() {
            WriteLine("\t" + MenuOptions.ADD_USER          + "  -  Add User");
            WriteLine("\t" + MenuOptions.SWITCH_USER       + "  -  Switch User");
            WriteLine("\t" + MenuOptions.REMOVE_ALL_USERS  + "  -  Remove All Users");
            WriteLine("\t" + MenuOptions.REMOVE_USER       + "  -  Remove Current User");
            WriteLine("\t" + MenuOptions.UPDATE_TOKEN      + "  -  Update Current Token");
        }

        private void ShowNewUserOptions() {
            WriteLine("\t" + MenuOptions.ADD_USER  + "  -  Add User");

            if(_usersExists) {
                WriteLine("\t" + MenuOptions.SWITCH_USER       + "  -  Switch User");
                WriteLine("\t" + MenuOptions.REMOVE_ALL_USERS  + "  -  Remove Users");
            }
        }

        private void HandleOption(string option)
        {
            if(option.Equals(MenuOptions.ADD_USER, StringComparison.InvariantCultureIgnoreCase))
                new AddUserMenu();
            else if (option.Equals(MenuOptions.REMOVE_USER, StringComparison.InvariantCultureIgnoreCase) && _isLoggedIn)
                new RemoveUserOption();
            else if (option.Equals(MenuOptions.UPDATE_TOKEN, StringComparison.InvariantCultureIgnoreCase) && _isLoggedIn)
                    new UpdateTokenOption();
            else if (option.Equals(MenuOptions.REMOVE_ALL_USERS, StringComparison.InvariantCultureIgnoreCase) && _usersExists)
                new RemoveAllUsersOption();
            else if (option.Equals(MenuOptions.SWITCH_USER, StringComparison.InvariantCultureIgnoreCase) && _usersExists)
                new SwitchUserOption();
            else if (option.Equals(MenuOptions.QUIT, StringComparison.InvariantCultureIgnoreCase))
                 return;
            else
                WriteLine("\tError: Invalid input.", ErrorColor);
        }


    }
}
