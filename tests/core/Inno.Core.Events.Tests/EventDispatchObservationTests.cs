using System;
using System.Collections.Generic;

using Inno.Core.Events;
using Xunit;

namespace Inno.Core.Events.Tests;

public sealed class EventDispatchObservationTests
{
    [Fact]
    public void ObservationRunsAfterPriorityConsumersIncludingGlobalConsumption()
    {
        var dispatcher = new EventDispatcher();
        using EventHub ui = dispatcher.CreateHub(100);
        using EventHub game = dispatcher.CreateHub();
        var sequence = new List<string>();
        using IDisposable uiSubscription = ui.Listen<TestEvent>(evnt =>
        {
            sequence.Add("ui");
            evnt.HandleInGlobal();
        });
        using IDisposable gameSubscription = game.Listen<TestEvent>(_ => sequence.Add("game"));
        dispatcher.dispatched += evnt => sequence.Add(evnt.isGlobalHandled ? "consumed" : "unhandled");

        dispatcher.Emit(new TestEvent());

        Assert.Equal(["ui", "consumed"], sequence);
    }

    [Fact]
    public void AlreadyConsumedEventsAreObservedAndUnsubscriptionStopsObservation()
    {
        var dispatcher = new EventDispatcher();
        using EventHub hub = dispatcher.CreateHub();
        int received = 0;
        int observed = 0;
        using IDisposable subscription = hub.Listen<TestEvent>(_ => received++);
        Action<Event> observer = evnt =>
        {
            Assert.True(evnt.isGlobalHandled);
            observed++;
        };
        dispatcher.dispatched += observer;
        var evnt = new TestEvent();
        evnt.HandleInGlobal();

        dispatcher.Emit(evnt);
        dispatcher.dispatched -= observer;
        dispatcher.Emit(evnt);

        Assert.Equal(0, received);
        Assert.Equal(1, observed);
    }

    [Fact]
    public void RecursivelyQueuedObservationWaitsForTheNextFlush()
    {
        var dispatcher = new EventDispatcher();
        var first = new TestEvent();
        var second = new TestEvent();
        var observed = new List<Event>();
        dispatcher.dispatched += evnt =>
        {
            observed.Add(evnt);
            if (ReferenceEquals(evnt, first))
                dispatcher.Enqueue(second);
        };
        dispatcher.Enqueue(first);

        dispatcher.Flush();

        Assert.Equal([first], observed);
        Assert.Equal(1, dispatcher.pendingCount);
        dispatcher.Flush();
        Assert.Equal([first, second], observed);
        Assert.Equal(0, dispatcher.pendingCount);
    }

    [Fact]
    public void FailedConsumerDoesNotReportSuccessfulDispatch()
    {
        var dispatcher = new EventDispatcher();
        using EventHub hub = dispatcher.CreateHub();
        using IDisposable subscription = hub.Listen<TestEvent>(_ => throw new InvalidOperationException("consumer"));
        int observed = 0;
        dispatcher.dispatched += _ => observed++;

        InvalidOperationException failure = Assert.Throws<InvalidOperationException>(() => dispatcher.Emit(new TestEvent()));

        Assert.Equal("consumer", failure.Message);
        Assert.Equal(0, observed);
    }

    [Fact]
    public void ObserverFailurePropagatesAfterConsumersWithoutReplayingThem()
    {
        var dispatcher = new EventDispatcher();
        using EventHub hub = dispatcher.CreateHub();
        int received = 0;
        using IDisposable subscription = hub.Listen<TestEvent>(_ => received++);
        dispatcher.dispatched += _ => throw new InvalidOperationException("observer");

        InvalidOperationException failure = Assert.Throws<InvalidOperationException>(() => dispatcher.Emit(new TestEvent()));

        Assert.Equal("observer", failure.Message);
        Assert.Equal(1, received);
    }

    private sealed class TestEvent : Event;
}
