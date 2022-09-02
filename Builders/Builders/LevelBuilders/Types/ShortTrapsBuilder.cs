using System;
using System.Collections.Generic;
using LevelModel.Models.Components;

using static Builders.Builders.LevelBuilders.Types.StartBlocksBuilder;
using static Builders.DataStructures.TrapworkInfo;

namespace Builders.Builders.LevelBuilders.Types
{
    internal class ShortTrapsBuilder
    {


        private int _width;
        private int _height;
        private int _difficulty;

        private static readonly Random _rnd = new Random();

        public List<Block> Blocks { get; set; }


        internal ShortTrapsBuilder(int width, int height, int difficulty)
        {
            _width = width;
            _height = height;
            _difficulty = difficulty;

            Build();
        }


        private void Build()
        {
            AddStart();
            AddTraps();
            AddFinish();
        }

        private void AddTraps()
        {
            for (int i = 1; i < _width; i++)
            {
                AddRoof();

                for (int j = 0; j < _height - 1; j++)
                    Blocks.Add(0, 1, GetBlockToAdd());

                AddFloor();
            }
        }

        private int GetBlockToAdd()
        {
            int random = _rnd.Next(0, MAX_DIFFICULTY + 1);

            if (random < _difficulty)
                return Block.ARROW_RIGHT;

            return Block.WATER;
        }


        private void AddStart()
        {
               Blocks = new StartBlocksBuilder(StartType.HatRemover, _height).Blocks;
        }

        private void AddFloor() => Blocks.Add(0,1, Block.ARROW_RIGHT);

        private void AddRoof() => Blocks.Add(1, -_height, Block.ARROW_RIGHT);

        private void AddFinish()
        {
            // Left Wall
            for (int i = 0; i < _height; i++)
                Blocks.Add(0, -1, Block.ARROW_RIGHT);

            // Roof
            Blocks.Add(1, 0, Block.BASIC_WHITE);
            Blocks.Add(1, 0, Block.BASIC_WHITE);

            // Right wall
            for (int i = 0; i < _height; i++)
                Blocks.Add(0, 1, Block.BASIC_WHITE);

            // Floor
            Blocks.Add(-1, 0, Block.ARROW_UP);

            // Goal
            Blocks.Add(0, -_height + 2,  Block.GOAL);
            Blocks.Add(0, 1, Block.BRICK);

        }


    }
}
