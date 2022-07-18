using System;
using System.IO;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using UserInterface.DataStructures;
using UserInterface.DataStructures.Constants;
using System.Linq;

namespace UserInterface.Handlers.FileHandlers
{
    internal class UserSettingsHandler : IOHandler
    {


        internal static User CurrentUser { get; private set; }
        internal static List<User> Users { get; private set; }

        private const string USERNAME = "username";
        private const string TOKEN = "token";
        private const string CURRENT = "current";


        internal static void Init() {
            try {
                CurrentUser = new User();
                Users       = new List<User>();

                ReadUsers();
            }
            catch (ArgumentException) { WriteLine(Environment.NewLine + "\tError: User settings file does not exist.", ErrorColor); }
            catch (Exception ex) { ShowExceptionToUser(ex); }
        }

        internal static void SetCurrentUser(string username, string token) {
            Users.ForEach(u => u.IsCurrent = false);
            var user = Users.FirstOrDefault(u => string.Equals(u.Name, username, StringComparison.InvariantCultureIgnoreCase));

            if (user != null) {
                user.IsCurrent = true;
                user.Token = token;
                CurrentUser = user;
            }
            else {
                CurrentUser = new User(username, token, true);
                Users.Add(CurrentUser);
                Users = Users.OrderBy(u => u.Name.ToLower(CultureInfo.InvariantCulture)).ToList();
            }

            WriteToFile();
        }

        internal static void RemoveCurrentUser() {
            Users.RemoveAll(u => string.Equals(CurrentUser.Name, u.Name, StringComparison.InvariantCultureIgnoreCase));
            CurrentUser = new User();

            WriteToFile();
        }


        internal static void RemoveAllUsers() {
            Users.Clear();
            WriteToFile();
        }


        private static void WriteToFile() {
            var sb = new StringBuilder();

            Users.ForEach(u => sb.Append(UserToString(u) + Environment.NewLine));
            File.WriteAllText(MyPaths.USER_SETTINGS_PATH, sb.ToString());
        }

        private static string UserToString(User user) {
            return USERNAME + " = " + user.Name + Environment.NewLine
                 + TOKEN + " = " + user.Token + Environment.NewLine
                 + CURRENT + " = " + user.IsCurrent + Environment.NewLine;
        }

        private static void ReadUsers() {
            if(!File.Exists(MyPaths.USER_SETTINGS_PATH))
                return;

            string[] lines = File.ReadAllLines(MyPaths.USER_SETTINGS_PATH);
            var user = new User();

            foreach (string line in lines)
                ReadLineContent(line, ref user);
        }

        private static void ReadLineContent(string line, ref User user) {
            string[] content = line.Split(new[] { '=' }, 2);

            if (content[0].Trim().Equals(USERNAME, StringComparison.InvariantCultureIgnoreCase) && content.Length > 1)
                user.Name = content[1].Trim();
            else if (content[0].Trim().Equals(TOKEN, StringComparison.InvariantCultureIgnoreCase) && content.Length > 1)
                user.Token = content[1].Trim();
            else if (content[0].Trim().Equals(CURRENT, StringComparison.InvariantCultureIgnoreCase) && content.Length > 1) {
                user.IsCurrent = bool.Parse(content[1].Trim());
                AddUser(ref user);
            }
        }

        private static void AddUser(ref User user) {
            if (user.IsCurrent)
                CurrentUser = user;

            Users.Add(user);
            user = new User();
        }


    }
}
