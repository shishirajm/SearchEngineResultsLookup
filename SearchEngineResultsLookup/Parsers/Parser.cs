using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using SearchEngineResultsLookup.Providers;

namespace SearchEngineResultsLookup.Parsers
{
    public class Parser : IParser
    {
        // I am unhappy with the crude parser I have written,
        // it would have been good I could have used 3rd Party library like Html Agility Pack
        // Assumption here the html structure wont change is brittle

        private readonly ILogger<Parser> _logger;
        private readonly IEnumerable<IParserConfiguration> _parserConfigurations;

        public Parser(ILogger<Parser> logger, IEnumerable<IParserConfiguration> parserConfigurations)
        {
            _logger = logger;
            _parserConfigurations = parserConfigurations;
        }

        public IEnumerable<string> ParseResults(string rawBody, string provider)
        {
            try
            {
                return GetAllResultDivs(rawBody, provider);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message + rawBody);
                throw;
            }
        }

        private IEnumerable<string> GetAllResultDivs(string rawBody, string provider)
        {
            
            var body = rawBody;
            var divs = new List<string>();

            while (body.Contains(GetParserInstance(provider).NodeStartPattern))
            {
                var beginningOfNode = body.IndexOf(GetParserInstance(provider).NodeStartPattern);
                body = body.Substring(beginningOfNode);
                var div = ExtractResultDiv(body, provider);
                body = (body.Length > div.Length) ? body.Substring(div.Length) : "";
                divs.Add(div);
            }

            var webResultDivs = GetParserInstance(provider).Filter.Item1 ? divs.Where(div => div.Contains(GetParserInstance(provider).Filter.Item2)) : divs;

            return webResultDivs;
        }

        private string ExtractResultDiv(string text, string provider)
        {
            var endOfNode = GetLastIndexOfNode(text, provider);
            return text.Substring(0, endOfNode);
        }

        private int GetLastIndexOfNode(string text, string provider)
        {
            var lastIndex = 0;
            var flag = true;
            var startIndex = 0;

            // Find the relavent div ending
            // Might feel like bit of magic
            while (flag)
            {
                lastIndex = text.IndexOf(GetParserInstance(provider).DivEndPattern, startIndex);
                var subText = text.Substring(0, lastIndex + GetParserInstance(provider).DivEndPattern.Length);
                var divStartNum = Regex.Matches(subText, GetParserInstance(provider).DivStartPattern).Count;
                var divCloseNum = Regex.Matches(subText, GetParserInstance(provider).DivEndPattern).Count;
                startIndex = lastIndex + GetParserInstance(provider).DivEndPattern.Length;
                flag = (divStartNum - divCloseNum) > 0;
            }

            return lastIndex + GetParserInstance(provider).DivEndPattern.Length;
        }

        private IParserConfiguration GetParserInstance(string provider)
        {
            switch (provider)
            {
                case SearchProviders.Google:
                    return _parserConfigurations.Where(p => p.Provider == SearchProviders.Google).First();
                case SearchProviders.Bing:
                    return _parserConfigurations.Where(p => p.Provider == SearchProviders.Bing).First();
                default:
                    throw new ArgumentException();
            }
        }
    }
}
