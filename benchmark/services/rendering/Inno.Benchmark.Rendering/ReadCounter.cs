using System;
using System.IO;

namespace Inno.Benchmark.Rendering;

internal sealed class ReadCounter(
    Stream source,
    Action<int> count
) : Stream
{
    public override bool CanRead => source.CanRead;
    public override bool CanSeek => source.CanSeek;
    public override bool CanWrite => false;
    public override long Length => source.Length;

    public override long Position
    {
        get => source.Position;
        set => source.Position = value;
    }

    public override int Read(Span<byte> buffer)
    {
        int read = source.Read(buffer);
        count(read);
        return read;
    }

    public override int Read(
        byte[] buffer,
        int offset,
        int length
    ) => Read(buffer.AsSpan(offset, length));

    public override long Seek(
        long offset,
        SeekOrigin origin
    ) => source.Seek(offset, origin);

    public override void Flush() => source.Flush();

    public override void SetLength(long value) => throw new NotSupportedException();

    public override void Write(
        byte[] buffer,
        int offset,
        int length
    ) => throw new NotSupportedException();

    protected override void Dispose(bool disposing)
    {
        if (disposing) source.Dispose();
        base.Dispose(disposing);
    }
}
