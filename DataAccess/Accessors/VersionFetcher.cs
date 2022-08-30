using DataAccess.DataStructures;
using Newtonsoft.Json.Linq;

namespace DataAccess.Accessors
{
    internal class VersionFetcher
    {

        public VersionInfo Info { get; private set; }

        public VersionFetcher()
        {
            Info = new VersionInfo();
            var path = "http://pr2hub.com/version.txt";
            var data = GetAccessor.Download(path);
            Parse(data);
        }

        private void Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return;

            var json = JObject.Parse(input);
            Info.Url = json?.GetValue("url")?.Value<string>() ?? string.Empty;
            Info.Time = json?.GetValue("time")?.Value<string>() ?? string.Empty;
            Info.Version = json?.GetValue("version")?.Value<string>() ?? string.Empty;
            Info.BuildVersion = GetBuildVersion(json);
        }

        private string GetBuildVersion(JObject json)
        {
            // the name of the build version inside the JSON file is unknown
            // as it has not been implemented yet
            // so i am just dummy trying some values and hope it will work once Bls199 update the API
            // so I dont have to make a new release of the tool later

            if (json == null)
                return string.Empty;

            var names = new string[] { "build_version", "buildversion", "buildVersion", "build", "BuildVersion",
                                       "client_version", "clientversion", "clientVersion", "client", "ClientVersion",
                                       "code_version", "codeversion", "codeVersion", "CodeVersion"};

            foreach (var name in names)
            {
                var value = json?.GetValue(name)?.Value<string>() ?? string.Empty;

                if (!string.IsNullOrWhiteSpace(value))
                    return value;
            }

            return string.Empty;

        }

        internal class UserFetcher
        {

            public UserInfo Info { get; private set; }

            public UserFetcher(uint id)
            {
                Info = new UserInfo();
                var path = "https://pr2hub.com/get_player_info.php?user_id=" + id;
                var data = GetAccessor.Download(path);
                Parse(data);
            }

            private void Parse(string input)
            {
                if (string.IsNullOrWhiteSpace(input))
                    return;

                var json = JObject.Parse(input);
                Info.Name = json?.GetValue("name")?.Value<string>() ?? string.Empty;
            }

        }
    }
}
