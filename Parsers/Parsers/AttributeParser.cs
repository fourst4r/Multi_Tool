using System;
using System.Collections.Generic;
using System.Globalization;

namespace Parsers.Parsers
{
    internal abstract class AttributeParser : BaseParser
    {


        protected Dictionary<string, Action<string>> attributeParser;

        internal AttributeParser()
        {
            attributeParser = new Dictionary<string, Action<string>>();
        }


        protected abstract void ParseAttribute(string name, string value);


        private void Parse(string response)
        {
            string[] attributes = response.Split('&');

            foreach(var attribute in attributes)
            {
                (string name, string value) = GetAttributeInfo(attribute);
                ParseAttribute(name, value);
            }
        }

        protected void ParseAttributes(string input, bool removeHash)
        {
            if (string.IsNullOrWhiteSpace(input))
                return;

            if(removeHash)
                input = RemoveHash(input);

            Parse(input);
        }

        private Tuple<string, string> GetAttributeInfo(string attribute) {
            string[] pair = attribute?.Split(new[] { '=' }, 2);

            if(pair == null || pair.Length < 2)
                return Tuple.Create(string.Empty, string.Empty);

            return Tuple.Create(pair[0], pair[1]);
        }

        private string RemoveHash(string data) {
            int hashLength = 32;

            if(data == null || data.Length < hashLength) 
                return string.Empty;

            var hash = data.Substring(data.Length - hashLength);

            if (!IsHexValue(hash)) 
                return data;

            return data.Substring(0, data.Length - 32);
        }

        private bool IsHexValue(string text)
        {
            try
            {
                return System.Text.RegularExpressions.Regex.IsMatch(text, @"\A\b[0-9a-fA-F]+\b\Z");
            }
            catch
            {
                return false;
            }
        }


    }
}
