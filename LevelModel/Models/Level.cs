using System.Text;
using System.Collections.Generic;
using LevelModel.Models.Components;
using LevelModel.Models.Components.Art;
using System.Linq;

namespace LevelModel.Models
{
    public class Level : BaseLevel
    {


        public double UserID { get; set; }

        public string Credits { get; set; }

        public int CowboyChance { get; set; }

        public double Time { get; set; }

        public Song Song { get; set; }

        public double Gravity { get; set; }

        public double MaxTime { get; set; }

        public bool HasPassword { get; set; }

        public List<Item> Items { get; set; }

        public List<int> BadHats { get; set; }


        public string DataVersion { get; set; }

        public string BackgroundColor { get; set; }

        public List<Block> Blocks { get; set; }

        public List<TextArt> TextArt1 { get; set; }

        public List<TextArt> TextArt2 { get; set; }

        public List<TextArt> TextArt3 { get; set; }

        public List<DrawArt> DrawArt1 { get; set; }

        public List<DrawArt> DrawArt2 { get; set; }

        public List<DrawArt> DrawArt3 { get; set; }

        public string BackgroundImage { get; set; }

        public List<TextArt> TextArt0 { get; set; }

        public List<TextArt> TextArt00 { get; set; }

        public List<DrawArt> DrawArt0 { get; set; }

        public List<DrawArt> DrawArt00 { get; set; }

        public string Hash { get; set; }


        public const int INVALID = -1;

        public const int MAX_BLOCK_LIMIT = 50_000;

        private static readonly Dictionary<string, string> _specialTextSymbols = new Dictionary<string, string>()
        {
            { "#", "#35" },
            { "+", "#43" }, 
            { "&", "#38" },
            { ",", "#44" },
            { "-", "#45" },
            { ";", "#59" },
            { "`", "#96" },
        };

        public enum ArtType 
        { 
            TextArt00, TextArt0, TextArt1, TextArt2, TextArt3,
            DrawArt00, DrawArt0, DrawArt1, DrawArt2, DrawArt3,
            All 
        };


        public Level() {
            Song      = new Song(Song.DEFAULT);
            Items     = new List<Item>();
            Blocks    = new List<Block>();
            BadHats   = new List<int>();

            TextArt00 = new List<TextArt>();
            TextArt0  = new List<TextArt>();
            TextArt1  = new List<TextArt>();
            TextArt2  = new List<TextArt>();
            TextArt3  = new List<TextArt>();

            DrawArt00 = new List<DrawArt>();
            DrawArt0  = new List<DrawArt>();
            DrawArt1  = new List<DrawArt>();
            DrawArt2  = new List<DrawArt>();
            DrawArt3  = new List<DrawArt>();
        }


        public void RemoveArt() {
            RemoveTextArt();
            RemoveDrawArt();
        }

        public void RemoveTextArt()
        {
            TextArt00.Clear();
            TextArt0.Clear();
            TextArt1.Clear();
            TextArt2.Clear();
            TextArt3.Clear();
        }

        public void RemoveDrawArt()
        {
            DrawArt00.Clear();
            DrawArt0.Clear();
            DrawArt1.Clear();
            DrawArt2.Clear();
            DrawArt3.Clear();
        }

        public string GetData() {
            StringBuilder data = new StringBuilder();

            data.Append(DataVersion ?? Constants.DATA_VERSION);
            data.Append("`");
            data.Append(BackgroundColor ?? Constants.DEFAULT_BACKGROUND_COLOR);
            data.Append("`");
            data.Append(Blocks.ToPr2String());
            data.Append("`");
            data.Append(TextArt1.ToPr2String());
            data.Append("`");
            data.Append(TextArt2.ToPr2String());
            data.Append("`");
            data.Append(TextArt3.ToPr2String());
            data.Append("`");
            data.Append(DrawArt1.ToPr2String());
            data.Append("`");
            data.Append(DrawArt2.ToPr2String());
            data.Append("`");
            data.Append(DrawArt3.ToPr2String());
            data.Append("`");
            data.Append(BackgroundImage ?? Constants.NO_BACKGROUND_IMAGE);
            data.Append("`");
            data.Append(TextArt0.ToPr2String());
            data.Append("`");
            data.Append(TextArt00.ToPr2String());
            data.Append("`");
            data.Append(DrawArt0.ToPr2String());
            data.Append("`");
            data.Append(DrawArt00.ToPr2String());

            return data.ToString();
        }

        public static string EncodeText(string text) {
            var myReturn = new StringBuilder(text);

            foreach(var symbol in _specialTextSymbols)
                myReturn.Replace(symbol.Key, symbol.Value);

            return myReturn.ToString();
        }

        public static string DecodeText(string text) {
            var myReturn = new StringBuilder(text);

            foreach(var symbol in _specialTextSymbols)
                myReturn.Replace(symbol.Value, symbol.Key);

            return myReturn.ToString();
        }


    }
}
