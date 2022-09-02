using System.Linq;
using System.Collections.Generic;
using LevelModel.Models.Components;
using Converters.DataStructures;
using Converters.Converters.Components.ReferenceBlock;
using LevelModel.DTO;

namespace Converters.Converters.Components
{
    internal class TmxToBlocks 
    {


        private int xPadding;
        private int yPadding;
        private int _width;
        private int _height;
        private Point _startPosition;
        private List<TmxBlockLayer> _layers;
        private List<StartBlockInfo> _startBlocks;
        private Messages _messages;
        public List<Block> Blocks { get; set; }


        public TmxToBlocks(List<TmxBlockLayer> layers, Point startPosition, Messages messages)
        {
            _layers        = layers;
            _messages      = messages;
            _startBlocks   = new List<StartBlockInfo>();
            _startPosition = startPosition;
            Blocks         = new List<Block>();

            Convert();
        }


        private void Convert()
        {
            if (_layers.Count != 0)
            {
                CalculateSize();
                HandleStartBlocks();
                ConvertBlocks();
            }
        }

        private void CalculateSize()
        {
            _height = _layers.Max(layer => layer.BlockArray.GetLength(0));
            _width  = _layers.Max(layer => layer.BlockArray.GetLength(1));
        }


        private void ConvertBlocks()
        {

            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    AddPosition(x, y);
                    xPadding++;
                }
                xPadding -= _width;
                yPadding++;
            }
        }

        private void AddPosition(int x, int y)
        {
            foreach(var layer in _layers)
            {
                if (layer.IsRefLayer)
                    continue;

                int id = GetBlockID(layer.BlockArray, x, y);
                if (id != TmxBlocks.NO_BLOCK && !Block.IsStartBlock(id))
                {
                    Blocks.Add(xPadding, yPadding, id);
                    xPadding = 0;
                    yPadding = 0;
                }
            }
        }

        private int GetBlockID(int[,] blockArray, int x, int y)
        {
            if (y < blockArray.GetLength(0) && x < blockArray.GetLength(1)) {
                int id = blockArray[y, x];
                return (id == TmxBlocks.NO_BLOCK) ? id : id + TmxBlocks.BLOCK_ID_ADJUSTER;
            }

            return TmxBlocks.NO_BLOCK;
        }


        private void HandleStartBlocks() {
            SaveStartBlocks();

            if(_startBlocks.Count == 4)
                AddStartBlocks();
            else
                throw new InvalidDataException(ErrorMessages.DEFAULT + ", it does not contain the player start blocks");
        }

        private void SaveStartBlocks() {
            foreach (var layer in _layers.Where(l => !l.IsRefLayer)) {
                for (int y = 0; y < _height; y++) {
                    for (int x = 0; x < _width; x++) {
                        int id = GetBlockID(layer.BlockArray, x, y);
                        if (Block.IsStartBlock(id)) {
                            SaveStartBlock(x, y, id);
                        }
                    }
                }
            }
        }

        private void SaveStartBlock(int x, int y, int id) {
            if(!_startBlocks.Exists(b => b.BlockID == id))
                _startBlocks.Add(new StartBlockInfo(x, y, id));
            else
                _messages.Add("There are too many player start blocks of the same type", Message.MessageType.Warning);
        }

        private void AddStartBlocks() {
            var indexP1 = _startBlocks.FindIndex(x => x.BlockID == Block.START_BLOCK_P1);
            var indexP2 = _startBlocks.FindIndex(x => x.BlockID == Block.START_BLOCK_P2);
            var indexP3 = _startBlocks.FindIndex(x => x.BlockID == Block.START_BLOCK_P3);
            var indexP4 = _startBlocks.FindIndex(x => x.BlockID == Block.START_BLOCK_P4);

            AddFirstStartBlock(indexP1);
            AddStartBlock(indexP2, indexP1);
            AddStartBlock(indexP3, indexP2);
            AddStartBlock(indexP4, indexP3);
        }

        private void AddFirstStartBlock(int index) {
            xPadding -= _startBlocks[index].X;
            yPadding -= _startBlocks[index].Y;

            var refBlock = new GetReferenceBlock(_layers, GetStartBlockPosition());

            Blocks.Add(_startPosition.X + refBlock.PositionChange.X, _startPosition.Y + refBlock.PositionChange.Y, _startBlocks[index].BlockID);
        }

        private Point GetStartBlockPosition() {
            var startBlockInfo = _startBlocks.Where(x => x.BlockID == Block.START_BLOCK_P1).First();
            return new Point(startBlockInfo.X, startBlockInfo.Y);
        }

        private void AddStartBlock(int index, int prevIndex) {
            int x = _startBlocks[index].X - _startBlocks[prevIndex].X;
            int y = _startBlocks[index].Y - _startBlocks[prevIndex].Y;

            xPadding -= x;
            yPadding -= y;

            Blocks.Add(x, y, _startBlocks[index].BlockID);
        }


    }
}
