using System;
using System.Collections.Generic;
using System.Text;

namespace UserInterface.DataStructures
{
    internal class User
    {

        internal string Name { get; set; }

        internal string Token { get; set; }

        internal bool IsCurrent {get; set; }

        public User() {
            Name  = string.Empty;
            Token = string.Empty;
        }

        public User(string name, string token, bool isCurrent) {
            Name = name;
            Token = token;
            IsCurrent = isCurrent;
        }

    }
}
