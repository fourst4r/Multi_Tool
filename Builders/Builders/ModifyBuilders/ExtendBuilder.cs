﻿using LevelModel.Models;
using LevelModel.Models.Components;
using System.Collections.Generic;
using Builders.DataStructures.DTO;
using Builders.DataStructures.Exceptions;

namespace Builders.Builders.ModifyBuilders
{
    internal class ExtendBuilder
    {


        private ExtendDTO _info;

        public Level Result { get; set; }


        internal ExtendBuilder (ExtendDTO info)
        {
            _info = info;

            Init();
            Extend();
        }


        private void Init()
        {
            Result       = _info.Level;
            Result.Title = _info.Title;
        }

        private void Extend()
        {
            switch(_info.Type)
            {
                case ExtendDTO.ExtendType.Blocks:
                    ExtendBlocks();
                    break;
                case ExtendDTO.ExtendType.Art0:
                    ExtendArt0();
                    break;
                case ExtendDTO.ExtendType.Art1:
                    ExtendArt1();
                    break;
                default:
                    throw new TypeException("Unknown type of extension");
            }
        }

        private void ExtendArt0()
        {
            if (_info.ArtToAdd == null || _info.ArtToAdd.Count == 0)
                return;

            Result.DrawArt0 = Result.DrawArt0.Merge(_info.ArtToAdd);
        }
        
        private void ExtendArt1()
        {
            if (_info.ArtToAdd == null || _info.ArtToAdd.Count == 0)
                return;

            Result.DrawArt1 = Result.DrawArt1.Merge(_info.ArtToAdd);
        }

        private void ExtendBlocks()
        {
            IList<Block> blocksToAdd = new List<Block>();

            for (int i = 0; i < _info.Multiplier; i++)
                blocksToAdd = blocksToAdd.Merge(_info.BlocksToAdd);


            blocksToAdd.SetStartPosition(_info.PaddingX, _info.PaddingY);
            Result.Blocks = Result.Blocks.Merge(blocksToAdd);
        }


    }
}
