namespace LevelModel.Models.Components.Art
{
    public abstract class Art
    {

        public int X { get; set; }

        public int Y { get; set; }

        public string Color { get; set; }

        public const int NOT_ASSIGNED = int.MinValue;

    }
}
