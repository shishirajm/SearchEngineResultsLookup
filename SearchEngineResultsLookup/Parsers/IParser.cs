using System;
using System.Collections.Generic;

namespace SearchEngineResultsLookup.Parsers
{
    public interface IParser
    {
        IEnumerable<string> ParseResults(string rawBody, string provider);
    }
}
