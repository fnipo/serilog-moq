using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            _mockLogger.VerifyWrite(logEvent => true);
        }

        [Fact]
        public void GivenInformation_WhenMessageApplied_LogShouldContainMessage()
        {
            _logger.Information("Test");

            _mockLogger.VerifyWrite(logEvent =>
            {
                return logEvent.VerifyLevel(LogEventLevel.Information)
                    && logEvent.VerifyLogContains("Test");
            });
                
        }

        [Fact]
        public void GivenInformation_WhenMessageApplied_LogShouldBeEqualsMessage()
        {
            _logger.Information("This is a full message test");

            _mockLogger.VerifyWrite(logEvent =>
            {
                return logEvent.VerifyLevel(LogEventLevel.Information)
                    && logEvent.VerifyLogIsEquals("This is a full message test");
            });
                
        }

        [Fact]
        public void GivenInformation_WhenPropertyApplied_LogShouldContainProperty()
        {
            _logger.Information("{Property1}", "Test");

            _mockLogger.VerifyWrite(logEvent =>
            {
                return logEvent.VerifyLevel(LogEventLevel.Information)
                    && logEvent.VerifyLogContains("Test");
            });
        }

        [Fact]
        public void GivenInformation_WhenMessageAndPropertiesApplied_LogShouldContainProperty()
        {
            _logger.Information("{Prefix} This is a {Type} {Property1}", "Hi!", "Unit", "Test");

            _mockLogger.VerifyWrite(logEvent =>
            {
                return logEvent.VerifyLevel(LogEventLevel.Information)
                    && logEvent.VerifyLogContains("Test");
            });
        }

        [Fact]
        public void GivenInformation_WhenMessageAndPropertiesApplied_LogShouldBeCorrect()
        {
            _logger.Information("{Prefix} This is a {Type} {Property1}", "Hi!", "Unit", "Test");

            _mockLogger.VerifyWrite(logEvent =>
            {
                return logEvent.VerifyLevel(LogEventLevel.Information)
                    && logEvent.VerifyLogIsEquals("Hi! This is a Unit Test");
            });
        }

        #endregion LogTests

        #region PropertyTests

        [Fact]
        public void GivenInformation_WhenMessageAndPropertiesApplied_PropertiesKeysShouldExist()
        {
            _logger.Information("{Prefix} This is a {Type} {Property1}", "Hi!", "Unit", "Test");

            _mockLogger.VerifyWrite(logEvent =>
            {
                return logEvent.VerifyLevel(LogEventLevel.Information)
                    && logEvent.VerifyPropertiesKeysExist(new string[] { "Prefix", "Type", "Property1" });
            });
        }

        [Fact]
        public void GivenInformation_WhenMessageAndPropertiesApplied_PropertiesKeyValuePairsShouldExist()
        {
            _logger.Information("{Prefix} This is {Count} {Type} {Property1}", "Hi!", 1, "Unit", "Test");

            _mockLogger.VerifyWrite(logEvent =>
            {
                return logEvent.VerifyLevel(LogEventLevel.Information)
                    && logEvent.VerifyPropertiesKeyValuePairsExist(
                        new Dictionary<string, object> {
                            { "Prefix", "Hi!" },
                            { "Count", 1 },
                            { "Type", "Unit" },
                            { "Property1", "Test" }
                        });
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

            _mockLogger.VerifyWrite(logEvent =>
            {
                return logEvent.VerifyLevel(LogEventLevel.Information)
                    && logEvent.VerifyPropertiesKeyValuePairsExist(
                        new Dictionary<string, object> {
                            { "Property1", "Moq" },
                            { "Property2", "Unit" },
                            { "Property3", 1 },
                            { "Property4", "Test" }
                        });
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

            // TODO: It must not be possible to validate all Properties, because they might come from Enrichers, both define in code or appsettings, from WithProperty, from PushProperty, that area outside Logger scope

            _mockLogger.VerifyLogEvent((logEvent =>
                logEvent
                    .VerifyLevelIs(LogEventLevel.Information) // no need to It.IsAny? If you want to verify ANY just dont use this verification call
                    .VerifyRenderedMessage(It.Is(s => s.Contains("Test")))  // Make Is<string> by default
                    .VerifyProperty(
                        It.Is(s => s.Equals("PropertyKeyExample")), // Make Is<string> by default
                        It.Is<string>(s => s.Contains("PropertyKeyValue")))), // It.IsAny desirable to ignore verifying Value or Key. It.Is<Type> and It.IsNotNull useful to check value is of a specific type or not null as it is object?
            Times.Once(),
            failMessage: "Error");
        }

        #endregion SintaxExperimentation
    }
}
