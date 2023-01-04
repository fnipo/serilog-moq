netcoreapp3.0 and csharp8 introduced Interface Default Methods

Default interface methods on ILogger available since [PR #1435 - Default ILogger implementations](https://github.com/serilog/serilog/pull/1435)

Interface default methods are read by Moq since ticket [PR #972 - Interface Default methods are ignored](https://github.com/moq/moq4/issues/972) was completed

# Use cases

- Your Serilog sinks to a logging platform, from where you run queries and/or you build alerts and dashboards from, filtering the message, hence you need to ensure logs are produced with specific values in the message. Furthermore you want to prevent any re-writting from changing the message format and breaking the monitoring.
- Your Serilog sinks to a logging platform, from where you run queries and/or build alerts and dashboards from, filtering by properties, hence you need to ensure logs are produced with these properties. Furthermore you want to prevent any re-writting from changing or removing relevant properties and breaking the monitoring.
- Your Serilog sinks metrics from the log properties and you want to ensure these metrics are produced

# TODO

### Better error messages
When logEvent verification fails, the message doesn't mention what failed at LogEvent level, but only at the Mock Write call

### Adopt It.Is style from Moq
```
_mockLogger.VerifyLogEvent((logEvent =>
    logEvent
        .VerifyLevelIs(LogEventLevel.Information) // no need to It.IsAny? If you want to verify ANY just dont use this verification call
        .VerifyRenderedMessage(It.Is(s => s.Contains("Test")))  // Make Is<string> by default
        .VerifyProperty(
            It.Is(s => s.Equals("PropertyKeyExample")), // Make Is<string> by default
            It.Is<string>(s => s.Contains("PropertyKeyValue")))), // It.IsAny desirable to ignore verifying Value or Key. It.Is<Type> and It.IsNotNull useful to check value is of a specific type or not null as it is object?
Times.Once(),
failMessage: "Error");
```

### Review testcase names
...

### Add summary comments
...

### Make property-based tests leveraging Bogus
...

### Test each parameter on each public function, if null, what to expect, and default values
...

### Consider making VerifyPropertyExists part of LogLevel extensions
One could want to check different properties for many log entries

### Component-level tests
Cover library use cases

# Done

### ~~Refactor code~~
Tests and extensions are mixed up in single files

### ~~Make interface Fluid~~
```
_mockLogger.VerifyLogEvent((logEvent =>
    logEvent
        .VerifyLevelIs(LogEventLevel.Information) // no need to It.IsAny? If you want to verify ANY just dont use this verification call
        .VerifyRenderedMessage(It.Is(s => s.Contains("Test")))  // Make Is<string> by default
        .VerifyProperty(
            It.Is(s => s.Equals("PropertyKeyExample")), // Make Is<string> by default
            It.Is<string>(s => s.Contains("PropertyKeyValue")))), // It.IsAny desirable to ignore verifying Value or Key. It.Is<Type> and It.IsNotNull useful to check value is of a specific type or not null as it is object?
Times.Once(),
failMessage: "Error");
```

### ~~Validate Properties define by Enrichers, WithProperty, and PushProperty~~
>Aborted: Won't validate application-wide settings such as enrichers, focus is on testing from function-level message and properties up to class level Context properties
>
ForContext add an Enricher to the Logger
PushProperty and WithProperty probably does the same by configing a new Logger with more Enrichers
Logger.Write -> Logger.Dispatch -> which get Enrichers and act in-place on the LogLevel probably adding the properties!
Logger.Dispatch then calls _sink.Emit with the final LogEvent!
Emit would be the best function to mock, but not sure how to mock _sync a private field inside Logger
It must not be possible to validate all Properties, because they might come from Enrichers, both define in code or appsettings, from WithProperty, from PushProperty, that area outside Logger scope

# Author
[felipenipo.com](http://felipenipo.com)