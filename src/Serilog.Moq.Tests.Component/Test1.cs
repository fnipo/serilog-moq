using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Serilog.Moq.Tests.Component
{
    class Test1
    {
        private readonly ILogger _logger;

        public Test1(ILogger logger)
        {
            _logger = logger.ForContext<Test1>();
        }

        public void Run()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            using (LogContext.PushProperty("A", "B"))
            {
                _logger.Information("Test");
            }
            _logger.ForContext("ElapsedMilliseconds", stopWatch.ElapsedMilliseconds)
                .Information("Start running {Operation}", nameof(Test1));

            try
            {
                _logger.Debug("Producing artificial exception inside class {Class} and function {Function}", nameof(Test1), nameof(Run));
                ProduceException();
            }
            catch (Exception e)
            {
                _logger.Error("An unexpected {ExceptionType} error happened", e.GetType(), e);
            }

            _logger.ForContext("ElapsedMilliseconds", stopWatch.ElapsedMilliseconds)
                .Information("Finished running {Operation}", nameof(Test1));
        }

        public void ProduceException()
        {
            throw new NotImplementedException();
        }
    }
}
