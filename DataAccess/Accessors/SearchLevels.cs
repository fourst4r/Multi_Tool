using DataAccess.DataStructures;
using System;
using System.Globalization;
using static DataAccess.DataStructures.SearchLevelInfo;

namespace DataAccess.Accessors
{

    internal class SearchLevels : PostAccessor
    {

        private const string SEARCH_LINK = "http://pr2hub.com/search_levels.php?";


        internal SearchLevels(SearchLevelInfo info)
        {
            if (info == null)
                return;

            string searchQuery = GetSearchQuery(info);

            Access(SEARCH_LINK, searchQuery);
        }


        private string GetSearchQuery(SearchLevelInfo info)
        {
            string searchQuery = "search_str=" + info.SearchValue
                + "&mode="  + Enum.GetName(typeof(SearchModeEnum), info.Mode).ToLowerInvariant()
                + "&order=" + Enum.GetName(typeof(SearchOrderEnum), info.Order).ToLowerInvariant()
                + "&dir="   + Enum.GetName(typeof(SearchDirectionEnum), info.Direction).ToLowerInvariant()
                + "&page="  + info.Page.ToString(CultureInfo.InvariantCulture);

            return searchQuery;
        }


    }
}
