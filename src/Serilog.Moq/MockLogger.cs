using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Moq;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Serilog.Moq
{
    using LogContext = List<(string, object?)>;

    public class MockLogger : ILogger
    {

        private readonly List<MockLogger> _children = new List<MockLogger>();
        private readonly List<(LogContext, LogEvent)> _localInvocations = new List<(LogContext, LogEvent)>();
        private readonly LogContext _localContext;
        private readonly List<ILogEventEnricher> _localEnrichers;
        private readonly ILogEventPropertyFactory _logEventPropertyFactory = new MockLogEventPropertyFactory();
        
        public MockLogger()
        {
            _localContext = new LogContext();
            _localEnrichers = new List<ILogEventEnricher>();
        }

        private MockLogger(LogContext context, List<ILogEventEnricher> enrichers)
        {
            _localContext = new LogContext(context);
            _localEnrichers = new List<ILogEventEnricher>(enrichers);
        }

        public void Write(LogEvent logEvent)
        {
            foreach (var enricher in _localEnrichers)
            {
                enricher.Enrich(logEvent, _logEventPropertyFactory);
            }
            _localInvocations.Add((new LogContext(_localContext), logEvent));
        }

        // TODO: Implement a VerifyInvocation function that iterate children and look for invocations applying a matcher

        // TODO: Implement all ForContext functions
        // it will create a new Context with new property and use to create a child MockLogger
        ILogger ForContext(ILogEventEnricher enricher)
        {
            var childEnrichers = new List<ILogEventEnricher>(_localEnrichers);
            childEnrichers.Add(enricher);
            var childMockLogger = new MockLogger(_localContext, childEnrichers);
            return childMockLogger;
        }

        ILogger ForContext(IEnumerable<ILogEventEnricher> enrichers)
        {
            var childEnrichers = new List<ILogEventEnricher>(_localEnrichers);
            childEnrichers.AddRange(enrichers);
            var childMockLogger = new MockLogger(_localContext, childEnrichers);
            return childMockLogger;
        }

        ILogger ForContext(string propertyName, object value, bool destructureObjects = false)
        {
            // TODO: how to handle destructureObjects?

            var childContext = new List<(string, object?)>(_localContext)
            {
                (propertyName, value)
            };
            var childMockLogger = new MockLogger(childContext, _localEnrichers);
            return childMockLogger;
        }

        ILogger ForContext(Type source)
        {
            var childContext = new List<(string, object?)>(_localContext)
            {
                (source.Name, null)
            };
            var childMockLogger = new MockLogger(childContext, _localEnrichers);
            return childMockLogger;
        }

        ILogger ForContext<TSource>()
        {
            return ForContext(typeof(TSource));
        }
    }
}
}
