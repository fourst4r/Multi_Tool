using System;
using DataAccess.Accessors;
using DataAccess.DataStructures;

namespace DataAccess
{
    public class PR2Accessor 
    {

        public string Download(int levelID) =>  new DownloadLevel(levelID).Result;

        public string Search(string username, int page) => new SearchLevels(username, page).Result;

        public string Upload(string levelData, Action<LevelExistArg> onLevelExist) => new UploadLevel(levelData, onLevelExist).Result;

        public string LoadMyLevels(string token) => new LoadMyLevels(token).Result;

        public VersionInfo Pr2Version() => new VersionFetcher().VersionInfo;

        public TokenInfo GetToken(string username, string password, string version) => new TokenFetcher(username, password, version).Result;

    }
}
