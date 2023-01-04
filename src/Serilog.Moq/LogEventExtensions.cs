using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Serilog.Moq
{
    public static class LogEventExtensions
    {
        public static bool VerifyLevelIs(this LogEvent logEvent, LogEventLevel logEventLevel)
        {
            return logEvent.Level == logEventLevel;
        }

        public static bool VerifyRenderedMessageMatches(this LogEvent logEvent, Func<string, bool> messageMatcher)
        {
            if (messageMatcher == null)
            {
                return true;
            }

            string renderedMessage = logEvent.RenderMessage();
            // TODO: Can this break some valid string containing \"?
            var unescapedRenderedMessage = renderedMessage.ToString().Replace("\"", String.Empty);
            return messageMatcher(unescapedRenderedMessage);
        }

        internal static bool VerifyPropertyExists<T>(this LogEvent logEvent,
            Func<string, bool> propertyKeyMatcher,
            Func<T, bool>? propertyValueMatcher = null)
        {
            return logEvent.Properties.Any(property =>
            {
                var value = property.Value;
                var scalarValue = (ScalarValue)value;

                var castResult = scalarValue.Value is T;
                if (!castResult)
                {
                    return false;
                }

                if (propertyValueMatcher == null)
                {
                    return propertyKeyMatcher(property.Key);
                }
                else
                {
                    var castedValue = (T)scalarValue.Value;
                    return propertyKeyMatcher(property.Key) && propertyValueMatcher(castedValue);
                }
            });
        }
    }
}