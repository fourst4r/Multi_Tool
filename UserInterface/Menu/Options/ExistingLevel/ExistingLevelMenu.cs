using System;
using System.Globalization;
using UserInterface.DataStructures.Constants;
using UserInterface.Menu.Options.ExistingLevel.Options;
using UserInterface.Menu.Options.ExistingLevel.Options.AnalyzeLevel;
using UserInterface.Menu.Options.ExistingLevel.Options.ModifyLevel;
using UserInterface.Menu.Options.ExistingLevel.Options.SearchLevels;

namespace UserInterface.Menu.Options.ExistingLevel
{
    class ExistingLevelMenu : IOHandler
    {

        public ExistingLevelMenu()
        {
            ShowOptions();
            Write(Environment.NewLine + "Pick option:  ", UserInputColor);
            HandleOption(ReadInput());
        }


        private void ShowOptions()
        {
            WriteLine("\t" + MenuOptions.ANALYZE_LEVEL  + "  -  Analyze Level");
            WriteLine("\t" + MenuOptions.COPY_LEVEL     + "  -  Copy Level");
            WriteLine("\t" + MenuOptions.MERGE_LEVELS   + "  -  Merge Levels");
            WriteLine("\t" + MenuOptions.MODIFY_LEVEL   + "  -  Modify Level");
            WriteLine("\t" + MenuOptions.SEARCH_LEVEL   + "  -  Search Levels");

            WriteLine();
            WriteLine("\t" + MenuOptions.QUIT + "  -  Quit/Back");
        }

        private void HandleOption(string option)
        {
            switch (option.ToLower(CultureInfo.InvariantCulture))
            {
                case MenuOptions.COPY_LEVEL:
                    new CopyLevelOption();
                    break;
                case MenuOptions.MODIFY_LEVEL:
                    new ModifyLevelMenu();
                    break;
                case MenuOptions.MERGE_LEVELS:
                    new MergeLevelsOption();
                    break;
                case MenuOptions.SEARCH_LEVEL:
                    new SearchLevelsOption();
                    break;
                case MenuOptions.ANALYZE_LEVEL:
                    new AnalyzeLevelMenu();
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
