using System;
using LevelModel.DTO;
using System.Globalization;
using LevelModel.Models.Components.Art;
using LevelModel.Models;
using Parsers.Parsers.Art;

namespace Parsers.Parsers
{
    internal class TextArtParser : BaseArtParser<TextArt>
    {


        // [Position] ; t ; [Text] ; [Color] ; [xSize] ; [ySize]


        private bool _isTextArt;
        private int _previousImageId = DEFAULT_IMAGE_ID;
        private const int DEFAULT_IMAGE_ID = 0;


        public TextArtParser(string artData, Messages messages, string type) :  base(messages, type) {
            if (artData == null || artData.Length == 0)
                return;

             SafeParse(() => Parse(artData));
        }


        private void Parse(string artData) {
            string[] art = artData.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach(var t in art) {
                var info = SplitTextArt(t);

                if(_isTextArt)
                    ParseText(info);
                else 
                    ParseImage(info);
            }
        }

        private void ParseImage(string[] info) {
            var textArt = new TextArt(false);

            textArt.X       = ParseInt(info[0]);
            textArt.Y       = ParseInt(info[1]);

            if(info.Length == 2) {
                textArt.ImageId = _previousImageId;
            }
            if(info.Length == 3) {
                textArt.ImageId = ParseImageId(info[2]);
            }
            else if(info.Length == 4) {
                textArt.ImageId = _previousImageId;
                textArt.Width   = ParseInt(info[2]);
                textArt.Height  = ParseInt(info[3]);
            }
            else if(info.Length == 5) {
                textArt.ImageId = ParseImageId(info[2]);
                textArt.Width   = ParseInt(info[3]);
                textArt.Height  = ParseInt(info[4]);
            }

            Result.Add(textArt);
        }

        private int ParseImageId(string input) {
            int id = ParseInt(input);
            _previousImageId = id;
            return id;
        }

        private void ParseText(string[] info) {
            var textArt = new TextArt(true);

            textArt.X      = ParseInt(info[0]);
            textArt.Y      = ParseInt(info[1]);
            textArt.Text   = Level.DecodeText(GetText(info));
            textArt.Color  = info[4];
            textArt.Width  = ParseInt(info[5]);
            textArt.Height = ParseInt(info[6]);

            Result.Add(textArt);
        }
       
        private string GetText(string[] info) {
            if(!info[2].Equals("t", StringComparison.InvariantCultureIgnoreCase))
                throw new InvalidDataException("Text art has invalid values");

            return info[3];
        }

        private string[] SplitTextArt(string textArt) {
            var textInfo = textArt.Split(';');

             _isTextArt = (textInfo.Length == 7) ? true : false;

             return textInfo;
        }


    }
}
