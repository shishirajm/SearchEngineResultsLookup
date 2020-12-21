using Microsoft.Extensions.Logging;

namespace SearchEngineResultsLookup.Providers
{
    public class GoogleSearchEngineProvider : ISearchEngineProvider
    {
        private readonly ILogger<GoogleSearchEngineProvider> _logger;

        public GoogleSearchEngineProvider(ILogger<GoogleSearchEngineProvider> logger)
        {
            _logger = logger;
        }

        public string GetUrl(string keyWords, int pageIndex)
        {
            var start = pageIndex * 10;
            var url = $"https://www.google.com.au/search?q={keyWords}&start={start}&safe=active";
            _logger.LogInformation(url);
            return url;
        }
    }
}
