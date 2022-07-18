using System;
using LevelModel.Models.Components;
using UserInterface.Handlers;
using UserInterface.DataStructures.Info;

using static Builders.DataStructures.DTO.TransformBlockDTO;
using UserInterface.Handlers.FileHandlers;

namespace UserInterface.Menu.Options.ExistingLevel.Options.ModifyLevel.Options
{
    class ReverseTrapsOption : BaseMenuOption
    {

        private BuildHandler _builder;
        private TransformInfo _info;

        internal ReverseTrapsOption()
        {
            _builder = new BuildHandler();
            _info = new TransformInfo();

            Init();
            GetRequiredInfo();

            if (IsInputValid)
                Transform();
            else
                WriteLine(Environment.NewLine + "\tError: Invalid input!", ErrorColor);
        }


        private void Init()
        {
            _info.Type = TransformType.ReverseTraps;
            SetReverseBlockIDs();
        }

        private void GetRequiredInfo()
        {
            IsInputValid = true;

            _info.LevelID = ReadInteger("Level ID:  ", 0);

            if (IsInputValid)
                _info.Title = ReadString("Title:  ");

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Name = GetUsername();

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Token = GetToken();
        }

        private void Transform()
        {
            WriteLine(Environment.NewLine + "\tReversing traps...");
            _builder.TransformLevel(_info);
        }

        private void SetReverseBlockIDs()
        {
            _info.BlockToTransform.Add(Block.ARROW_LEFT);
            _info.BlockToTransform.Add(Block.ARROW_RIGHT);

            _info.BlockToTransformTo.Add(Block.ARROW_RIGHT);
            _info.BlockToTransformTo.Add(Block.ARROW_LEFT);
        }

    }
}
