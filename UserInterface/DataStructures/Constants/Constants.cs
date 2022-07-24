using System;
using System.Globalization;

namespace UserInterface.DataStructures.Constants
{
    public class Constants
    {

        public const string APPLICATION_VERSION = "5.2";

        public const string LINK_TO_TUTORIALS = "https://youtu.be/16HxTywHVWM";

        public const string CREATOR = "FreeRunner";

        public const string CREDITS = "Oxy & Suuper";

        public static readonly string START_DATE = new DateTime(2019, 07, 01).ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureInfo.CurrentCulture);

        public const int InfoLevelId = 6505469;

        public const string DownloadLink = "https://drive.google.com/drive/folders/1HApitzIw8qZ6ir1fOJQNZttn_Tth-fG2?usp=sharing";
    }
}
