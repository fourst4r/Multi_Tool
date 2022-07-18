using System;
using System.Globalization;
using System.IO;
using LevelModel.Models;
using SkiaSharp;
using UserInterface.Handlers.FileHandlers;

using static Builders.DataStructures.DTO.ImageDTO;
using static UserInterface.DataStructures.Constants.MyPaths;

namespace UserInterface.Menu.Options
{
    internal class BaseMenuOption : IOHandler
    {


        protected const int ERROR = int.MinValue;

        protected bool IsInputValid { get; set; }


        protected string ReadString(string message) {
            Write(message, UserInputColor);
            string input = ReadInput();

            if (!string.IsNullOrWhiteSpace(input))
                return input;

            IsInputValid = false;
            return string.Empty;
        }

        protected string GetUsername(bool useCurrent = true) {
            if (useCurrent && UserSettingsHandler.CurrentUser.Name != null)
                return UserSettingsHandler.CurrentUser.Name;

            Write("Username:  ", UserInputColor);
            string username = ReadInput();

            if (username.Length > 1)
                return username;

            IsInputValid = false;
            return string.Empty;
        }

        protected string GetToken(bool useCurrent = true) {
            if (useCurrent && UserSettingsHandler.CurrentUser.Token != null)
                return UserSettingsHandler.CurrentUser.Token;

            Write("Token:  ", UserInputColor);
            string token = ReadInput();

            if (token.Length > 0)
                return token;

            IsInputValid = false;
            return string.Empty;
        }

        protected int ReadInteger(string message, int? min = null, int? max = null) {
            Write(message, UserInputColor);
            string input = ReadInput();

            if (int.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out int value) && ValueBetween(value, min, max))
                return value;

            IsInputValid = false;
            return ERROR;
        }

        private bool ValueBetween(int value, int? min, int? max) {
            if (min != null && value < min)
                return false;

            if (max != null && value > max)
                return false;

            return true;
        }

        protected SKBitmap LoadImage(string filename) {
            if (filename == null || filename.Length == 0)
                return null;

            try {
                string path = Path.IsPathRooted(filename) ? filename : Path.Combine(USER_IMAGE_FOLDER, filename);
                using (var stream = new SKManagedStream(File.OpenRead(path))) {
                    return SKBitmap.Decode(stream);
                }
            }
            catch (FileNotFoundException) { WriteLine(Environment.NewLine + "\tError: Image file not found.", ErrorColor); }

            return null;
        }

        protected IgnoreColor GetIgnoreColor() {
            WriteLine("Ignore pixel color:  ");
            WriteLine("\t" + (int)IgnoreColor.Black + "  -  Black");
            WriteLine("\t" + (int)IgnoreColor.White + "  -  White");
            WriteLine("\t" + (int)IgnoreColor.None + "  -  None");
            WriteLine();

            int value = ReadInteger("Pick option:  ");

            if (value != ERROR && Enum.IsDefined(typeof(IgnoreColor), value))
                return (IgnoreColor)value;

            IsInputValid = false;
            return IgnoreColor.None;
        }

        protected int GetImageSize(ImageType type) {
            int maxSize = (type == ImageType.Art0 || type == ImageType.Art1) ? MAX_SIZE_ART : MAX_SIZE_BLOCKS;
            WriteLine("\tSize of image (1 - " + maxSize + ")");
            WriteLine();

            int value = ReadInteger("Input value:  ", 1, maxSize);

            if (value != ERROR)
                return value;

            IsInputValid = false;
            return 0;
        }

        protected ColorSensitivty GetColorSensitivity()
        {
            WriteLine("Color Sensitivity:  ");
            WriteLine("\t" + (int)ColorSensitivty.VeryLow  + "  -  Very Low");
            WriteLine("\t" + (int)ColorSensitivty.Low      + "  -  Low");
            WriteLine("\t" + (int)ColorSensitivty.Medium   + "  -  Medium");
            WriteLine("\t" + (int)ColorSensitivty.High     + "  -  High");
            WriteLine("\t" + (int)ColorSensitivty.VeryHigh + "  -  Very High");
            WriteLine();
            
            int value = ReadInteger("Pick option:  ");

            if (value != ERROR && Enum.IsDefined(typeof(ColorSensitivty), value))
                return (ColorSensitivty)value;

            IsInputValid = false;
            return ColorSensitivty.Medium;
        }

        protected bool GetOptimizedImage()
        {
            var yes = 1;
            var no = 2;

            WriteLine("Optimize the image drawing:  ");
            WriteLine("\t" + yes + "  -  Yes");
            WriteLine("\t" + no  + "  -  No");
            WriteLine();

            int value = ReadInteger("Pick option:  ");

            if (value != ERROR)
            {
                if (value == yes)
                    return true;

                if(value == no)
                    return false;
            }

            IsInputValid = false;
            return false;
        }

        protected SKBitmap GetImage() {
            Write("Filename of image:  ", UserInputColor);
            string path = ReadInput();

            if (path != null && path.Length != 0) {
                var image = LoadImage(path);

                if (image != null)
                    return image;
            }

            IsInputValid = false;
            return null;
        }

        protected void ShowBlockIdPath(int tabs)
        {
            string msg = "";

            for (int i = 0; i < tabs; i++)
                msg += "\t";

            msg = "Note: To see all block IDs, go to" + Environment.NewLine;

            for (int i = 0; i < tabs; i++)
                msg += "\t";

            msg += "      Main Menu --> Settings --> Block IDs" + Environment.NewLine;

            WriteLine(msg, NoteColor);
        }

    }
}
