using System;
using DataAccess.Accessors;
using DataAccess.DataStructures;
using static DataAccess.Accessors.VersionFetcher;

namespace DataAccess
{
    public static class PR2Accessor 
    {

        public static string Download(int levelID) =>  new DownloadLevel(levelID).Result;

        public static string Search(SearchLevelInfo info) => new SearchLevels(info).Result;

        public static string Newest(int page) => new NewestLevels(page).Result;

        public static string BestWeek(int page) => new BestWeekLevels(page).Result;

        public static string Upload(string levelData, Action<LevelExistArg> onLevelExist) => new UploadLevel(levelData, onLevelExist).Result;

        public static string LoadMyLevels(string token) => new LoadMyLevels(token).Result;

        public static VersionInfo Pr2Version() => new VersionFetcher().Info;

        public static UserInfo GetUser(uint id) => new UserFetcher(id).Info;

        public static TokenInfo GetToken(string username, string password, string version) => new TokenFetcher(username, password, version).Result;

    }
}
