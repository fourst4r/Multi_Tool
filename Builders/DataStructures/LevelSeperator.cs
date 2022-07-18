
namespace Builders.DataStructures
{
    internal class LevelSeperator
    {


        internal const int DEFAULT_WIDTH = 13;
        internal const int SMALL_WIDTH = 9;


        internal bool AddInEnd { get; set; }
        internal bool AddInMiddle { get; set; }
        internal bool AddInStart { get; set; }


        internal int StartDirection{ get; set; }
        internal int EndDirection { get; set; }
        internal int MiddleDirection { get; set; }

        internal int StartWidth { get; set; }
        internal int MiddleWidth { get; set; }
        internal int EndWidth { get; set; }


        internal LevelSeperator(int direction)
        {
            //default
            AddInStart  = false;
            AddInMiddle = true;
            AddInEnd    = false;

            MiddleDirection = direction;
            StartDirection  = direction;
            EndDirection    = direction;

            StartWidth  = DEFAULT_WIDTH;
            MiddleWidth = DEFAULT_WIDTH;
            EndWidth    = DEFAULT_WIDTH;
        }


    }
}
