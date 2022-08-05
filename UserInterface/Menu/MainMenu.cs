using System;
using System.Globalization;
using UserInterface.Menu.Options.NewLevel;
using UserInterface.Menu.Options.Settings;
using UserInterface.Handlers.FileHandlers;
using UserInterface.DataStructures.Constants;
using UserInterface.Menu.Options.ExistingLevel;
using UserInterface.Menu.Options.LevelConverters;

namespace UserInterface.Menu
{
    internal class MainMenu : IOHandler
    {

        public void Start()
        {
            try
            {
                Init();
                MainMenuLoop();
            }
            catch (Exception ex)
            {
                ShowExceptionToUser(ex);
                WriteLine(Environment.NewLine + Environment.NewLine + "\t Press enter to close the application..");
                ReadInput();
            }
        }

        private void Init()
        {
            Console.Title = "PR2 Multi Tool";
            MyPaths.CreateOutputFolders();
            UserSettingsHandler.Init();
            ShowIntroText();
        }

        private void MainMenuLoop()
        {
            string option = String.Empty;
            while (!option.Equals(MenuOptions.QUIT, StringComparison.InvariantCultureIgnoreCase))
            {
                ShowOptions();
                option = ReadInput();
                HandleOption(option);
            }
        }

        private void ShowIntroText()
        {
            WriteLine("This is a multi-tool for Platform Racing 2!" + Environment.NewLine);
            WriteLine("User folder:  " + MyPaths.USER_FOLDER + Environment.NewLine);
            WriteLine("Tutorial videos:  " + Constants.LINK_TO_TUTORIALS + Environment.NewLine);
        }

        private void ShowOptions()
        {
            WriteLine(Environment.NewLine + "-------   Main Menu  -------" + Environment.NewLine);
            WriteLine("\t" + MenuOptions.NEW_LEVEL        + "  -  New Level");
            WriteLine("\t" + MenuOptions.EXISTING_LEVEL   + "  -  Existing Level");
            WriteLine("\t" + MenuOptions.LEVEL_CONVERTERS + "  -  Level Converters");
            WriteLine("\t" + MenuOptions.SETTINGS         + "  -  Settings");
            WriteLine();
            WriteLine("\t" + MenuOptions.QUIT + "  -  Quit");

            Write(Environment.NewLine + "Pick option:  ", UserInputColor);
        }

        private void HandleOption(string option)
        {
            switch (option.ToLower(CultureInfo.InvariantCulture))
            {
                case MenuOptions.NEW_LEVEL:
                    new NewLevelMenu();
                    break;
                case MenuOptions.EXISTING_LEVEL:
                    new ExistingLevelMenu();
                    break;
                case MenuOptions.LEVEL_CONVERTERS:
                    new LevelConvertersMenu();
                    break;
                case MenuOptions.SETTINGS:
                    new SettingsMenu();
                    break;
                case MenuOptions.QUIT:
                    break;
                default:
                    WriteLine("\tError: Invalid input. Redo!", ErrorColor);
                    break;
            }
        }

    }
}
