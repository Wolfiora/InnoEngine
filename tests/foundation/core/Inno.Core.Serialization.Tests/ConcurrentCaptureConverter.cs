using System;
using System.Threading;
using Inno.Core.Serialization.Converters;

namespace Inno.Core.Serialization.Tests;

internal sealed class ConcurrentCaptureConverter : SerializationConverter<ConcurrentCaptureValue>, IDisposable
{
    internal static int constructions;
    internal static int disposals;

    /// <summary>
    /// Records construction of the converter owned by the captured generation.
    /// </summary>
    public ConcurrentCaptureConverter()
    {
        Interlocked.Increment(ref constructions);
    }

    /// <inheritdoc/>
    public override void Write(
        SerializationWriter writer,
        ConcurrentCaptureValue value
    ) => writer.Write("count", value.count);

    /// <inheritdoc/>
    public override ConcurrentCaptureValue Read(SerializationReader reader) => new(reader.Read<int>("count"));

    /// <inheritdoc/>
    public void Dispose() => Interlocked.Increment(ref disposals);
}
