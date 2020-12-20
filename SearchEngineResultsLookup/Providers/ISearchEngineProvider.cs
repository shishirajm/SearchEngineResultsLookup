namespace SearchEngineResultsLookup.Providers
{
    public interface ISearchEngineProvider
    {
        string Provider { get; }
        string GetUrl(string keyWords, int pageIndex);
    }

    public delegate ISearchEngineProvider SearchEngineResolver(string key);
}
