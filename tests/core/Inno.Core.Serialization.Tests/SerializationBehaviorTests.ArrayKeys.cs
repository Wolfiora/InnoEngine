using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Inno.Core.Identity;
using Inno.Core.Serialization.Converters;
using Xunit;

namespace Inno.Core.Serialization.Tests;

public sealed partial class SerializationBehaviorTests
{
    [Fact]
    public void KeyedEncodingIsCanonicalUnderConcurrentReadersAndFailedDestinations()
    {
        var identities = new IdentityAllocator();
        var row = new CaptureArrayRow { number = 7 };
        Assert.True(identities.Register(row));
        var source = new CaptureArrayRoot { rows = [row] };
        SerializationSnapshot snapshot = m_serialization.CaptureSnapshot(source);
        byte[] expected = m_serialization.Serialize(source);
        Parallel.For(0, 64, _ =>
        {
            using var destination = new MemoryStream();
            snapshot.WriteTo(destination);
            Assert.Equal(expected, destination.ToArray());
        });
        using var rejected = new RejectCaptureStream();
        Assert.Throws<IOException>(() => snapshot.WriteTo(rejected));
        byte[] callerOwned = snapshot.ToArray();
        callerOwned[0] ^= 0x7F;
        Assert.Equal(expected, snapshot.ToArray());
        Assert.Same(snapshot, m_serialization.CaptureSnapshot(source, previous: snapshot));
    }

    [Fact]
    public void LargeKeyedObjectsRemainStreamedWithoutAContiguousPayloadAllocation()
    {
        var identities = new IdentityAllocator();
        var source = new CaptureBlobRoot { data = new byte[256 * 1024] };
        Array.Fill(source.data, (byte)0xA5);
        Assert.True(identities.Register(source));
        SerializationSnapshot snapshot = m_serialization.CaptureSnapshot(source);
        Assert.Equal(m_serialization.Serialize(source), snapshot.ToArray());
        snapshot.WriteTo(Stream.Null);
        long before = GC.GetAllocatedBytesForCurrentThread();
        snapshot.WriteTo(Stream.Null);
        Assert.InRange(GC.GetAllocatedBytesForCurrentThread() - before, 0, 16 * 1024);
    }

    [Fact]
    public void KeyedArrayCapturesOnlyChangedRowsAndKeepsOrdinarySerializationAuthoritative()
    {
        var identities = new IdentityAllocator();
        var first = new CaptureArrayRow { number = 1 };
        var second = new CaptureArrayRow { number = 2 };
        Assert.True(identities.Register(first));
        Assert.True(identities.Register(second));
        var source = new CaptureArrayRoot { rows = [first, second] };
        SerializationSnapshot before = m_serialization.CaptureSnapshot(source);
        Assert.Same(before, m_serialization.CaptureSnapshot(source, previous: before));
        Assert.Equal(1, first.writes);
        Assert.Equal(1, second.writes);
        first.number = 3;
        first.revision++;
        SerializationSnapshot changed = m_serialization.CaptureSnapshot(source, previous: before);
        Assert.Equal(2, first.writes);
        Assert.Equal(1, second.writes);
        int keys = source.keys;
        Assert.Equal(m_serialization.Serialize(source), changed.ToArray());
        Assert.Equal(keys, source.keys);
        Assert.Equal(3, first.writes);
        Assert.Equal(2, second.writes);
        source.mode = 4;
        changed = m_serialization.CaptureSnapshot(source, previous: changed);
        Assert.Equal(m_serialization.Serialize(source), changed.ToArray());
        source.rows.Reverse();
        changed = m_serialization.CaptureSnapshot(source, previous: changed);
        Assert.Equal(m_serialization.Serialize(source), changed.ToArray());
        source.rows.RemoveAt(0);
        changed = m_serialization.CaptureSnapshot(source, previous: changed);
        Assert.Equal(m_serialization.Serialize(source), changed.ToArray());
        source.rows.Clear();
        changed = m_serialization.CaptureSnapshot(source, previous: changed);
        Assert.Equal(m_serialization.Serialize(source), changed.ToArray());
        Assert.Same(changed, m_serialization.CaptureSnapshot(source, previous: changed));
    }

    [Fact]
    public void ArrayKeysVerifyExactIdentifiersAndPreservePreviousSnapshotsOnFailure()
    {
        var identities = new IdentityAllocator();
        var row = new CaptureArrayRow();
        Assert.True(identities.Register(row));
        var source = new CaptureArrayRoot { rows = [row] };
        SerializationSnapshot before = m_serialization.CaptureSnapshot(source);
        byte[] bytes = before.ToArray();
        source.scope = Guid.NewGuid();
        SerializationSnapshot changed = m_serialization.CaptureSnapshot(source, previous: before);
        Assert.NotEqual(bytes, changed.ToArray());
        row.revision++;
        source.onWrite = () => source.scope = Guid.NewGuid();
        Assert.Throws<InvalidOperationException>(() => m_serialization.CaptureSnapshot(source, previous: changed));
        source.onWrite = null;
        Assert.Equal(bytes, before.ToArray());
        Assert.Throws<InvalidOperationException>(() => source.savedBuilder!.AddValue(1UL));
        source.failKey = true;
        Assert.Throws<InvalidOperationException>(() => m_serialization.CaptureSnapshot(source, previous: changed));
        source.failKey = false;
        Assert.True(identities.Unregister(row));
        Assert.True(identities.Register(row));
        changed = m_serialization.CaptureSnapshot(source, previous: changed);
        Assert.Equal(m_serialization.Serialize(source), changed.ToArray());
    }

    [Fact]
    public void NestedStatefulConvertersPreventEnclosingArrayKeyReuse()
    {
        var identities = new IdentityAllocator();
        var row = new CaptureArrayRow();
        Assert.True(identities.Register(row));
        var source = new CaptureArrayRoot { rows = [row], stateful = true };
        SerializationSnapshot before = m_serialization.CaptureSnapshot(source);
        int calls = CaptureConvertedValueConverter.calls;
        SerializationSnapshot changed = m_serialization.CaptureSnapshot(source, previous: before);
        Assert.Equal(calls + 1, CaptureConvertedValueConverter.calls);
        Assert.Equal(2, row.writes);
        Assert.NotEqual(before.ToArray(), changed.ToArray());
    }

    [Fact]
    public void DeclinedArrayKeysCannotReuseAnEarlierCertifiedElement()
    {
        var identities = new IdentityAllocator();
        var row = new CaptureArrayRow();
        Assert.True(identities.Register(row));
        var source = new CaptureArrayRoot { rows = [row] };
        SerializationSnapshot before = m_serialization.CaptureSnapshot(source);
        source.allowKeys = false;
        row.number = 8;
        SerializationSnapshot changed = m_serialization.CaptureSnapshot(source, previous: before);
        Assert.Equal(2, row.writes);
        Assert.Equal(m_serialization.Serialize(source), changed.ToArray());
        row.number = 11;
        changed = m_serialization.CaptureSnapshot(source, previous: changed);
        Assert.Equal(m_serialization.Serialize(source), changed.ToArray());
        source.allowKeys = true;
        row.revision++;
        changed = m_serialization.CaptureSnapshot(source, previous: changed);
        Assert.Equal(m_serialization.Serialize(source), changed.ToArray());
    }
}

internal sealed class CaptureArrayRoot : ISerializable
{
    internal List<CaptureArrayRow> rows = [];
    internal Guid scope = Guid.NewGuid();
    internal int mode;
    internal int keys;
    internal bool allowKeys = true;
    internal bool stateful;
    internal bool failKey;
    internal Action? onWrite;
    internal SerializationCaptureKeyBuilder? savedBuilder;
}

internal sealed class CaptureArrayRow : IdentityObject, ISerializable
{
    internal int number;
    internal int writes;
    internal ulong? revision = 0;
    internal ConvertedCaptureProperties state = new();
}

internal sealed class CaptureArrayRootConverter : SerializationConverter<CaptureArrayRoot>
{
    public override void Write(
        SerializationWriter writer,
        CaptureArrayRoot value
    ) {
        writer.Write("scope", value.scope);
        writer.WriteObjectArray("rows", value.rows, (
            child,
            row
        ) => {
            row.writes++;
            value.onWrite?.Invoke();
            child.Write("number", row.number);
            child.Write("mode", value.mode);
            if (value.stateful)
                child.WritePropertiesData("state", row.state);
        }, captureKey: (
            row,
            key
        ) => {
            value.keys++;
            value.savedBuilder = key;
            if (value.failKey)
                throw new InvalidOperationException("Array key failure.");
            if (!value.allowKeys || !key.TryAddOwner(row, row.revision))
                return false;
            key.AddValue(value.scope);
            key.AddValue(unchecked((ulong)value.mode));
            key.AddValue(value.stateful ? 1UL : 0UL);
            return true;
        });
    }

    public override CaptureArrayRoot Read(SerializationReader reader) => throw new NotSupportedException();
}

internal sealed class CaptureBlobRoot : IdentityObject, ISerializable
{
    internal byte[] data = [];
}

internal sealed class CaptureBlobRootConverter : SerializationConverter<CaptureBlobRoot>
{
    public override void Write(
        SerializationWriter writer,
        CaptureBlobRoot value
    ) => writer.Write("data", value.data);

    public override CaptureBlobRoot Read(SerializationReader reader) => throw new NotSupportedException();

    protected override bool TryCaptureKey(
        CaptureBlobRoot value,
        SerializationCaptureKeyBuilder key
    ) => key.TryAddOwner(value, 0);
}

internal sealed class RejectCaptureStream : MemoryStream
{
    public override void Write(ReadOnlySpan<byte> buffer) => throw new IOException("The test destination rejects writes.");
}
