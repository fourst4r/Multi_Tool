using System;
using System.Collections.Generic;
using System.Text;

namespace Converters.DataStructures
{
    internal class Chunk
    {

        public int XPadding { get; set; }
        public int YPadding { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string BlockData { get; set; }

    }
}
