using SearchEngineResultsLookup.Providers;

namespace SearchEngineResultsLookup.Parsers
{
    public class GoogleParserConfiguration : IParserConfiguration
    {
        public string NodeStartPattern => "<div class=\"ZINbbc xpd O9g5cc uUPGi\">";

        public string DivStartPattern => "<div";

        public string DivEndPattern => "</div>";

        public (bool, string) Filter => (true, "/url?q=");
    }
}
