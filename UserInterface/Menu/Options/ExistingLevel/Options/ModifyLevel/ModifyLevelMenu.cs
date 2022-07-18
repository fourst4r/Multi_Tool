using System;
using System.Globalization;
using UserInterface.DataStructures.Constants;
using UserInterface.Menu.Options.ExistingLevel.Options.ModifyLevel.Options;

namespace UserInterface.Menu.Options.ExistingLevel.Options.ModifyLevel
{
    class ModifyLevelMenu : IOHandler
    {

        public ModifyLevelMenu()
        {
            ShowOptions();
            Write(Environment.NewLine + "Pick option:  ", UserInputColor);
            HandleOption(ReadInput());
        }


        private void ShowOptions()
        {
            WriteLine("\t" + MenuOptions.ADD_ART_BLOCKS.PadRight(2)  + "  -  Add Art Blocks");
            WriteLine("\t" + MenuOptions.ADD_BLOCKS.PadRight(2)      + "  -  Add Blocks");
            WriteLine("\t" + MenuOptions.ADD_IMAGE.PadRight(2)       + "  -  Add Image");
            WriteLine("\t" + MenuOptions.CHANGE_ART_TYPE.PadRight(2) + "  -  Change Art Type");
            WriteLine("\t" + MenuOptions.CHANGE_BLOCKS.PadRight(2)   + "  -  Change Blocks");
            WriteLine("\t" + MenuOptions.MOVE_ALL.PadRight(2)        + "  -  Move All");
            WriteLine("\t" + MenuOptions.MOVE_ART.PadRight(2)        + "  -  Move Art");
            WriteLine("\t" + MenuOptions.MOVE_BLOCKS.PadRight(2)     + "  -  Move Blocks");
            WriteLine("\t" + MenuOptions.REMOVE_ART.PadRight(2)      + "  -  Remove Art");
            WriteLine("\t" + MenuOptions.REMOVE_BLOCKS.PadRight(2)   + "  -  Remove Blocks");
            WriteLine("\t" + MenuOptions.REVERSE_TRAPS.PadRight(2)   + "  -  Reverse Traps");
            WriteLine();
            WriteLine("\t" + MenuOptions.QUIT + "  -  Quit/Back");
        }

        private void HandleOption(string option)
        {
            switch (option.ToLower(CultureInfo.InvariantCulture))
            {
                case MenuOptions.ADD_IMAGE:
                    new AddImageOption();
                    break;
                case MenuOptions.REMOVE_ART:
                    new RemoveArtOption();
                    break;
                case MenuOptions.REMOVE_BLOCKS:
                    new RemoveBlocksOption();
                    break;
                case MenuOptions.MOVE_ALL:
                    new MoveBlocksOption(true);
                    break;
                case MenuOptions.MOVE_ART:
                    new MoveArtOption();
                    break;
                case MenuOptions.MOVE_BLOCKS:
                    new MoveBlocksOption(false);
                    break;
                case MenuOptions.ADD_BLOCKS:
                    new AddBlocksOption();
                    break;
                case MenuOptions.CHANGE_ART_TYPE:
                    new ChangeArtTypeOption();
                    break;
                case MenuOptions.CHANGE_BLOCKS:
                    new ChangeBlocksOption();
                    break;
                case MenuOptions.REVERSE_TRAPS:
                    new ReverseTrapsOption();
                    break;
                case MenuOptions.ADD_ART_BLOCKS:
                    new AddArtBlocksOption();
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
