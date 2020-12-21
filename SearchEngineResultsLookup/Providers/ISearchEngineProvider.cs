namespace SearchEngineResultsLookup.Providers
{
    public interface ISearchEngineProvider
    {
        string GetUrl(string keyWords, int pageIndex);
    }

    public delegate ISearchEngineProvider SearchEngineResolver(string key);
}
