using Moq;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serilog.Moq
{
    public static class MockLoggerExtensions
    {
        public static void VerifyLogEvent(this Mock<ILogger> loggerMock,
            Func<LogEvent, bool> logEventMatcher,
            Times? times = null,
            string failMessage = null)
        {
            var timesValue = Times.Once();
            if (times.HasValue)
            {
                timesValue = times.Value;
            }

            loggerMock.Verify(
                logger => logger.Write(It.Is<LogEvent>(logEvent => logEventMatcher(logEvent))),
                timesValue,
                failMessage);
        }

        public static void VerifyForContext(this Mock<ILogger> loggerMock,
            string propertyKey,
            object propertyValue)
        {
            loggerMock.Verify(
                // ILogger ForContext(string propertyName, object? value, bool destructureObjects = false);
                logger => logger.ForContext(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<bool>()));
        }
    }
}
