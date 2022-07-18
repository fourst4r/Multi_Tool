namespace Builders.DataStructures
{
    internal class TrapworkInfo
    {


        internal const int STRAIGHT = 0;
        internal const int UPHILL = 1;
        internal const int DOWNHILL = -1;

        internal const int MAX_DIFFICULTY = 25;
        internal const int MIN_DIFFICULTY = 1;


        internal int Width { get;  private set; }

        internal int Height { get; private set; }

        internal int NumberOfLevels { get; private set; }

        internal LevelSeperator LevelSeperator { get; set; }

        internal int Direction { get; set; }

        internal int LevelLength { get; set; }

        internal int Difficulty { get; set; }


        internal TrapworkInfo(int width, int height, int numberOfLevels)
        {
            Height = height;
            NumberOfLevels = numberOfLevels;
            LevelLength = width / numberOfLevels;

            Width = LevelLength * NumberOfLevels;  //make sure each level is equally long
        }

        internal TrapworkInfo(int width, int height)
        {
            Width = width;
            Height = height;
        }


    }
}
