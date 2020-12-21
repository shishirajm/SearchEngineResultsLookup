using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SearchEngineResultsLookup.Parsers;
using SearchEngineResultsLookup.Providers;

namespace SearchEngineResultsLookup.Services
{
    public class SearchEngineService: ISearchEngineService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IParser _parser;
        private readonly ILogger<SearchEngineService> _logger;
        private readonly IMemoryCache _cache;
        private readonly IEnumerable<ISearchEngineProvider> _searchEngineProviders;
        const int _hoursToCacheResults = 1; // This can be placed in App configuration or Provider Configuration
        const int _numOfResults = 100;

        public SearchEngineService(IHttpClientFactory clientFactory, IParser parser, ILogger<SearchEngineService> logger, IMemoryCache memoryCache, IEnumerable<ISearchEngineProvider> searchEngineProviders)
        {
            _clientFactory = clientFactory;
            _parser = parser;
            _logger = logger;
            _cache = memoryCache;
            _searchEngineProviders = searchEngineProviders;
        }

        public async Task<IEnumerable<int>> FindTheUrlOccurenceForKeyWordSearch(string keyword, string url, string provider)
        {
            IEnumerable<string> results;
            var key = GetCacheKey(keyword, provider);

            if (!_cache.TryGetValue(key, out results))
            {
                var rawBody = await Search(_numOfResults, keyword, provider);
                results = _parser.ParseResults(rawBody, provider).Take(_numOfResults);

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(_hoursToCacheResults));
                _cache.Set(key, results, cacheEntryOptions);
            }

            var index = 0;
            var positionOfUrl = new List<int>();
            foreach (var result in results)
            {
                if (result.Contains(url)) positionOfUrl.Add(index + 1);
                index++;
            }
            return positionOfUrl;
        }

        private async Task<string> Search(int numOfResults, string keyword, string provider)
        {
            var tasks = new List<Task>();
            var numOfPages = (numOfResults % 10 == 0) ? numOfResults / 10 : numOfResults / 10 + 1;
            var searchResults = new string[numOfPages];
            for (int i = 0; i < numOfPages; i++)
            {
                tasks.Add(QuerySearchEngineAsync(i, keyword, searchResults, provider));
            }

            Task t = Task.WhenAll(tasks);
            try
            {
                await t;
            }
            catch { }

            if (t.Status == TaskStatus.RanToCompletion)
            {
                return searchResults.Aggregate((acc, item) => acc + item);
            }
            else
            {
                throw new Exception($"Failed to Query for Key word: {keyword}");
            }
        }

        private async Task QuerySearchEngineAsync(int i, string keyword, string[] searchResults, string provider)
        {
            try
            {
                _logger.LogInformation($"Starting Request {i}");
                var request = new HttpRequestMessage(HttpMethod.Get, GetProviderInstance(provider).GetUrl(keyword, i));
                var httpClient = _clientFactory.CreateClient();
                var response = await httpClient.SendAsync(request);
                _logger.LogInformation($"Response {i}");

                if (response.IsSuccessStatusCode)
                {
                    searchResults[i] = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    searchResults[i] = "";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        private ISearchEngineProvider GetProviderInstance(string provider)
        {
            // I tried to setit up using named instances and since it was taking 
            // longer to figure out had to implement this way
            switch (provider)
            {
                case SearchProviders.Google:
                    return _searchEngineProviders.Where(p => p.Provider == SearchProviders.Google).First();
                case SearchProviders.Bing:
                    return _searchEngineProviders.Where(p => p.Provider == SearchProviders.Bing).First();
                default:
                    throw new ArgumentException();
            }
        }

        private string GetCacheKey(string keyWords, string provider)
        {
            // If needed url can be added to form the key so lookup time reduces
            return keyWords.GetHashCode() + provider;
        }
    }
}
