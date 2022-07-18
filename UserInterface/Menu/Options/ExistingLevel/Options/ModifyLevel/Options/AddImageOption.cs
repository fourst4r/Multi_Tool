using System;
using LevelModel.Models;
using Builders.DataStructures.DTO;
using UserInterface.Handlers;
using UserInterface.DataStructures.Info;
using UserInterface.Handlers.FileHandlers;

using static Builders.DataStructures.DTO.ExtendDTO;
using static Builders.DataStructures.DTO.ImageDTO;
using System.Globalization;

namespace UserInterface.Menu.Options.ExistingLevel.Options.ModifyLevel.Options
{
    class AddImageOption : BaseMenuOption
    {

        private BuildHandler _builder;
        private ExtendInfo _info;


        internal AddImageOption()
        {
            _builder = new BuildHandler();
            _info = new ExtendInfo();

            Init();
            GetRequiredInfo();

            if (IsInputValid)
                Extend();
            else
                WriteLine(Environment.NewLine + "\tError: Invalid input!", ErrorColor);
        }


        private void GetRequiredInfo()
        {
            IsInputValid = true;

            _info.LevelID = ReadInteger("Level ID:  ", 0);

            if (IsInputValid)
                _info.ArtInfo.ImageInfo.Type = GetImageType();

            if (IsInputValid) 
                _info.Type = _info.ArtInfo.ImageInfo.Type == ImageType.Art0 ? ExtendType.Art0 : ExtendType.Art1;

            if (IsInputValid)
                _info.ArtInfo.ImageInfo.Image = GetImage();

            if (IsInputValid)
                _info.ArtInfo.ImageInfo.CreateDrawImage = GetOptimizedImage();

            if (IsInputValid && _info.ArtInfo.ImageInfo.CreateDrawImage)
                _info.ArtInfo.ImageInfo.Sensitivty = GetColorSensitivity();

            if (IsInputValid)
                _info.ArtInfo.ImageInfo.Size = GetImageSize(ImageType.Art1);

            if (IsInputValid)
                _info.ArtInfo.ImageInfo.ColorToIgnore = GetIgnoreColor();

            if (IsInputValid)
                _info.SetPaddingX(ReadInteger("Pixel padding, X-axis:  "));

            if (IsInputValid)
                _info.SetPaddingY(ReadInteger("Pixel padding, Y-axis:  "));

            if (IsInputValid)
                _info.Title = ReadString("Title:  ");

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Name = GetUsername();

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Token = GetToken();
        }

        private ImageType GetImageType()
        {
            WriteLine("Image type to generate: ");
            WriteLine("\t" + (int)ImageType.Art0 + "  -  Art0");
            WriteLine("\t" + (int)ImageType.Art1 + "  -  Art1");
            Write(Environment.NewLine + "Pick option:  ", UserInputColor);

            string input = ReadInput();


            if (int.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out int value) && Enum.IsDefined(typeof(ImageType), value) && value != (int)ImageType.Blocks)
                return (ImageType)value;

            IsInputValid = false;
            return ImageType.None;
        }

        private void Init()
        {
            _info.ArtInfo.Type = BuildDTO.BuildType.Simple;
            _info.ArtInfo.Title = "temp";
            _info.ArtInfo.ImageInfo.Position = ImageDTO.ImagePosition.Custom;
        }

        private void Extend()
        {
            WriteLine(Environment.NewLine + "\tAdding image...");

            GetArtData();

            if (IsInputValid)
                _builder.ExtendLevel(_info);
        }

        private void GetArtData()
        {
            Level artLevel = _builder.CreateLevel(_info.ArtInfo);

            if (artLevel != null)
               _info.ArtToAdd = _info.ArtInfo.ImageInfo.Type == ImageType.Art0 ? artLevel.DrawArt0 : artLevel.DrawArt1;
           else
               IsInputValid = false;
        }

    }
}
