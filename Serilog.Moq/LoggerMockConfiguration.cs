using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Serilog.Moq
{
    public static class LoggerMockConfiguration
    {
        public static Mock<ILogger> CreateLoggerMock()
        {
            // TODO: Not a good strategy to encapsulate and limit construction of Mock
            // Maybe don't need Loose if I Setup what is needed?
            var loggerMock = new Mock<ILogger>(MockBehavior.Loose)
            {
                CallBase = true
            };

            loggerMock.Setup(p => p.ForContext<It.IsAnyType>())
                .Returns(loggerMock.Object);

            loggerMock.Setup(p => p.ForContext(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<bool>()))
                .Returns(loggerMock.Object);

            return loggerMock;
        }
    }
}
