using SkiaSharp;
using System.Collections.Generic;
using LevelModel.Models.Components;
using LevelModel.Models.Components.Art;
using Builders.DataStructures.DTO;
using System.Linq;

namespace Builders.Builders.LevelBuilders.Types.ImageBuilders
{
    internal class ImageToArtBuilder : ImageBuilder
    {


        private int _startPositionX;
        private int _startPositionY;

        internal List<DrawArt> Result { get; set; }


        internal ImageToArtBuilder(ImageDTO imageInfo) : base(imageInfo)
        {
            Result          = new List<DrawArt>();
            _startPositionX = imageInfo.GetPaddingX();
            _startPositionY = imageInfo.GetPaddingY();

            if(imageInfo.CreateDrawImage)
                BuildDrawImage();
            else
                BuildImage(); 
        }


        private void BuildDrawImage()
        {
            if(ImageInfo.Image == null)
                return;

            var arrayBuilder = new ImageArrayBuilder(ImageInfo.Image);

            foreach(var array in arrayBuilder.Arrays)
            {
                var builder   = new StrokeBuilder(array, ImageInfo.Sensitivty, (c) => IgnorePixelColor(c, out _));
                var optimizer = new StrokeOptimizer(builder.Strokes);
                var count     = optimizer.Strokes.Count;

                for (int i = 0; i < count; i++)
                    MakeStroke(optimizer.Strokes[i], array.Offset);
            }
        }

        private void MakeDot(string color, int xPos, int yPos)
        {
            int dotSize = 2;

            Result.AddDot(color, dotSize, xPos, yPos);
        }

        private void MakeStroke(Stroke s, Index offset)
        {
            if(s?.Movement == null || s.Movement.Count == 0)
                return;

            if (IgnorePixelColor(s.Color, out _))
                return;

            var da   = new DrawArt();
            da.Color = ColorToRGB(s.Color);
            da.Size  = 2;
            da.X     = s.Movement[0].Right + _startPositionX + offset.Right;
            da.Y     = s.Movement[0].Down  + _startPositionY + offset.Down;

            foreach(var pos in s.Movement.Skip(1))
            {
                da.Movement.Add(pos.Right);
                da.Movement.Add(pos.Down);
            }

            Result.Add(da);
        }

        protected override void AddPixel(SKColor color, int x, int y)
        {
            if (!IgnorePixelColor(color, out string c))
            {
                MakeDot(c,  _startPositionX + x, _startPositionY + y);
            }
        }


    }
}
