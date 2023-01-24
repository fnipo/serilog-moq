using Moq;
using Serilog.Events;
using System;
using Xunit;

namespace Serilog.Moq.Tests.Component
{
    public class MockLoggerExtensionsTests
    {

    /* ILogger invocations at Assert step
    * -		Invocations	{Moq.InvocationCollection}	Moq.IInvocationList {Moq.InvocationCollection} Results View	Expanding the Results View will enumerate the IEnumerable	
	+		[0]	{ILogger.ForContext<Test1>()}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[1]	{ILogger.ForContext("ElapsedMilliseconds", 0, False)}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[2]	{ILogger.Information<string>("Start running {Operation}", "Test1")}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[3]	{ILogger.Write<string>(LogEventLevel.Information, "Start running {Operation}", "Test1")}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[4]	{ILogger.IsEnabled(LogEventLevel.Information)}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[5]	{ILogger.Write(LogEventLevel.Information, "Start running {Operation}", ["Test1"])}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[6]	{ILogger.Write(LogEventLevel.Information, null, "Start running {Operation}", ["Test1"])}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[7]	{ILogger.IsEnabled(LogEventLevel.Information)}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[8]	{ILogger.BindMessageTemplate("Start running {Operation}", ["Test1"], Start running {Operation}, Enumerable.SelectArrayIterator<EventProperty, LogEventProperty>)}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[9]	{ILogger.Write(LogEvent)}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[10]	{ILogger.Debug<string, string>("Producing artificial exception inside class {Class} and function {Function}", "Test1", "Run")}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[11]	{ILogger.Write<string, string>(LogEventLevel.Debug, "Producing artificial exception inside class {Class} and function {Function}", "Test1", "Run")}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[12]	{ILogger.IsEnabled(LogEventLevel.Debug)}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[13]	{ILogger.Write(LogEventLevel.Debug, "Producing artificial exception inside class {Class} and function {Function}", ["Test1", "Run"])}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[14]	{ILogger.Write(LogEventLevel.Debug, null, "Producing artificial exception inside class {Class} and function {Function}", ["Test1", "Run"])}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[15]	{ILogger.IsEnabled(LogEventLevel.Debug)}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[16]	{ILogger.BindMessageTemplate("Producing artificial exception inside class {Class} and function {Function}", ["Test1", "Run"], Producing artificial exception inside class {Class} and function {Function}, Enumerable.SelectArrayIterator<EventProperty, LogEventProperty>)}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[17]	{ILogger.Write(LogEvent)}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[18]	{ILogger.Error<Type, Exception>("An unexpected {ExceptionType} error happened", System.NotImplementedException, System.NotImplementedException: The method or operation is not implemented.
	at Serilog.Moq.Tests.Component.Test1.ProduceException() in C:\Projects\serilog-moq\src\Serilog.Moq.Tests.Component\Test1.cs:line 41
	at Serilog.Moq.Tests.Component.Test1.Run() in C:\Projects\serilog-moq\src\Serilog.Moq.Tests.Component\Test1.cs:line 28)}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[19]	{ILogger.Write<Type, Exception>(LogEventLevel.Error, "An unexpected {ExceptionType} error happened", System.NotImplementedException, System.NotImplementedException: The method or operation is not implemented.
	at Serilog.Moq.Tests.Component.Test1.ProduceException() in C:\Projects\serilog-moq\src\Serilog.Moq.Tests.Component\Test1.cs:line 41
	at Serilog.Moq.Tests.Component.Test1.Run() in C:\Projects\serilog-moq\src\Serilog.Moq.Tests.Component\Test1.cs:line 28)}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[20]	{ILogger.IsEnabled(LogEventLevel.Error)}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[21]	{ILogger.Write(LogEventLevel.Error, "An unexpected {ExceptionType} error happened", [System.NotImplementedException, System.NotImplementedException: The method or operation is not implemented.
	at Serilog.Moq.Tests.Component.Test1.ProduceException() in C:\Projects\serilog-moq\src\Serilog.Moq.Tests.Component\Test1.cs:line 41
	at Serilog.Moq.Tests.Component.Test1.Run() in C:\Projects\serilog-moq\src\Serilog.Moq.Tests.Component\Test1.cs:line 28])}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[22]	{ILogger.Write(LogEventLevel.Error, null, "An unexpected {ExceptionType} error happened", [System.NotImplementedException, System.NotImplementedException: The method or operation is not implemented.
	at Serilog.Moq.Tests.Component.Test1.ProduceException() in C:\Projects\serilog-moq\src\Serilog.Moq.Tests.Component\Test1.cs:line 41
	at Serilog.Moq.Tests.Component.Test1.Run() in C:\Projects\serilog-moq\src\Serilog.Moq.Tests.Component\Test1.cs:line 28])}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[23]	{ILogger.IsEnabled(LogEventLevel.Error)}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[24]	{ILogger.BindMessageTemplate("An unexpected {ExceptionType} error happened", [System.NotImplementedException, System.NotImplementedException: The method or operation is not implemented.
	at Serilog.Moq.Tests.Component.Test1.ProduceException() in C:\Projects\serilog-moq\src\Serilog.Moq.Tests.Component\Test1.cs:line 41
	at Serilog.Moq.Tests.Component.Test1.Run() in C:\Projects\serilog-moq\src\Serilog.Moq.Tests.Component\Test1.cs:line 28], An unexpected {ExceptionType} error happened, Enumerable.SelectArrayIterator<EventProperty, LogEventProperty>)}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[25]	{ILogger.Write(LogEvent)}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[26]	{ILogger.ForContext("ElapsedMilliseconds", 336, False)}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[27]	{ILogger.Information<string>("Finished running {Operation}", "Test1")}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[28]	{ILogger.Write<string>(LogEventLevel.Information, "Finished running {Operation}", "Test1")}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[29]	{ILogger.IsEnabled(LogEventLevel.Information)}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[30]	{ILogger.Write(LogEventLevel.Information, "Finished running {Operation}", ["Test1"])}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[31]	{ILogger.Write(LogEventLevel.Information, null, "Finished running {Operation}", ["Test1"])}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[32]	{ILogger.IsEnabled(LogEventLevel.Information)}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[33]	{ILogger.BindMessageTemplate("Finished running {Operation}", ["Test1"], Finished running {Operation}, Enumerable.SelectArrayIterator<EventProperty, LogEventProperty>)}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
	+		[34]	{ILogger.Write(LogEvent)}	Moq.IInvocation {Moq.CastleProxyFactory.Invocation}
    */

        [Fact]
        public void Test1_ShouldProduceLogs()
        {
            // Arrange
            var loggerMock = LoggerMockConfiguration.CreateILoggerMock();
            var test1 = new Test1(loggerMock.Object);

            // Act
            test1.Run();

			// Assert
			// TODO: Even with using a nuget Serilog, the Sourcecontext isnt called for some reason, only ForContext<TSource> is called
			// It seems default interface implementations that usues the => operator isnt executed, or isnt recorded in Mock invocations at least, because a lambda might be just an Expression
			loggerMock.VerifyForContext<string>(key => key.Equals("SourceContext"), value => value.Equals(nameof(Test1)));

			loggerMock
				.VerifyLogEvent(logEvent => logEvent.VerifyLevelIs(LogEventLevel.Error))
				.VerifyPropertyExists<string>(key => key.Equals("ExceptionType"), value => value.Equals(nameof(NotImplementedException)));

			// TODO: Verify an Information log for Start was produced with elapsedtime

			// TODO: Verify an Information log for Finish was produced with elapsedtime

			// TODO: Verify an Error log was produced and with some specific Property

			// NB: Needs to verify the property was called with the log event, not for the whole logger object
			// Will probably need more code to keep state per log entry somehow
			// Maybe on each ForContext call and DestructContext(?) keep a list of current active contexts,
			// and on each log call store the list of context at the time
			// then verify happens at log entry level
		}

		private void PlaygroundSinkLoggerToListToVerify()
		{
            new LoggerConfiguration()
				.WriteTo.
                .CreateLogger()
        }
    }
}
