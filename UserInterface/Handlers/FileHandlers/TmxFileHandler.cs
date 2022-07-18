using System;
using System.IO;
using System.Xml;

using static UserInterface.DataStructures.Constants.MyPaths;

namespace UserInterface.Handlers.FileHandlers
{
    internal class TmxFileHandler : IOHandler
    {


        internal static XmlDocument Read(string filename)
        {
            if(filename == null || filename.Length == 0)
                return null;
            try
            {
                var document = new XmlDocument();
                string path = Path.IsPathRooted(filename) ? filename : Path.Combine(USER_LEVEL_FOLDER, filename);
                document.Load(filename);

                return document;
            }
            catch (Exception ex) { ShowExceptionToUser(ex); }

            return null;
        }


        internal static void Save(string path, XmlDocument tmx)
        {
            try
            {
                tmx.Save(path);
            }
            catch (Exception ex) { ShowExceptionToUser(ex); }
        }


    }
}
