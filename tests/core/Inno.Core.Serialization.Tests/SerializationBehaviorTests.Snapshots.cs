using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Inno.Core.Serialization.Converters;
using Xunit;

namespace Inno.Core.Serialization.Tests;

public sealed partial class SerializationBehaviorTests
{
    [Fact]
    public void BinaryValuesFreezeAtTheirOwningCaptureBoundary()
    {
        byte[] ordinary = m_serialization.Encode(writer =>
        {
            byte[] bytes = [1];
            writer.Write("bytes", bytes);
            bytes[0] = 2;
        });
        Assert.Equal(new byte[] { 1 }, m_serialization.Decode(ordinary, reader => reader.Read<byte[]>("bytes")));

        var value = new MutatingBinaryCapture();
        SerializationSnapshot captured = m_serialization.CaptureSnapshot(value);
        Assert.Equal(new byte[] { 1 }, m_serialization.Deserialize<MutatingBinaryCapture>(captured.ToArray()).bytes);
        value.bytes[0] = 1;
        Assert.Same(captured, m_serialization.CaptureSnapshot(value, previous: captured));
        value.bytes[0] = 9;
        Assert.Equal(new byte[] { 1 }, m_serialization.Deserialize<MutatingBinaryCapture>(captured.ToArray()).bytes);

        var properties = new MutatingPropertyCapture();
        byte[] expected = m_serialization.EncodePropertySnapshots(m_serialization.CaptureProperties(properties));
        properties.Reset();
        Assert.Equal(expected, m_serialization.CapturePropertiesData(properties));
    }

    [Fact]
    public void SnapshotStreamingUsesTheSameBytesAndLeavesTheDestinationOwnedByItsCaller()
    {
        var value = new SnapshotEnvelope();
        value.state.bytes = new byte[128 * 1024];
        SerializationSnapshot snapshot = m_serialization.CaptureSnapshot(value);
        using var stream = new MemoryStream();
        snapshot.WriteTo(stream);
        Assert.Equal(m_serialization.Serialize(value), stream.ToArray());
        Assert.True(stream.CanWrite);
        Assert.Throws<ArgumentNullException>(() => snapshot.WriteTo(null!));
        using var readOnly = new MemoryStream([1, 2, 3], writable: false);
        Assert.Throws<ArgumentException>(() => snapshot.WriteTo(readOnly));
    }

    [Fact]
    public void ConverterTypeQueriesUseTheirPinnedGenerationAndRejectExpiredWriters()
    {
        var value = new SnapshotEnvelope();
        using SerializationGeneration captured = m_serialization.CaptureGeneration();
        _ = captured.Serialize(value);
        long original = value.observedTypeGeneration;
        m_types.Rebuild();
        _ = m_serialization.Serialize(value);
        Assert.True(value.observedTypeGeneration > original);
        _ = captured.Serialize(value);
        Assert.Equal(original, value.observedTypeGeneration);
        SerializationWriter? expired = null;
        _ = m_serialization.Encode(writer => expired = writer);
        Assert.Throws<InvalidOperationException>(() => expired!.typeSnapshot);
    }

    [Fact]
    public void OrdinaryWriterRetainsItsOperationLifetimeAndRejectsDuplicateObjectsBeforeCallbacks()
    {
        bool called = false;
        Assert.Throws<InvalidOperationException>(() => m_serialization.Encode(writer =>
        {
            writer.Write("value", 1);
            writer.WriteObject("value", _ => called = true);
        }));
        Assert.False(called);
        byte[] bytes = m_serialization.Encode(writer =>
        {
            SerializationWriter? child = null;
            writer.WriteObject("nested", value => child = value);
            child!.Write("late", 42);
        });
        Assert.Equal(42, m_serialization.Decode(bytes, reader => reader.ReadObject("nested").Read<int>("late")));
    }

    [Fact]
    public void SnapshotCapturePreservesOrdinaryBytesAndEveryDirectValueChange()
    {
        var value = new DefaultSample { count = 7, ratio = -0f, bytes = [1, 2], values = [3, 4] };
        SerializationSnapshot snapshot = m_serialization.CaptureSnapshot(value);
        byte[] original = snapshot.ToArray();
        Assert.Equal(m_serialization.Serialize(value), original);
        Assert.Same(snapshot, m_serialization.CaptureSnapshot(value, previous: snapshot));
        for (int index = 0; index < 12; index++)
        {
            value.count++;
            value.ratio = BitConverter.Int32BitsToSingle(index % 2 == 0 ? unchecked((int)0x80000000) : 0x7fc00001 + index);
            value.bytes[index % 2]++;
            value.values.Add(index);
            SerializationSnapshot next = m_serialization.CaptureSnapshot(value, previous: snapshot);
            Assert.NotSame(snapshot, next);
            Assert.Equal(m_serialization.Serialize(value), next.ToArray());
            Assert.Same(next, m_serialization.CaptureSnapshot(value, previous: next));
            snapshot = next;
        }
        Assert.NotEqual(original, snapshot.ToArray());
    }

    [Theory]
    [InlineData(3)]
    [InlineData(70)]
    [InlineData(8192)]
    public void SnapshotObjectCaptureHandlesReplacementRemovalAndEmptyObjects(int count)
    {
        var value = new SnapshotEnvelope { omitState = true };
        for (int index = 0; index < count; index++)
            value.fields.Add("item-" + index, index);
        SerializationSnapshot snapshot = m_serialization.CaptureSnapshot(value);
        value.fields.Remove("item-0");
        value.fields.Add("replacement", 91);
        SerializationSnapshot replaced = m_serialization.CaptureSnapshot(value, previous: snapshot);
        Assert.Equal(m_serialization.Serialize(value), replaced.ToArray());
        Assert.NotSame(snapshot, replaced);
        Assert.Same(replaced, m_serialization.CaptureSnapshot(value, previous: replaced));
        value.fields.Clear();
        SerializationSnapshot empty = m_serialization.CaptureSnapshot(value, previous: replaced);
        Assert.Equal(m_serialization.Serialize(value), empty.ToArray());
        Assert.Same(empty, m_serialization.CaptureSnapshot(value, previous: empty));
    }

    [Fact]
    public void SnapshotStateBlobsUseTheExistingPropertyWireFormatAndDetectArrayShrink()
    {
        var value = new SnapshotEnvelope();
        value.items.AddRange([1, 2, 3]);
        value.state.count = 42;
        SerializationSnapshot before = m_serialization.CaptureSnapshot(value);
        byte[] bytes = before.ToArray();
        Assert.Equal(m_serialization.CapturePropertiesData(value.state),
            m_serialization.Decode(bytes, reader => reader.Read<byte[]>("state")));
        value.items.RemoveAt(1);
        value.state.name = "changed";
        SerializationSnapshot after = m_serialization.CaptureSnapshot(value, previous: before);
        Assert.Equal(m_serialization.Serialize(value), after.ToArray());
        Assert.Equal(bytes, before.ToArray());
        Assert.Same(after, m_serialization.CaptureSnapshot(value, previous: after));
    }

    [Fact]
    public void SnapshotFailurePreservesTheOriginalAndStillExecutesConverters()
    {
        var value = new SnapshotEnvelope();
        SerializationSnapshot before = m_serialization.CaptureSnapshot(value);
        byte[] bytes = before.ToArray();
        int calls = value.writeCalls;
        Assert.Same(before, m_serialization.CaptureSnapshot(value, previous: before));
        Assert.Equal(calls + 1, value.writeCalls);
        value.fail = true;
        Assert.Throws<InvalidOperationException>(() => m_serialization.CaptureSnapshot(value, previous: before));
        Assert.Equal(bytes, before.ToArray());
        value.fail = false;
        Assert.Same(before, m_serialization.CaptureSnapshot(value, previous: before));
        value.duplicate = true;
        Assert.Throws<InvalidOperationException>(() => m_serialization.CaptureSnapshot(value, previous: before));
        Assert.Equal(bytes, before.ToArray());
    }

    [Fact]
    public void SnapshotReadersShareOnlyImmutableDataAcrossIndependentCapturesAndGenerations()
    {
        var source = new DefaultSample { count = 7, bytes = [1, 2, 3] };
        SerializationSnapshot snapshot = m_serialization.CaptureSnapshot(source);
        byte[] original = snapshot.ToArray();
        m_types.Rebuild();
        Parallel.For(0, 32, index =>
        {
            var next = new DefaultSample { count = index, bytes = [1, 2, 3] };
            SerializationSnapshot captured = m_serialization.CaptureSnapshot(next, previous: snapshot);
            Assert.Equal(m_serialization.Serialize(next), captured.ToArray());
            byte[] mutable = captured.ToArray();
            mutable[0]++;
            Assert.Equal(original, snapshot.ToArray());
        });
        Assert.Same(snapshot, m_serialization.CaptureSnapshot(source, previous: snapshot));
        m_serialization.Dispose();
        Assert.Equal(original, snapshot.ToArray());
        Assert.Throws<ObjectDisposedException>(() => m_serialization.CaptureSnapshot(source, previous: snapshot));
    }

    [Fact]
    public void SnapshotDoesNotOwnTheSourceInstance()
    {
        (SerializationSnapshot snapshot, WeakReference reference) = CaptureWeakSource();
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        Assert.False(reference.IsAlive);
        Assert.NotEmpty(snapshot.ToArray());
    }

    [Fact]
    public void UnchangedLargeBinaryCaptureDoesNotCopyItsPayload()
    {
        var source = new SnapshotEnvelope();
        source.state.bytes = new byte[1024 * 1024];
        SerializationSnapshot snapshot = m_serialization.CaptureSnapshot(source);
        for (int index = 0; index < 8; index++)
            Assert.Same(snapshot, m_serialization.CaptureSnapshot(source, previous: snapshot));
        long started = GC.GetAllocatedBytesForCurrentThread();
        for (int index = 0; index < 16; index++)
            Assert.Same(snapshot, m_serialization.CaptureSnapshot(source, previous: snapshot));
        long allocated = (GC.GetAllocatedBytesForCurrentThread() - started) / 16;
        Assert.True(allocated < 16_384, $"An unchanged 1 MiB payload allocated {allocated} bytes per capture.");
        source.state.bytes[500] = 12;
        SerializationSnapshot changed = m_serialization.CaptureSnapshot(source, previous: snapshot);
        Assert.NotSame(snapshot, changed);
        Assert.Equal(m_serialization.Serialize(source), changed.ToArray());
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private (SerializationSnapshot, WeakReference) CaptureWeakSource()
    {
        var value = new DefaultSample { name = "Weak owner" };
        return (m_serialization.CaptureSnapshot(value), new WeakReference(value));
    }
}

internal sealed class MutatingBinaryCapture : ISerializable
{
    internal byte[] bytes = [1];
}

internal sealed class MutatingBinaryCaptureConverter : SerializationConverter<MutatingBinaryCapture>
{
    public override void Write(
        SerializationWriter writer,
        MutatingBinaryCapture value
    ) {
        writer.Write("bytes", value.bytes);
        value.bytes[0] = 2;
    }

    public override MutatingBinaryCapture Read(SerializationReader reader) => new() { bytes = reader.Read<byte[]>("bytes") };
}

internal sealed class MutatingPropertyCapture : ISerializable
{
    private readonly byte[] m_bytes = [1];

    [SerializableProperty(PropertyVisibility.Serialize)] public byte[] bytes => m_bytes;
    [SerializableProperty(PropertyVisibility.Serialize)] public int mutate => m_bytes[0] = 2;

    internal void Reset() => m_bytes[0] = 1;
}

internal sealed class SnapshotEnvelope : ISerializable
{
    internal Dictionary<string, int> fields { get; } = [];
    internal List<int> items { get; } = [];
    internal DefaultSample state { get; } = new();
    internal bool omitState;
    internal bool fail;
    internal bool duplicate;
    internal int writeCalls;
    internal long observedTypeGeneration;
}

internal sealed class SnapshotEnvelopeConverter : SerializationConverter<SnapshotEnvelope>
{
    public override void Write(
        SerializationWriter writer,
        SnapshotEnvelope value
    ) {
        value.writeCalls++;
        value.observedTypeGeneration = writer.typeSnapshot.version;
        foreach (var pair in value.fields)
            writer.Write(pair.Key, pair.Value);
        if (value.duplicate)
        {
            writer.Write("duplicate", 1);
            writer.Write("duplicate", 2);
        }
        if (!value.omitState)
        {
            writer.WritePropertiesData("state", value.state);
            writer.WriteObjectArray("items", value.items, static (
                child,
                number
            ) => child.Write("value", number));
        }
        if (value.fail)
            throw new InvalidOperationException("Capture rejected by its source converter.");
    }

    public override SnapshotEnvelope Read(SerializationReader reader) => throw new NotSupportedException();
}
