using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace LevelModel.Models.Components
{
    public static class BlockExtensions
    {


        public static (int min, int max) GetMinAndMax(this IEnumerable<int> values)
        {
            if (values == null)
                return (0, 0);

            int current = values.First();
            int min     = current;
            int max     = current;

            foreach (var value in values.Skip(1))
            {
                current += value;

                if (current < min)
                    min = current;

                if (current > max)
                    max = current;
            }

            return (min, max);
        }

        public static void Add(this List<Block> blocks, int x, int y, int id, string options = "") {
            var block = new Block(x, y, id, options);
            blocks.Add(block);
        }

        public static List<Block> Remove(this List<Block> blocks, Predicate<Block> match) {
            List<Block> myReturn = new List<Block>();
            var padding = Point.Empty;

            foreach (var b in blocks) {
                bool isMatch = match(b);

                BlockPadding(b, isMatch, ref padding);

                if (isMatch == false)
                    myReturn.Add(b);
            }

            return myReturn;
        }

        public static void SetStartPosition(this List<Block> blocks, int x, int? y = null) {
            if (blocks.Count > 0) {
                blocks[0].X = x;

                if (y != null)
                    blocks[0].Y = (int)y;
            }
        }

        public static Point GetPosition(this List<Block> blocks, int index) {
            if (blocks.Count > index) {
                var block = blocks[index];
                return new Point(block.X, block.Y);
            }

            throw new ArgumentOutOfRangeException("The index is out of range for the block list.");
        }

        public static string ToPr2String(this List<Block> blocks) {
            StringBuilder sb = new StringBuilder();
            int lastId = -1;

            foreach (var b in blocks) {
                sb.Append(b.X + ";" + b.Y);
                sb.Append(GetID(b.Id, ref lastId));

                if(!string.IsNullOrWhiteSpace(b.Options))
                    sb.Append(";" + b.Options);

                sb.Append(',');
            }

            return sb.TrimEnd();
        }


        private static string GetID(int id, ref int lastId) {
            string myReturn = string.Empty;

            if (lastId != id || id == Block.TELEPORT) {
                myReturn = ";" + id;
                lastId = id;
            }

            return myReturn;
        }


        private static void BlockPadding(Block b, bool isMatch, ref Point padding) {
            b.X += padding.X;
            b.Y += padding.Y;

            padding = isMatch ? new Point(b.X, b.Y) : Point.Empty;
        }

    }
}
