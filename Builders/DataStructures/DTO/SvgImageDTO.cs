using SkiaSharp;

using System.Collections.Generic;
using System.Xml;

using static LevelModel.Models.Components.Block;

namespace Builders.DataStructures.DTO
{
    public class SvgImageDTO
    {
        
        public XmlReader XmlReader { get; set; }
        public IEnumerable<SKPaint> Paths { get; set; }
        public SKPath ye;

        public struct Path
        {
            public IEnumerable<SKPoint> Points { get; set; }
            public SKColor Fill { get; set; }
            public FillRule FillRule { get; set; }
        }

        public enum FillRule
        {
            NonZero, EvenOdd
        }

        public enum IgnoreColor { Black = 1, White, None };
        public enum ColorSensitivty { VeryLow = 1, Low, Medium, High, VeryHigh };

        public enum ImagePosition { Above = 1, Below, Custom }

        public enum ImageType { Art0 = 1, Art1, None }

        public ImageType Type { get; set; }

        public ImagePosition Position { get; set; }



    }
}
