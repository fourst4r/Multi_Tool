using Converters.DataStructures;
using LevelModel.Models.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Converters.Converters.Components.ReferenceBlock
{
    internal class GetReferenceBlock
    {


        private Point _refBlockPosition;
        private Point  _startBlockPosition;
        private List<TmxBlockLayer> _layers;

        private readonly Point NO_REFERENCE_BLOCK = new Point(-1 ,-1);

        public Point PositionChange { get; private set; }


        public GetReferenceBlock(List<TmxBlockLayer> layers, Point startBlockPosition) 
        {
            _layers             = layers;
            _startBlockPosition = startBlockPosition;
            _refBlockPosition   = NO_REFERENCE_BLOCK;
            PositionChange      = new Point(0, 0);

            GetBlock();

            if (_refBlockPosition != NO_REFERENCE_BLOCK)
                ChangeStartPosition();
        }


        private void ChangeStartPosition() {
            int x = _startBlockPosition.X - _refBlockPosition.X;
            int y = _startBlockPosition.Y - _refBlockPosition.Y;

            PositionChange = new Point(x, y);
        }

        private void GetBlock() {
            foreach (var layer in _layers.Where(l => l.IsRefLayer)) {
                for (int y = 0; y < layer.BlockArray.GetLength(0); y++) {
                    for (int x = 0; x < layer.BlockArray.GetLength(1); x++) {
                        if (layer.BlockArray[y, x] == AddReferenceBlock.REFERENCE_BLOCK_ID) {
                            _refBlockPosition = new Point(x + layer.LeftPadding, y + layer.TopPadding);
                            return;
                        }
                    }
                }
            }
        }

    }
}
