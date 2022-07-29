using System;
using System.Collections.Generic;
using LevelModel.Models.Components;
using LevelModel.DTO;

namespace Parsers.Parsers
{
    internal class BlockParser
    {


        public List<Block> Result { get; set; }

        private int _previousBlockID;


        public BlockParser(string blockData) {
            Result = new List<Block>();

            Parse(blockData);
        }


        public void Parse(string blockData) {
            if (blockData == null)
                return;

            foreach (var b in blockData.Split(',')) {
                if (string.IsNullOrWhiteSpace(b) == false && b.Length >= 2)
                    HandleBlock(b.Split(';'));
            }
        }

        private void HandleBlock(string[] blockInfo) {
            int x  = Convert.ToInt32(blockInfo[0]);
            int y  = Convert.ToInt32(blockInfo[1]);
            int id = GetId(blockInfo);
            var options = string.Empty;

            if(blockInfo.Length > 3)
                options = blockInfo[3];

            Result.Add(x, y, id, options);
            _previousBlockID = id;
        }

        private int GetId(string[] blockInfo) {
            if (blockInfo.Length >= 3) {
                int id = Convert.ToInt32(blockInfo[2]);
                return (id >= Block.BASIC_BROWN) ? id : id + Block.BLOCK_ID_ADJUSTER;
            }
            else {
                if (_previousBlockID == default)
                    throw new InvalidDataException("Invalid block data, the first block does not have an ID");
                return _previousBlockID;
            }
        }


    }
}
