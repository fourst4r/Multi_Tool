using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace DataAccess.Accessors
{

    internal class SearchLevels : PostAccessor
    {


        private const string SEARCH_LINK = "http://pr2hub.com/search_levels.php?";


        internal SearchLevels(string username, int page)
        {
            string searchQuery = GetSearchQuery(username, page);

            Access(SEARCH_LINK, searchQuery);
        }


        private string GetSearchQuery(string username, int page)
        {
            string searchBy = "user";   // title or user
            string sortOrder = "desc";  // asc or desc
            string sortBy = "date";

            string searchQuery = "search_str=" + username + "&mode=" + (searchBy == "Level Title" ? "title" : "user") +
                "&order=" + sortBy + "&dir=" + sortOrder +
                "&page=" + page.ToString(CultureInfo.InvariantCulture);

            return searchQuery;
        }


    }
}
