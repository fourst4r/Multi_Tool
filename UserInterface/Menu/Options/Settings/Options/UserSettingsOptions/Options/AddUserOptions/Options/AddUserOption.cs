using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UserInterface.DataStructures.Constants;
using UserInterface.Handlers;
using UserInterface.Handlers.FileHandlers;

namespace UserInterface.Menu.Options.Settings.Options.UserSettingsOptions.Options.AddUserOptions
{
    class AddUserOption : BaseMenuOption
    {

        private string _username;
        private string _token;
        private bool showInvalidInputMessage;
        private BuildHandler _builder;
        private AccessHandler _accessor;

        public AddUserOption(bool usePassword) {
            _builder  = new BuildHandler();
            _accessor = new AccessHandler();
            showInvalidInputMessage = true;

            GetRequiredInfo(usePassword);

            if (IsInputValid)
                UserSettingsHandler.SetCurrentUser(_username, _token);
            else if(showInvalidInputMessage)
                WriteLine(Environment.NewLine + "\tError, invalid input!", ErrorColor);
        }

        private void GetRequiredInfo(bool usePassword)
        {
            IsInputValid = true;
            _username = GetUsername(false);

            if (IsInputValid)
            {
                if (usePassword)
                    _token = GetTokenFromPassword();
                else
                    _token = GetToken(false);
            }
        }

        private string GetPr2BuildVersionManually()
        {
            try
            {
                // This is an ugly trick
                // I have a map that I need to manually update with the Build Version number

                int levelId = Constants.InfoLevelId;
                var level  = _builder.DownloadLevel(levelId);
                var version = level?.TextArt1?.FirstOrDefault()?.Text;

                if (string.IsNullOrWhiteSpace(version))
                    return string.Empty;

                var text = System.Web.HttpUtility.UrlDecode(version).Trim();
                var result = ReplaceAsciiValues(text);

                return result;

            }
            catch
            {
                return string.Empty;
            }
        }

        private string ReplaceAsciiValues(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            var culture = CultureInfo.InvariantCulture;

            for (int i = 32; i < 256; i++)
            {
                if (i == 35)
                    continue;

                var value = "#" + i.ToString(culture);
                var symbol = ((char)i).ToString(culture);

                text = text.Replace(value, symbol);
            }

            return text.Replace("#35", "#");
        }

        private string GetPr2BuildVersion()
        {
            WriteLine("\tChecking PR2 version...");

            var info = _accessor.GetPr2Version();

            if (string.IsNullOrWhiteSpace(info?.BuildVersion))
            {
                var version = GetPr2BuildVersionManually();

                if(!string.IsNullOrWhiteSpace(version)) 
                    return version;

                WriteLine("\tFailed to read the newest PR2 build version.", ErrorColor);
                showInvalidInputMessage = false;
                IsInputValid = false;

                return string.Empty;
            }

            return info.BuildVersion;
        }

        private string GetTokenFromPassword()
        {
            Write("Password:  ", UserInputColor);

            string pass  = ReadInput(false);
            var version  = GetPr2BuildVersion();

            if(!IsInputValid)
                return string.Empty;

            var token = _accessor.GetToken(_username, pass, version, out var errorMsg);

            if (!string.IsNullOrWhiteSpace(errorMsg))
            {
                WriteLine(Environment.NewLine + "\t" + errorMsg, ErrorColor);
                showInvalidInputMessage = false;
            }

            if (!string.IsNullOrWhiteSpace(token))
                return token;

            IsInputValid = false;
            return string.Empty;
        }

    }
}
