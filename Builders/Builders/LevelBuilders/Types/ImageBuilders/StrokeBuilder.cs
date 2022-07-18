using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;

using static Builders.Builders.LevelBuilders.Types.ImageBuilders.Direction;
using SkiaSharp;
using static Builders.DataStructures.DTO.ImageDTO;

namespace Builders.Builders.LevelBuilders.Types.ImageBuilders
{

    internal struct Index : IEquatable<Index>
    {
        public int Right { get; set; }
        public int Down { get; set; }

        [DebuggerStepThrough]
        public Index(int down, int right)
        {
            Right = right;
            Down = down;
        }

        public static Index operator +(Index a, Index b)
        {
            return new Index(a.Down + b.Down, a.Right + b.Right);
        }

        public static Index operator -(Index a, Index b)
        {
            return new Index(a.Down - b.Down, a.Right - b.Right);
        }

        public static Index operator *(Index a, int multi)
        {
            return new Index(a.Down * multi, a.Right * multi);
        }

        public static bool operator ==(Index a, Index b) => a.Right == b.Right && a.Down == b.Down;
        public static bool operator !=(Index a, Index b) => !(a == b);

        public override bool Equals(object o)
        {
            if (o is Index p)
                return Equals(p);

            return false;
        }

        public bool Equals(Index other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;

            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Right.GetHashCode();
            hashCode = hashCode * -1521134295 + Down.GetHashCode();

            return hashCode;
        }
    }

    internal class Stroke
    {
        public List<Index> Movement { get; }
        public SKColor Color { get; set; }

        [DebuggerStepThrough]
        public Stroke()
        {
            Movement = new List<Index>();
        }

        [DebuggerStepThrough]
        public Stroke(Index p) : this()
        {
            Movement.Add(p);
        }

        public Stroke Copy()
        {
            var s     = new Stroke();
            s.Color   = Color;
            var count = Movement.Count;

            for (int i = 0; i < count; i++)
            {
                var item = Movement[i];
                s.Movement.Add(new Index(item.Down, item.Right));
            }

            return s;
        }

        public bool Contains(Index p)
        {
            var count = Movement.Count;

            for (int i = 0; i < count; i++)
            {
                if(p == Movement[i])
                    return true;
            }

            return false;
        }
    }

    internal static class Direction
    {
        public static readonly Index Up = new Index(-1, 0);
        public static readonly Index Right = new Index(0, 1);
        public static readonly Index Down = new Index(1, 0);
        public static readonly Index Left = new Index(0, -1);

        public static readonly Index UpRight = new Index(-1, 1);
        public static readonly Index DownRight = new Index(1, 1);
        public static readonly Index DownLeft = new Index(1, -1);
        public static readonly Index UpLeft = new Index(-1, -1);

        public enum DirectionEnum { None, Up, UpRight, Right, DownRight, Down, DownLeft, Left, UpLeft };

        public static IEnumerable<Index> GetAllDirections()
        {
            return new[]
            {
                Right,
                Down,
                Left,
                Up,
                //UpRight,
                //DownRight,
                //DownLeft,
                //UpLeft
            };

        }

        public static DirectionEnum GetDirectionEnum(this Stroke s, int i)
        {
            if (s == null || i + 1 >= s.Movement.Count)
                return DirectionEnum.None;

            var p1 = s.Movement[i];
            var p2 = s.Movement[i + 1];
            var diff = p2 - p1;

            if (diff.Down > 0)
            {   // Down
                if (diff.Right > 0)
                    return DirectionEnum.DownRight;
                else if (diff.Right == 0)
                    return DirectionEnum.Down;
                else
                    return DirectionEnum.DownLeft;
            }
            else if (diff.Down == 0)
            {   // Sideways
                if (diff.Right > 0)
                    return DirectionEnum.Right;
                else if (diff.Right == 0)
                    return DirectionEnum.None;
                else
                    return DirectionEnum.Left;
            }
            else
            {   // Up
                if (diff.Right > 0)
                    return DirectionEnum.UpRight;
                else if (diff.Right == 0)
                    return DirectionEnum.Up;
                else
                    return DirectionEnum.UpLeft;
            }
        }

        public static Index GetVector(this DirectionEnum dir, int magnitude)
        {
            switch (dir)
            {
                case DirectionEnum.Up: return Direction.Up * magnitude;
                case DirectionEnum.UpRight: return Direction.UpRight * magnitude;
                case DirectionEnum.Right: return Direction.Right * magnitude;
                case DirectionEnum.DownRight: return Direction.DownRight * magnitude;
                case DirectionEnum.Down: return Direction.Down * magnitude;
                case DirectionEnum.DownLeft: return Direction.DownLeft * magnitude;
                case DirectionEnum.Left: return Direction.Left * magnitude;
                case DirectionEnum.UpLeft: return Direction.UpLeft * magnitude;
                default: return new Index(0, 0);
            }
        }
    }

    internal class ImageArray
    {
        public SKColor[,] Pixels { get; set; }

        public Index Offset { get; set; }

        public ImageArray(Index size, Index offset)
        {
            Pixels = new SKColor[size.Down, size.Right];
            Offset = offset;
        }

    }

    internal class ImageArrayBuilder
    {
        public List<ImageArray> Arrays { get; }

        private int _height;
        private int _width;
        private SKBitmap _image;

        public ImageArrayBuilder(SKBitmap image)
        {
            Arrays = new List<ImageArray>();

            if (image == null)
                return;

            _height = image.Height;
            _width  = image.Width;
            _image  = image;

            AddArrays();
        }

        private void AddArrays()
        {
            int size = 50;

            for (int down = 0; down < _height; down += size)
            {
                for (int right = 0; right < _width; right += size)
                {
                    AddArray(down, down + size, right, right + size);
                }
            }
        }

        private void AddArray(int startRow, int endRow, int startColumn, int endColumn)
        {
            if (endColumn > _width)
                endColumn = _width;

            if (endRow > _height)
                endRow = _height;

            var rowSize    = endRow - startRow;
            var columnSize = endColumn - startColumn;
            var size       = new Index(rowSize, columnSize);
            var offset     = new Index(startRow, startColumn);

            if(rowSize <= 0 || columnSize <= 0)
                return;

            var array = new ImageArray(size, offset);

            for (int y = startRow; y < endRow; y++)
                for (int x = startColumn; x < endColumn; x++)
                    array.Pixels[y - startRow, x - startColumn] = _image.GetPixel(x, y);

            Arrays.Add(array);
        }
    }

    internal class StrokeBuilder
    {
        public List<Stroke> Strokes { get; }
        private SKColor[,] _image;

        private readonly int _width;
        private readonly int _height;

        private readonly ColorSensitivty _sensitivty;

        private readonly Predicate<SKColor?> _ignoreColor;

        public StrokeBuilder(ImageArray array, ColorSensitivty sensitivty, Predicate<SKColor?> ignoreColor)
        {
            Strokes      = new List<Stroke>();
            _image       = array?.Pixels ?? new SKColor[0,0];
            _ignoreColor = ignoreColor;
            _sensitivty  = sensitivty;

            _height = _image?.GetLength(0) ?? 0;
            _width  = _image?.GetLength(1) ?? 0;

            CreateStrokes();
        }

        private void CreateStrokes()
        {
            if (_image == null)
                return;

            for (int down = 0; down < _height; down++)
            {
                for (int right = 0; right < _width; right++)
                {
                    var s = CreateStroke(new Index(down, right));

                    if (s != null)
                        Strokes.Add(s);
                }
            }
        }

        private SKColor? GetColor(Index p)
        {
            SKColor? fallback = null;

            if (p.Right < 0 || p.Down < 0)
                return fallback;

            if (p.Right >= _width || p.Down >= _height)
                return fallback;

            return _image[p.Down, p.Right];
        }

        private Stroke CreateStroke(Index point, Stroke stroke = null)
        {
            if (IsStroked(point))
                return stroke;

            var color = GetColor(point);

            if (_ignoreColor != null && _ignoreColor.Invoke(color))
                return stroke;

            if (stroke == null)
                stroke = new Stroke(point) { Color = color.HasValue ? color.Value : default(SKColor) };

            foreach (var dir in GetAllDirections())
            {
                var newStroke = StrokeDirection(point, stroke, dir);

                if (newStroke.Movement.Count > stroke.Movement.Count)
                    return newStroke;
            }

            return stroke;
        }

        private bool IsStroked(Index p)
        {
            return Strokes.Any(s => s.Contains(p));
        }

        private Stroke StrokeDirection(Index p, Stroke s, Index direction)
        {
            var color = GetColor(p);
            var newPoint = p + direction;
            var rightColor = GetColor(newPoint);

            // check if current stroke contains the point
            if (s.Movement.Contains(newPoint))
                return s;

            // check if previous strokes contains the point
            if (IsStroked(newPoint))
                return s;

            if (!IsColorEqual(color, rightColor))
                return s;

            var extendedStroke = s.Copy();
            extendedStroke.Movement.Add(newPoint);

            return CreateStroke(newPoint, extendedStroke);
        }

        private bool IsColorEqual(SKColor? c1, SKColor? c2)
        {
            if (c1 == null || c2 == null)
                return false;

            var distance = GetColorDistance(c1.Value, c2.Value);

            switch (_sensitivty)
            {
                case ColorSensitivty.VeryLow:  return distance < 100;
                case ColorSensitivty.Low:      return distance < 75;
                case ColorSensitivty.Medium:   return distance < 50;
                case ColorSensitivty.High:     return distance < 25;
                case ColorSensitivty.VeryHigh: return distance < 10;

                default: return distance < 50;
            }
        }

        static double GetColorDistance(SKColor e1, SKColor e2)
        {
            long rmean = ((long)e1.Red + e2.Red) / 2;
            long r = e1.Red - (long)e2.Red;
            long g = e1.Green - (long)e2.Green;
            long b = e1.Blue - (long)e2.Blue;

            return Math.Sqrt((((512 + rmean) * r * r) >> 8) + 4 * g * g + (((767 - rmean) * b * b) >> 8));
        }

    }

    internal class StrokeOptimizer
    {
        public List<Stroke> Strokes { get; }

        public StrokeOptimizer(List<Stroke> input)
        {
            Strokes = new List<Stroke>();

            OptimizeStrokes(input);
        }

        private void OptimizeStrokes(List<Stroke> input)
        {
            if (input == null)
                return;

            foreach (var s in input)
            {
                var newStroke = OptimizeStroke(s);

                if (newStroke != null)
                    Strokes.Add(newStroke);
            }
        }

        private Stroke OptimizeStroke(Stroke s)
        {
            if (s?.Movement == null || s.Movement.Count == 0)
                return null;

            var newStroke = new Stroke(s.Movement.First());
            var count = s.Movement.Count;
            newStroke.Color = s.Color;

            for (int i = 0; i < count - 1;)
            {
                var dir = s.GetDirectionEnum(i);
                var j = i + 1;

                while (dir == s.GetDirectionEnum(j) && dir != DirectionEnum.None)
                    j++;

                var vector = dir.GetVector(j - i);

                newStroke.Movement.Add(new Index(vector.Down, vector.Right));
                i = j;
            }

            return newStroke;
        }
    }
}
