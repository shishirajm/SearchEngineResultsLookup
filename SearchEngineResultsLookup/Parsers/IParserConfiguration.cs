namespace SearchEngineResultsLookup.Parsers
{
    public interface IParserConfiguration
    {
        string Provider { get; }
        string NodeStartPattern { get; }
        string DivStartPattern { get; }
        string DivEndPattern { get; }
        (bool, string) Filter { get; }
    }
}
