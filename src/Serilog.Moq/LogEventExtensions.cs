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

        public static bool VerifyPropertyExists(this LogEvent logEvent, Func<string, object, bool> propertyMatcher)
        {
            if (propertyMatcher == null)
            {
                return true;
            }

            return logEvent.Properties.Any(property =>
            {
                var value = property.Value;
                var scalarValue = (ScalarValue)value;

                // TODO: This fails for integer, how to compare?
                //if (scalarValue.Value != propertyKeyValuePair.Value)
                //{
                //    return false;
                //}

                //var value = p.ToString().Replace("\"", String.Empty);

                return propertyMatcher(property.Key, scalarValue.Value);
            });
        }
    }
}