using Microsoft.Extensions.Logging;

namespace SearchEngineResultsLookup.Providers
{
    public class BingSearchEngineProvider : ISearchEngineProvider
    {
        private readonly ILogger<BingSearchEngineProvider> _logger;

        public BingSearchEngineProvider(ILogger<BingSearchEngineProvider> logger)
        {
            _logger = logger;
        }

        public string GetUrl(string keyWords, int pageIndex)
        {
            var first = (pageIndex * 10) + 1;
            var url = $"https://www.bing.com/search?q={keyWords}&first={first}";
            _logger.LogInformation(url);
            return url;
        }
    }
}
