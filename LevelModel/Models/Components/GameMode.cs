using System;
using System.Diagnostics;
using System.Globalization;

namespace LevelModel.Models.Components
{
    public class GameMode
    {


        public string FullName { get; set; }
        public const string DEFAULT = "race";

        //This is used as a name for it in PR2 server
        public string PR2Name { get; set; }


        public GameMode(string name)
        {
            PR2Name  = name;
            FullName = GetFullName(name);
        }


        private string GetFullName(string value)
        {
            switch (value.ToLower(CultureInfo.InvariantCulture))
            {
                case "r":
                case "race": return "Race";

                case "d":
                case "deathmatch": return "Deathmatch";

                case "o":
                case "objective": return "Objective";

                case "e":
                case "aliens eggs": return "Aliens Eggs";

                case "h":
                case "hat": return "Hat Attack";

                default: return string.Empty;
            }
        }


    }
}
