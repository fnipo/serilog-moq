netcoreapp3.0 and csharp8 introduced Interface Default Methods

Default interface methods on ILogger available since [PR #1435 - Default ILogger implementations](https://github.com/serilog/serilog/pull/1435)

Interface default methods are read by Moq since ticket [PR #972 - Interface Default methods are ignored](https://github.com/moq/moq4/issues/972) was completed

# Use cases

- Your Serilog sinks to a logging platform, from where you need to run queries or you build Alerts, filtering the message, hence you need to ensure logs are produced with specific values in the message
- Your Serilog sinks to a logging platform, from where you need to run queries or build alerts, filtering by properties, hence you need to ensure logs are produced with these properties
- Your Serilog sinks metrics from the log properties and you need to ensure a function produce these properties

# TODO

## Validate Properties define by Enrichers, WithProperty, and PushProperty
It must not be possible to validate all Properties, because they might come from Enrichers, both define in code or appsettings, from WithProperty, from PushProperty, that area outside Logger scope

## Better error messages
When logEvent verification fails, the message doesn't mention what failed at LogEvent level, but only at the Mock Write call

## Make interface Fluid
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

## Refactor code
Tests and extensions are mixed up in single files

## Adopt It.Is style from Moq
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

- 