using System.Linq;
using System.Collections.Generic;
using LevelModel.Models.Components;
using Converters.DataStructures;

namespace Converters.Converters.Components.ReferenceBlock
{
    internal class AddReferenceBlock
    {


        private List<TmxBlockLayer> _layers;
        private readonly Point NO_START_BLOCK = new Point(-1, -1);
        private Point _startPosition;

        public const int INVALID_LAYER_ID = -1;
        public const int LAYER_ID = 63;

        public bool BlockAdded { get; set; }

        public const int REFERENCE_BLOCK_ID = Block.HAPPY_BLOCK - TmxBlocks.BLOCK_ID_ADJUSTER;


        public AddReferenceBlock(List<TmxBlockLayer> layers) {
            _layers = layers;
            _startPosition = NO_START_BLOCK;
            BlockAdded = false;

            StartPosition();

            if (_startPosition != NO_START_BLOCK)
                SetReferenceBlock();
        }


        private Point GetMapLimit() {
            return new Point(_layers.Max(layer => layer.BlockArray.GetLength(0)),  _layers.Max(layer => layer.BlockArray.GetLength(1)));
        }

        private void SetReferenceBlock() {
            Point limits = GetMapLimit();

            var blockArray = new int[limits.X, limits.Y];
            blockArray[_startPosition.Y, _startPosition.X] = REFERENCE_BLOCK_ID;

            _layers.Add(new TmxBlockLayer(LAYER_ID, blockArray) { IsRefLayer = true });
            BlockAdded = true;
        }

        private void StartPosition() {
            foreach(var layer in _layers) 
            {
                for(int y = 0; y < layer.BlockArray.GetLength(0); y++) 
                {
                    for (int x = 0; x < layer.BlockArray.GetLength(1); x++) {
                        if (layer.BlockArray[y, x] == Block.START_BLOCK_P1 - TmxBlocks.BLOCK_ID_ADJUSTER) {
                            _startPosition = new Point(x, y);
                            return;
                        }
                    }
                }
            }
        }


    }
}
