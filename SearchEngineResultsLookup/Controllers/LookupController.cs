using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SearchEngineResultsLookup.Services;
using Microsoft.Extensions.Logging;
using SearchEngineResultsLookup.Providers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SearchEngineResultsLookup.Controllers
{
    [Route("api/[controller]")]
    public class LookupController : Controller
    {
        private readonly ISearchEngineService _searchEngineService;
        private readonly ILogger<LookupController> _logger;

        public LookupController(ISearchEngineService searchEngineService, ILogger<LookupController> logger)
        {
            _searchEngineService = searchEngineService;
            _logger = logger;
        }

        // GET: api/lookup?keyWord=e-settlements&url=.com.au&searchEngine=bing
        [HttpGet]
        public async Task<IActionResult> Get(string keyWord, string url, string searchEngine)
        {
            var start = DateTime.Now;

            _logger.LogInformation($"Input: {keyWord} and {url}");

            // This validation can be a different class
            if (string.IsNullOrWhiteSpace(keyWord) || string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(searchEngine)) return BadRequest("Invalid keyword");

            var results = await _searchEngineService.FindTheUrlOccurenceForKeyWordSearch(keyWord, url, MapToSearchProvider(searchEngine));
            _logger.LogInformation($"Input: {keyWord} and {url} took {DateTime.Now - start}");
            return Ok(results);
        }

        private string MapToSearchProvider(string searchEngine)
        {
            return searchEngine.ToLower().Contains("bing") ? SearchProviders.Bing : SearchProviders.Google;
        }
    }
}
