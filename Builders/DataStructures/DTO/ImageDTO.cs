using SkiaSharp;

using static LevelModel.Models.Components.Block;

namespace Builders.DataStructures.DTO
{
    public class ImageDTO
    {


        public const int CENTER_X = 13250;
        public const int CENTER_Y = 9950;

        public const int MAX_SIZE_BLOCKS = 20;
        public const int MAX_SIZE_ART    = 8;

        public const int DEFAULT_LOGO_SIZE = 6;


        public enum IgnoreColor { Black = 1, White, None };
        public enum ColorSensitivty { VeryLow=1, Low, Medium, High, VeryHigh };

        public enum ImagePosition { Above = 1, Below, Custom }

        public enum ImageType { Art0 = 1, Art1, Blocks, None }


        public SKBitmap Image { get; set; }

        public IgnoreColor ColorToIgnore { get; set; }

        public int Size { get; set; }

        public ImageType Type { get; set; }

        public ImagePosition Position { get; set; }

        public int BlockType { get; set; }

        public int PaddingX { private get; set; }

        public int PaddingY { private get; set; }

        public bool CreateDrawImage { get; set; }

        public ColorSensitivty Sensitivty { get; set; }


        public int GetImageSize()
        {
            switch (Size)
            {
                case 1:  return 1  * PIXELS_PER_BLOCK;
                case 2:  return 2  * PIXELS_PER_BLOCK;
                case 3:  return 3  * PIXELS_PER_BLOCK;
                case 4:  return 5  * PIXELS_PER_BLOCK;
                case 5:  return 7  * PIXELS_PER_BLOCK;
                case 6:  return 10 * PIXELS_PER_BLOCK;
                case 7:  return 13 * PIXELS_PER_BLOCK;
                case 8:  return 16 * PIXELS_PER_BLOCK;
                case 9:  return 19 * PIXELS_PER_BLOCK;
                case 10: return 22 * PIXELS_PER_BLOCK;
                case 11: return 25 * PIXELS_PER_BLOCK;
                case 12: return 28 * PIXELS_PER_BLOCK;
                case 13: return 31 * PIXELS_PER_BLOCK;
                case 14: return 34 * PIXELS_PER_BLOCK;
                case 15: return 37 * PIXELS_PER_BLOCK;
                case 16: return 40 * PIXELS_PER_BLOCK;
                case 17: return 43 * PIXELS_PER_BLOCK;
                case 18: return 46 * PIXELS_PER_BLOCK;
                case 19: return 49 * PIXELS_PER_BLOCK;
                case 20: return 52 * PIXELS_PER_BLOCK;

                default: return 7  * PIXELS_PER_BLOCK;
            }
        }

        public int GetPaddingX()
        {
            switch (Position)
            {
                case ImagePosition.Below:   return CENTER_X + 28;
                case ImagePosition.Above:   return CENTER_X + 50;
                case ImagePosition.Custom:  return PaddingX;

                default: return CENTER_X - 50;
            }

        }

        public int GetPaddingY()
        {
            switch (Position)
            {
                case ImagePosition.Below:   return CENTER_Y + 215;
                case ImagePosition.Above:   return CENTER_Y - 80;
                case ImagePosition.Custom:  return PaddingY;

                default: return CENTER_Y - 80;
            }
        }


    }
}
