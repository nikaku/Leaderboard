namespace Leaderboard.API
{
    public class AppSettings
    {
        public string SqlServerHostName { get; set; }
        public string SqlServerPort { get; set; }
        public string SqlServerCatalog { get; set; }
        public string SqlServerUser { get; set; }
        public string SqlServerPassword { get; set; }
        public bool EnableSSL { get; set; }
    }
}
