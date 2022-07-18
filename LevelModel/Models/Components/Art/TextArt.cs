using System.Globalization;

namespace LevelModel.Models.Components.Art
{
    public class TextArt : Art
    {


        public int Width { get; set; }

        public int Height { get; set; }

        public string Text { get; set; }

        public bool IsText { get; set; }

        public int ImageId { get; set; }


        public TextArt(bool isText) {
            IsText = isText;

            Width  = NOT_ASSIGNED;
            Height = NOT_ASSIGNED;
        }


    }
}
