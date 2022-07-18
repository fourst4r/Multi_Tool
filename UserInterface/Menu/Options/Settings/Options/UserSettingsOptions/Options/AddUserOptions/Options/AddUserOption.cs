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

        private void GetRequiredInfo(bool usePassword) {
            IsInputValid = true;
            _username = GetUsername(false);

            if (IsInputValid)
            {
                if(usePassword)
                    _token = GetTokenFromPassword();
                else
                    _token = GetToken(false);
            }
        }

        private string GetPr2BuildVersion()
        {
            WriteLine("\tChecking PR2 version..." + Environment.NewLine);

            var info = _accessor.GetPr2Version();

            if (string.IsNullOrWhiteSpace(info?.BuildVersion))
            {
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
