using LevelModel.Models.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Converters.DataStructures
{
    internal class TmxBlocks
    {


        private List<List<int>> _blocks;
        public const int NO_BLOCK = 0;

        public const int BLOCK_ID_ADJUSTER = Block.BLOCK_ID_ADJUSTER - 1;

        public int LayerID { get; set; }
        public TmxBlocks NextLayer { get; set; }

        private int xCurrent;
        private int yCurrent;


        public TmxBlocks(int layerID)
        {
            _blocks = new List<List<int>>();
            LayerID = layerID;
        }

        public TmxBlocks(int layerID, int width, int height) : this(layerID)
        {
            AddBlock(width, height, NO_BLOCK); //Make sure the lists gets the right size
        }


        internal void AddBlock(int x, int y, int id)
        {
            AdjustPosition(x, y);
            AdjustHeight();
            AdjustWidth();

            InsertBlockToList(id);
        }

        internal int GetMaxHeight() => _blocks.Count;

        internal int GetMaxWidth() => (_blocks.Count > 0) ? _blocks[0].Count : 0;


        private void AdjustPosition(int x, int y)
        {
            if (LayerID == 1)
            {
                xCurrent += x;
                yCurrent += y;
            }
            else
            {
                xCurrent = x;
                yCurrent = y;
            }
        }

        private void InsertBlockToList(int id)
        {
            if (_blocks[yCurrent][xCurrent] == NO_BLOCK)
            {
                _blocks[yCurrent][xCurrent] = id;
                AddToNextLayer(NO_BLOCK); 
            }
            else
            {
                AddToNextLayer(id);
            }
        }

        private void AddToNextLayer(int id)
        {
            if (NextLayer == null && id == NO_BLOCK)
                return;

            if (NextLayer == null)
                NextLayer = new TmxBlocks(LayerID + 1, GetMaxWidth() - 1, GetMaxHeight() - 1);

            NextLayer.AddBlock(xCurrent, yCurrent, id);
        }

        private void IncreaseHeight(bool insert)
        {
            if (insert)
                _blocks.Insert(0, createList());
            else
                _blocks.Add(createList());

            if (NextLayer == null)
                return;

            NextLayer.IncreaseHeight(insert);
        }

        private void IncreaseWidth(int row, bool insert)
        {
            if (insert)
                _blocks[row].Insert(0, NO_BLOCK);
            else
                _blocks[row].Add(NO_BLOCK);

            if (NextLayer == null)
                return;

            NextLayer.IncreaseWidth(row, insert);
        }

        private void AdjustHeight()
        {
            if (yCurrent < 0)
            {
                while (yCurrent < 0)
                {
                    IncreaseHeight(true);
                    yCurrent++;
                }
            }
            else if(yCurrent >= _blocks.Count)
            {
                for (int i = _blocks.Count; i <= yCurrent; i++)
                    IncreaseHeight(false);
            }
        }

        private void AdjustWidth()
        {
            if (xCurrent < 0)
            {
                while (xCurrent < 0)
                {
                    for (int j = 0; j < _blocks.Count; j++)
                        IncreaseWidth(j, true);
                    xCurrent++;
                }
            }
            else if(xCurrent >= _blocks[yCurrent].Count)
            {
                for (int i = _blocks[yCurrent].Count; i <= xCurrent; i++)
                    for (int j = 0; j < _blocks.Count; j++)
                        IncreaseWidth(j, false);
            }
        }

        internal int[,] GetBlockArray()
        {
            int height = GetMaxHeight();
            int width  = GetMaxWidth();

            int[,] blockArray = new int[height, width];

            for(int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    blockArray[y, x] = _blocks[y][x];
                }
            }

             return blockArray;
        }

        private List<int> createList()
        {
            int width = GetMaxWidth();
            var list = new List<int>();

            for (int i = 0; i < width; i++)
                list.Add(NO_BLOCK);

            return list;
        }

    }
}
