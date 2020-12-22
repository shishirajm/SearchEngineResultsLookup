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
        // I have written Unit tests only for parser, I would say there are lot more tests I can write
        // This is just to showcase, how I think about unit tests.

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
            _mockParser = new Parser(_logger, _parserConfigurations);
        }

        [TearDown]
        public void TearDown()
        {
            _parserConfigurations.ClearReceivedCalls();
        }

        [Test]
        public void ParseResults_ShouldParseTheHtmlToExtractANode_WhenCorrespondingDivStartEndArePassed()
        {
            // Arrange
            _parserConfiguration.NodeStartPattern.Returns("<something someAttr=\"someVal\">");
            _parserConfiguration.DivStartPattern.Returns("<something");
            _parserConfiguration.DivEndPattern.Returns("</something>");
            _parserConfigurations[Arg.Any<string>()].Returns(_parserConfiguration);
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
            _parserConfigurations[Arg.Any<string>()].Returns(_parserConfiguration);
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
        public void ParseResults_ShouldNotExtractAnyNode_WhenInvalidHtmlIsPassed()
        {
            // Arrange
            _parserConfiguration.NodeStartPattern.Returns("<something someAttr=\"someVal\">");
            _parserConfiguration.DivStartPattern.Returns("<something");
            _parserConfiguration.DivEndPattern.Returns("</something>");
            _parserConfigurations[Arg.Any<string>()].Returns(_parserConfiguration);
            var rawBody = "<UnwantedJunk><something someAttr=\"someVal\"><a href=\"someUrl\">Some val</a><something></UnwantedJunk>";

            // Act
            var actual = _mockParser.ParseResults(rawBody, "some_provider");

            // Assert
            Assert.AreEqual(0, actual.ToList().Count);
        }

        [Test]
        [TestCase("<somethingThatsAbsent>")]
        [TestCase("")]
        public void ParseResults_ShouldNotExtractAnyNode_WhenUnailableOrInvalidNodePatternHtmlIsPassed(string nodePattern)
        {
            // Arrange
            _parserConfiguration.NodeStartPattern.Returns(nodePattern);
            _parserConfiguration.DivStartPattern.Returns("<something");
            _parserConfiguration.DivEndPattern.Returns("</something>");
            _parserConfigurations[Arg.Any<string>()].Returns(_parserConfiguration);
            var rawBody = "<UnwantedJunk><something someAttr=\"someVal\"><a href=\"someUrl\">Some val</a></something></UnwantedJunk>";

            // Act
            var actual = _mockParser.ParseResults(rawBody, "some_provider");

            // Assert
            Assert.AreEqual(0, actual.ToList().Count);
        }

        [Test]
        public void ParseResults_UseBingParser_WhenProviderIsBing()
        {
            // Arrange
            _parserConfiguration.NodeStartPattern.Returns("<something someAttr=\"someVal\">");
            _parserConfiguration.DivStartPattern.Returns("<something");
            _parserConfiguration.DivEndPattern.Returns("</something>");
            var rawBody = "<UnwantedJunk><something someAttr=\"someVal\"><a href=\"someUrl\">Some val</a></something></UnwantedJunk>";
            _parserConfigurations[SearchProviders.Bing].Returns(_parserConfiguration);
            _parserConfigurations[SearchProviders.Google].Returns(_parserConfiguration);

            // Act
            var actual = _mockParser.ParseResults(rawBody, SearchProviders.Bing);

            // Assert
            _parserConfigurations[SearchProviders.Bing].ReceivedCalls();
            _parserConfigurations[SearchProviders.Google].DidNotReceive();
        }

        [Test]
        [TestCase(SearchProviders.Google)]
        [TestCase("AnyThing")]
        public void ParseResults_UseGoogleParser_WhenProviderAnythingOtherThanBing(string provider)
        {
            // Arrange
            _parserConfiguration.NodeStartPattern.Returns("<something someAttr=\"someVal\">");
            _parserConfiguration.DivStartPattern.Returns("<something");
            _parserConfiguration.DivEndPattern.Returns("</something>");
            var rawBody = "<UnwantedJunk><something someAttr=\"someVal\"><a href=\"someUrl\">Some val</a></something></UnwantedJunk>";
            _parserConfigurations[SearchProviders.Bing].Returns(_parserConfiguration);
            _parserConfigurations[SearchProviders.Google].Returns(_parserConfiguration);

            // Act
            var actual = _mockParser.ParseResults(rawBody, SearchProviders.Bing);

            // Assert
            _parserConfigurations[SearchProviders.Bing].DidNotReceive();
            _parserConfigurations[SearchProviders.Google].ReceivedCalls();
        }
    }
}
