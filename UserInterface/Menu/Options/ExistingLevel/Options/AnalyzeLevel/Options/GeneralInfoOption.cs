using LevelModel.Models;
using LevelModel.Models.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UserInterface.Handlers;
using UserInterface.Handlers.FileHandlers;

namespace UserInterface.Menu.Options.ExistingLevel.Options.AnalyzeLevel.Options
{
    class GeneralInfoOption : BaseMenuOption
    {

        private int _levelID;
        private BuildHandler _builder;
        private const int NameSize = 18;

        internal GeneralInfoOption()
        {
            _builder = new BuildHandler();
            GetRequiredInfo();

            if (IsInputValid)
                ShowLevel();
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

        private void ShowLevel()
        {
            var level = _builder.DownloadLevel(_levelID);

            if (level == null)
                return;

            foreach (PropertyInfo info in GetSortedInfo(level))
            {
                if (info.Name == "Items")
                    ShowItems(info, level);
                else if (info.Name == "Song")
                    ShowSong(info, level);
                else if (info.Name == "GameMode")
                    ShowGameMode(info, level);
                else if (info.Name == "Note")
                    ShowNote(info, level);
                else if (info.Name != "Data" && ShowProperty(info.Name))
                {
                    var value = info.GetValue(level, null).ToString();

                    if(string.IsNullOrWhiteSpace(value) || value.StartsWith("System."))
                        continue;

                    WriteLine(GetName(info) + value);
                }
            }
        }

        private string GetName(PropertyInfo info)
        {
            var chars  = new List<char>();
            bool first = true;
            bool added = false;

            foreach(var c in info.Name)
            {

                if (!first && !added && Char.IsUpper(c)) {
                    added = true;
                    chars.Add(' ');
                }
                else
                {
                    added = false;
                }
                
                chars.Add(c);
                first = false;
            }

            var name = new string(chars.ToArray());
            return "\t" + name.PadRight(NameSize) + ":  ";
        }

        private PropertyInfo[] GetSortedInfo(Level level) 
        {
            var sortedList = level.GetType()?.GetProperties()?.ToList();

            if(sortedList == null)
                return new PropertyInfo[0];

            sortedList.Sort((pair1, pair2) => pair1.Name.CompareTo(pair2.Name));

            return sortedList.ToArray();
        }

        private void ShowItems(PropertyInfo info, Level level)
        {
            List<Item> items = ((List<Item>)info.GetValue(level, null)) ?? new List<Item>();

            Write(GetName(info));
            foreach (Item item in items)
                Write(item.Name + "(" + item.ID + ")  ");
            WriteLine();
        }

        private void ShowSong(PropertyInfo info, Level level)
        {
            var song = info.GetValue(level, null) as Song;
            if (song != null)
                WriteLine(GetName(info) + song.Name + "(" + song.ID + ")  ");
        }

        private void ShowGameMode(PropertyInfo info, Level level)
        {
            var gameMode = info.GetValue(level, null) as GameMode;
            if (gameMode != null)
                WriteLine(GetName(info) + gameMode.FullName + "(" + gameMode.PR2Name + ")  ");
        }

        private void ShowNote(PropertyInfo info, Level level)
        {
            string note;
            note = info.GetValue(level, null) as string;
            note = RemoveSpecialSymbols(note);
            note = AlignAfterNewLine(note);

            WriteLine(GetName(info) + note);
        }

        private string AlignAfterNewLine(string note)
        {
            if(string.IsNullOrEmpty(note))
                return string.Empty;

            var chars    = new List<char>();
            var count    = 0;
            var newLine  = false;

            foreach (var c in note)
            {
                if (newLine && c != ' ')
                {
                    newLine = false;
                    count   = 0;

                    chars.Add('\n');
                    chars.Add('\t');
                    for (int i = 0; i < NameSize + 3; i++) chars.Add(' ');
                }

                count++;
                chars.Add(c);

                if (count > 45 && c == ' ')
                    newLine = true;
            }

            return new string(chars.ToArray());
        }

        private string RemoveSpecialSymbols(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            return s.Replace("\r", "  ");
        }

        private bool ShowProperty(string name)
        {
            if (name.Equals(nameof(Level.Blocks), StringComparison.InvariantCultureIgnoreCase))
                return false;
            if (name.Equals(nameof(Level.TextArt1), StringComparison.InvariantCultureIgnoreCase))
                return false;
            if (name.Equals(nameof(Level.TextArt2), StringComparison.InvariantCultureIgnoreCase))
                return false;
            if (name.Equals(nameof(Level.TextArt3), StringComparison.InvariantCultureIgnoreCase))
                return false;
            if (name.Equals(nameof(Level.DrawArt1), StringComparison.InvariantCultureIgnoreCase))
                return false;
            if (name.Equals(nameof(Level.DrawArt2), StringComparison.InvariantCultureIgnoreCase))
                return false;
            if (name.Equals(nameof(Level.DrawArt3), StringComparison.InvariantCultureIgnoreCase))
                return false;
            if (name.Equals(nameof(Level.TextArt0), StringComparison.InvariantCultureIgnoreCase))
                return false;
            if (name.Equals(nameof(Level.TextArt00), StringComparison.InvariantCultureIgnoreCase))
                return false;
            if (name.Equals(nameof(Level.DrawArt0), StringComparison.InvariantCultureIgnoreCase))
                return false;
            if (name.Equals(nameof(Level.DrawArt00), StringComparison.InvariantCultureIgnoreCase))
                return false;
            if (name.Equals(nameof(Level.Hash), StringComparison.InvariantCultureIgnoreCase))
                return false;

            return true;
        }
    }
}
