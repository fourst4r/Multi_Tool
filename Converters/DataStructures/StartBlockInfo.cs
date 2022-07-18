using System;
using System.Collections.Generic;
using System.Text;

namespace Converters.DataStructures
{
    internal class StartBlockInfo
    {

        public int X { get; set; }

        public int Y { get; set; }

        public int BlockID { get; set; }


        public StartBlockInfo(int x, int y, int id) {
            X = x;
            Y = y;
            BlockID     = id;
        }
    }
}
