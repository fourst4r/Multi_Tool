using System.Xml;
using System.Globalization;
using System.Collections.Generic; 
using LevelModel;
using LevelModel.Models;
using LevelModel.Models.Components;
using Converters.DataStructures;
using Converters.DataStructures.DTO;
using Converters.Converters.Components;
using Converters.Converters.Verifiers;
using System;

namespace Converters.Converters
{
    internal class TmxToLevel
    {


        public Level Result { get; set; }

        private TmxPropertiesToSettings _settings;
        private string _backgroundColor;
        private List<Block> _blocks;
        private ToLevelDTO _info;
        private bool _removeArt;


        public TmxToLevel(ToLevelDTO info)
        {
            _info = info;

            VerifyInput();
            Convert();
            SetResult();
        }


        private void Convert() {
            ConvertCostumProperties();
            ConvertAttributes();
            ConvertBlocks();
        }

        private void ConvertCostumProperties() {
            _settings = new TmxPropertiesToSettings(_info.Level, _info.Messages);

            Result         = _settings.LevelSettings;
            _removeArt     = _settings.RemoveArt;
        }

        private void ConvertAttributes()
        {
            _backgroundColor = _info.Level.SelectSingleNode("map")?.Attributes["backgroundcolor"]?.Value;

            if (_backgroundColor != null && _backgroundColor.Length > 0 && _backgroundColor[0].Equals('#'))
                _backgroundColor = _backgroundColor.Substring(1, _backgroundColor.Length - 1);
            else
                _backgroundColor = Constants.DEFAULT_BACKGROUND_COLOR;
        }

        private void ConvertBlocks()
        {
            List<TmxBlockLayer> layers = GetLayers();

            new BlockArraysToEqualSize(layers);
            VerifyReferenceLayer(layers);

            _blocks = new TmxToBlocks(layers, _settings.StartPosition, _info.Messages).Blocks;
        }

        private void VerifyReferenceLayer(List<TmxBlockLayer> layers) {
            var verifier = new ReferenceLayerVerifier(layers, _settings.RefLayerID, _info.Messages);
            _removeArt = _removeArt || !verifier.IsValid;
        }

        private List<TmxBlockLayer> GetLayers() {
            List<TmxBlockLayer> layers = new List<TmxBlockLayer>();

            foreach (XmlNode node in _info.Level.SelectSingleNode("map").ChildNodes) {
                if (node.Name.Equals("layer", StringComparison.InvariantCultureIgnoreCase))
                    layers.Add(CreateTmxBlockLayer(node, _settings.RefLayerID));
            }

            return layers;
        }

        private void SetResult()
        {
            Result.Blocks          = _blocks;
            Result.BackgroundColor = _backgroundColor;

            if (_removeArt) {
                Result.RemoveArt();
                if(_settings.IsDownloaded)
                    _info.Messages.Add("Removing art from level in order to convert successfully.");
            }
        }

        private TmxBlockLayer CreateTmxBlockLayer(XmlNode layerNode, int refLayerID)
        {
            int layerID    = ToInteger(layerNode.Attributes["id"].Value);
            var blockArray = new ChunksToBlockArray(layerNode, _info.Messages);

            return new TmxBlockLayer(layerID, blockArray.Result)
            {
                IsRefLayer  = layerID == refLayerID,
                TopPadding  = blockArray.LayerTopPadding,
                LeftPadding = blockArray.LayerLeftPadding
            };
        }

        private int ToInteger(string input)
        {
            if (int.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out int value))
                return value;

            throw new System.Exception(ErrorMessages.DEFAULT + ", failed to convert " + input + "to an integer");
        }

        private void VerifyInput() => new TmxFileVerifier(_info.Level);


    }
}
