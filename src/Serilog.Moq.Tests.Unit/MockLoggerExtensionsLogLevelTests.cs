using Moq;
using Serilog.Events;
using Xunit;

namespace Serilog.Moq.Tests.Unit
{
    public class MockLoggerExtensionsLogLevelTests
    {
        private readonly Mock<ILogger> _mockLogger;
        private readonly ILogger _logger;

        public MockLoggerExtensionsLogLevelTests()
        {
            _mockLogger = LoggerMockConfiguration.CreateLoggerMock();
            _logger = _mockLogger.Object;
        }

        [Fact]
        public void GivenVerbose_LogShouldHaveCorrectLevel()
        {
            // Act
            _logger.Verbose("");

            // Assert
            _mockLogger.VerifyLogEvent(logEvent => logEvent.VerifyLevelIs(LogEventLevel.Verbose));
        }

        [Fact]
        public void GivenDebug_LogShouldHaveCorrectLevel()
        {
            // Act
            _logger.Debug("");

            // Assert
            _mockLogger.VerifyLogEvent(logEvent => logEvent.VerifyLevelIs(LogEventLevel.Debug));
        }

        [Fact]
        public void GivenInformation_LogShouldHaveCorrectLevel()
        {
            // Act
            _logger.Information("");

            // Assert
            _mockLogger.VerifyLogEvent(logEvent => logEvent.VerifyLevelIs(LogEventLevel.Information));
        }

        [Fact]
        public void GivenWarning_LogShouldHaveCorrectLevel()
        {
            // Act
            _logger.Warning("");

            // Assert
            _mockLogger.VerifyLogEvent(logEvent => logEvent.VerifyLevelIs(LogEventLevel.Warning));
        }

        [Fact]
        public void GivenError_LogShouldHaveCorrectLevel()
        {
            // Act
            _logger.Error("");

            // Assert
            _mockLogger.VerifyLogEvent(logEvent => logEvent.VerifyLevelIs(LogEventLevel.Error));
        }

        [Fact]
        public void GivenFatal_LogShouldHaveCorrectLevel()
        {
            // Act
            _logger.Fatal("");

            // Assert
            _mockLogger.VerifyLogEvent(logEvent => logEvent.VerifyLevelIs(LogEventLevel.Fatal));
        }
    }
}
