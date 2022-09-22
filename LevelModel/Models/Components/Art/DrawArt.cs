using System.Text;
using System.Collections.Generic;

namespace LevelModel.Models.Components.Art
{
    public class DrawArt : Art
    {


        public int Size { get; set; }

        public bool IsErase { get; set; }

        public List<int> Movement { get; set; }


        public const string ERASE = "merase";
        public const string DRAW  = "mdraw";


        public DrawArt() {
            Movement = new List<int>();

            Size = NOT_ASSIGNED;
            X    = NOT_ASSIGNED;
            Y    = NOT_ASSIGNED;
        }

        public DrawArt Clone()
        {
            return new DrawArt
            {
                Size = Size,
                IsErase = IsErase,
                Movement = Movement,
                X = X,
                Y = Y,
                Color = Color,
            };
        }

    }

}
