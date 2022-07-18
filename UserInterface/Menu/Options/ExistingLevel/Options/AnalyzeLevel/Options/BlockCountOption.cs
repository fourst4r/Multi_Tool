using LevelModel.Models;
using LevelModel.Models.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserInterface.Handlers;
using LevelModel.Models.Components;
using UserInterface.Handlers.FileHandlers;

namespace UserInterface.Menu.Options.ExistingLevel.Options.AnalyzeLevel.Options
{
    class BlockCountOption : BaseMenuOption
    {

        private int _levelID;
        private Level _level;
        private BuildHandler _builder;
        private const int PadSize = 16;
        internal BlockCountOption()
        {
            _builder = new BuildHandler();
            GetRequiredInfo();

            if (IsInputValid)
                ShowInfo();
            else
                WriteLine(Environment.NewLine + "\tError: Invalid input!", ErrorColor);
        }

        private void GetRequiredInfo()
        {
            IsInputValid = true;

            _levelID = ReadInteger("Level ID:  ", 0);

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Name = GetUsername();

            if (IsInputValid)
                UserSettingsHandler.CurrentUser.Token = GetToken();
        }

        private Dictionary<int, int> CountBlocks()
        {
            var blockCount = new Dictionary<int, int>();

            if (_level == null)
                return blockCount;

            foreach (var b in _level.Blocks)
            {
                if(blockCount.TryGetValue(b.Id, out var current))
                    blockCount[b.Id] = ++current;
                else
                    blockCount.Add(b.Id, 1);
            }

            return blockCount;
        }

        private List<KeyValuePair<int, int>> Sort(Dictionary<int, int> blockInfo)
        {
            var sortedList = blockInfo.ToList();

            sortedList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

            return sortedList;
        }

        private void ShowTotalBlockCount()
        {
            var sum = _level.Blocks.Count();
            WriteLine("\t" + "Block Count".PadRight(PadSize) + ":  " + sum + Environment.NewLine);
        }

        private void ShowBlockCount()
        {
            var blockInfo = CountBlocks();

            if (blockInfo == null)
                return;

            foreach(var keyValue in Sort(blockInfo))
                WriteLine("\t" + Block.GetBlockName(keyValue.Key).PadRight(PadSize) + ":  " + keyValue.Value);
        }

        private void ShowMapSize()
        {
            var x = _level.Blocks.Select(b => b.X).GetMinAndMax();
            var y = _level.Blocks.Select(b => b.Y).GetMinAndMax();

            WriteLine("\t" + "Level Width".PadRight(PadSize)  + ":  " + (1 + x.max - x.min));
            WriteLine("\t" + "Level Height".PadRight(PadSize) + ":  " + (1 + y.max - y.min));
        }

        private void ShowInfo()
        {
            _level = _builder.DownloadLevel(_levelID);

            if(_level == null)
                return;

            WriteLine("\t" + "Level Title".PadRight(PadSize) + ":  " + _level.Title);
            ShowMapSize();
            ShowTotalBlockCount();
            ShowBlockCount();
        }

    }
}
