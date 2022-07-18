using System;
using UserInterface.Handlers;
using UserInterface.DataStructures.Info;
using UserInterface.Handlers.FileHandlers;

namespace UserInterface.Menu.Options.ExistingLevel.Options
{
    internal class CopyLevelOption : BaseMenuOption
    {


        private BuildHandler _builder;
        private CopyInfo _info;


        internal CopyLevelOption()
        {
            _builder = new BuildHandler();
            _info    = new CopyInfo();

            GetRequiredInfo();

            if(IsInputValid)
                CopyLevel();
            else
                WriteLine(Environment.NewLine + "\tError: Invalid input!", ErrorColor);
        }


        private void GetRequiredInfo()
        {
            IsInputValid = true;

            _info.LevelID = ReadInteger("Level ID:  ", 0);

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Name = GetUsername();

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Token = GetToken();

            if (IsInputValid)
                _info.Title = ReadString("Title:  ");
        }

        private void CopyLevel()
        {
            WriteLine(Environment.NewLine + "\tCopying level...");
            _builder.CopyLevel(_info);
        }


    }
}
