using System.Globalization;

namespace DataAccess.Accessors
{
    internal class NewestLevels : PostAccessor
    {

        private const string SEARCH_LINK = "https://pr2hub.com/files/lists/newest/";


        internal NewestLevels(int page)
        {
            string link = SEARCH_LINK + page.ToString(CultureInfo.InvariantCulture);

            Access(SEARCH_LINK, string.Empty);
        }

    }
}
