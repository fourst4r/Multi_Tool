using System;
using SkiaSharp;
using Builders.DataStructures.DTO;

using static Builders.DataStructures.DTO.ImageDTO;
using LevelModel.Models.Components;

namespace Builders.Builders.LevelBuilders.Types.ImageBuilders
{
    internal abstract class ImageBuilder
    {


        protected ImageDTO ImageInfo;


        internal ImageBuilder(ImageDTO imageInfo)
        {
            ImageInfo = imageInfo;

            if (ImageInfo?.Image != null)
            {
                ScaleImage();
            }
        }


        protected abstract void AddPixel(SKColor color, int x, int y);

        protected bool IgnorePixelColor(SKColor? cValue, out string rgbValue)
        {
            if(cValue == null)
            {
                rgbValue = string.Empty;
                return false;
            }

            int blackLimit  = 35;
            int whiteLimit  = 230;
            int transparent = 0;

            rgbValue = ColorToRGB(cValue.Value);

            if (ImageInfo.ColorToIgnore == IgnoreColor.Black && IsRGBLessThan(cValue.Value, blackLimit))
                return true;

            if (ImageInfo.ColorToIgnore == IgnoreColor.White && IsRGBGreaterThan(cValue.Value, whiteLimit))
                return true;

            if (cValue.Value.Alpha == transparent)
                return true;

            return false;
        }

        protected void BuildImage()
        {
            if (ImageInfo?.Image != null)
            {
                Build(ImageInfo.Image);
            }
        }


        private void Build(SKBitmap bmp)
        {
            if(bmp == null)
                return;

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    AddPixel(bmp.GetPixel(x, y), x, y);
                } 
            }
        }

        protected bool IsRGBLessThan(SKColor c, int value) => c.Red < value && c.Green < value && c.Blue < value;

        protected bool IsRGBGreaterThan(SKColor c, int value) => c.Red > value && c.Green > value && c.Blue > value;

        private void ScaleImage()
        {
            var Size         = GetScaledSize(ImageInfo.Image);
            SKImageInfo info = new SKImageInfo(Size.X, Size.Y, SKColorType.Argb4444);
            ImageInfo.Image  = ImageInfo.Image.Resize(info, SKFilterQuality.Medium);
        }

        private Point GetScaledSize(SKBitmap image) {
            int denominator     = (image.Width > image.Height) ? image.Width : image.Height;
            decimal scaleFactor = ((decimal)ImageInfo.GetImageSize()) / denominator;

            int xSize = (int)Math.Round(image.Width * scaleFactor);
            int ySize = (int)Math.Round(image.Height * scaleFactor);

            return new Point(xSize, ySize);
        }


        protected string ColorToRGB(SKColor c) => c.Red.ToString("x2") + c.Green.ToString("x2") + c.Blue.ToString("x2");


    }
}
