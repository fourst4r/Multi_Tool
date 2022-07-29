using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using Builders;
using LevelModel.Models;
using Builders.DataStructures.DTO;
using UserInterface.DataStructures.Info;
using UserInterface.DataStructures.Constants;
using LevelModel.Models.Components.Art;
using LevelModel.Models.Components;
using LevelModel;

namespace UserInterface.Handlers
{
    internal class BuildHandler : IOHandler
    {


        private AccessHandler _accessHandler;
        private ParseHandler  _parseHandler;


        public BuildHandler()
        {
            _accessHandler = new AccessHandler();
            _parseHandler  = new ParseHandler();
        }


        internal void BuildLevel(BuildDTO info)
        {
            try
            {
                UploadLevel(PR2Builder.BuildLevel(info));
            }
            catch (Exception ex) { ShowExceptionToUser(ex); }
        }

        internal Level CreateLevel(BuildDTO info)
        {
            Level level = null;

            try   { level = PR2Builder.BuildLevel(info);      }
            catch (Exception ex) { ShowExceptionToUser(ex); }

            return level;
        }

        internal void BuildMerge(MergeInfo info)
        {
            try
            {
                info.Level1 = DownloadLevel(info.LevelID1);
                info.Level2 = DownloadLevel(info.LevelID2);

                if (info.Level1 != null && info.Level2 != null)
                    UploadLevel(PR2Builder.Merge(info));
            }
            catch (Exception ex) { ShowExceptionToUser(ex); }
        }

        internal void CopyLevel(CopyInfo info)
        {
            try
            {
                Level level = DownloadLevel(info.LevelID);

                if (level == null) 
                    return;

                level.Title = info.Title;
                UploadLevel(level);
            }
            catch (Exception ex) { ShowExceptionToUser(ex); }
        }

        internal void TransformLevel(TransformInfo info)
        {
            try
            {
                info.Level = DownloadLevel(info.LevelID);

                if (info.Level == null)
                    return;

                UploadLevel(PR2Builder.Transform(info));
            }
            catch (Exception ex) { ShowExceptionToUser(ex); }
        }

        internal void ExtendLevel(ExtendInfo info)
        {
            try
            {
                info.Level = DownloadLevel(info.LevelID);

                if (info.Level != null)
                    UploadLevel(GetExtendedLevel(info));
            }
            catch (Exception ex) { ShowExceptionToUser(ex); }
        }

        internal void AddArtBlocks(AddArtBlocksInfo info)
        {
            try
            {
                info.DTO.Level = DownloadLevel(info.LevelID);

                if (info.DTO.Level != null)
                {
                    info.DTO.Level.Title = info.Title;
                    WriteLine("\tAdding Art...");
                    PR2Builder.AddArtBlocks(info.DTO);
                    UploadLevel(info.DTO.Level);
                }
            }
            catch (Exception ex) { ShowExceptionToUser(ex); }
        }

        internal void MoveArt(MoveArtInfo info)
        {
            try
            {
                info.DTO.Level = DownloadLevel(info.LevelID);

                if (info.DTO.Level == null)
                    return;

                WriteLine("\tMoving Art...");
                info.DTO.Level.Title = info.Title;
                PR2Builder.MoveArt(info.DTO);
                UploadLevel(info.DTO.Level);
            }
            catch (Exception ex) { ShowExceptionToUser(ex); }
        }

        internal void MoveBlocks(MoveBlocksInfo info)
        {
            try
            {
                WriteLine("\tMoving Blocks...");
                info.Level.Title = info.Title;
                info.Level.Blocks.First().X += info.X;
                info.Level.Blocks.First().Y += info.Y;

                if(info.MoveArt)
                    PR2Builder.MoveArt(info.ArtDTO);

                UploadLevel(info.Level);
            }
            catch (Exception ex) { ShowExceptionToUser(ex); }
        }

        internal Level DownloadLevel(int levelID)
        {
            string levelData = _accessHandler.Download(levelID);

            if (levelData != AccessHandler.FAILED)
                return _parseHandler.Parse(levelData);

            return null;
        }

        internal void RemoveBlocks(RemoveBlocksInfo info)
        {
            try
            {
                info.DTO.Level = DownloadLevel(info.LevelID);

                if (info.DTO.Level == null)
                    return;

                WriteLine("\tRemoving Blocks...");
                info.DTO.Level.Title = info.Title;
                PR2Builder.RemoveBlocks(info.DTO);
                UploadLevel(info.DTO.Level);
            }
            catch (Exception ex) { ShowExceptionToUser(ex); }
        }

        internal void RemoveArt(RemoveArtInfo info)
        {
            try
            {
                info.DTO.Level = DownloadLevel(info.LevelID);

                if (info.DTO.Level == null)
                    return;

                WriteLine("\tRemoving Art...");
                info.DTO.Level.Title = info.Title;
                PR2Builder.RemoveArt(info.DTO);
                UploadLevel(info.DTO.Level);
            }
            catch (Exception ex) { ShowExceptionToUser(ex); }
        }

        internal void ChangeArtType(ChangeArtTypeInfo info)
        {
            try
            {
                var level = DownloadLevel(info.LevelID);

                if (level == null)
                    return;

                WriteLine("\tChanging Art Type...");
                ChangeArtType(info.Art0ToArt1, level);
                level.Title = info.Title;
                UploadLevel(level);
            }
            catch (Exception ex) { ShowExceptionToUser(ex); }
        }


        private void ChangeArtType(bool art0toart1, Level level)
        {
            if(art0toart1)
            {
                level.DrawArt1 = level.DrawArt1.Merge(new List<DrawArt>(level.DrawArt0));
                level.DrawArt0.Clear();
            }
            else
            {
                level.DrawArt0 = level.DrawArt0.Merge(new List<DrawArt>(level.DrawArt1));
                level.DrawArt1.Clear();
            }
        }

        private Level GetExtendedLevel(ExtendInfo info) {
            if(info.Type == ExtendDTO.ExtendType.Blocks) 
                info.BlocksToAdd = _parseHandler.ParseBlocks(info.InputBlocks);
            

            return PR2Builder.Extend(info);
        }

        private void UploadLevel(Level level) {
            if (level == null)
                return;

            level.Published = false;
            if (SizeControl(level))
                _accessHandler.Upload(level);
        }

        private bool SizeControl(Level level)
        {
            if(!PositionControl(level))
                WriteLine(Environment.NewLine + "\tWarning: The level contains blocks that are outside the boundaries of LE.", WarningColor);

            return BlockCountControl(level);
        }

        private bool PositionControl(Level level)
        {
            var x = level.Blocks.Select(b => b.X).GetMinAndMax();
            var y = level.Blocks.Select(b => b.Y).GetMinAndMax();

            if (x.min < 0 || x.max >= LevelModel.Constants.LEVEL_EDITOR_WIDTH)
                return false;
            if (y.min < 0 || y.max >= LevelModel.Constants.LEVEL_EDITOR_HEIGHT)
                return false;

            return true;
        }

        private bool BlockCountControl(Level level) {
            if (level.Blocks.Count() > Level.MAX_BLOCK_LIMIT) {
                WriteSizeControlOptions();

                string input = ReadInput();
                if (int.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out int value) && value == int.Parse(MenuOptions.UPLOAD_LEVEL, NumberStyles.Any, CultureInfo.InvariantCulture))
                    return true;

                WriteLine(Environment.NewLine + "\tCanceled!");
                return false;
            }

            return true;
        }

        private void  WriteSizeControlOptions() {
            WriteLine(Environment.NewLine + "\tWarning: The built level exceeds the maximum block limit on PR2" + Environment.NewLine, WarningColor);
            WriteLine("What action do you wish to take?");
            WriteLine("\t" + MenuOptions.UPLOAD_LEVEL + "  -  Upload Level");
            WriteLine("\t" + MenuOptions.CANCEL_UPLOAD + "  -  Cancel");

            Write(Environment.NewLine + "Pick option:  ", UserInputColor);
        }


    }
}
