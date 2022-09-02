using System;
using System.Collections.Generic;
using LevelModel.Models.Components;

using static Builders.Builders.LevelBuilders.Types.StartBlocksBuilder;

namespace Builders.Builders.LevelBuilders.Types
{
    internal class LabyrinthBuilder
    {


        private int _xSize;
        private int _ySize;
        private int _verticalPosition;

        private static readonly Random _rnd = new Random();

        public List<Block> Blocks { get; set; }


        internal LabyrinthBuilder(int xSize, int ySize)
        {
            _xSize = xSize;
            _ySize = ySize;
            _verticalPosition = 0;

            Build();
        }


        private void Build()
        {
            AddStart();
            AddLabyrinth();
            AddFinish();
        }

        private void AddStart()
        {
            Blocks = new StartBlocksBuilder(StartType.HatRemover).Blocks;
            Blocks.Add(3, _ySize / 3, Block.BASIC_WHITE);
        }

        private void AddLabyrinth()
        {
            for (int i = 0; i < _xSize; i++)
            {
                Blocks.Add(1, -(_ySize - _verticalPosition), Block.BASIC_WHITE);
                _verticalPosition = 0;
                for (int j = 0; j < _ySize; j++)
                {
                    _verticalPosition++;
                    if (_rnd.Next(0, 5) == 0)
                    {
                        Blocks.Add(0, _verticalPosition, Block.BASIC_WHITE);
                        _verticalPosition = 0;
                    }
                }
            }
        }

        private void AddFinish()
        {
            //Close map from roof runners
            Blocks.Add(1, -(_ySize - _verticalPosition), Block.BASIC_WHITE);
            Blocks.Add(1, 0, Block.BASIC_WHITE);

            for (int j = 0; j < _ySize; j++)
                Blocks.Add(0, 1, Block.BASIC_WHITE);

            Blocks.Add(-1, -(_ySize / 2), Block.GOAL);
        }


    }
}
