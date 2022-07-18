using Converters.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Converters.Converters.Components
{
    internal class BlockArraysToEqualSize
    {

        private List<TmxBlockLayer> _layers;
        private int _width;
        private int _height;
        private int _maxTopPadding;
        private int _maxLeftPadding;


        public BlockArraysToEqualSize(List<TmxBlockLayer> layers) {
            _layers = layers;

            CalculateSize();
            Convert();
        }


        private void Convert() {
            foreach(var layer in _layers) {

                if (layer.Height < _height)
                    ChangeHeight(layer);

                if (layer.Width < _width)
                    ChangeWidth(layer);
            }
        }

        private void ChangeHeight(TmxBlockLayer layer) {
            int[,] myArray      = new int[_height,  _width];
            int myHeight        = 0;
            int topPaddingLimit = _maxTopPadding + layer.TopPadding;
            int arrayBlockLimit = layer.BlockArray.GetLength(0) + topPaddingLimit;

            for(int i = 0; i < _height; i++) {
                bool copyBlocks = (myHeight < topPaddingLimit) ? false : (myHeight < arrayBlockLimit) ? true : false; 
                InsertRow(myArray, myHeight, (copyBlocks) ? layer.BlockArray : null, topPaddingLimit);
                myHeight++;
            }

            layer.TopPadding = 0;
            layer.Height     = _height;
            layer.BlockArray = myArray;
        }

        private void ChangeWidth(TmxBlockLayer layer) {
            int[,] myArray = new int[layer.Height, _width];
            int myWidth = 0;
            int leftPaddingLimit = _maxLeftPadding + layer.LeftPadding;
            int arrayBlockLimit = layer.BlockArray.GetLength(1) + leftPaddingLimit;

            for (int i = 0; i < _width; i++) {
                bool copyBlocks = (myWidth < leftPaddingLimit) ? false : (myWidth < arrayBlockLimit) ? true : false;
                InsertColumn(myArray, myWidth, (copyBlocks) ? layer.BlockArray : null, leftPaddingLimit);
                myWidth++;
            }

            layer.LeftPadding = 0;
            layer.Width       = _width;
            layer.BlockArray = myArray;
        }

        private void InsertRow(int[,] array, int row, int[,] arrayToCopy = null, int padding = 0) {
            int width = array.GetLength(1);
            int arrayToCopyWidth = (arrayToCopy == null) ? -1 : arrayToCopy.GetLength(1);

            for (int i = 0; i < width; i++) { 
                if (arrayToCopy != null && i < arrayToCopyWidth)
                    array[row, i] = arrayToCopy[row - padding, i];
                else
                    array[row, i] = TmxBlocks.NO_BLOCK;
            }
        }

        private void InsertColumn(int[,] array, int column, int[,] arrayToCopy = null, int padding = 0) {
            int height = array.GetLength(0);
            int arrayToCopyHeight = (arrayToCopy == null) ? -1 : arrayToCopy.GetLength(0);

            for (int i = 0; i < height; i++) {
                if (arrayToCopy != null && i < arrayToCopyHeight)
                    array[i, column] = arrayToCopy[i, column - padding];
                else
                    array[i, column] = TmxBlocks.NO_BLOCK;
            }
        }

        private void CalculateSize() {
            _maxTopPadding  = _layers.Min(layer => layer.TopPadding)  * -1;
            _maxLeftPadding = _layers.Min(layer => layer.LeftPadding) * -1;

            _height = _layers.Max(layer => layer.BlockArray.GetLength(0) + layer.TopPadding)  + _maxTopPadding;
            _width  = _layers.Max(layer => layer.BlockArray.GetLength(1) + layer.LeftPadding) + _maxLeftPadding;

        }

    }
}
