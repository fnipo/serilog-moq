using Moq;
using Serilog.Context;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Serilog.Moq.Tests.Unit
{
    public class ExtensionsTests
    {
        private readonly Mock<ILogger> _mockLogger;
        private readonly ILogger _logger;

        public ExtensionsTests()
        {
            _mockLogger = LoggerMockConfiguration.CreateLoggerMock();
            _logger = _mockLogger.Object;
        }

        // TODO: Test each parameter on each public function, if null, what to expect, and default values
        // TODO: Adopt property-based tests, leverage Bogus

        #region LogTests

        [Fact]
        public void GivenInformation_LogShouldHaveCorrectLevel()
        {
            _logger.Information("");

            _mockLogger.VerifyLogEvent(logEvent => true);
        }

        [Fact]
        public void GivenInformation_WhenMessageApplied_LogShouldEqualsMessage()
        {
            _logger.Information("Test");

            _mockLogger.VerifyLogEvent(logEvent =>
            {
                return logEvent.VerifyLevelIs(LogEventLevel.Information)
                    && logEvent.VerifyRenderedMessageMatches(m => m.Equals("Test"));
            });
        }

        [Fact]
        public void GivenInformation_WhenMessageApplied_LogShouldBeContainsMessage()
        {
            _logger.Information("This is a full message test");

            _mockLogger.VerifyLogEvent(logEvent =>
            {
                return logEvent.VerifyLevelIs(LogEventLevel.Information)
                    && logEvent.VerifyRenderedMessageMatches(m => m.Contains("message"));
            });
        }

        [Fact]
        public void GivenInformation_WhenPropertyApplied_LogShouldContainsProperty()
        {
            _logger.Information("{Property1}", "Test");

            _mockLogger.VerifyLogEvent(logEvent =>
            {
                return logEvent.VerifyLevelIs(LogEventLevel.Information)
                    && logEvent.VerifyRenderedMessageMatches(m => m.Contains("Test"));
            });
        }

        [Fact]
        public void GivenInformation_WhenMessageAndPropertiesApplied_LogShouldBeCorrect()
        {
            _logger.Information("{Prefix} This is a {Type} {Property1}", "Hi!", "Unit", "Test");

            _mockLogger.VerifyLogEvent(logEvent =>
            {
                return logEvent.VerifyLevelIs(LogEventLevel.Information)
                    && logEvent.VerifyRenderedMessageMatches(m => m.Equals("Hi! This is a Unit Test"));
            });
        }

        #endregion LogTests

        #region PropertyTests

        [Fact]
        public void GivenInformation_WhenMessageWithPropertiesApplied_PropertiesKeysShouldExist()
        {
            using (LogContext.PushProperty("A", 1))
            {

            }
            _logger.Information("{Prefix} This is a {Type} {Property1}", "Hi!", "Unit", "Test");

            _mockLogger.VerifyLogEvent(logEvent => logEvent.VerifyLevelIs(LogEventLevel.Information))
                .VerifyPropertyExists(key => key.Equals("Prefix"))
                .VerifyPropertyExists(key => key.Equals("Type"))
                .VerifyPropertyExists(key => key.Equals("Property1"));
        }

        [Fact]
        public void GivenInformation_WhenMessageWithPropertiesApplied_PropertiesKeyValuePairsShouldExist()
        {
            _logger.Information("{Prefix} This is {Count} {Type} {Property1}", "Hi!", 1, "Unit", "Test");

            _mockLogger.VerifyLogEvent(logEvent => logEvent.VerifyLevelIs(LogEventLevel.Information))
                .VerifyPropertyExists<string>(key => key.Equals("Prefix"), value => value.Equals("Hi!"))
                .VerifyPropertyExists<int>(key => key.Equals("Count"), value => value == 1)
                .VerifyPropertyExists<string>(key => key.Equals("Type"), value => value == "Unit")
                .VerifyPropertyExists<string>(key => key.Equals("Property1"), value => value == "Test");
        }

        [Fact]
        public void GivenInformation_WhenContextsApplied_PropertiesKeyValuePairsShouldExist()
        {
            _logger
                .ForContext<ExtensionsTests>()
                .ForContext("Property1", "Moq")
                .ForContext("Property2", "Unit")
                .ForContext("Property3", 1)
                .ForContext("Property4", 1.8)
                .Information("{Property5}", "Test");

            _mockLogger
                .VerifyLogEvent(logEvent => logEvent.VerifyLevelIs(LogEventLevel.Information))
                .VerifyPropertyExists<string>(key => key.Equals("Property1"), value => value == "Moq")
                .VerifyPropertyExists<string>(key => key.Equals("Property2"), value => value == "Unit")
                .VerifyPropertyExists<int>(key => key.Equals("Property3"), value => value == 1)
                .VerifyPropertyExists<double>(key => key.Equals("Property4"), value => value == 1.8)
                .VerifyPropertyExists<string>(key => key.Equals("Property5"), value => value == "Test");
        }

        #endregion PropertyTests

        #region RenderedMessageUseCases

        // Rendered Message must contain a text

        // Rendered Message must be equals a text

        // ? Rendered Message must be different from

        // ? Rendered Message must be valid according Expression

        #endregion RenderedMessageUseCases

        #region FieldExistUseCases

        // A field name must exist, either in Context or Message Properties

        // A field name and its value, either in Context or Message Properties, must match

        // There must be a field of any name holding this value, either in Context or Message Properties

        // ? A field name and its value, either in Context or Message Properties, must be valid according Expression

        #endregion FieldExistUseCases
    }
}
