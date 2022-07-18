using System;
using System.Text;
using System.Globalization;

namespace Converters.DataStructures
{
    internal class TmxBlockLayer
    {


        public int LayerID { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public int[,] BlockArray { get; set; }

        public bool IsRefLayer { get; set; }

        public int LeftPadding { get; set; }
        public int TopPadding { get; set; }


        internal TmxBlockLayer(int layerID, int[,] blockarray)
        {
            LayerID    = layerID;
            BlockArray = blockarray;
            Height     = blockarray.GetLength(0);
            Width      = blockarray.GetLength(1);
        }


        public override string ToString() {
            var result = new StringBuilder(Environment.NewLine);

            for (int y = 0; y < Height; y++) {
                for (int x = 0; x < Width; x++) {
                    result.Append(BlockArray[y, x].ToString(CultureInfo.InvariantCulture));
                    result.Append(",");
                }

                if (y != (Height - 1))
                    result.Append(Environment.NewLine);
            }

            return TrimEndOfBlocks(result);
        }

        private string TrimEndOfBlocks(StringBuilder blocks) {
            if(blocks.Length > 0)
                blocks.Length--;

            blocks.Append(Environment.NewLine);
            return blocks.ToString();
        }


    }
}
