using System;
using System.Globalization;
using System.Collections.Generic;
using UserInterface.Handlers;
using UserInterface.Handlers.FileHandlers;
using UserInterface.DataStructures.Info;
using LevelModel.Models.Components;

using static Builders.DataStructures.DTO.TransformBlockDTO;

namespace UserInterface.Menu.Options.ExistingLevel.Options.ModifyLevel.Options
{
    internal class ChangeBlocksOption : BaseMenuOption
    {

        private BuildHandler _builder;
        private TransformInfo _info;


        internal ChangeBlocksOption()
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


        private void Init() {
            _info.Type = TransformType.Blocks;
        }

        private void GetRequiredInfo()
        {
            IsInputValid = true;

            _info.LevelID = ReadInteger("Level ID:  ", 0);

            if (IsInputValid)
                ShowBlockIdPath(0);

            if (IsInputValid)
                _info.BlockToTransform = GetBlockIDs("Blocks to transform, block ID:  ");

            if (IsInputValid)
                _info.BlockToTransformTo = GetBlockIDs("Blocks to transform to, block ID:  ");

            if (IsInputValid)
                VerifyBlockIDs();

            if (IsInputValid)
                _info.Title = ReadString("Title:  ");

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Name = GetUsername();

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Token = GetToken();
        }

        private void VerifyBlockIDs()
        {
            if (_info.BlockToTransform.Count == 0 || _info.BlockToTransformTo.Count == 0)
                IsInputValid = false;

            if (_info.BlockToTransform.Count != _info.BlockToTransformTo.Count)
                IsInputValid = false;
        }

        private void Transform()
        {
            WriteLine(Environment.NewLine + "\tChanging blocks...");
            _builder.TransformLevel(_info);
        }

        private List<int> GetBlockIDs(string s)
        {
            Write(s, UserInputColor);
            List<int> list = new List<int>();

            ParseBlockIDs(ReadInput(), list);

            return list;
        }

        private void ParseBlockIDs(string input, List<int> list)
        {
            try
            {
                foreach (var id in input.Split(","))
                    AddID(list, id);
            }
            catch {
                WriteLine("Error: Invalid format on input.", ErrorColor);
                IsInputValid = false;
            }
        }

        private void AddID(List<int> list, string id)
        {
            if (int.TryParse(id, NumberStyles.Any, CultureInfo.InvariantCulture, out int value))
            {
                if (Block.IsValidBlock(value) && !Block.IsStartBlock(value))
                    list.Add(value);
                else
                    IsInputValid = false;
            }
            else
                IsInputValid = false;
        }

    }
}
