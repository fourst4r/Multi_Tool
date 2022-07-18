using System.Xml;
using System.Globalization;
using LevelModel.DTO;
using Converters.DataStructures;
using System;

namespace Converters.Converters.Verifiers
{

    // This class could be replace with an XML Schema .xsd
    internal class TmxFileVerifier
    { 


        private XmlDocument _input;


        public TmxFileVerifier(XmlDocument input)
        {
            _input = input;

            if (_input == null)
                throw new InvalidDataException("Invalid parameters, input is null");

            VerifyInput();
        }


        public static void Chunk(XmlNode chunk)
        {
            AttributeExistance(chunk, "x");
            AttributeExistance(chunk, "y");
            AttributeExistance(chunk, "width");
            AttributeExistance(chunk, "height");

            if (chunk.InnerText == null)
                throw new InvalidDataException(ErrorMessages.DEFAULT + ",  'chunk' nodes must have block data");
        }


        private void VerifyInput()
        {
            var map   = NodeExistance(_input, "map");
            var layer = NodeExistance(map, "layer");
            var data  = NodeExistance(layer, "data");

            MapAttributes(map);
            LayerAttributes(layer);
            DataAttributes(data);
        }

        private static XmlNode NodeExistance(XmlNode input, string nodeToCheck)
        {
            var node = input.SelectSingleNode(nodeToCheck);
            if (node == null)
                throw new InvalidDataException(ErrorMessages.DEFAULT + ", it has no '" + nodeToCheck + "' node");

            return node;
        }

        private static XmlAttribute AttributeExistance(XmlNode input, string attributeToCheck)
        {
            var attribute = input.Attributes[attributeToCheck];

            if (attribute == null)
                throw new InvalidDataException(ErrorMessages.DEFAULT + ", the node '" + input.Name + "' is missing the attribute '" + attributeToCheck+ "'");

            if(attribute.Value == null)
                throw new InvalidDataException(ErrorMessages.DEFAULT + ", the node '" + input.Name + "' is missing a value on the the attribute '" + attributeToCheck + "'");

            return attribute;
        }

        private void MapAttributes(XmlNode map)
        {
            if (!AttributeExistance(map, "orientation").Value.Equals("orthogonal", StringComparison.InvariantCultureIgnoreCase))
                throw new InvalidDataException(ErrorMessages.DEFAULT + ", the 'Orientation' property must be Orthogonal");

            if (!AttributeExistance(map, "renderorder").Value.Equals("right-down", StringComparison.InvariantCultureIgnoreCase))
                throw new InvalidDataException(ErrorMessages.DEFAULT + ", the 'Render Order' property must be Right-Down");

            if (!AttributeExistance(map, "infinite").Value.Equals("1", StringComparison.InvariantCultureIgnoreCase))
                throw new InvalidDataException(ErrorMessages.DEFAULT + ", the 'Infinite' property must be True");
        }

        private void LayerAttributes(XmlNode layer)
        {
            if (AttributeExistance(layer, "id").Value.Length == 0)
                throw new InvalidDataException(ErrorMessages.DEFAULT + ", the layer ID is missing");
        }

        private void DataAttributes(XmlNode data)
        {
            DataEncoding(data);
        }

        private void DataEncoding(XmlNode data) {
            try {
                if (AttributeExistance(data, "encoding").Value.Equals("csv", StringComparison.InvariantCultureIgnoreCase))
                    return;
            }
            catch {}
                throw new InvalidDataException(ErrorMessages.DEFAULT + ", the 'Layer Format' property must be CSV");
        }


    }
}
