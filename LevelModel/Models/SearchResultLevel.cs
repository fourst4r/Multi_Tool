namespace LevelModel.Models
{
    /// <summary>
    /// Datastructure of the search result received after making a search for levels to pr2 server
    /// </summary>
    public class SearchResultLevel : BaseLevel
    {

        public int PlayCount { get; set; }
        public double Rating { get; set; }
        public string Group { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public int Time { get; set; }

    }
}
