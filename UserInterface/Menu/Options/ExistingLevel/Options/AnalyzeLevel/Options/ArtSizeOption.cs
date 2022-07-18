using System;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using LevelModel.Models;
using UserInterface.Handlers;
using UserInterface.Handlers.FileHandlers;
using LevelModel.Models.Components.Art;
using LevelModel.Models.Components;

namespace UserInterface.Menu.Options.ExistingLevel.Options.AnalyzeLevel.Options
{
    class ArtSizeOption : BaseMenuOption
    {

        private int _levelID;
        private Level _level;
        private BuildHandler _builder;

        internal ArtSizeOption()
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

        private int GetSize(string s)
        {
            return Encoding.UTF8.GetByteCount(s);
        }

        private string FormatSize(int value)
        {
            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";

            return value.ToString("N0", nfi);
        }

        private int GetTotalArtSize()
        {
            return GetSize(_level.DrawArt00.ToPr2String())
                 + GetSize(_level.DrawArt0.ToPr2String())
                 + GetSize(_level.DrawArt1.ToPr2String())
                 + GetSize(_level.DrawArt2.ToPr2String())
                 + GetSize(_level.DrawArt3.ToPr2String())
                 + GetSize(_level.TextArt00.ToPr2String())
                 + GetSize(_level.TextArt0.ToPr2String())
                 + GetSize(_level.TextArt1.ToPr2String())
                 + GetSize(_level.TextArt2.ToPr2String())
                 + GetSize(_level.TextArt3.ToPr2String());
        }

        private void ShowAllArtSizes()
        {
            WriteLine("\tText Art 00  :  " + FormatSize(GetSize(_level.TextArt00.ToPr2String())));
            WriteLine("\tText Art 0   :  " + FormatSize(GetSize(_level.TextArt0.ToPr2String())));
            WriteLine("\tText Art 1   :  " + FormatSize(GetSize(_level.TextArt1.ToPr2String())));
            WriteLine("\tText Art 2   :  " + FormatSize(GetSize(_level.TextArt2.ToPr2String())));
            WriteLine("\tText Art 3   :  " + FormatSize(GetSize(_level.TextArt3.ToPr2String())));
            WriteLine();                    
            WriteLine("\tDraw Art 00  :  " + FormatSize(GetSize(_level.DrawArt00.ToPr2String())));
            WriteLine("\tDraw Art 0   :  " + FormatSize(GetSize(_level.DrawArt0.ToPr2String())));
            WriteLine("\tDraw Art 1   :  " + FormatSize(GetSize(_level.DrawArt1.ToPr2String())));
            WriteLine("\tDraw Art 2   :  " + FormatSize(GetSize(_level.DrawArt2.ToPr2String())));
            WriteLine("\tDraw Art 3   :  " + FormatSize(GetSize(_level.DrawArt3.ToPr2String())));
        }

        private void ShowInfo()
        {
            _level = _builder.DownloadLevel(_levelID);

            if (_level == null)
                return;

            WriteLine("\tLevel Title  :  " + _level.Title);
            WriteLine("\tArt Size     :  " + FormatSize(GetTotalArtSize()) + " bytes" + Environment.NewLine);
            ShowAllArtSizes();
        }

    }
}
