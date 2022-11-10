using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Serilog.Moq
{
    public static class LogEventExtensions
    {
        public static bool VerifyLevel(
            this LogEvent logEvent,
            LogEventLevel logEventLevel)
        {
            return logEvent.Level == logEventLevel;
        }

        public static bool VerifyLogIsEquals(
            this LogEvent logEvent,
            string message,
            StringComparison comparisonType = StringComparison.InvariantCultureIgnoreCase)
        {
            if (String.IsNullOrEmpty(message))
            {
                return true;
            }

            string renderedMessage = logEvent.RenderMessage();
            // TODO: Can this break some valid string containing \"?
            var unescapedRenderedMessage = renderedMessage.ToString().Replace("\"", String.Empty);
            return String.Equals(unescapedRenderedMessage, message, comparisonType);
        }

        public static bool VerifyLogContains(
            this LogEvent logEvent,
            string message,
            StringComparison comparisonType = StringComparison.InvariantCultureIgnoreCase)
        {
            if (String.IsNullOrEmpty(message))
            {
                return true;
            }

            string renderedMessage = logEvent.RenderMessage();
            var unescapedRenderedMessage = renderedMessage.ToString().Replace("\"", String.Empty);
            return unescapedRenderedMessage.Contains(message, comparisonType);
        }

        public static bool VerifyPropertiesKeysExist(
            this LogEvent logEvent,
            string[] propertiesKeys)
        {
            foreach (var propertyKey in propertiesKeys)
            {
                if (!logEvent.Properties.ContainsKey(propertyKey))
                {
                    return false;
                }
            }
            return true;

            //var isMessageInPropertiesValues = logEvent.Properties.Values.Any(p =>
            //{
            //    var scalarValue = (ScalarValue)p;
            //    //var value = p.ToString().Replace("\"", String.Empty);
            //    //return string.Equals(propertyValue, value, comparisonType);
            //    return scalarValue.Value == propertyValue;
            //});
            //if (isMessageInPropertiesValues)
            //{
            //    return true;
            //}

            //return false;
        }

        public static bool VerifyPropertiesKeyValuePairsExist(
            this LogEvent logEvent,
            IDictionary<string, object> propertiesKeyValuePairs)
        {
            foreach (var propertyKeyValuePair in propertiesKeyValuePairs)
            {
                if (!logEvent.Properties.ContainsKey(propertyKeyValuePair.Key))
                {
                    return false;
                }

                var value = logEvent.Properties[propertyKeyValuePair.Key];
                var scalarValue = (ScalarValue) value;

                // TODO: This fails for integer, how to compare?
                if (scalarValue.Value != propertyKeyValuePair.Value)
                {
                    return false;
                }
            }
            return true;
        }

            //public static bool VerifyMessageContains(
            //    this LogEvent logEvent,
            //    string message,
            //    StringComparison comparisonType = StringComparison.InvariantCultureIgnoreCase)
            //{
            //    if (String.IsNullOrEmpty(message))
            //    {
            //        return true;
            //    }

            //    var isMessageInTemplate = logEvent.MessageTemplate.ToString().Contains(message, StringComparison.InvariantCultureIgnoreCase);
            //    if (isMessageInTemplate)
            //    {
            //        return true;
            //    }

            //    var isMessageInPropertiesValues = VerifyPropertyValue(logEvent, message);
            //    if (isMessageInPropertiesValues)
            //    {
            //        return true;
            //    }

            //    return false;
            //}
        }
}
