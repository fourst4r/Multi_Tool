using LevelModel.Models;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace Parsers.Parsers
{
    internal abstract class BaseParser
    {


        protected const int NOT_FOUND = -1;
        protected const int ERROR     = -1;
        protected const bool UNKNOWN  = false;


        protected double ParseDouble(string s) {
            if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
                return value;

            return ERROR;
        }

        protected int ParseInt(string s) {
            if (int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out int value))
                return value;

            return ERROR;
        }

        protected bool ParseBool(string s) {
            if (int.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out int value))
                return value == 1;

            if (s.Equals("true", StringComparison.InvariantCultureIgnoreCase))
                return true;

            if (s.Equals("false", StringComparison.InvariantCultureIgnoreCase))
                return false;

            return UNKNOWN;
        }

        protected static string ParsePr2Text(string text) {
            if (text == null || text.Length == 0)
                return string.Empty;

            return System.Web.HttpUtility.UrlDecode(text);
        }


    }
}
