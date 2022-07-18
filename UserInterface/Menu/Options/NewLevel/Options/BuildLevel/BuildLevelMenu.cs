using System;
using System.Globalization;
using UserInterface.DataStructures.Constants;
using UserInterface.Menu.Options.NewLevel.Options.BuildLevel.Options;

using static Builders.DataStructures.DTO.BuildDTO;

namespace UserInterface.Menu.Options.NewLevel.Options.BuildLevel
{
    class BuildLevelMenu : IOHandler
    {

        public BuildLevelMenu()
        {
            ShowOptions();
            Write(Environment.NewLine + "Pick option:  ", UserInputColor);
            HandleOption(ReadInput());
        }


        private void ShowOptions()
        {
            WriteLine("\t" + MenuOptions.SIMPLE          + "  -  Simple");
            WriteLine("\t" + MenuOptions.SMALL_LABYRINTH + "  -  Small Labyrinth");
            WriteLine("\t" + MenuOptions.LARGE_LABYRINTH + "  -  Large Labyrinth");
            WriteLine("\t" + MenuOptions.SHORT_TRAPS     + "  -  Short Traps");
            WriteLine("\t" + MenuOptions.TRAPWORK        + "  -  Trapwork");
            WriteLine("\t" + MenuOptions.MAX_BLOCK_LIMIT + "  -  Max Block Limit");
            WriteLine();
            WriteLine("\t" + MenuOptions.QUIT + "  -  Quit/Back");
        }

        private void HandleOption(string option)
        {
            switch (option.ToLower(CultureInfo.InvariantCulture))
            {
                case MenuOptions.SIMPLE:
                    new BuildLevelOption(BuildType.Simple);
                    break;
                case MenuOptions.SMALL_LABYRINTH:
                    new BuildLevelOption(BuildType.SmallLabyrinth);
                    break;
                case MenuOptions.LARGE_LABYRINTH:
                    new BuildLevelOption(BuildType.LargeLabyrinth);
                    break;
                case MenuOptions.SHORT_TRAPS:
                    new BuildLevelOption(BuildType.ShortTraps);
                    break;
                case MenuOptions.TRAPWORK:
                    new BuildLevelOption(BuildType.Trapwork);
                    break;
                case MenuOptions.MAX_BLOCK_LIMIT:
                    new BuildLevelOption(BuildType.MaxBlockLimit);
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
