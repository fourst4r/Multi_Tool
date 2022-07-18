using System;
using static LevelModel.Models.Components.Block;

namespace UserInterface.Menu.Options.Settings.Options
{
    internal class ViewBlocksOption : IOHandler
    {


        public ViewBlocksOption()
        {
            ViewBlocks();
        }

        private void ViewBlocks()
        {
            WriteLine("Block IDs:" + Environment.NewLine);

            WriteLine("\t" + BASIC_BROWN    + "  -  " + GetBlockName(BASIC_BROWN));
            WriteLine("\t" + BASIC_WHITE    + "  -  " + GetBlockName(BASIC_WHITE));
            WriteLine("\t" + BASIC_RED      + "  -  " + GetBlockName(BASIC_RED));
            WriteLine("\t" + BASIC_WAFFLE   + "  -  " + GetBlockName(BASIC_WAFFLE));
            WriteLine("\t" + BRICK          + "  -  " + GetBlockName(BRICK));
            WriteLine("\t" + ARROW_DOWN     + "  -  " + GetBlockName(ARROW_DOWN));
            WriteLine("\t" + ARROW_UP       + "  -  " + GetBlockName(ARROW_UP));
            WriteLine("\t" + ARROW_LEFT     + "  -  " + GetBlockName(ARROW_LEFT));
            WriteLine("\t" + ARROW_RIGHT    + "  -  " + GetBlockName(ARROW_RIGHT));
            WriteLine("\t" + MINE           + "  -  " + GetBlockName(MINE));
            WriteLine("\t" + ITEM_BLUE      + "  -  " + GetBlockName(ITEM_BLUE));
            WriteLine("\t" + START_BLOCK_P1 + "  -  " + GetBlockName(START_BLOCK_P1));
            WriteLine("\t" + START_BLOCK_P2 + "  -  " + GetBlockName(START_BLOCK_P2));
            WriteLine("\t" + START_BLOCK_P3 + "  -  " + GetBlockName(START_BLOCK_P3));
            WriteLine("\t" + START_BLOCK_P4 + "  -  " + GetBlockName(START_BLOCK_P4));
            WriteLine("\t" + ICE            + "  -  " + GetBlockName(ICE));
            WriteLine("\t" + GOAL           + "  -  " + GetBlockName(GOAL));
            WriteLine("\t" + CRUMBLE        + "  -  " + GetBlockName(CRUMBLE));
            WriteLine("\t" + VANISH         + "  -  " + GetBlockName(VANISH));
            WriteLine("\t" + MOVE_BLOCK     + "  -  " + GetBlockName(MOVE_BLOCK));
            WriteLine("\t" + WATER          + "  -  " + GetBlockName(WATER));
            WriteLine("\t" + ROTATE_RIGHT   + "  -  " + GetBlockName(ROTATE_RIGHT));
            WriteLine("\t" + ROTATE_LEFT    + "  -  " + GetBlockName(ROTATE_LEFT));
            WriteLine("\t" + PUSH_BLOCK     + "  -  " + GetBlockName(PUSH_BLOCK));
            WriteLine("\t" + NET            + "  -  " + GetBlockName(NET));
            WriteLine("\t" + ITEM_RED       + "  -  " + GetBlockName(ITEM_RED));
            WriteLine("\t" + HAPPY_BLOCK    + "  -  " + GetBlockName(HAPPY_BLOCK));
            WriteLine("\t" + SAD_BLOCK      + "  -  " + GetBlockName(SAD_BLOCK));
            WriteLine("\t" + HEART          + "  -  " + GetBlockName(HEART));
            WriteLine("\t" + CLOCK          + "  -  " + GetBlockName(CLOCK));
            WriteLine("\t" + EGG            + "  -  " + GetBlockName(EGG));
            WriteLine("\t" + CUSTOM_STATS   + "  -  " + GetBlockName(CUSTOM_STATS));
            WriteLine("\t" + TELEPORT       + "  -  " + GetBlockName(TELEPORT));


        }

    }
}
