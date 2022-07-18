using System;
using System.Globalization;
using System.Xml;
using Converters.Converters.Verifiers;
using Converters.DataStructures;
using LevelModel.DTO;
using LevelModel.Models.Components;

namespace Converters.Converters.Components
{
    internal class ChunksToBlockArray
    {


        public int[,] Result { get; set; }

        public int LayerLeftPadding { get; set; }
        public int LayerTopPadding { get; set; }

        private int _topLimit;
        private int _downLimit;
        private int _rightLimit;
        private int _leftLimit;

        private XmlNode _layerNode;

        private readonly Messages _messages;
        private bool _hasWarnedAboutUnknownBlockId = false;


        public ChunksToBlockArray(XmlNode layerNode, Messages messages)
        {
            _layerNode = layerNode;
            _messages  = messages;
            Init();
            Convert();
        }


        private void Init()
        {
            foreach (XmlNode node in _layerNode.SelectSingleNode("data").ChildNodes)
            {
                if (node.Name.Equals("chunk", StringComparison.InvariantCultureIgnoreCase))
                {
                    TmxFileVerifier.Chunk(node);
                    CalculateDimensions(node);
                }
            }
        }

        private void CalculateDimensions(XmlNode node)
        {
            int width    = ToInteger(node.Attributes["width"].Value);
            int xPadding = ToInteger(node.Attributes["x"].Value);
            int height   = ToInteger(node.Attributes["height"].Value);
            int yPadding = ToInteger(node.Attributes["y"].Value);

            LayerLeftPadding = (xPadding < LayerLeftPadding) ? xPadding : LayerLeftPadding;
            LayerTopPadding  = (yPadding < LayerTopPadding)  ? yPadding : LayerTopPadding;

            _topLimit   = (_topLimit   < yPadding + height) ? yPadding + height : _topLimit;
            _downLimit  = (_downLimit  > yPadding)          ? yPadding          : _downLimit;
            _rightLimit = (_rightLimit < xPadding + width)  ? xPadding + width  : _rightLimit;
            _leftLimit  = (_leftLimit  > xPadding)          ? xPadding          : _leftLimit;
        }


        private void Convert()
        {
            Result = new int[_topLimit - _downLimit, _rightLimit - _leftLimit];
             
            foreach (XmlNode node in _layerNode.SelectSingleNode("data").ChildNodes)
            {
                if (node.Name.Equals("chunk", StringComparison.InvariantCultureIgnoreCase))
                    AppendChunk(node);
            }
        }

        private void AppendChunk(XmlNode node)
        {
            var chunk = new Chunk()
            {
                YPadding  = ToInteger(node.Attributes["y"].Value),
                XPadding  = ToInteger(node.Attributes["x"].Value),
                Height    = ToInteger(node.Attributes["height"].Value),
                Width     = ToInteger(node.Attributes["width"].Value),
                BlockData = node.InnerText.Replace("\r\n", String.Empty)
            };

            AppendBlockData(chunk);
        }

        private static int ToInteger(string input)
        {
            if (int.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out int value))
                return value;

            throw new InvalidDataException(ErrorMessages.DEFAULT + ", failed to convert " + input + "to an integer");
        }

        private void AppendBlockData(Chunk chunk)
        {
            string[] blockIDs = Split(chunk.BlockData, ',');
            int i = 0;

            for (int y = 0; y < chunk.Height; y++)
            {
                for (int x = 0; x < chunk.Width; x++, i++)
                {
                    Result[y + chunk.YPadding - _downLimit, x + chunk.XPadding - _leftLimit] = ToBlockID(blockIDs[i]);
                }
            }
        }

        private bool IsValidBlock(long value)
        {
            if(value > int.MaxValue)
                return false;

            if(value < int.MinValue)
                return false;

            try
            {
                int intValue = unchecked((int)value);

                return Block.IsValidBlock(intValue + TmxBlocks.BLOCK_ID_ADJUSTER);
            }
            catch
            {
                return false;
            }
        }

        private int ToBlockID(string input)
        {
            if (long.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
            {
                // you can rotate a block in 4 different directions plus the mirror reversed way
                // which gives 4*2=8 different rotations
                // one of them are the normal one

                long[] rotations = {
                    0, // normal one
                    1610612736,
                    2147483648,
                    536870912,
                    3221225472,
                    2684354560,
                    1073741824,
                    3758096384
                };

                if(value == TmxBlocks.NO_BLOCK)
                    return TmxBlocks.NO_BLOCK;

                foreach (var rotation in rotations)
                {
                    var rotated = value - rotation;

                    if(IsValidBlock(rotated))
                        return (int)rotated;
                }
            }

            var fallback = Block.GOAL;

            if(_hasWarnedAboutUnknownBlockId == false)
            {
                _hasWarnedAboutUnknownBlockId = true;
                _messages.Add($"Unknown blocks will be converted to the {Block.GetBlockName(fallback)} block.", Message.MessageType.Warning);
            }

            return fallback - Block.BLOCK_ID_ADJUSTER + 1;
        }

        private string[] Split(string data, char separator)
        {
            if (data != null && data.Length > 0)
            {
                return data.Split(separator);
            }

            return new string[] { };
        }


    }
}
