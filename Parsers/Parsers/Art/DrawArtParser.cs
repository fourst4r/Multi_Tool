using System;
using System.Globalization;
using System.Collections.Generic;
using LevelModel.DTO;
using LevelModel.Models.Components.Art;
using Parsers.Parsers.Art;

using static LevelModel.Models.Components.Art.DrawArt;

namespace Parsers.Parsers
{

    internal class DrawArtParser : BaseArtParser<DrawArt>
    {


        // Stroke = [Color] , [Size] , [Position]
        // Color starts with a 'c' then short RGB
        // Size  starts with a 't' then an int
        // Position = [Start_X] ; [Start_Y] ; [Movement]


        private DrawArt  _currentStroke;

        protected Dictionary<int, Action<string>> strokeParser;

        private const int COLOR_INDEX    = 0;
        private const int SIZE_INDEX     = 1;
        private const int POSITION_INDEX = 2;
        private const int ERASE_INDEX    = 3;


        public DrawArtParser(string artData, Messages messages, string type) : base(messages, type) {
            if (artData == null || artData.Length == 0)
                return;

            Init();
            SafeParse(() => Parse(artData));
        }


        private void Init() {
            InitCurrentStroke();
            BuildParser();
        }

        private void InitCurrentStroke() {
            _currentStroke = new DrawArt()
            {
                IsErase = false,
                Size    = 4,
                Color   = "0"
            };
        }

        private void BuildParser() {
            strokeParser = new Dictionary<int, Action<string>>();

            strokeParser.Add(COLOR_INDEX, ParseColor);
            strokeParser.Add(SIZE_INDEX, ParseSize);
            strokeParser.Add(POSITION_INDEX, ParsePosition);
            strokeParser.Add(ERASE_INDEX, ParseErase);
        }


        private void Parse(string artData) {
            string[] strokes = artData.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach(var value in strokes) {
                if (strokeParser.TryGetValue(GetParseIndex(value), out Action<string> action)) {
                    action?.Invoke(value);
                }
            }
        }

        private void StrokeCompleted() {
            Result.Add(_currentStroke);

            _currentStroke = new DrawArt()
            {
                IsErase = _currentStroke.IsErase,
                Size    = _currentStroke.Size,
                Color   = _currentStroke.Color
            };
        }

        private void ParseColor(string s) {
            _currentStroke.Color = s.Substring(1, s.Length - 1);
        }

        private void ParseSize(string s) {
            _currentStroke.Size = ParseInt(s.Substring(1, s.Length - 1));
        }

        private void ParsePosition(string s) {
            var values = s.Substring(1, s.Length - 1).Split(';');

            ParsePosition(values);
            ParseMovement(values);

            StrokeCompleted();
        }

        private void ParseErase(string s) {
            if (s.Equals(ERASE, StringComparison.InvariantCultureIgnoreCase))
                _currentStroke.IsErase = true;
            else
                _currentStroke.IsErase = false;
        }

        private void ParsePosition(string[] values) {
            if(values.Length < 2)
                throw new InvalidDataException("The level has invalid art stroke position");

            _currentStroke.X = ParseInt(values[0]);
            _currentStroke.Y = ParseInt(values[1]);
        }

        private void ParseMovement(string[] values) {
            for (int i = 2; i < values.Length; i++)
                _currentStroke.Movement.Add(ParseInt(values[i]));
        }

        private int GetParseIndex(string value) {
            if (value.StartsWith("c", StringComparison.InvariantCultureIgnoreCase))
                return COLOR_INDEX;

            if (value.StartsWith("t", StringComparison.InvariantCultureIgnoreCase))
                return SIZE_INDEX;

            if (value.StartsWith("d", StringComparison.InvariantCultureIgnoreCase))
                return POSITION_INDEX;

            if (value.Equals(ERASE, StringComparison.InvariantCultureIgnoreCase))
                return ERASE_INDEX;

            if (value.Equals(DRAW, StringComparison.InvariantCultureIgnoreCase))
                return ERASE_INDEX;

            throw new InvalidDataException("Invalid art value, value = " + value);
        }


    }

}
