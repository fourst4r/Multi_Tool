using System;

namespace DataAccess.DataStructures
{
    public class SearchLevelInfo
    {

        public enum SearchDirectionEnum { Descending, Ascending }
        public enum SearchModeEnum { User, Title }
        public enum SearchOrderEnum { Date, Popularity, Rating, Alphabetical }


        public string SearchValue { get; set; }
        public SearchModeEnum Mode { get; set; }
        public SearchOrderEnum Order { get; set; }
        public SearchDirectionEnum Direction { get; set; }


        public int Page { get; set; }


        public SearchLevelInfo(string value, int page)
        {
            SearchValue = value ?? string.Empty;
            Mode = SearchModeEnum.User;
            Order = SearchOrderEnum.Date;
            Direction = SearchDirectionEnum.Descending;
            Page = page;
        }

    }

}
