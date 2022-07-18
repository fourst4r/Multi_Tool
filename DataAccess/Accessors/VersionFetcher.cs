using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.DataStructures;
using Newtonsoft.Json.Linq;

namespace DataAccess.Accessors
{
    internal class VersionFetcher
    {

        public VersionInfo VersionInfo { get; private set; }

        public VersionFetcher()
        {
            VersionInfo = new VersionInfo();
            var path    = "http://pr2hub.com/version.txt";
            var data    = GetAccessor.Download(path);
            Parse(data);
        }

        private void Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return;

            var json                 = JObject.Parse(input);
            VersionInfo.Url          = json?.GetValue("url")?.Value<string>() ?? string.Empty;
            VersionInfo.Time         = json?.GetValue("time")?.Value<string>() ?? string.Empty;
            VersionInfo.Version      = json?.GetValue("version")?.Value<string>() ?? string.Empty;
            VersionInfo.BuildVersion = json?.GetValue("build_version")?.Value<string>() ?? string.Empty;
        }
    }
}
