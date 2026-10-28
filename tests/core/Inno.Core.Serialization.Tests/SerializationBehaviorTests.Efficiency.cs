using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Inno.Adapter.Serialization.DotNet;
using Inno.Extensibility.Types;
using Xunit;

namespace Inno.Core.Serialization.Tests;

public sealed partial class SerializationBehaviorTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ReplacingContextServicesReleasesTheReplacedValueAtEveryDepth(bool nested)
    {
        (SerializationContext context, WeakReference replaced) = ReplaceContextService(nested);
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        Assert.False(replaced.IsAlive);
        Assert.NotNull(context.GetRequired<object>());
        if (nested)
        {
            Assert.Equal("marker", context.GetRequired<string>());
            Assert.Equal(new[] { 1, 2 }, context.GetRequired<IReadOnlyList<int>>());
        }
        GC.KeepAlive(context);
    }

    [Fact]
    public void PreparedSerializationContextsAvoidRepeatedOwnerCopies()
    {
        SerializationContext? prepared = null;
        _ = m_serialization.Encode(writer => prepared = writer.context);
        var source = new DefaultSample { count = 73 };
        byte[] expected = m_serialization.CapturePropertyData(source, nameof(DefaultSample.count));
        long fresh = Measure(null);
        long inherited = Measure(prepared);

        Assert.True(inherited < fresh && inherited <= 1024,
            $"Prepared context allocated {inherited} bytes; fresh context allocated {fresh} bytes.");

        long Measure(SerializationContext? context)
        {
            for (int index = 0; index < 16; index++)
                _ = m_serialization.CapturePropertyData(source, nameof(DefaultSample.count), context);
            byte[]? actual = null;
            long before = GC.GetAllocatedBytesForCurrentThread();
            for (int index = 0; index < 128; index++)
                actual = m_serialization.CapturePropertyData(source, nameof(DefaultSample.count), context);
            long allocated = (GC.GetAllocatedBytesForCurrentThread() - before) / 128;
            Assert.Equal(expected, actual);
            return allocated;
        }
    }

    [Fact]
    public void SerializationRebindsForeignOwnersAndPreservesCallerContext()
    {
        var metadata = new ReflectionSerializationMetadataSource();
        using var foreign = new SerializationRegistry(m_types, metadata);
        var service = new SampleContext();
        SerializationContext? prepared = null;
        _ = foreign.Encode(writer => prepared = writer.context, SerializationContext.empty.With<ISampleContext>(service));
        ISerializationMetadataSource? currentMetadata = null;
        _ = m_serialization.Encode(writer => currentMetadata = writer.context.GetRequired<ISerializationMetadataSource>());

        _ = m_serialization.Encode(writer =>
        {
            Assert.Same(m_serialization, writer.context.GetRequired<SerializationRegistry>());
            Assert.Same(m_types, writer.context.GetRequired<TypeCatalog>());
            Assert.Same(currentMetadata, writer.context.GetRequired<ISerializationMetadataSource>());
            Assert.Same(service, writer.context.GetRequired<ISampleContext>());
        }, prepared);

        Assert.Same(foreign, prepared!.GetRequired<SerializationRegistry>());
        Assert.Same(metadata, prepared.GetRequired<ISerializationMetadataSource>());
    }

    [Fact]
    public void BinaryObjectKeysRemainOrdinalAndIndependentOfInsertionOrder()
    {
        string[] keys = ["z", "A", "a", "é", "中文", "😀"];
        byte[] forward = m_serialization.Encode(writer =>
        {
            foreach (string key in keys)
                writer.Write(key, key);
        });
        byte[] reverse = m_serialization.Encode(writer =>
        {
            for (int index = keys.Length - 1; index >= 0; index--)
                writer.Write(keys[index], keys[index]);
        });

        Assert.Equal(forward, reverse);
    }

    [Fact]
    public void SmallBinaryDocumentsStayWithinABoundedAllocationBudget()
    {
        byte[] expected = m_serialization.Encode(static writer => writer.Write("value", 73));
        for (int index = 0; index < 16; index++)
            _ = m_serialization.Encode(static writer => writer.Write("value", 73));

        const int C_ITERATIONS = 128;
        byte[]? actual = null;
        long before = GC.GetAllocatedBytesForCurrentThread();
        for (int index = 0; index < C_ITERATIONS; index++)
            actual = m_serialization.Encode(static writer => writer.Write("value", 73));
        long allocated = GC.GetAllocatedBytesForCurrentThread() - before;

        Assert.Equal(expected, actual);
        Assert.InRange(allocated / C_ITERATIONS, 0, 4096);
    }

    [Fact]
    public void PropertyCaptureDoesNotReadUnrelatedAnnotatedGetters()
    {
        var source = new SelectiveCaptureSample { count = 73 };
        byte[] data = m_serialization.CapturePropertyData(source, nameof(SelectiveCaptureSample.count));
        var target = new DefaultSample { count = 2, name = "Preserved" };

        SerializationPropertyRestoreResult result = m_serialization.RestorePropertiesData(target, data);

        Assert.True(result.success);
        Assert.Equal(1, result.restoredCount);
        Assert.Equal(73, target.count);
        Assert.Equal("Preserved", target.name);
        Assert.Equal(0, source.unrelatedReadCount);
    }

    [Theory]
    [InlineData("missing")]
    [InlineData(nameof(SelectiveCaptureSample.ignored))]
    public void PropertyCaptureRejectsMissingMembersWithoutReadingUnrelatedGetters(string name)
    {
        var source = new SelectiveCaptureSample();

        Assert.Throws<ArgumentException>(() => m_serialization.CapturePropertyData(source, name));

        Assert.Equal(0, source.unrelatedReadCount);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(63)]
    [InlineData(257)]
    [InlineData(10_240)]
    [InlineData(200_000)]
    public void BinaryDocumentsRoundTripAcrossBufferGrowthBoundaries(int length)
    {
        var payload = new byte[length];
        for (int index = 0; index < payload.Length; index++)
            payload[index] = unchecked((byte)index);

        byte[] data = m_serialization.Serialize(new DefaultSample { bytes = payload });
        DefaultSample restored = m_serialization.Deserialize<DefaultSample>(data);

        Assert.Equal(payload, restored.bytes);
    }

    private sealed class SelectiveCaptureSample : ISerializable
    {
        private int m_unrelatedReads;

        [SerializableProperty]
        public int count { get; set; }

        [SerializableProperty]
        public int unrelated
        {
            get
            {
                m_unrelatedReads++;
                throw new InvalidOperationException("An unrelated getter must not be evaluated.");
            }
            set { }
        }

        public int ignored { get; set; }
        public int unrelatedReadCount => m_unrelatedReads;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static (SerializationContext, WeakReference) ReplaceContextService(bool nested)
    {
        var replaced = new object();
        SerializationContext context = SerializationContext.empty.With(replaced);
        if (nested)
            context = context.With("marker").With<IReadOnlyList<int>>(new[] { 1, 2 });
        return (context.With(new object()), new WeakReference(replaced));
    }

}
