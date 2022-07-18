using System;
using System.Globalization;
using Builders.DataStructures.DTO;
using UserInterface.Handlers;
using UserInterface.Handlers.FileHandlers;
using UserInterface.DataStructures.Constants;

using static Builders.DataStructures.DTO.BuildDTO;
using static Builders.DataStructures.DTO.ImageDTO;

namespace UserInterface.Menu.Options.NewLevel.Options.BuildLevel.Options
{
    internal class BuildLevelOption : BaseMenuOption
    {

        private BuildHandler _builder;
        private BuildDTO _info;


        internal BuildLevelOption(BuildType type)
        {
            _builder  = new BuildHandler();
            _info     = new BuildDTO(type);

            GetRequiredInfo();

            if (IsInputValid)
                Build();
            else
                WriteLine(Environment.NewLine + "\tError: Invalid input!", ErrorColor);
        }


        private void GetRequiredInfo()
        {
            IsInputValid = true;

            if (IsInputValid && HasLogo(_info.Type))
                _info.ImageInfo = GetLogo();


            if (IsInputValid && _info.Type == BuildType.ShortTraps)
                _info.Difficulty = GetTrapDifficulty();

            if(IsInputValid)
                _info.Title = ReadString("Title:  ");

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Name = GetUsername();

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Token = GetToken();
        }

        private void Build()
        {
            WriteLine(Environment.NewLine + "\tBuilding level...");
            _builder.BuildLevel(_info);
        }

        private bool HasLogo(BuildType type)
        {
            return type == BuildType.ShortTraps
                || type == BuildType.Trapwork
                || type == BuildType.SmallLabyrinth
                || type == BuildType.LargeLabyrinth;
        }

        private ImageDTO GetLogo()
        {
            var imageDTO = new ImageDTO();

            imageDTO.Size          = DEFAULT_LOGO_SIZE;
            imageDTO.Image         = LoadImage(MyPaths.LOGO_PATH);
            imageDTO.Type          = ImageType.Art1;
            imageDTO.Position      = (_info.Type == BuildType.MaxBlockLimit) ? ImagePosition.Above: ImagePosition.Below;
            imageDTO.ColorToIgnore = IgnoreColor.None;

            return imageDTO;
        }

        private int GetTrapDifficulty() {
            WriteLine("Difficulty:  ");
            WriteLine("\t" + (int)TrapDifficulty.Easy   + "  -  Easy");
            WriteLine("\t" + (int)TrapDifficulty.Medium + "  -  Medium");
            WriteLine("\t" + (int)TrapDifficulty.Hard   + "  -  Hard");
            Write(Environment.NewLine + "Pick option:  ", UserInputColor);

            string input = ReadInput();
            if (int.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out int value) && Enum.IsDefined(typeof(TrapDifficulty), value)) {
                switch((TrapDifficulty) value) {
                    case TrapDifficulty.Easy:
                        return 6;
                    case TrapDifficulty.Medium:
                        return 9;
                    case TrapDifficulty.Hard:
                        return 12;
                }
            }

            IsInputValid = false;
            return (int)TrapDifficulty.Medium;
        }

    }
}
