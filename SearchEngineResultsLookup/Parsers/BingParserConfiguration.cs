using SearchEngineResultsLookup.Providers;

namespace SearchEngineResultsLookup.Parsers
{
    public class BingParserConfiguration : IParserConfiguration
    {   
        public string NodeStartPattern => "<li class=\"b_algo\">";

        public string DivStartPattern => "<li";

        public string DivEndPattern => "</li>";

        public (bool, string) Filter => (false, "");
    }
}
