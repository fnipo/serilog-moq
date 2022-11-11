using Moq;
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
            _logger.Information("{Prefix} This is a {Type} {Property1}", "Hi!", "Unit", "Test");

            _mockLogger.VerifyLogEvent(logEvent =>
            {
                return logEvent.VerifyLevelIs(LogEventLevel.Information)
                    && logEvent.VerifyPropertyExists((key, _value) => key.Equals("Prefix"))
                    && logEvent.VerifyPropertyExists((key, _value) => key.Equals("Type"))
                    && logEvent.VerifyPropertyExists((key, _value) => key.Equals("Property1"));
            });
        }

        [Fact]
        public void GivenInformation_WhenMessageWithPropertiesApplied_PropertiesKeyValuePairsShouldExist()
        {
            _logger.Information("{Prefix} This is {Count} {Type} {Property1}", "Hi!", 1, "Unit", "Test");

            _mockLogger.VerifyLogEvent(logEvent =>
            {
                return logEvent.VerifyLevelIs(LogEventLevel.Information)
                    && logEvent.VerifyPropertyExists((key, value) => key.Equals("Prefix") && value == "Hi!")
                    // TODO: Need better interface than object
                    && logEvent.VerifyPropertyExists((key, value) => key.Equals("Count") && value.ToString() == 1.ToString())
                    && logEvent.VerifyPropertyExists((key, value) => key.Equals("Type") && value == "Unit")
                    && logEvent.VerifyPropertyExists((key, value) => key.Equals("Property1") && value == "Test");
            });
        }

        // TODO: Context doesn't go to LogEvent level, how to verify it?
        [Fact]
        public void GivenInformation_WhenContextsApplied_PropertiesKeyValuePairsShouldExist()
        {
            _logger
                .ForContext<ExtensionsTests>()
                .ForContext("Property1", "Moq")
                .ForContext("Property2", "Unit")
                .ForContext("Property3", 1)
                .Information("{Property4}", "Test");

            _mockLogger.VerifyLogEvent(logEvent =>
            {
                return logEvent.VerifyLevelIs(LogEventLevel.Information)
                    && logEvent.VerifyPropertyExists((key, value) => key.Equals("Property1") && value == "Moq")
                    && logEvent.VerifyPropertyExists((key, value) => key.Equals("Property2") && value == "Unit")
                    && logEvent.VerifyPropertyExists((key, value) => key.Equals("Property3") && value.ToString() == 1.ToString())
                    && logEvent.VerifyPropertyExists((key, value) => key.Equals("Property4") && value == "Test");
            });
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

        #region SintaxExperimentation

        public void Experimentation1()
        {
            _logger.Information("");

            _mockLogger.VerifyLogEvent(
                logEvent =>
                    logEvent.VerifyLevelIs(LogEventLevel.Information)
                    && logEvent.VerifyRenderedMessageMatches(s => s.Contains("Test"))
                    && logEvent.VerifyPropertyExists((key, value) =>
                        key.Equals("PropertyKeyExample")
                        && value == "PropertyKeyValue"),
                Times.Once(),
                failMessage: "Error");
        }

        #endregion SintaxExperimentation
    }
}
