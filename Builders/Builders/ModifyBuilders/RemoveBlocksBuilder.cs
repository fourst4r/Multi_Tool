using Builders.DataStructures.DTO;
using LevelModel.Models.Components;
using System.Collections.Generic;

namespace Builders.Builders.ModifyBuilders
{
    internal class RemoveBlocksBuilder
    {

        private RemoveBlocksDTO _info;
        private List<Block> _blocks;
        private int _X;
        private int _Y;

        public RemoveBlocksBuilder(RemoveBlocksDTO info)
        {
            _info   = info;
            _blocks = new List<Block>();
            _X      = 0;
            _Y      = 0;

            if (_info?.Level == null)
                return;

            RemoveBlocks();
        }

        private void AddBlock(Block b)
        {
            b.X = _X;
            b.Y = _Y;

            _blocks.Add(b);

            _X = 0;
            _Y = 0;
        }

        public void RemoveBlocks()
        {
            foreach (var b in _info.Level.Blocks)
            {
               _X += b.X;
               _Y += b.Y;

                if(ShouldAddBlock(b.Id))
                    AddBlock(b);
            }

            _info.Level.Blocks = _blocks;
        }

        private bool ShouldAddBlock(int blockID)
        {
            if(_info.RemoveAll) 
                return Block.IsStartBlock(blockID);
            else
                return blockID != _info.BlockID;
        }

    }
}
