using System;
using System.Threading.Tasks;
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
