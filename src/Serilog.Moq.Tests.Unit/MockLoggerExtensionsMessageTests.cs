using Moq;
using Serilog.Events;
using Xunit;

namespace Serilog.Moq.Tests.Unit
{
    // Rendered Message must contain a text

    // Rendered Message must be equals a text

    // ? Rendered Message must be different from

    // ? Rendered Message must be valid according Expression

    public class MockLoggerExtensionsMessageTests
    {
        private readonly Mock<ILogger> _mockLogger;
        private readonly ILogger _logger;

        public MockLoggerExtensionsMessageTests()
        {
            _mockLogger = LoggerMockConfiguration.CreateLoggerMock();
            _logger = _mockLogger.Object;
        }

        [Fact]
        public void GivenInformation_LogShouldHaveCorrectLevel()
        {
            // Act
            _logger.Information("");

            // Assert
            _mockLogger.VerifyLogEvent(logEvent => true);
        }

        [Fact]
        public void GivenInformation_WhenMessageApplied_LogShouldEqualsMessage()
        {
            // Act
            _logger.Information("Test");

            // Assert
            _mockLogger.VerifyLogEvent(logEvent =>
            {
                return logEvent.VerifyRenderedMessageMatches(m => m.Equals("Test"));
            });
        }

        [Fact]
        public void GivenInformation_WhenMessageApplied_LogShouldBeContainsMessage()
        {
            // Act
            _logger.Information("This is a full message test");

            // Assert
            _mockLogger.VerifyLogEvent(logEvent =>
            {
                return logEvent.VerifyRenderedMessageMatches(m => m.Contains("message"));
            });
        }

        [Fact]
        public void GivenInformation_WhenPropertyApplied_LogShouldContainsProperty()
        {
            // Act
            _logger.Information("{Property1}", "Test");

            // Assert
            _mockLogger.VerifyLogEvent(logEvent =>
            {
                return logEvent.VerifyRenderedMessageMatches(m => m.Contains("Test"));
            });
        }

        [Fact]
        public void GivenInformation_WhenMessageAndPropertiesApplied_LogShouldBeCorrect()
        {
            // Act
            _logger.Information("{Prefix} This is a {Type} {Property1}", "Hi!", "Unit", "Test");

            // Assert
            _mockLogger.VerifyLogEvent(logEvent =>
            {
                return logEvent.VerifyRenderedMessageMatches(m => m.Equals("Hi! This is a Unit Test"));
            });
        }
    }
}
