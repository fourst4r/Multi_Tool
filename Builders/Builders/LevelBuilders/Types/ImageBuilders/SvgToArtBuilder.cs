using Builders.DataStructures.DTO;

using LevelModel.Models.Components.Art;

using System;
using System.Collections.Generic;
using System.Text;

using SkiaSharp;
using SharpVectors.Converters;
using SharpVectors.Dom.Svg;
using SharpVectors.Dom.Stylesheets;
using System.IO;
using System.Xml;
using SharpVectors.Dom.Css;
using SharpVectors.Dom;
using System.Linq;

namespace Builders.Builders.LevelBuilders.Types.ImageBuilders
{
    class StrokeSvgRenderer : ISvgRenderer, IDisposable
    {
        private bool disposedValue;

        public ISvgWindow Window { get; set; }
        public SvgRectF InvalidRect { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public RenderEvent OnRender { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public delegate void StrokeEvent(SKPoint[] points, int size, string color, bool erase);
        public event StrokeEvent OnStroke;

        public StrokeSvgRenderer()
        {

        }

        public ISvgRect GetRenderedBounds(ISvgElement element, float margin)
        {
            throw new NotImplementedException();
        }

        public void InvalidateRect(SvgRectF rect)
        {
            throw new NotImplementedException();
        }

        public void Render(ISvgDocument docElement)
        {
            SvgSvgElement root = docElement.RootElement as SvgSvgElement;

            if (root != null)
            {
                this.Render(root);
            }
        }

        public void Render(ISvgElement svgElement)
        {
            if (svgElement == null)
            {
                return;
            }

            string elementName = svgElement.LocalName;

            if (string.Equals(elementName, "use"))
            {
                throw new NotImplementedException();
                //RenderUseElement(svgElement);
            }
            else
            {
                RenderElement(svgElement);
            }
        }

        private void RenderPath(SvgPathElement pathElement)
        {
            const int strokesize = 2;
            const int fillsize = 1;
            var path = SKPath.ParseSvgPathData(pathElement.PathScript);

            bool shouldFill = true;
            path.Transform(SKMatrix.MakeScale(5f, 5f));

            string fillrule = pathElement.GetAttribute("fill-rule");
            path.FillType = (fillrule == "evenodd") ? SKPathFillType.EvenOdd : SKPathFillType.Winding;

            string strokecolor = GetRGB(pathElement, "stroke");

            if (strokecolor != null && strokesize > 0)
            {
                RenderPathStroke(path, strokesize, strokecolor);
            }
            
            //var bounds = path.Bounds;
            //var bounds = path.TightBounds;
            //OnStroke?.Invoke(points: new[] {
            //    new SKPoint(bounds.Top, bounds.Left),
            //    new SKPoint(bounds.Top, bounds.Right),
            //    new SKPoint(bounds.Bottom, bounds.Right),
            //    new SKPoint(bounds.Bottom, bounds.Left),
            //    new SKPoint(bounds.Top, bounds.Left),
            //}, size: 1, color: "0000ff", erase: false);
            //bounds = path.Bounds;
            //OnStroke?.Invoke(points: new[] {
            //    new SKPoint(bounds.Top, bounds.Left),
            //    new SKPoint(bounds.Top, bounds.Right),
            //    new SKPoint(bounds.Bottom, bounds.Right),
            //    new SKPoint(bounds.Bottom, bounds.Left),
            //    new SKPoint(bounds.Top, bounds.Left),
            //}, size: 1, color: "00ffff", erase: false);

            string fill = GetRGB(pathElement, "fill");

            if (shouldFill && fill != null && fillsize > 0)
            {
                RenderPathFill2(path, fillsize, fill);
            }
        }

        private void RenderPathStroke(SKPath path, int penSize, string color)
        {
            var pts = new SKPoint[4];
            var movePt = new SKPoint();
            var penPos = new SKPoint();
            using var iter = path.CreateIterator(forceClose: false);
            SKPathVerb pathVerb;
            while ((pathVerb = iter.Next(pts)) != SKPathVerb.Done)
            {
                switch (pathVerb)
                {
                    case SKPathVerb.Move:
                        Console.WriteLine("Move");
                        movePt = pts[0];
                        break;
                    case SKPathVerb.Close:
                        Console.WriteLine("Close");
                        if (movePt != penPos)
                            OnStroke?.Invoke(points: new[] { movePt, penPos }, size: penSize, color: color, erase: false);
                        break;
                    case SKPathVerb.Line:
                        Console.WriteLine("Line");
                        OnStroke?.Invoke(points: new[] { pts[0], pts[1] }, size: penSize, color: color, erase: false);
                        penPos = pts[1];
                        break;
                    case SKPathVerb.Quad:
                        Console.WriteLine("Quad");
                        var quad = FlattenQuadratic(pts[0], pts[1], pts[2]);
                        OnStroke?.Invoke(points: quad, size: penSize, color: color, erase: false);
                        penPos = quad[quad.Length - 1];
                        break;
                    case SKPathVerb.Cubic:
                        Console.WriteLine("Cubic");
                        var cubic = FlattenCubic(pts[0], pts[1], pts[2], pts[3]);
                        OnStroke?.Invoke(points: cubic, size: penSize, color: color, erase: false);
                        penPos = cubic[cubic.Length - 1];
                        break;
                    case SKPathVerb.Conic:
                        Console.WriteLine("Conic");
                        break;
                }
            }
        }

        class FillNode
        {
            public SKPoint Pos;
            public FillNode North;
            public FillNode East;
            public FillNode South;
            public FillNode West;

            public FillNode(SKPoint pos, FillNode north = null, FillNode east = null, FillNode south = null, FillNode west = null)
            {
                Pos = pos;
                North = north;
                East = east;
                South = south;
                West = west;
            }
        }

        /*
            procedure backtrack(c) is
                if reject(P, c) then return
                if accept(P, c) then output(P, c)
                s ← first(P, c)
                while s ≠ NULL do
                    backtrack(s)
                    s ← next(P, s)
         */

        private enum Direction
        {
            North, East, South, West
        }

        private void RenderPathFill2(SKPath path, int penSize, string color)
        {
            var bounds = path.Bounds;

            // TODO: consider quadtree instead of list
            var fillPts = new List<SKPoint>();
            var fillPtsHash = new HashSet<SKPoint>();

            for (int y = (int)bounds.Top; y < bounds.Bottom; y++)
                for (int x = (int)bounds.Left; x < bounds.Right; x++)
                    if (path.Contains(x, y))
                    {
                        fillPts.Add(new SKPoint(x, y));
                        fillPtsHash.Add(new SKPoint(x, y));
                    }

            var visited = new List<SKPoint>();
            visited.Add(fillPts[0]);

            var visitedHash = new HashSet<SKPoint>();
            visitedHash.Add(fillPts[0]);
            
            var stack = new Stack<SKPoint>();
            stack.Push(fillPts[0]);
            
            fillPtsHash.Remove(fillPts[0]);
            fillPts.Remove(fillPts[0]);
            
            while (stack.Count > 0)
            {
                var origin = stack.Peek();

                var north = origin + new SKPoint(0, -1);
                var east = origin + new SKPoint(1, 0);
                var south = origin + new SKPoint(0, 1);
                var west = origin + new SKPoint(-1, 0);

                bool foundNeighbour = false;

                if (fillPtsHash.Contains(north) && !visitedHash.Contains(north))
                {
                    stack.Push(north);
                    visited.Add(north);
                    visitedHash.Add(north);
                    fillPts.Remove(north);
                    fillPtsHash.Remove(north);
                    foundNeighbour = true;
                }
                if (fillPtsHash.Contains(east) && !visitedHash.Contains(east))
                {
                    stack.Push(east);
                    visited.Add(east);
                    visitedHash.Add(east);
                    fillPts.Remove(east);
                    fillPtsHash.Remove(east);
                    foundNeighbour = true;
                }
                if (fillPtsHash.Contains(west) && !visitedHash.Contains(west))
                {
                    stack.Push(west);
                    visited.Add(west);
                    visitedHash.Add(west);
                    fillPts.Remove(west);
                    fillPtsHash.Remove(west);
                    foundNeighbour = true;
                }
                if (fillPtsHash.Contains(south) && !visitedHash.Contains(south))
                {
                    stack.Push(south);
                    visited.Add(south);
                    visitedHash.Add(south);
                    fillPts.Remove(south);
                    fillPtsHash.Remove(south);
                    foundNeighbour = true;
                }
                
                
                if (!foundNeighbour)
                {
                    // BACKTRACK!
                    stack.Pop();
                }
            }
            var visitedOrderer = visited.OrderBy(p => p.Y).ThenBy(p => p.X);
            //visited.Sort((a, b) =>
            //{
            //    if (a.Y < b.Y) return -1;
            //    //if (a.X < b.X && a.Y == b.Y) return -1;
            //    if (a.Y == b.Y) return 0;
            //    return 1;
            //});
            //visited.Sort((a, b) =>
            //{
            //    if (a.X < b.X && a.Y == b.Y) return -1;
            //    if (a.X > b.X && a.Y == b.Y) return 1;
            //    return 0;
            //});
            Console.WriteLine("asdas");
        }

        private void RenderPathFill(SKPath path, int penSize, string color)
        {
            var bounds = path.Bounds;
            for (int y = (int)bounds.Top; y < bounds.Bottom; y += 1)
            {
                SKPoint? penBegin = null;
                for (int x = (int)bounds.Left; x < bounds.Right; x++)
                {
                    bool inPath = path.Contains(x, y);
                    if (penBegin == null && inPath)
                    {
                        penBegin = new SKPoint(x, y);
                    }
                    else if (penBegin != null && !inPath)
                    {
                        OnStroke?.Invoke(points: new[] { penBegin.Value, new SKPoint(x, y) }, size: penSize, color: color, erase: false);
                        penBegin = null;
                    }
                }

                if (penBegin != null)
                {
                    OnStroke?.Invoke(points: new[] { penBegin.Value, new SKPoint((int)bounds.Right, y) }, size: penSize, color: color, erase: false);
                }
            }
        }

        private string GetRGB(SvgPathElement element, string prop)
        {
            string fill2 = element.GetPropertyValue(prop);
            if (fill2 == "none")
                return null;
            var fill5 = new SvgColor(fill2);
            var rgb = fill5.RgbColor;
            int red = Convert.ToInt32(rgb.Red.GetFloatValue(CssPrimitiveType.Number));
            int green = Convert.ToInt32(rgb.Green.GetFloatValue(CssPrimitiveType.Number));
            int blue = Convert.ToInt32(rgb.Blue.GetFloatValue(CssPrimitiveType.Number));
            return (red << 16 | green << 8 | blue).ToString("x");
        }

        private IEnumerable<SKPath> SegmentPath(SKPath path)
        {
            var pm = new SKPathMeasure(path);
            float start = 0;
            SKPath tmppath;
            do
            {
                tmppath = new SKPath();
                tmppath.Rewind();
                pm.GetSegment(0, pm.Length, tmppath, true);
                tmppath.Close();
                yield return tmppath;
            } while (pm.NextContour());

            //float length = pm.Length;
            //float start = 0;
            ////float delta = 

            //while (start <= length)
            //{
            //    float end = start;// + delta;
            //    if (end > length)
            //        end = length;

            //    var segment = new SKPath();
            //    pm.GetSegment(start, end, segment, true);
            //    yield return segment;

            //    start += delta;
            //}
        }


        static SKPoint[] FlattenCubic(SKPoint pt0, SKPoint pt1, SKPoint pt2, SKPoint pt3)
        {
            int count = (int)Math.Max(1, Length(pt0, pt1) + Length(pt1, pt2) + Length(pt2, pt3));
            SKPoint[] points = new SKPoint[count];

            for (int i = 0; i < count; i++)
            {
                float t = (i + 1f) / count;
                float x = (1 - t) * (1 - t) * (1 - t) * pt0.X +
                          3 * t * (1 - t) * (1 - t) * pt1.X +
                          3 * t * t * (1 - t) * pt2.X +
                          t * t * t * pt3.X;
                float y = (1 - t) * (1 - t) * (1 - t) * pt0.Y +
                          3 * t * (1 - t) * (1 - t) * pt1.Y +
                          3 * t * t * (1 - t) * pt2.Y +
                          t * t * t * pt3.Y;
                points[i] = new SKPoint(x, y);
            }

            return points;
        }

        static SKPoint[] FlattenQuadratic(SKPoint pt0, SKPoint pt1, SKPoint pt2)
        {
            int count = (int)Math.Max(1, Length(pt0, pt1) + Length(pt1, pt2));
            var points = new List<SKPoint>();
            var prevPt = new SKPoint(float.MinValue, float.MinValue);
            for (int i = 0; i < count; i++)
            {
                float t = (i + 1f) / count;
                float x = (1 - t) * (1 - t) * pt0.X + 2 * t * (1 - t) * pt1.X + t * t * pt2.X;
                float y = (1 - t) * (1 - t) * pt0.Y + 2 * t * (1 - t) * pt1.Y + t * t * pt2.Y;
                var pt = new SKPoint((float)Math.Round(x), (float)Math.Round(y));
                if (SKPoint.Distance(prevPt, pt) >= 1f)
                    points.Add(pt);
            }

            return points.ToArray();
        }

        static SKPoint[] FlattenConic(SKPoint pt0, SKPoint pt1, SKPoint pt2, float weight)
        {
            int count = (int)Math.Max(1, Length(pt0, pt1) + Length(pt1, pt2));
            SKPoint[] points = new SKPoint[count];

            for (int i = 0; i < count; i++)
            {
                float t = (i + 1f) / count;
                float denominator = (1 - t) * (1 - t) + 2 * weight * t * (1 - t) + t * t;
                float x = (1 - t) * (1 - t) * pt0.X + 2 * weight * t * (1 - t) * pt1.X + t * t * pt2.X;
                float y = (1 - t) * (1 - t) * pt0.Y + 2 * weight * t * (1 - t) * pt1.Y + t * t * pt2.Y;
                x /= denominator;
                y /= denominator;
                points[i] = new SKPoint(x, y);
            }

            return points;
        }

        static double Length(SKPoint pt0, SKPoint pt1)
        {
            return Math.Sqrt(Math.Pow(pt1.X - pt0.X, 2) + Math.Pow(pt1.Y - pt0.Y, 2));
        }

        private void RenderElement(ISvgElement svgElement)
        {
            bool isNotRenderable = !svgElement.IsRenderable || string.Equals(svgElement.LocalName, "a");

            if (isNotRenderable)
            {
                return;
            }

            switch (svgElement.LocalName)
            {
                case "svg":
                    var svg = (SvgSvgElement)svgElement;
                    float x = (float)svg.X.AnimVal.Value;
                    float y = (float)svg.Y.AnimVal.Value;
                    float width = (float)svg.Width.AnimVal.Value;
                    float height = (float)svg.Height.AnimVal.Value;
                    break;
                case "path":
                    RenderPath((SvgPathElement)svgElement);
                    break;
                case "":
                    break;
            }

            if (/*!renderingNode.IsRecursive &&*/ svgElement.HasChildNodes)
            {
                RenderElementChildren(svgElement);
            }
        }

        private void RenderElementChildren(ISvgElement svgElement)
        {
            foreach (XmlNode node in svgElement.ChildNodes)
            {
                if (node.NodeType != XmlNodeType.Element)
                {
                    continue;
                }

                SvgElement element = node as SvgElement;
                if (element != null)
                {
                    this.Render(element);
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~MySvgRenderer()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    class SvgToArtBuilder : SvgWindow
    {
        public List<DrawArt> Result { get; private set; }
        private ImageDTO _image;

        public SvgToArtBuilder(ImageDTO imageInfo) : base(9999, 9999, new StrokeSvgRenderer())
        {
            _image = imageInfo;
            BuildResult(imageInfo.SvgStream);
        }

        private void BuildResult(Stream svgStream)
        {
            Result = new List<DrawArt>();

            var doc = new SvgDocument(this);
            doc.Load(svgStream);
            //doc.Load("C:/Users/oxyge/Desktop/shapes/scion.svg");

            var renderer = (StrokeSvgRenderer)Renderer;
            renderer.OnStroke += Renderer_OnStroke;
            renderer.Render(doc);
        }

        private void Renderer_OnStroke(SKPoint[] points, int size, string color, bool erase)
        {
            points = AbsoluteToRelative(points);

            var da = new DrawArt();
            da.Color = color;//color.ToString("x");
            da.Size = size;
            da.X = (int)points[0].X + _image.GetPaddingX();
            da.Y = (int)points[0].Y + _image.GetPaddingY();

            for (int i = 1; i < points.Length; i++)
            {
                da.Movement.Add((int)points[i].X);
                da.Movement.Add((int)points[i].Y);
            }

            Result.Add(da);
        }

        private SKPoint[] AbsoluteToRelative(SKPoint[] abs)
        {
            var rel = new SKPoint[abs.Length];
            rel[0] = abs[0];
            for (int i = 1; i < abs.Length; i++)
            {
                rel[i] = (abs[i] - abs[i - 1]);
            }
            return rel;
        }

        public override DirectoryInfo WorkingDir => throw new NotImplementedException();

        public override string Source { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void Alert(string message)
        {
            throw new NotImplementedException();
        }

        public override SvgWindow CreateOwnedWindow(long innerWidth, long innerHeight)
        {
            throw new NotImplementedException();
        }
    }
}
