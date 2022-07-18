using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UserInterface.DataStructures.Constants;
using UserInterface.DataStructures.Info;
using UserInterface.Handlers;
using UserInterface.Handlers.FileHandlers;

namespace UserInterface.Menu.Options.ExistingLevel.Options.ModifyLevel.Options
{
    class ChangeArtTypeOption : BaseMenuOption
    {
        private BuildHandler _builder;
        private ChangeArtTypeInfo _info;
        private bool _quit;


        internal ChangeArtTypeOption()
        {
            _builder = new BuildHandler();
            _info    = new ChangeArtTypeInfo();

            GetRequiredInfo();

            if (IsInputValid)
                ChangeArt();
            else if (!_quit)
                WriteLine(Environment.NewLine + "\tError: Invalid input!", ErrorColor);
        }

        private void ShowArtOptions()
        {
            WriteLine("Note: This only apply to draw art." + Environment.NewLine, NoteColor);

            WriteLine("\t" + MenuOptions.Art0ToArt1 + "  -  Art0 to Art1");
            WriteLine("\t" + MenuOptions.Art1ToArt0 + "  -  Art1 to Art0");
            WriteLine();
            WriteLine("\t" + MenuOptions.QUIT + "  -  Quit/Back");

            Write(Environment.NewLine + "Pick Option:  ", UserInputColor);
        }


        private void HandleArtOption(string option)
        {
            switch (option.ToLower(CultureInfo.InvariantCulture))
            {
                case MenuOptions.Art0ToArt1:
                    _info.Art0ToArt1 = true;
                    break;

                case MenuOptions.Art1ToArt0:
                    _info.Art0ToArt1 = false;
                    break;

                case MenuOptions.QUIT:
                    IsInputValid = false;
                    _quit = true;
                    break;

                default:
                    IsInputValid = false;
                    break;
            }
        }

        private void SetArtType()
        {
            ShowArtOptions();
            HandleArtOption(ReadInput());
        }

        private void GetRequiredInfo()
        {
            IsInputValid = true;

            if (IsInputValid)
                SetArtType();

            if (IsInputValid)
                _info.LevelID = ReadInteger("Level ID:  ", 0);

            if (IsInputValid)
                _info.Title = ReadString("Title:  ");

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Name = GetUsername();

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Token = GetToken();
        }

        private void ChangeArt()
        {
            _builder.ChangeArtType(_info);
        }

    }
}
