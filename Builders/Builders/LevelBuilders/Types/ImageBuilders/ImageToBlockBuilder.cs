using SkiaSharp;
using System.Collections.Generic;
using LevelModel.Models.Components;
using Builders.DataStructures.DTO;

using static Builders.DataStructures.DTO.ImageDTO;
using static Builders.Builders.LevelBuilders.Types.StartBlocksBuilder;

namespace Builders.Builders.LevelBuilders.Types.ImageBuilders
{
    internal class ImageToBlocksBuilder : ImageBuilder
    {


        private int _previousX;
        private int _previousY;
        private int _blockID;

        internal List<Block> Blocks { get; set; }


        internal ImageToBlocksBuilder(ImageDTO imageInfo) : base(imageInfo)
        {
            _previousX    = -3;
            _previousY    = imageInfo.Image.Height / 2;
            _blockID      = imageInfo.BlockType;

            AddStart();

            if (imageInfo != null && imageInfo.Type == ImageType.Blocks)
                BuildImage();
        }


        private void AddStart()
        {
            Blocks = new StartBlocksBuilder(StartType.Image).Blocks;
        }

        protected override void AddPixel(SKColor color, int x, int y)
        {
            int newX = x - _previousX;
            int newY = y - _previousY;

            if (!IgnorePixelColor(color, out string ignore))
            {
                Blocks.Add(newX, newY, _blockID);
                _previousX = x;
                _previousY = y;
            }
        }


    }
}
