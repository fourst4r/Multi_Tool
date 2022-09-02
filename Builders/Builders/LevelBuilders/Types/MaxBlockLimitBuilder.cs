using LevelModel.Models.Components;
using System.Collections.Generic;

using static Builders.Builders.LevelBuilders.Types.StartBlocksBuilder;

namespace Builders.Builders.LevelBuilders.Types
{
    internal class MaxBlockLimitBuilder
    {


        private int _height;
        private int _width;

        private const int OFFSET_X= -450;

        public List<Block> Blocks { get; set; }


        internal MaxBlockLimitBuilder()
        {
            CalculateSize();

            Blocks = new StartBlocksBuilder(StartType.JustStartBlocks).Blocks;
            AddBlocks();
        }


        private void CalculateSize() {
            _height = 25;
            _width  = (LevelModel.Models.Level.MAX_BLOCK_LIMIT / _height) - 2;
        }

        private void AddBlocks()
        {
            Blocks.Add(OFFSET_X, 2, Block.BASIC_WAFFLE);

            for (int i = 0; i < 25; i++)
            {
                for (int j = 0; j < _width ; j++) {
                    Blocks.Add(1, 0, Block.BASIC_WAFFLE);
                }
                Blocks.Add(-_width + 1, 1, Block.CLOCK);
            }
            AddMissingBlocks();
        }

        private void AddMissingBlocks()
        {
            for (int i = 0; i < _height - 6; i++)
                Blocks.Add(0, -1, Block.BASIC_WAFFLE);
            Blocks.Add(0, -1, Block.BASIC_WAFFLE);
        }


    }
}
