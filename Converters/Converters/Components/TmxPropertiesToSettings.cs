using System.Xml;
using System.Globalization;
using Parsers;
using LevelModel;
using LevelModel.Models;
using LevelModel.Models.Components;
using LevelModel.DTO;
using Converters.Converters.Components.ReferenceBlock;

using static LevelModel.DTO.Message;
using System;

namespace Converters.Converters.Components
{
    internal class TmxPropertiesToSettings
    {


        internal Level LevelSettings { get; private set; }
        internal Point StartPosition { get; private set; }

        internal int RefLayerID { get; private set; }

        public bool RemoveArt { get; set; }

        public bool IsDownloaded{ get => _isADownloadedLevel; }


        private XmlDocument _document;
        private bool _settingsFound;
        private bool _startPositionFound;
        private bool _refLayerIdFound;
        private bool _isADownloadedLevel;

        private Messages _messages;


        internal TmxPropertiesToSettings(XmlDocument document, Messages messages)
        {
            _document = document;
            _messages = messages;

            Convert();
            AddMessages();
        }


        private void Convert()
        {
            XmlNode properties = _document.SelectSingleNode("map").SelectSingleNode("properties");

            if (properties != null)
                ParseProperties(properties);

            if (!_settingsFound)
                DefaultSettings();

            if (!_startPositionFound)
                DefaultStartPosition();

            if (!_refLayerIdFound)
                DefaultRefLayerId();
        }

        private void AddMessages() {
            if (_isADownloadedLevel == false) {
                _messages.Add("PR2 level settings not found, creating default settings...", MessageType.Normal);
                return;
            }

            if (_settingsFound == false) {
                _messages.Add("Can't find the map property: Level Settings");
                _messages.Add("Creating default setting of the level in order to convert successfully.");
            }


            if (_startPositionFound == false) 
                _messages.Add("Can't find the map property: Start Position");

            if (_refLayerIdFound == false) 
                _messages.Add("Can't find the map property: Reference Layer ID");
        }

        private void ParseProperties(XmlNode properties)
        {
            foreach (XmlNode property in properties.ChildNodes)
            {
                if (property.Attributes["name"].Value.Equals("settings", StringComparison.InvariantCultureIgnoreCase))
                    ParseLevel(property.Attributes["value"].Value);

                if (property.Attributes["name"].Value.Equals("startposition", StringComparison.InvariantCultureIgnoreCase))
                    ParseStartPosition(property.Attributes["value"].Value);

                if (property.Attributes["name"].Value.Equals("referencelayerid", StringComparison.InvariantCultureIgnoreCase))
                    ParseRefLaterID(property.Attributes["value"].Value);
            }
        }

        private void ParseRefLaterID(string input) {
            _isADownloadedLevel = true;
            bool valid = int.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out int value);

            if (valid) {
                RefLayerID = value;
                _refLayerIdFound = true;
            }
        }

        private void ParseStartPosition(string input) {
            try {
                _isADownloadedLevel = true;
                string[] positions = input.Split(',');
                StartPosition = new Point(int.Parse(positions[0], NumberStyles.Any, CultureInfo.InvariantCulture), int.Parse(positions[1], NumberStyles.Any, CultureInfo.InvariantCulture));
                _startPositionFound = true;
            }
            catch { } //handled in the calling method
        }

        private void ParseLevel(string levelData)
        {
            _isADownloadedLevel = true;
            if (levelData != null)
            {
                var result    = PR2Parser.Level(levelData);
                LevelSettings = result?.Level;
                _messages.Add(result?.Messages);

                if (LevelSettings != null)
                    _settingsFound = true;
            }
        }


        private void DefaultRefLayerId() {
            RefLayerID = AddReferenceBlock.INVALID_LAYER_ID;
            RemoveArt = true;
        }

        private void DefaultSettings() {
            LevelSettings = new Level();
            LevelSettings.Gravity = Constants.DEFAULT_GRAVITY;
            LevelSettings.Version = 1;
        }

        private void DefaultStartPosition() {
            StartPosition = Block.START_POSITION;
            RemoveArt = true;
        }


    }
}
