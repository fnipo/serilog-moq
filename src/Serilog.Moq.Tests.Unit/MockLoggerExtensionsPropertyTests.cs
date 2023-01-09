using Moq;
using Serilog.Events;
using Xunit;

namespace Serilog.Moq.Tests.Unit
{
    // A field name must exist, either in Context or Message Properties

    // A field name and its value, either in Context or Message Properties, must match

    // There must be a field of any name holding this value, either in Context or Message Properties

    public class MockLoggerExtensionsPropertyTests
    {
        private readonly Mock<ILogger> _mockLogger;
        private readonly ILogger _logger;

        public MockLoggerExtensionsPropertyTests()
        {
            _mockLogger = LoggerMockConfiguration.CreateILoggerMock();
            _logger = _mockLogger.Object;
        }

        [Fact]
        public void GivenInformation_WhenMessageWithPropertiesApplied_PropertiesKeysShouldExist()
        {
            // Act
            _logger.Information("{Prefix} This is a {Type} {Property1}", "Hi!", "Unit", "Test");

            // Assert
            _mockLogger.VerifyPropertyExists(key => key.Equals("Prefix"))
                .VerifyPropertyExists(key => key.Equals("Type"))
                .VerifyPropertyExists(key => key.Equals("Property1"))
                .VerifyPropertyExists(key => key.Equals("A"));
        }

        [Fact]
        public void GivenInformation_WhenMessageWithPropertiesApplied_PropertiesKeyValuePairsShouldExist()
        {
            // Act
            _logger.Information("{Prefix} This is {Count} {Type} {Property1}", "Hi!", 1, "Unit", "Test");

            // Assert
            _mockLogger.VerifyPropertyExists<string>(key => key.Equals("Prefix"), value => value.Equals("Hi!"))
                .VerifyPropertyExists<int>(key => key.Equals("Count"), value => value == 1)
                .VerifyPropertyExists<string>(key => key.Equals("Type"), value => value == "Unit")
                .VerifyPropertyExists<string>(key => key.Equals("Property1"), value => value == "Test");
        }

        [Fact]
        public void GivenInformation_WhenContextsApplied_PropertiesKeyValuePairsShouldExist()
        {
            // Act
            _logger
                .ForContext<MockLoggerExtensionsPropertyTests>()
                .ForContext("Property1", "Moq")
                .ForContext("Property2", "Unit")
                .ForContext("Property3", 1)
                .ForContext("Property4", 1.8)
                .Information("{Property5}", "Test");

            // Assert
            _mockLogger.VerifyPropertyExists(key => key.Equals(nameof(MockLoggerExtensionsPropertyTests)))
                .VerifyPropertyExists<string>(key => key.Equals("Property1"), value => value == "Moq")
                .VerifyPropertyExists<string>(key => key.Equals("Property2"), value => value == "Unit")
                .VerifyPropertyExists<int>(key => key.Equals("Property3"), value => value == 1)
                .VerifyPropertyExists<double>(key => key.Equals("Property4"), value => value == 1.8)
                .VerifyPropertyExists<string>(key => key.Equals("Property5"), value => value == "Test");
        }
    }
}
