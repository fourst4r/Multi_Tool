using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UserInterface.DataStructures.Constants;
using UserInterface.Handlers;

namespace UserInterface.Menu.Options.Settings.Options
{
    class NewUpdateOption : IOHandler
    {


        private BuildHandler _builder;

        public NewUpdateOption()
        {
            _builder = new BuildHandler();

            IsNewUpdate();
        }

        private void IsNewUpdate()
        {
            // This is an ugly trick
            // I have a map that I need to manaully update with the version number

            WriteLine("\tChecking for updates..." + Environment.NewLine);

            int levelId = Constants.InfoLevelId;
            var level = _builder.DownloadLevel(levelId);
            var version = level?.TextArt2?.FirstOrDefault()?.Text;

            if (string.IsNullOrWhiteSpace(version))
            {
                WriteLine("\tFailed check if there is an new update availabe.", ErrorColor);
                return;
            }

            var myVersion = Constants.APPLICATION_VERSION;

            if (string.Equals(version, myVersion, StringComparison.InvariantCultureIgnoreCase))
                WriteLine("\tYou have the newest version of the Multi-Tool.");
            else
            {
                WriteLine("\tYou need to update your Multi-Tool!" + Environment.NewLine, WarningColor);
                WriteLine("\tDownload newest version at:", NoteColor);
                WriteLine("\t" + Constants.DownloadLink, NoteColor);
            }
        }


    }
}
