using System;
using System.Linq;
using Autofac.Features.Indexed;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using SearchEngineResultsLookup.Parsers;
using SearchEngineResultsLookup.Providers;

namespace SearchEngineResultsLookupTest.Parsers
{
    public class ParserTests
    {

        private ILogger<Parser> _logger;
        private IIndex<string, IParserConfiguration> _parserConfigurations;
        private IParserConfiguration _parserConfiguration;
        private Parser _mockParser;

        [SetUp]
        public void Setup()
        {
            _logger = Substitute.For<ILogger<Parser>>();
            _parserConfigurations = Substitute.For<IIndex<string, IParserConfiguration>>();
            _parserConfiguration = Substitute.For<IParserConfiguration>();
            _parserConfigurations[Arg.Any<string>()].Returns(_parserConfiguration);
            _mockParser = new Parser(_logger, _parserConfigurations);
        }

        [Test]
        public void ParseResults_ShouldParseTheHtmlToExtractANode_WhenCorrespondingDivStartEndArePassed()
        {
            // Arrange
            _parserConfiguration.NodeStartPattern.Returns("<something someAttr=\"someVal\">");
            _parserConfiguration.DivStartPattern.Returns("<something");
            _parserConfiguration.DivEndPattern.Returns("</something>");
            var rawBody = "<UnwantedJunk><something someAttr=\"someVal\"><a href=\"someUrl\">Some val</a></something></UnwantedJunk>";
            var expected = "<something someAttr=\"someVal\"><a href=\"someUrl\">Some val</a></something>";

            // Act
            var actual = _mockParser.ParseResults(rawBody, "some_provider");

            // Assert
            Assert.AreEqual(1, actual.ToList().Count);
            Assert.AreEqual(expected, actual.First());
        }

        [Test]
        public void ParseResults_ShouldParseTheHtmlToExtractMultipleNode_WhenCorrespondingDivStartEndArePassed()
        {
            // Arrange
            _parserConfiguration.NodeStartPattern.Returns("<something someAttr=\"someVal\">");
            _parserConfiguration.DivStartPattern.Returns("<something");
            _parserConfiguration.DivEndPattern.Returns("</something>");
            var rawBody = "<UnwantedJunk><something someAttr=\"someVal\"><a href=\"someUrl\">Some val</a></something><something someAttr=\"someVal\"><a href=\"someUrl\">Some val</a></something></UnwantedJunk>";
            var expected = "<something someAttr=\"someVal\"><a href=\"someUrl\">Some val</a></something>";

            // Act
            var actual = _mockParser.ParseResults(rawBody, "some_provider");

            // Assert
            Assert.AreEqual(2, actual.ToList().Count);
            Assert.AreEqual(expected, actual.First());
            Assert.AreEqual(expected, actual.Last());
        }

        [Test]
        public void ParseResults_ShouldNotExtractANode_WhenInvalidHtmlIsPassed()
        {
            // Arrange
            _parserConfiguration.NodeStartPattern.Returns("<something someAttr=\"someVal\">");
            _parserConfiguration.DivStartPattern.Returns("<something");
            _parserConfiguration.DivEndPattern.Returns("</something>");
            var rawBody = "<UnwantedJunk><something someAttr=\"someVal\"><a href=\"someUrl\">Some val</a><something></UnwantedJunk>";

            // Act
            var actual = _mockParser.ParseResults(rawBody, "some_provider");

            // Assert
            Assert.AreEqual(0, actual.ToList().Count);
        }

    }
}
