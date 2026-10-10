using System;
using System.Collections.Generic;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using Inno.Content;
using Inno.Content.Testing;
using Inno.Core.Logging;
using Inno.Core.Serialization;
using Xunit;

namespace Inno.Runtime.Tests;

public sealed partial class PlayerWithoutFileSystemTests
{
    [Fact]
    public void PlayerSessionsReadOwnedMemoryContentAndRetireOnlyTheirOwnLogSinks()
    {
        using EngineHost host = CreateHost();
        using SerializationGeneration serialization = host.serialization.CaptureGeneration();
        byte[] catalog = serialization.Encode(writer =>
        {
            writer.Write("revision", 0L);
            writer.Write("entries", Array.Empty<byte[]>());
        });
        using var content = new ContentTestStore(new Dictionary<ContentKey, byte[]>
        {
            [new ContentKey("AssetDatabase/Catalog.snapshot")] = catalog
        });
        var firstLog = new MemoryLog();
        var secondLog = new MemoryLog();
        using RuntimeSession first = host.CreateSession(new RuntimeSessionOptions
        {
            kind = RuntimeSessionKind.Player,
            applicationId = "memory.first",
            contentStore = content,
            jobExecutionMode = RuntimeJobExecutionMode.SingleThread,
            createLogSink = _ => firstLog
        });
        using RuntimeSession second = host.CreateSession(new RuntimeSessionOptions
        {
            kind = RuntimeSessionKind.Player,
            applicationId = "memory.second",
            contentStore = content,
            jobExecutionMode = RuntimeJobExecutionMode.SingleThread,
            createLogSink = _ => secondLog
        });
        using (first.EnterExecutionScope())
            Log.Info("first");
        using (second.EnterExecutionScope())
            Log.Info("second");
        first.Tick(0.01f);
        second.Tick(0.02f);
        Assert.Equal("first", Assert.Single(firstLog.messages));
        Assert.Equal("second", Assert.Single(secondLog.messages));
        first.Dispose();
        Assert.Equal(1, firstLog.disposals);
        Assert.Equal(0, secondLog.disposals);
        using ContentReadLease retained = content.Acquire(new ContentKey("AssetDatabase/Catalog.snapshot"));
        second.Dispose();
        Assert.Equal(1, secondLog.disposals);
        Assert.Throws<ObjectDisposedException>(() => second.Tick(0.01f));
    }

    [Fact]
    public void AFailingHostLogFactoryCompensatesStartupAndKeepsTheHostUsable()
    {
        using EngineHost host = CreateHost();
        Assert.Throws<InvalidOperationException>(() => host.CreateSession(new RuntimeSessionOptions
        {
            kind = RuntimeSessionKind.Play,
            applicationId = "memory.invalid",
            jobExecutionMode = RuntimeJobExecutionMode.SingleThread,
            createLogSink = _ => null!
        }));
        host.generations.EnsureReady("start after rejected log factory");
        using RuntimeSession valid = host.CreateSession(new RuntimeSessionOptions
        {
            kind = RuntimeSessionKind.Play,
            applicationId = "memory.valid",
            jobExecutionMode = RuntimeJobExecutionMode.SingleThread
        });
        valid.Tick(0.01f);
    }

    private static EngineHost CreateHost() => new EngineHostBuilder()
        .UseMetadataSources(new DotNetAssemblyCatalogSource(typeof(PlayerWithoutFileSystemTests).Assembly),
            new ReflectionTypeCatalogSource(), new ReflectionSerializationMetadataSource())
        .UseLogDelivery(LogDeliveryMode.Inline)
        .Build();

    private sealed class MemoryLog : ILogSink, IDisposable
    {
        internal readonly List<string> messages = [];
        internal int disposals;

        public void Receive(LogEntry entry) => messages.Add(entry.message);
        public void Dispose() => disposals++;
    }
}
