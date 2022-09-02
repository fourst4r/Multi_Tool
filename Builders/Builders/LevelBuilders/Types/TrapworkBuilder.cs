using System;
using System.Collections.Generic;
using Builders.DataStructures;
using LevelModel.Models.Components;

using static Builders.DataStructures.TrapworkInfo;
using static Builders.DataStructures.LevelSeperator;
using static Builders.Builders.LevelBuilders.Types.StartBlocksBuilder;

namespace Builders.Builders.LevelBuilders.Types
{
    internal class TrapworkBuilder
    {


        private TrapworkInfo _info;

        private static readonly Random _rnd = new Random();

        public List<Block> Blocks { get; set; }


        internal TrapworkBuilder(int width, int height)
        {
            _info = new TrapworkInfo(width, height);

            Build();
        }

        private TrapworkBuilder(TrapworkInfo trapworkInfo)
        {
            _info   = trapworkInfo;
            Blocks  = new List<Block>();

            if (_info.LevelSeperator.AddInStart)
                AddLevelSeperator(_info.LevelSeperator.StartDirection, _info.LevelSeperator.StartWidth);

            AddTraps();

            if (_info.LevelSeperator.AddInEnd)
                AddLevelSeperator(_info.LevelSeperator.EndDirection, _info.LevelSeperator.EndWidth);
        }


        private void Build()
        {
            AddStart();

            // Map consist of 4 trap parts, see minimap to understand
            AddPart1();
            AddPart2();
            AddPart3();
            AddPart4();

            AddFinish();
        }

        private void AddPart1()
        {
            int width = (int)Math.Floor(_info.Width * 0.5);
            int height = _info.Height;
            int numberOfLevels = 5;

            var info = new TrapworkInfo(width, height , numberOfLevels);
            info.Direction = STRAIGHT;
            info.Difficulty = 4;
            info.LevelSeperator = new LevelSeperator(info.Direction) { AddInEnd = true } ;

            Blocks = Blocks.Merge(new TrapworkBuilder(info).Blocks);
        }

        private void AddPart2()
        {
            int width = (int)Math.Floor(_info.Width * 0.1);
            int height = _info.Height;
            int numberOfLevels = 2;

            var info = new TrapworkInfo(width, height, numberOfLevels);
            info.Direction = UPHILL;
            info.Difficulty = 7;
            info.LevelSeperator = new LevelSeperator(UPHILL) { MiddleWidth = SMALL_WIDTH };

            Blocks = Blocks.Merge(new TrapworkBuilder(info).Blocks);
        }

        private void AddPart3() 
        {
            int width = (int)Math.Floor(_info.Width * 0.1);
            int height = _info.Height;
            int numberOfLevels = 2;

            var info = new TrapworkInfo(width, height, numberOfLevels);
            info.Direction = DOWNHILL;
            info.Difficulty = 8;
            info.LevelSeperator = new LevelSeperator(DOWNHILL) { AddInEnd = true, EndDirection = STRAIGHT, MiddleWidth = SMALL_WIDTH };

            Blocks = Blocks.Merge(new TrapworkBuilder(info).Blocks);

        }

        private void AddPart4()
        {
            int width = (int)Math.Floor(_info.Width * 0.2);
            int height = _info.Height;
            int numberOfLevels = 3;

            var info = new TrapworkInfo(width, height, numberOfLevels);

            info.Direction = STRAIGHT;
            info.Difficulty = 9;
            info.LevelSeperator = new LevelSeperator(STRAIGHT);

            Blocks = Blocks.Merge(new TrapworkBuilder(info).Blocks);
        }

        private void AddTraps()
        {
            for (int i = 1; i < _info.Width; i++)
            {
                AddRoof();
                AddTrapsVertical();
                AddFloor();
                CheckEndOfLevel(i);
            }
        }

        private void AddTrapsVertical()
        {
            for (int j = 0; j < _info.Height - 1; j++)
            {
                Blocks.Add(0, 1, GetBlockToAdd());
            }
        }

        private void CheckEndOfLevel(int i)
        {
            if (i % _info.LevelLength == 0)
            {
                _info.Difficulty++;
                if (_info.LevelSeperator.AddInMiddle)
                    AddLevelSeperator(_info.LevelSeperator.MiddleDirection, _info.LevelSeperator.MiddleWidth);
            }
        }

        private void AddLevelSeperator(int direction, int width)
        {
            // Roof
            Blocks.Add(1,  -_info.Height - direction, Block.BASIC_WHITE);
            for (int i = 0; i < width; i++)
                Blocks.Add(1, -direction, Block.BASIC_WHITE);

            // Rectangle
            Blocks.Add(-width, _info.Height +(direction * width), Block.BASIC_WHITE);
            Blocks.Add(0, -1, Block.BASIC_WHITE);
            Blocks.Add(0, -1, Block.BASIC_WHITE);
            for (int i = 1; i < width; i++)
            {
                Blocks.Add(1, 2 - direction, Block.BASIC_WHITE);
                Blocks.Add(0, -2, Block.BASIC_WHITE);
            }
            Blocks.Add(1, 0 - direction, Block.BASIC_WHITE);
            Blocks.Add(0, 1, Block.BASIC_WHITE);
            Blocks.Add(0, 1, Block.BASIC_WHITE);
        }

        private int GetBlockToAdd()
        {
            int random = _rnd.Next(0, MAX_DIFFICULTY + 1);

            if (random < _info.Difficulty)
                return Block.ARROW_RIGHT;

            return Block.WATER;
        }


        private void AddStart()
        {
            int width = 10;
            Blocks = new StartBlocksBuilder(StartType.HatRemover, _info.Height).Blocks;
           
            //Roof
            Blocks.Add(1, -_info.Height, Block.ARROW_RIGHT);
            for (int i = 0; i < width; i++)
                Blocks.Add(1, 0, Block.ARROW_RIGHT);


            //Floor
            Blocks.Add(-width, _info.Height, Block.ARROW_RIGHT);
            for (int i = 0; i < width - 1; i++)
                Blocks.Add(1, 0, Block.ARROW_RIGHT);

            //item
            Blocks.Add(-(width / 2) + 1, -6, Block.ITEM_BLUE);
            Blocks.Add((width / 2), 6, Block.ARROW_RIGHT);
        }

        private void AddFloor() => Blocks.Add(0,1, Block.ARROW_RIGHT);

        private void AddRoof() => Blocks.Add(1, -_info.Height - _info.Direction, Block.ARROW_RIGHT);

        private void AddFinish()
        {
            int width = 20;
            int amountOfNets = 5;

            Blocks.Add(1, -_info.Height, Block.BASIC_WHITE);

            for (int i = 0; i < width; i++)
                Blocks.Add(1, 0, Block.BASIC_WHITE);

            for (int i = 0; i < _info.Height; i++)
                Blocks.Add(0, 1, Block.BASIC_WHITE);

            for (int i = 0; i < (width - amountOfNets); i++)
                Blocks.Add(-1, 0, Block.BASIC_WHITE);

            for (int i = 0; i < amountOfNets; i++)
                Blocks.Add(-1, 0, Block.NET);

            Blocks.Add((width / 2 + 3), -(_info.Height / 2), Block.BRICK);
            Blocks.Add(0,-1,  Block.GOAL);
        }


    }
}
