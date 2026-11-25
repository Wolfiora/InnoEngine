using System;
using Xunit;

namespace Inno.Core.Serialization.Tests;

public sealed partial class SerializationBehaviorTests
{
    [Fact]
    public void ReusedScalarKeysPreserveCanonicalBytesAndRejectOutOfOrderDuplicates()
    {
        var value = new OrdinalCapture();
        SerializationSnapshot before = m_serialization.CaptureSnapshot(value);
        byte[] bytes = before.ToArray();
        value.names = ["third", "first", "second"];
        SerializationSnapshot reordered = m_serialization.CaptureSnapshot(value, previous: before);
        Assert.Equal(m_serialization.Serialize(value), reordered.ToArray());
        Assert.Equal(bytes, reordered.ToArray());
        value.names = ["third", "first", "third"];
        Assert.Throws<InvalidOperationException>(() => m_serialization.CaptureSnapshot(value, previous: before));
        Assert.Equal(bytes, before.ToArray());
        value.names = ["first", "second", "third"];
        Assert.Same(before, m_serialization.CaptureSnapshot(value, previous: before));
    }
}
