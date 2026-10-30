using System;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.Serialization.Converters;
using Xunit;

namespace Inno.Core.Serialization.Tests;

public sealed partial class SerializationBehaviorTests
{
    [Fact]
    public void ConcurrentCapturedOperationsPublishOneConverterAndRetireItAfterTheLastLease()
    {
        ConcurrentCaptureConverter.constructions = 0;
        ConcurrentCaptureConverter.disposals = 0;
        using SerializationGeneration generation = m_serialization.CaptureGeneration();
        var scalar = new DefaultSample { count = 73, name = "Pinned" };
        byte[] expected = generation.Serialize(scalar);

        Parallel.For(0, 256, new ParallelOptions { MaxDegreeOfParallelism = 8 }, index =>
        {
            var value = new ConcurrentCaptureValue(index);
            byte[] bytes = generation.Serialize(value);
            Assert.Equal(index, generation.Deserialize<ConcurrentCaptureValue>(bytes).count);
            Assert.Equal(expected, generation.Serialize(scalar));
        });

        Assert.Equal(1, ConcurrentCaptureConverter.constructions);
        m_serialization.Dispose();
        Assert.Equal(0, ConcurrentCaptureConverter.disposals);
        Assert.Equal(expected, generation.Serialize(scalar));
        generation.Dispose();
        Assert.Equal(1, ConcurrentCaptureConverter.disposals);
        Assert.Throws<ObjectDisposedException>(() => generation.Serialize(scalar));
    }
}

internal sealed class ConcurrentCaptureValue(int count) : ISerializable
{
    internal int count { get; } = count;
}

internal sealed class ConcurrentCaptureConverter : SerializationConverter<ConcurrentCaptureValue>, IDisposable
{
    internal static int constructions;
    internal static int disposals;

    /// <summary>
    /// Records construction of the converter owned by the captured generation.
    /// </summary>
    public ConcurrentCaptureConverter() => Interlocked.Increment(ref constructions);

    /// <inheritdoc />
    public override void Write(
        SerializationWriter writer,
        ConcurrentCaptureValue value
    ) => writer.Write("count", value.count);

    /// <inheritdoc />
    public override ConcurrentCaptureValue Read(SerializationReader reader) => new(reader.Read<int>("count"));

    /// <inheritdoc />
    public void Dispose() => Interlocked.Increment(ref disposals);
}
