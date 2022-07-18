using System;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using UserInterface.Handlers;
using UserInterface.DataStructures.Info;
using UserInterface.Handlers.FileHandlers;
using UserInterface.DataStructures.Constants;

using static LevelModel.Models.Level;

namespace UserInterface.Menu.Options.ExistingLevel.Options.ModifyLevel.Options
{
    class MoveArtOption : BaseMenuOption
    {

        private BuildHandler _builder;
        private MoveArtInfo _info;
        private bool _quit;

        internal MoveArtOption()
        {
            _builder = new BuildHandler();
            _info    = new MoveArtInfo();
            _quit    = false;

            GetRequiredInfo();

            if (IsInputValid)
                _builder.MoveArt(_info);
            else if(!_quit)
                WriteLine(Environment.NewLine + "\tError: Invalid input!", ErrorColor);
        }

        private void ShowArtOptions()
        {
            WriteLine("Art Types:");
            WriteLine("\t" + MenuOptions.MOVE_ALL_ART     .PadRight(2) + "  -  All Art");
            WriteLine("\t" + MenuOptions.MOVE_TEXT_ART_00 .PadRight(2) + "  -  Text Art 00");
            WriteLine("\t" + MenuOptions.MOVE_TEXT_ART_0  .PadRight(2) + "  -  Text Art 0");
            WriteLine("\t" + MenuOptions.MOVE_TEXT_ART_1  .PadRight(2) + "  -  Text Art 1");
            WriteLine("\t" + MenuOptions.MOVE_TEXT_ART_2  .PadRight(2) + "  -  Text Art 2");
            WriteLine("\t" + MenuOptions.MOVE_TEXT_ART_3  .PadRight(2) + "  -  Text Art 3");
            WriteLine("\t" + MenuOptions.MOVE_DRAW_ART_00 .PadRight(2) + "  -  Draw Art 00");
            WriteLine("\t" + MenuOptions.MOVE_DRAW_ART_0  .PadRight(2) + "  -  Draw Art 0");
            WriteLine("\t" + MenuOptions.MOVE_DRAW_ART_1  .PadRight(2) + "  -  Draw Art 1");
            WriteLine("\t" + MenuOptions.MOVE_DRAW_ART_2  .PadRight(2) + "  -  Draw Art 2");
            WriteLine("\t" + MenuOptions.MOVE_DRAW_ART_3  .PadRight(2) + "  -  Draw Art 3");

            WriteLine();
            WriteLine("\t" + MenuOptions.QUIT + "  -  Quit/Back");

            Write(Environment.NewLine + "Pick Option:  ", UserInputColor);
        }


        private void HandleArtOption(string option)
        {
            switch (option.ToLower(CultureInfo.InvariantCulture))
            {
                case MenuOptions.MOVE_ALL_ART:
                    _info.DTO.ArtType = ArtType.All;
                    break;

                case MenuOptions.MOVE_TEXT_ART_00:
                    _info.DTO.ArtType = ArtType.TextArt00;
                    break;

                case MenuOptions.MOVE_TEXT_ART_0:
                    _info.DTO.ArtType = ArtType.TextArt0;
                    break;

                case MenuOptions.MOVE_TEXT_ART_1:
                    _info.DTO.ArtType = ArtType.TextArt1;
                    break;

                case MenuOptions.MOVE_TEXT_ART_2:
                    _info.DTO.ArtType = ArtType.TextArt2;
                    break;

                case MenuOptions.MOVE_TEXT_ART_3:
                    _info.DTO.ArtType = ArtType.TextArt3;
                    break;

                case MenuOptions.MOVE_DRAW_ART_00:
                    _info.DTO.ArtType = ArtType.DrawArt00;
                    break;

                case MenuOptions.MOVE_DRAW_ART_0:
                    _info.DTO.ArtType = ArtType.DrawArt0;
                    break;

                case MenuOptions.MOVE_DRAW_ART_1:
                    _info.DTO.ArtType = ArtType.DrawArt1;
                    break;

                case MenuOptions.MOVE_DRAW_ART_2:
                    _info.DTO.ArtType = ArtType.DrawArt2;
                    break;

                case MenuOptions.MOVE_DRAW_ART_3:
                    _info.DTO.ArtType = ArtType.DrawArt3;
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
                WriteLine("Note: Size of a block is 30." + Environment.NewLine, NoteColor);

            if (IsInputValid)
                _info.DTO.X = ReadInteger("Distance, X-axis:  ");

            if (IsInputValid)
                _info.DTO.Y = ReadInteger("Distance, Y-axis:  ");

            if (IsInputValid)
                _info.Title = ReadString("Title:  ");

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Name = GetUsername();

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Token = GetToken();
        }
        
    }
}
