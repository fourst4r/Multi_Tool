using System.Xml;
using System.Globalization;
using Converters.DataStructures;
using Converters.DataStructures.DTO;
using Converters.Converters.Components;
using Converters.Converters.Components.ReferenceBlock;
using LevelModel.Models.Components;
using static LevelModel.DTO.Message;
using System.Linq;

namespace Converters.Converters
{
    internal class LevelToTmx
    {


        public XmlDocument Result { get; private set; }

        private ToTmxDTO _info;
        private BlocksToTmx _blockConverter;

        private const string TILED_VERSION = "1.2.4";
        private const string ENCODING_TYPE = "csv";

        private int _width;
        private int _height;
        private int _referenceLayerID;
        private Point _startPosition;
        private int _nextLayerID;


        internal LevelToTmx(ToTmxDTO info)
        {
            _info = info;

            Convert();
        }


        private void Convert() {
            if(_info.Level.Blocks.Any(b => !string.IsNullOrWhiteSpace(b.Options)))
                _info.Messages.Add("Removing block options...", MessageType.Warning);

            HandleUnknownBlocks();
            ConvertBlocks();
            BuildResult();
        }

        private void HandleUnknownBlocks()
        {
            bool handled = false;
            foreach (var b in _info.Level.Blocks)
            {
                if(Block.IsValidBlock(b.Id))
                    continue;

                handled = true;
                b.Id = Block.GOAL;
            }

            if(handled)
                _info.Messages.Add("Unknown block converted to goal block...", MessageType.Warning);
        }

        private void ConvertBlocks()
        {
            _blockConverter = new BlocksToTmx(_info.Level.Blocks);

            _height = _blockConverter.MaxHeight;
            _width  = _blockConverter.MaxWidth;
            _startPosition = _blockConverter.RelativeStartPosition;
            _nextLayerID   = _blockConverter.Layers.Count + 1;
        }

        private void BuildResult()
        {
            Result = new XmlDocument();

            Result.AppendChild(GetDeclaration());
            Result.AppendChild(GetMap());
        }

        private XmlNode GetDeclaration()
        {
            return Result.CreateXmlDeclaration("1.0", "UTF-8", null);
        }

        private XmlElement GetMap()
        {
            XmlElement map = Result.CreateElement("map");

            map.SetAttribute("version"         , _info.Level.Version.ToString(CultureInfo.InvariantCulture));
            map.SetAttribute("tiledversion"    , TILED_VERSION);
            map.SetAttribute("orientation"     , "orthogonal");
            map.SetAttribute("renderorder"     , "right-down");
            map.SetAttribute("width"           , _width.ToString(CultureInfo.InvariantCulture));
            map.SetAttribute("height"          , _height.ToString(CultureInfo.InvariantCulture));
            map.SetAttribute("tilewidth"       , "30");
            map.SetAttribute("tileheight"      , "30");
            map.SetAttribute("infinite"        , "1");
            map.SetAttribute("backgroundcolor" , "#" + _info.Level.BackgroundColor.PadLeft(6, '0'));
            map.SetAttribute("nextlayerid"     , _nextLayerID.ToString(CultureInfo.InvariantCulture));
            map.SetAttribute("nextobjectid"    , "1");

            AddTileset(map);
            AddBlockLayers(map);
            AddProperties(map);

            return map;
        }

        private void AddProperties(XmlElement map)
        {
            XmlElement properties = Result.CreateElement("properties");

            AddProperty(properties, "settings", _info.LevelData);
            AddProperty(properties, "StartPosition", _startPosition.X + "," + _startPosition.Y);

            if(_referenceLayerID == AddReferenceBlock.LAYER_ID)
                AddProperty(properties, "ReferenceLayerID", _referenceLayerID.ToString(CultureInfo.InvariantCulture));

            map.AppendChild(properties);
        }

        private void AddProperty(XmlElement properties, string name, string value) {
            XmlElement property = Result.CreateElement("property");

            property.SetAttribute("name", name);
            property.SetAttribute("value", value);

            properties.AppendChild(property);
        }

        private void AddTileset(XmlElement map) => map.AppendChild(GetTileset());

        private void AddBlockLayers(XmlElement map)
        {
            AddReferenceLayer();

            foreach (var layer in _blockConverter.Layers)
                map.AppendChild(GetLayer(layer));
        }

        private void AddReferenceLayer() {
            var refLayer = new AddReferenceBlock(_blockConverter.Layers);
            _referenceLayerID = (refLayer.BlockAdded) ? AddReferenceBlock.LAYER_ID : AddReferenceBlock.INVALID_LAYER_ID;
        }


        private XmlElement GetTileset()
        {
            XmlElement tileset = Result.CreateElement("tileset");

            tileset.SetAttribute("firstgid", "1");
            tileset.SetAttribute("source", _info.TilesetPath);

            return tileset;
        }

        private XmlElement GetLayer(TmxBlockLayer blockLayer)
        {
            XmlElement layer = Result.CreateElement("layer");

            layer.SetAttribute("id"     , blockLayer.LayerID.ToString(CultureInfo.InvariantCulture));
            layer.SetAttribute("name"   , (blockLayer.IsRefLayer) ? "Reference Layer" : "Tile Layer " + blockLayer.LayerID);
            layer.SetAttribute("width"  , _width.ToString(CultureInfo.InvariantCulture));
            layer.SetAttribute("height" , _height.ToString(CultureInfo.InvariantCulture));
            layer.SetAttribute("opacity", (blockLayer.IsRefLayer) ? "0" : (1.0 / blockLayer.LayerID).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture));
            layer.SetAttribute("visible", (blockLayer.IsRefLayer) ? "0" : "1");

            layer.AppendChild(GetData(blockLayer));
            return layer;
        }

        private XmlElement GetData(TmxBlockLayer blockLayer)
        {
            XmlElement data = Result.CreateElement("data");

            data.SetAttribute("encoding", ENCODING_TYPE);

            data.AppendChild(GetChunk(blockLayer));
            return data;
        }

        private XmlElement GetChunk(TmxBlockLayer blockLayer)
        {
            XmlElement chunk = Result.CreateElement("chunk");

            chunk.SetAttribute("x", "0");
            chunk.SetAttribute("y", "0");
            chunk.SetAttribute("width", _width.ToString(CultureInfo.InvariantCulture));
            chunk.SetAttribute("height", _height.ToString(CultureInfo.InvariantCulture));

            chunk.InnerXml = blockLayer.ToString();
            return chunk;
        }


    }
}
