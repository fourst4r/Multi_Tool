using System;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using Builders.DataStructures.DTO;
using UserInterface.Handlers;
using UserInterface.Handlers.FileHandlers;
using UserInterface.DataStructures.Info;
using UserInterface.DataStructures.Constants;
using LevelModel.Models;
using LevelModel.Models.Components;

using static LevelModel.Models.Level;

namespace UserInterface.Menu.Options.ExistingLevel.Options.ModifyLevel.Options
{

    class AddArtBlocksOption : BaseMenuOption
    {

        private BuildHandler _builder;
        private AddArtBlocksInfo _info;


        internal AddArtBlocksOption()
        {
            _builder = new BuildHandler();
            _info    = new AddArtBlocksInfo();

            GetRequiredInfo();

            if (IsInputValid)
                _builder.AddArtBlocks(_info);
            else
                WriteLine(Environment.NewLine + "\tError: Invalid input!", ErrorColor);
        }


        private void ShowArtOptions()
        {
            WriteLine("Art Types:");
            WriteLine("\t" + MenuOptions.Add_ART_00 + "  -  Art 00");
            WriteLine("\t" + MenuOptions.Add_ART_0  + "  -  Art 0");
            WriteLine("\t" + MenuOptions.Add_ART_1  + "  -  Art 1");
            WriteLine("\t" + MenuOptions.Add_ART_2  + "  -  Art 2");
            WriteLine("\t" + MenuOptions.Add_ART_3  + "  -  Art 3");

            Write("\nPick Option:  ", UserInputColor);
        }

        private void HandleArtOption(string option)
        {
            switch (option.ToLower(CultureInfo.InvariantCulture))
            {
                case MenuOptions.Add_ART_00:
                    _info.DTO.ArtType = ArtType.TextArt00;
                    break;

                case MenuOptions.Add_ART_0:
                    _info.DTO.ArtType = ArtType.TextArt0;
                    break;

                case MenuOptions.Add_ART_1:
                    _info.DTO.ArtType = ArtType.TextArt1;
                    break;

                case MenuOptions.Add_ART_2:
                    _info.DTO.ArtType = ArtType.TextArt2;
                    break;

                case MenuOptions.Add_ART_3:
                    _info.DTO.ArtType = ArtType.TextArt3;
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

        private int GetFakeArtId()
        {
            WriteLine("Note: Art ID, can be any block ID." + Environment.NewLine, NoteColor);

            var id = ReadInteger("Art ID:  ");

            if (Block.IsValidBlock(id))
                WriteLine("\tWarning: Art ID is not a valid block ID." + Environment.NewLine, WarningColor);

            return id;
        }

        private int ReadSize(string msg)
        {
            int size = ReadInteger(msg, 0);

            if(size == ERROR)
                return ERROR;

            double rounded = Math.Round(size / 0.3);

            if(rounded < int.MaxValue)
                return (int) rounded;
            
            IsInputValid = false;
            return ERROR;
        }
        
        private void GetRequiredInfo()
        {
            IsInputValid = true;

            if (IsInputValid)
                _info.LevelID = ReadInteger("Level ID:  ", 0);

            if (IsInputValid)
                ShowBlockIdPath(0);

            if (IsInputValid)
                _info.DTO.BlockId = ReadInteger("Block ID:  ", Block.BASIC_BROWN, Block.MaxBlockId);

            if (IsInputValid)
                _info.DTO.ArtBlockId = GetFakeArtId();

            WriteLine("Note: The size of a block is 30." + Environment.NewLine,  NoteColor);

            if (IsInputValid)
                _info.DTO.Width = ReadSize("Art Width:  ");

            if (IsInputValid)
                _info.DTO.Height = ReadSize("Art Height:  ");

            if (IsInputValid)
                SetArtType();

            if (IsInputValid)
                _info.Title = ReadString("Title:  ");

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Name = GetUsername();

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Token = GetToken();
        }
      
    }
}
