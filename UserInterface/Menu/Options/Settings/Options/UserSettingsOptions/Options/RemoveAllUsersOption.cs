using UserInterface.Handlers.FileHandlers;

namespace UserInterface.Menu.Options.Settings.Options.UserSettingsOptions.Options
{
    internal class RemoveAllUsersOption
    {

        public RemoveAllUsersOption() {
            UserSettingsHandler.RemoveCurrentUser();
            UserSettingsHandler.RemoveAllUsers();
        }

    }
}
