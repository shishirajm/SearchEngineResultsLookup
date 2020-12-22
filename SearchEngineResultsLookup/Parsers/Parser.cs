using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Autofac.Features.Indexed;
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
        private readonly IIndex<string, IParserConfiguration> _parserConfigurations;

        public Parser(ILogger<Parser> logger, IIndex<string, IParserConfiguration> parserConfigurations)
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

            if (string.IsNullOrWhiteSpace(GetParserConfigInstance(provider).NodeStartPattern)) return divs;

            while (body.Contains(GetParserConfigInstance(provider).NodeStartPattern))
            {
                var beginningOfNode = body.IndexOf(GetParserConfigInstance(provider).NodeStartPattern);
                body = body.Substring(beginningOfNode);
                var div = ExtractResultDiv(body, provider);
                if (div.Item1) // If found
                {
                    divs.Add(div.Item2);
                    body = body.Substring(div.Item2.Length); // Remove the processed part
                }
                else // If not found
                {
                    body = "";
                }
            }

            var webResultDivs = GetParserConfigInstance(provider).Filter.Item1 ? divs.Where(div => div.Contains(GetParserConfigInstance(provider).Filter.Item2)) : divs;

            return webResultDivs;
        }

        private (bool, string) ExtractResultDiv(string text, string provider)
        {
            var endOfNode = GetLastIndexOfNode(text, provider);
            return endOfNode < 0? (false, ""): (true, text.Substring(0, endOfNode));
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
                lastIndex = text.IndexOf(GetParserConfigInstance(provider).DivEndPattern, startIndex);
                var subText = text.Substring(0, lastIndex + GetParserConfigInstance(provider).DivEndPattern.Length);
                var divStartNum = Regex.Matches(subText, GetParserConfigInstance(provider).DivStartPattern).Count;
                var divCloseNum = Regex.Matches(subText, GetParserConfigInstance(provider).DivEndPattern).Count;
                startIndex = lastIndex + GetParserConfigInstance(provider).DivEndPattern.Length;
                flag = (divStartNum - divCloseNum) > 0 && startIndex < text.Length && lastIndex >= 0;
            }

            return lastIndex < 0? -1: lastIndex + GetParserConfigInstance(provider).DivEndPattern.Length;
        }

        private IParserConfiguration GetParserConfigInstance(string provider)
        {
            switch (provider)
            {
                case SearchProviders.Bing:
                    return _parserConfigurations[SearchProviders.Bing];
                default:
                    return _parserConfigurations[SearchProviders.Google];
            }
        }
    }
}
