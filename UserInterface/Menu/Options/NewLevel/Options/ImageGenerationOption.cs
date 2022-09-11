using System;
using System.Globalization;
using Builders.DataStructures.DTO;
using UserInterface.Handlers;
using UserInterface.Handlers.FileHandlers;
using LevelModel.Models.Components;

using static Builders.DataStructures.DTO.BuildDTO;
using static Builders.DataStructures.DTO.ImageDTO;
using SkiaSharp;

namespace UserInterface.Menu.Options.NewLevel.Options
{
    internal class ImageGenerationOption : BaseMenuOption
    {

        private BuildHandler _builder;
        private BuildDTO _info;


        internal ImageGenerationOption()
        {
            _builder = new BuildHandler();
            _info    = new BuildDTO();

            GetRequiredInfo();
            
            //var paint = new SKPaint();
            //paint.GetFillPath()
            //paint.Color = SKColor.Parse("663300");
            //var bmp = new SKBitmap(500, 500);
            //bmp.Erase(SKColors.White, SKRectI.Create(500, 500));
            //var canvas = new SKCanvas(bmp);
            //canvas.DrawPath(path, paint);
            //var wstream = SKFileWStream.OpenStream("test.png");
            //bmp.Encode(wstream, SKEncodedImageFormat.Png, 100);
            //wstream.Dispose();

            //do
            //{
            //    var segment = new SKPath();
            //    pm.GetSegment(0, pm.Length, segment, true);
            //    bool fill = paint.GetFillPath(segment) != null;
            //    Console.WriteLine("fill: " + Convert.ToString(fill));
            //} while (pm.NextContour());
            //path.Transform(SKMatrix.MakeScale(15, 15));
            _info.ImageInfo.SvgStream = GetSvgStream();
            
            if (IsInputValid)
                BuildLevel();
            else
                WriteLine(Environment.NewLine + "\tError: Invalid input!", ErrorColor);
        }


        private void GetRequiredInfo()
        {
            IsInputValid = true;

            _info.Type = BuildType.Image;

            if (IsInputValid)
                _info.ImageInfo.Image = GetImage();

            if(IsInputValid)
                _info.ImageInfo.CreateDrawImage = GetOptimizedImage();

            if (IsInputValid && _info.ImageInfo.CreateDrawImage)
                _info.ImageInfo.Sensitivty = GetColorSensitivity();

            if (IsInputValid)
                _info.ImageInfo.Type = GetImageType();

            if (IsInputValid)
                _info.ImageInfo.Size = GetImageSize(_info.ImageInfo.Type);

            if (IsInputValid)
                _info.ImageInfo.ColorToIgnore = GetIgnoreColor();

            if (IsInputValid)
                _info.ImageInfo.Position = ImagePosition.Above;

            if (IsInputValid && _info.ImageInfo.Type == ImageType.Blocks)
                _info.ImageInfo.BlockType = GetBlockType();

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
            WriteLine("\t" + (int)ImageType.Art0    + "  -  Art0");
            WriteLine("\t" + (int)ImageType.Art1    + "  -  Art1");
            WriteLine("\t" + (int)ImageType.Blocks  + "  -  Blocks");
            Write(Environment.NewLine + "Pick option:  ", UserInputColor);

            string input = ReadInput();


            if (int.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out int value) && Enum.IsDefined(typeof(ImageType), value))
                return (ImageType)value;

            IsInputValid = false;
            return ImageType.None;
        }

        private int GetBlockType()
        {
            ShowBlockIdPath(0);
            Write("Block ID:  ", UserInputColor);
            string input = ReadInput();

            if (input.Equals(String.Empty, StringComparison.InvariantCultureIgnoreCase))
            {
                WriteLine(Environment.NewLine + "Using deafult block, white basic.");
                return Block.BASIC_WHITE;
            }


            if (int.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out int value) && Block.IsValidBlock(value) && !Block.IsStartBlock(value))
                return value;

            IsInputValid = false;
            return Block.BASIC_WHITE;
        }

        private void BuildLevel()
        {
            WriteLine(Environment.NewLine + "\tGenerating image...");

            _builder.BuildLevel(_info);
        }

    }
}
