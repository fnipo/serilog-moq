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
        #region NewInterface

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

        #endregion NewInterface

        #region OldInterface

        //private static bool VerifyPropertyKeyValuePair(LogEvent logEvent, 
        //    string propertyKey,
        //    string propertyValue,
        //    StringComparison comparisonType = StringComparison.InvariantCultureIgnoreCase)
        //{
        //    if (String.IsNullOrEmpty(propertyKey))
        //    {
        //        return false;
        //    }

        //    if (!logEvent.Properties.ContainsKey(propertyKey))
        //    {
        //        return false;
        //    }

        //    var value = logEvent.Properties[propertyKey].ToString();
        //    var unescapedValue = value.Replace("\"", String.Empty);

        //    return string.Equals(unescapedValue, propertyValue, comparisonType);
        //}

        public static void VerifyForContext(this Mock<ILogger> loggerMock,
            string propertyKey,
            object propertyValue)
        {
            loggerMock.Verify(
                // ILogger ForContext(string propertyName, object? value, bool destructureObjects = false);
                logger => logger.ForContext(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<bool>()));
            
        }

        //public static void VerifyWrite(this Mock<ILogger> loggerMock,
        //    LogEventLevel level,
        //    string message = null,
        //    IDictionary<string, object> properties = null,
        //    Times? times = null,
        //    string failMessage = null)
        //{
        //    var timesValue = Times.Once();
        //    if (times.HasValue)
        //    {
        //        timesValue = times.Value;
        //    }

        //    loggerMock.Verify(
        //        logger => logger.Write(It.Is<LogEvent>(logEvent =>
        //            logEvent.Level == level
        //            && VerifyMessage(logEvent, message))),
        //        //properties
        //        timesValue,
        //        failMessage);
        //}

        #endregion OldInterface
    }
}
