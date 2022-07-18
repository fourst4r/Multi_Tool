using System.Collections.Generic;
using System.Linq;
using LevelModel.Models;

namespace UserInterface.Menu.Options.ExistingLevel.Options.SearchPublicLevels.Formatters
{
    internal class SearchFormatter : IOHandler
    {

        internal static void ShowLevel(Level l)
        {
            WriteLine("\tTitel       :  " + l.Title);
            WriteLine("\tLevel ID    :  " + l.LevelID);

            if (l.Version > 0)
                WriteLine("\tVersion     :  " + l.Version);
            if (l.GameMode != null)
                WriteLine("\tGame Mode   :  " + l.GameMode.FullName);

            WriteLine("\tUser ID     :  " + l.UserID);
            WriteLine("\tBlock Count :  " + l.Blocks.Count());
        }

        internal static void ShowSearch(List<BaseLevel> levels)
        {
            for (int i = 0; i < levels.Count; i++)
                WriteLine("\t" + (i + 1) + "  -  " + levels[i].Title);
        }

    }
}
