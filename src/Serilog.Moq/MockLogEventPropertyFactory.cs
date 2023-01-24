using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Serilog.Moq
{
    class MockLogEventPropertyFactory : ILogEventPropertyFactory
    {
        public LogEventProperty CreateProperty(string name, object value, bool destructureObjects = false)
        {
            // Create a property
            throw new NotImplementedException();
        }
    }
}
