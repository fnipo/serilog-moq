using Moq;
using Serilog.Events;
using Serilog.Moq.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Serilog.Moq
{
    public static class MockLoggerExtensions
    {
        /// <summary>
        /// Verify a log event entry
        /// </summary>

        /// <summary>
        /// Verify if any log event entry matches
        /// </summary>
        /// <param name="loggerMock">The mock logger instance</param>
        /// <param name="logEventMatcher">A function that flags when a log event matches</param>
        /// <param name="times">How many matching log entries. Default is Once</param>
        /// <param name="failMessage"></param>
        /// <returns></returns>
        public static Mock<ILogger> VerifyLogEvent(this Mock<ILogger> loggerMock,
            Func<LogEvent, bool> logEventMatcher,
            Times? times = null,
            string? failMessage = null)
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

            return loggerMock;
        }

        private static Mock<ILogger> VerifyForContextInternal<T>(this Mock<ILogger> loggerMock,
            Expression<Func<string, bool>> contextKeyMatcher,
            Expression<Func<T, bool>>? contextValueMatcher = null,
            string? failMessage = null)
        {
            if (contextValueMatcher == null)
            {
                loggerMock.Verify(
                    logger => logger.ForContext(
                        It.Is(contextKeyMatcher),
                        It.IsAny<T>(),
                        It.IsAny<bool>()),
                    failMessage);
            }
            else
            {
                loggerMock.Verify(
                    logger => logger.ForContext(
                        It.Is(contextKeyMatcher),
                        It.Is(contextValueMatcher),
                        It.IsAny<bool>()),
                    failMessage);
            }

            return loggerMock;
        }

        /// <summary>
        /// Verify that a property was setup on the logger context
        /// </summary>
        public static Mock<ILogger> VerifyForContext<T>(this Mock<ILogger> loggerMock,
            Expression<Func<string, bool>> contextKeyMatcher,
            string? failMessage = null)
        {
            return loggerMock.VerifyForContextInternal<object>(contextKeyMatcher, null, failMessage);
        }

        /// <summary>
        /// Verify that a property was setup on the logger context
        /// </summary>
        public static Mock<ILogger> VerifyForContext<T>(this Mock<ILogger> loggerMock,
            Expression<Func<string, bool>> contextKeyMatcher,
            Expression<Func<T, bool>> contextValueMatcher,
            string? failMessage = null)
        {
            return loggerMock.VerifyForContextInternal(contextKeyMatcher, contextValueMatcher, failMessage);
        }

        private static Mock<ILogger> VerifyPropertyExistsInternal<T>(this Mock<ILogger> loggerMock,
            Expression<Func<string, bool>> propertyKeyMatcher,
            Expression<Func<T, bool>>? propertyValueMatcher,
            string? failMessage = null)
        {
            var propertyKeyMatcherFunc = propertyKeyMatcher.Compile();
            var propertyValueMatcherFunc = propertyValueMatcher?.Compile();

            loggerMock.VerifyAny(
                mock => mock.VerifyForContextInternal(propertyKeyMatcher, propertyValueMatcher, failMessage),
                mock => mock.VerifyLogEvent(
                    logEvent => logEvent.VerifyPropertyExists(propertyKeyMatcherFunc, propertyValueMatcherFunc),
                    /*times:*/ Times.Once(),
                    /*failMessage:*/ failMessage));

            return loggerMock;
        }

        /// <summary>
        /// Verify that a property was setup either on the logger context or in the log event properties
        /// </summary>
        public static Mock<ILogger> VerifyPropertyExists(this Mock<ILogger> loggerMock,
            Expression<Func<string, bool>> propertyKeyMatcher,
            string? failMessage = null)
        {
            return loggerMock.VerifyPropertyExistsInternal<object>(propertyKeyMatcher, propertyValueMatcher: null, failMessage);
        }

        /// <summary>
        /// Verify that a property was setup either on the logger context or in the log event properties
        /// </summary>
        public static Mock<ILogger> VerifyPropertyExists<T>(this Mock<ILogger> loggerMock,
            Expression<Func<string, bool>> propertyKeyMatcher,
            Expression<Func<T, bool>> propertyValueMatcher,
            string? failMessage = null)
        {
            return loggerMock.VerifyPropertyExistsInternal(propertyKeyMatcher, propertyValueMatcher, failMessage);
        }
    }
}
