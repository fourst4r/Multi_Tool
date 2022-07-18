using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using LevelModel.Models;
using LevelModel.DTO;
using Converters;
using Converters.DataStructures.DTO;
using UserInterface.Handlers.FileHandlers;
using UserInterface.DataStructures.Info;

using static UserInterface.DataStructures.Constants.MyPaths;
using LevelModel;

namespace UserInterface.Handlers
{
    internal class ConvertHandler : IOHandler
    {


        private AccessHandler _accessHandler;
        private ParseHandler _parseHandler;


        public ConvertHandler()
        {
            _accessHandler = new AccessHandler();
            _parseHandler  = new ParseHandler();
        }


        internal bool Pr2ToTmx(ConvertInfo info)
        {
            try
            {
                string levelData = _accessHandler.Download(info.LevelID);

                if (levelData != AccessHandler.FAILED)
                {
                    var success = TxtToTmx(info, levelData);
                    return success;
                }
            }
            catch (Exception ex) { ShowExceptionToUser(ex); }

            return false;
        }

        internal bool Pr2ToTxt(string filepath, int levelID)
        {
            try
            {
                string levelData = _accessHandler.Download(levelID);

                if (levelData != AccessHandler.FAILED && !string.IsNullOrWhiteSpace(levelData))
                    return TxtFileHandler.Save(filepath, levelData);
            }
            catch (Exception ex) { ShowExceptionToUser(ex); }

            return false;
        }

        internal bool TxtToTmx(ConvertInfo info, string levelData)
        {
            try
            {
                Level level = _parseHandler.Parse(levelData);

                if (level != null)
                {
                    info.ToTmxDTO.LevelData   = levelData;
                    info.Level                = level;
                    info.FilePath             = GetFilePath(level);
                    info.ToTmxDTO.TilesetPath = new Uri(USER_BLOCK_FOLDER).MakeRelative(new Uri(USER_TILESET_PATH));

                    var tmx = PR2Converter.LevelToTMX(info.ToTmxDTO);
                    ShowMessages(info.ToTmxDTO.Messages);

                    if (tmx != null)
                    {
                        SaveLevel(info, tmx, levelData);
                        return true;
                    }
                }
            }
            catch (Exception ex) { ShowExceptionToUser(ex); }

            return false;
        }

        internal void TxtToPr2(string levelData)
        {
            if (string.IsNullOrWhiteSpace(levelData))
                return;

            try
            {
                var level = _parseHandler.Parse(levelData);

                if (level != null)
                    _accessHandler.Upload(level, false);
            }
            catch (Exception ex) { ShowExceptionToUser(ex); }
        }

        internal void TmxToPr2(ConvertInfo info)
        {
            try
            {
                info.ToLevelDTO.Level = TmxFileHandler.Read(info.FilePath);
                ConvertToLevel(info);

                if (info.Level != null)
                {
                    info.Level.Published = false;
                    _accessHandler.Upload(info.Level);
                }
            }
            catch (Exception ex) { ShowExceptionToUser(ex); }
        }

        internal bool TmxToTxt(string txmFilepath, string txtFilepath)
        {
            try
            {
                var tmx       = TmxFileHandler.Read(txmFilepath);
                var level     = ConvertToLevel(tmx);
                var info      = new ToPr2DTO
                {
                    Level          = level,
                    Username       = UserSettingsHandler.CurrentUser.Name,
                    Token          = UserSettingsHandler.CurrentUser.Token,
                    EnableEncoding = false,
                    OverWrite      = false
                };
                var levelData = LevelToPr2(info);

                if (!string.IsNullOrWhiteSpace(levelData))
                    return TxtFileHandler.Save(txtFilepath, levelData);
            }
            catch (Exception ex) { ShowExceptionToUser(ex); }

            return false;
        }

        internal string LevelToPr2(ToPr2DTO info)
        {
            try
            {
                return PR2Converter.LevelToPr2(info);
            }
            catch(Exception ex)
            {
                ShowExceptionToUser(ex);
            }

            return string.Empty;
        }


        private Level ConvertToLevel(XmlDocument tmx)
        {
            Level level = null;

            if (tmx != null)
            {
                var info  = new ToLevelDTO { Level = tmx };
                level     = PR2Converter.TmxToLevel(info);
                ShowMessages(info.Messages);
            }

            return level;
        }

        private void ConvertToLevel(ConvertInfo info)
        {
            if (info.ToLevelDTO.Level != null)
            {
                info.Level = PR2Converter.TmxToLevel(info.ToLevelDTO);
                ShowMessages(info.ToLevelDTO.Messages);

                if (info.Level != null)
                {
                    info.Level.Title = info.Title;
                }
            }
        } 

        private void ShowMessages(IEnumerable<Message> warnings) {
            foreach (var msg in warnings) {
                if (msg.Type == Message.MessageType.Warning)
                    WriteLine(Environment.NewLine + "\tWarning: " + msg.Content, WarningColor);
                else
                    WriteLine(Environment.NewLine + "\t" + msg.Content);
            }
        }

        private void SaveLevel(ConvertInfo info, XmlDocument tmx, string levelData)
        {
            TmxFileHandler.Save(info.FilePath, tmx);
        }

        private string GetFilePath(Level level) => Path.Combine(USER_LEVEL_FOLDER, GetFileName(level));

        private string GetFileName(Level level)
        {
            var title = new StringBuilder();

            foreach(var c in level.Title.Replace(" ", "_"))
            {
                if (char.IsLetterOrDigit(c) || c.Equals('_'))
                    title.Append(c);
            }

            return title + ".tmx";
        }


    }
}
