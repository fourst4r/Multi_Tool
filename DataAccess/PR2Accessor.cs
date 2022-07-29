using System;
using DataAccess.Accessors;
using DataAccess.DataStructures;

namespace DataAccess
{
    public static class PR2Accessor 
    {

        public static string Download(int levelID) =>  new DownloadLevel(levelID).Result;

        public static string Search(string username, int page) => new SearchLevels(username, page).Result;

        public static string Newest(int page, string token) => new NewestLevels(page, token).Result;

        public static string Upload(string levelData, Action<LevelExistArg> onLevelExist) => new UploadLevel(levelData, onLevelExist).Result;

        public static string LoadMyLevels(string token) => new LoadMyLevels(token).Result;

        public static VersionInfo Pr2Version() => new VersionFetcher().VersionInfo;

        public static TokenInfo GetToken(string username, string password, string version) => new TokenFetcher(username, password, version).Result;

    }
}
