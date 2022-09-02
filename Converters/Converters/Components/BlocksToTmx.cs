using System.Linq;
using LevelModel.Models.Components;
using Converters.DataStructures;
using System.Collections.Generic;

using static Converters.DataStructures.TmxBlocks;

namespace Converters.Converters.Components
{
    internal class BlocksToTmx 
    {


        internal List<TmxBlockLayer> Layers { get; private set; }

        internal int MaxHeight { get; private set; }
        internal int MaxWidth { get; private set; }

        internal Point RelativeStartPosition { get; private set; }

        private TmxBlocks _tmxBlocks;


        public BlocksToTmx(List<Block> blocks)
        {
            _tmxBlocks = new TmxBlocks(1);

            Convert(blocks);
            SetResult();
        }


        private void SetResult()
        {
            MaxHeight = _tmxBlocks.GetMaxHeight(); 
            MaxWidth  = _tmxBlocks.GetMaxWidth();

            Layers = new List<TmxBlockLayer>();

            var blockLayer = _tmxBlocks;
            while (blockLayer != null)
            {
                Layers.Add(new TmxBlockLayer(blockLayer.LayerID, blockLayer.GetBlockArray()));
                blockLayer = blockLayer.NextLayer;
            }
        }

        private void Convert(List<Block> blocks)
        {
            HandleStartPosition(blocks);

            foreach (var b in blocks) {

                b.Id = b.Id - BLOCK_ID_ADJUSTER;
                _tmxBlocks.AddBlock(b.X, b.Y, b.Id);
            }
        }

        private void HandleStartPosition(List<Block> blocks) {
            if(blocks != null && blocks.Count() > 0) {
                var firstBlock = blocks[0];
                RelativeStartPosition = new Point(firstBlock.X, firstBlock.Y);
                blocks.SetStartPosition(0,0);
            }
        }


    }
}
