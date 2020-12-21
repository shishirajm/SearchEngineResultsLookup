namespace SearchEngineResultsLookup.Providers
{
    public class SearchProviders
    {
        public const string Google = "google";
        public const string Bing = "bing";
    }

    public class Config
    {
        public const int HoursToCacheResults = 1;
        public const int NumOfResults = 100;
    }
}
