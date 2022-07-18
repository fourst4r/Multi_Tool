using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using static UserInterface.DataStructures.Constants.MyPaths;


namespace UserInterface.Handlers.FileHandlers
{
    class TxtFileHandler : IOHandler
    {

        internal static string Read(string filename)
        {

            if (string.IsNullOrWhiteSpace(filename))
                return string.Empty;

            string path = null;
            try
            {
                path = Path.IsPathRooted(filename) ? filename : Path.Combine(USER_LEVEL_FOLDER, filename);
                return File.ReadAllText(path);
            }
            catch (FileNotFoundException) { WriteLine("File not found: " + (path ?? filename), ErrorColor); }
            catch (Exception ex) { ShowExceptionToUser(ex); }


            return string.Empty;
        }


        internal static bool Save(string filepath, string text)
        {
            if(string.IsNullOrWhiteSpace(text))
                return false;

            if (string.IsNullOrWhiteSpace(filepath))
            {
                WriteLine("Error: Invalid filepath", ErrorColor);
                return false;
            }

            try
            {
                var path = Path.IsPathRooted(filepath) ? filepath : Path.Combine(USER_LEVEL_FOLDER, filepath);
                File.WriteAllText(path, text);
                return true;
            }
            catch (Exception ex) { ShowExceptionToUser(ex); }

            return false;
        }
    }
}
