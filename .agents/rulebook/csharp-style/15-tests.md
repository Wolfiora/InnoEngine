---
paths: ["**/*.cs"]
tags: [csharp-style]
---
# 15. Tests

## TS-01 Structure

- Tests use xUnit. A test class is `public sealed class XxxTests` in `XxxTests.cs`, in the namespace of the test project.
- A test method is `public`, returns `void` or `async Task`, and carries `[Fact]`, or `[Theory]` with `[InlineData]`.
- Test classes and test methods do not need XML documentation.
- Fixtures and test doubles follow the same naming and layout rules as product code. A nested test double is `private sealed` and placed at the end of the test class.

```csharp
using System;
using Xunit;
using Inno.Core.Events;

namespace Inno.Core.Events.Tests;

public sealed class EventDispatcherTests
{
    [Fact]
    public void NewDispatcherHasNoSinks()
    {
        var dispatcher = new EventDispatcher();

        Assert.Empty(dispatcher.sinks);
    }

    private sealed class RecordingSink : IEventSink
    {
        public int published { get; private set; }

        public void Publish(EventRecord record) => published++;
    }
}
```

Avoid:

```csharp
public class EventDispatcherTest
{
    [Fact]
    void NewDispatcherHasNoSinks() {}
}
```

## TS-02 Test method names

A test method name is a PascalCase statement of what happens under which condition. It has no underscores, no `Async` suffix and no `Test` prefix or suffix.

```csharp
public sealed class EventDispatcherTests
{
    [Fact]
    public void DispatchAfterDisposeThrowsObjectDisposedException()
    {
        var dispatcher = new EventDispatcher();
        dispatcher.Dispose();

        Assert.Throws<ObjectDisposedException>(() => dispatcher.Dispatch(new EventRecord()));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void NonPositiveCapacityIsRejected(int capacity)
        => Assert.Throws<ArgumentOutOfRangeException>(() => new EventDispatcher(capacity));

    [Fact]
    public async Task PendingEventsAreDeliveredBeforeShutdownCompletes()
    {
        var dispatcher = new EventDispatcher();
        List<EventRecord> received = [];
        dispatcher.dispatched += received.Add;

        dispatcher.Dispatch(new EventRecord());
        await dispatcher.ShutdownAsync(CancellationToken.None);

        Assert.Single(received);
    }
}
```

Avoid:

```csharp
public sealed class EventDispatcherTests
{
    [Fact]
    public void Dispatch_AfterDispose_Throws()
    {
        var dispatcher = new EventDispatcher();
        dispatcher.Dispose();
    }

    [Fact]
    public async Task TestShutdownAsync()
    {
        var dispatcher = new EventDispatcher();
        await dispatcher.ShutdownAsync(CancellationToken.None).ConfigureAwait(false);
    }
}
```

## TS-03 Assertions and layout

- Separate the arrange, act and assert phases with one blank line. A very short test may omit the separation.
- `Assert.Equal(expected, actual)`: the expected value comes first.
- Source text used as test data (generator inputs and expected outputs) is data, not code; this rulebook does not apply to it.

```csharp
[Fact]
public void ResizeUpdatesBothDimensions()
{
    var viewport = new Viewport(m_target, "Main", 640);

    viewport.Resize(width: 1280, height: 720);

    Assert.Equal(1280, viewport.width);
    Assert.Equal(720, viewport.height);
}
```

Avoid (actual value first, phases not separated):

```csharp
[Fact]
public void ResizeUpdatesBothDimensions()
{
    var viewport = new Viewport(m_target, "Main", 640);
    viewport.Resize(width: 1280, height: 720);
    Assert.Equal(viewport.width, 1280);
}
```
