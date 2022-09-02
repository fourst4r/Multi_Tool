using LevelModel.Models.Components;
using System.Collections.Generic;

using static LevelModel.Models.Components.Block;

namespace Builders.Builders.LevelBuilders.Types
{
    internal class StartBlocksBuilder
    {


        private int _ySize;

        internal enum StartType { JustStartBlocks, Simple, HatRemover, Image }

        internal const int DEFAULT_HEIGHT = 13;

        public List<Block> Blocks { get; set; }


        internal StartBlocksBuilder(StartType type, int ySize = DEFAULT_HEIGHT)
        {
            _ySize = ySize;
            Blocks = new List<Block>();

            Build(type);
        }


        private void Build(StartType type) {
            if (type == StartType.HatRemover)
                AddHatRemover();
            else if (type == StartType.Simple)
                AddSimple();
            else if (type == StartType.Image)
                AddMinimapView();
            else
                AddStartBlocks(1);
        }

        private void AddSimple() {
            AddStartBlocks(1);

            Blocks.Add(-11, 5, BASIC_WHITE);
            for (int i = 0; i < 19; i++)
                Blocks.Add(1, 0, BASIC_WHITE);
        }

        private void AddMinimapView() {
            AddStartBlocks(1);

            Blocks.Add(-3, 2, BASIC_WHITE);
            Blocks.Add(1, 0, BASIC_WHITE);
            Blocks.Add(1, 0, BASIC_WHITE);
            Blocks.Add(1, 0, BASIC_WHITE);
        }

        private void AddStartBlocks(int space)
        {
            Blocks.Add(START_POSITION.X, START_POSITION.Y, START_BLOCK_P1);
            Blocks.Add(space, 0, START_BLOCK_P2);
            Blocks.Add(space, 0, START_BLOCK_P3);
            Blocks.Add(space, 0, START_BLOCK_P4);
        }

        private void AddHatRemover() {
            int width = 12;

            AddStartBlocks(2);
            AddMines();
            AddRectangleAroundMines(width);
            AddFloor(width);
        }

        private void AddMines()
        {
            Blocks.Add(-6, -1, MINE);
            Blocks.Add(2,   0, MINE);
            Blocks.Add(2,   0, MINE);
            Blocks.Add(2,   0, MINE);
        }

        private void AddRectangleAroundMines(int width)
        {
            Blocks.Add(((width - 6) / 2), -1, BASIC_WHITE);

            // Up side
            for (int i = 0; i < width; i++)
                Blocks.Add(-1, 0, BASIC_WHITE);

            // Left side
            Blocks.Add(0, 1, BASIC_WHITE);
            Blocks.Add(0, 1, BASIC_WHITE);
            Blocks.Add(0, 1, BASIC_WHITE);

            // Down side
            Blocks.Add(1, -1, BASIC_WHITE);
            for (int i = 0; i < width - 1; i++)
                Blocks.Add(1, 0, BASIC_WHITE);

            // Right side
            Blocks.Add(0, 1, BASIC_WHITE);
            Blocks.Add(0, -2, BASIC_WHITE);
        }

        private void AddFloor(int width)
        {
            Blocks.Add(-width, (_ySize - 1), BASIC_WHITE);

            for (int i = 0; i < width; i++)
                Blocks.Add(1, 0, BASIC_WHITE);
        }


    }
}
