namespace DataAccess.Accessors
{
    internal class DownloadLevel
    {
        internal string Result { get; set; }

        internal DownloadLevel(int levelID)
        {
            string url = GetUrl(levelID);

            Result = GetAccessor.Download(url);
        }

        private string GetUrl(double levelID)
        {
            string query = "http://pr2hub.com/levels/"
                        + levelID + ".txt";
                        // + "?version=" + version;  //PR2 server will auto take last version

            return query;
        }

    }
}
