using System;
using System.IO;
using System.Reflection;

namespace UserInterface.DataStructures.Constants
{
    internal static class MyPaths
    {


        // Names
        internal static readonly string TILESET_NAME      = "Blocks.tsx";
        internal static readonly string BLOCK_FOLDER_NAME = "Blocks";

        // Resources Folders
        internal static readonly string CURRENT_FOLDER     = GetCurrentFolder();
        internal static readonly string RESOURCE_FOLDER    = Path.Combine(CURRENT_FOLDER,  "Resources");
        internal static readonly string PICTURE_FOLDER     = Path.Combine(RESOURCE_FOLDER, "Pictures");
        internal static readonly string TEXT_FILES_FOLDER  = Path.Combine(RESOURCE_FOLDER, "TextFiles");

        // Resources Paths
        internal static readonly string LOGO_PATH          = Path.Combine(PICTURE_FOLDER,    "Logo.jpg");
        internal static readonly string USER_SETTINGS_PATH = Path.Combine(TEXT_FILES_FOLDER, "UserSettings.txt");

        // User Folders
        internal static readonly string USER_FOLDER             = Path.Combine(CURRENT_FOLDER, "User");
        internal static readonly string USER_LEVEL_FOLDER       = Path.Combine(USER_FOLDER,    "Levels");
        internal static readonly string USER_IMAGE_FOLDER       = Path.Combine(USER_FOLDER,    "Images");
        internal static readonly string USER_BLOCK_FOLDER       = Path.Combine(USER_LEVEL_FOLDER, BLOCK_FOLDER_NAME);
        internal static readonly string USER_BLOCK_IMAGE_FOLDER = Path.Combine(USER_LEVEL_FOLDER, BLOCK_FOLDER_NAME);

        // User Paths
        internal static readonly string USER_TILESET_PATH       = Path.Combine(USER_BLOCK_FOLDER, TILESET_NAME);


        private static string GetCurrentFolder()
        {
            // Use file outside build folder, so its reusable under debug mode
            if (System.Diagnostics.Debugger.IsAttached)
                return Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;

            return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }

        internal static void CreateOutputFolders()
        {
            if (!Directory.Exists(USER_FOLDER))
                Directory.CreateDirectory(USER_FOLDER);

            if (!Directory.Exists(USER_IMAGE_FOLDER))
                Directory.CreateDirectory(USER_IMAGE_FOLDER);

            if (!Directory.Exists(USER_LEVEL_FOLDER))
                Directory.CreateDirectory(USER_LEVEL_FOLDER);

            if (!Directory.Exists(USER_BLOCK_FOLDER))
                Directory.CreateDirectory(USER_BLOCK_FOLDER);

            if (!Directory.Exists(USER_BLOCK_IMAGE_FOLDER))
                Directory.CreateDirectory(USER_BLOCK_IMAGE_FOLDER);
        }


    }
}
