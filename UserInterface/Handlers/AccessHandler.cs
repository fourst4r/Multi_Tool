using System;
using System.Net;
using Converters.DataStructures.DTO;
using DataAccess;
using DataAccess.DataStructures;
using LevelModel;
using LevelModel.Models;
using UserInterface.Handlers.FileHandlers;

namespace UserInterface.Handlers
{
    internal class AccessHandler : IOHandler
    {

        public static readonly string FAILED = string.Empty;


        internal string GetToken(string username, string password, string version, out string errorMsg)
        {
            errorMsg = string.Empty;
            WriteLine(Environment.NewLine + "\tChecking password...");

            try
            {
                var tokenInfo = PR2Accessor.GetToken(username, password, version);

                if (!tokenInfo.Success || string.IsNullOrWhiteSpace(tokenInfo.Token))
                {
                    errorMsg = tokenInfo.ErrorMsg;
                    return FAILED;
                }
                 
                WriteLine(Environment.NewLine + "\tSuccessful.");
                return tokenInfo.Token;
            }
            catch (Exception ex) { ShowExceptionToUser(ex); }

            return FAILED;
        }

        internal VersionInfo GetPr2Version()
        {
            try
            {
                return PR2Accessor.Pr2Version();
            }
            catch (Exception ex) { ShowExceptionToUser(ex); return null; }
        }
        internal string Download(int levelID)
        {
            try
            {
                string levelData = PR2Accessor.Download(levelID);

                if (string.IsNullOrWhiteSpace(levelData))
                {
                    WriteLine(Environment.NewLine + "\tLevel not found!");
                    levelData = FAILED;
                }

                return levelData;
            }
            catch (WebException ex) when (IsLevelNotFoundException(ex)) { WriteLine(Environment.NewLine + "\tError: Level does not exist", ErrorColor); }
            catch (Exception ex) { ShowExceptionToUser(ex); }

            return FAILED;

        }

        internal void Upload(Level level, bool askToUpdate = true)
        {
            if (level == null)
                return;

            try
            {
                var versionHandler = new LevelVersionHandler(level, askToUpdate);
              
                if(versionHandler.Cancel == true)
                    return;

                WriteLine(Environment.NewLine + "\tUploading...");
                var levelData    = ConvertToPr2(level);
                var onLevelExist = OnLevelExist(level);
                var response     = PR2Accessor.Upload(levelData, onLevelExist);

                if (!string.IsNullOrWhiteSpace(response))
                    WriteLine(Environment.NewLine + "\tResponse from server: \"" + response + "\"");
            }
            catch (Exception ex) { ShowExceptionToUser(ex); }
        }

        internal string Search(string userToSearch, int page)
        {
            try
            {
                string result = PR2Accessor.Search(new SearchLevelInfo(userToSearch, page));

                if (string.IsNullOrWhiteSpace(result))
                    WriteLine(Environment.NewLine + "\tNo levels found!", ErrorColor);

                return (result == null) ? string.Empty : result;
            }
            catch (Exception ex) { ShowExceptionToUser(ex); }

            return string.Empty;
        }

        internal string LoadMyLevels()
        {
            try
            {
                string result = PR2Accessor.LoadMyLevels(UserSettingsHandler.CurrentUser.Token);

                if (result == null || result.Length == 0)
                    WriteLine(Environment.NewLine + "\tNo levels found!", ErrorColor);

                return (result == null) ? string.Empty : result;
            }
            catch (Exception ex) { ShowExceptionToUser(ex); }

            return string.Empty;
        }


        private string ConvertToPr2(Level level)
        {
            var info = new ToPr2DTO
            {
                Level          = level,
                Username       = UserSettingsHandler.CurrentUser.Name,
                Token          = UserSettingsHandler.CurrentUser.Token,
                EnableEncoding = true,
                OverWrite      = false
            };

            return new ConvertHandler().LevelToPr2(info);
        }

        private Action<LevelExistArg> OnLevelExist(Level level)
        {
            return new OnLevelExistHandler(level).OnLevelExist;
        }

        private bool IsLevelNotFoundException(WebException response)
        {
            var r = response.Response as HttpWebResponse;

            if (r != null && r.StatusCode == HttpStatusCode.NotFound)
                return true;

            return false;

        }


    }
}
