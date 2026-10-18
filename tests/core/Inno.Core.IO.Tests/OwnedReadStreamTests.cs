using System;
using System.IO;
using Xunit;

namespace Inno.Core.IO.Tests;

public sealed class OwnedReadStreamTests
{
    [Fact]
    public void ReaderClosesBeforeItsIndependentOwnerAndReleaseCanBeRetried()
    {
        var input = new MemoryStream(new byte[] { 1, 2, 3 });
        var pin = new Pin(input);
        var reader = new OwnedReadStream(input, pin);
        Assert.Equal(1, reader.ReadByte());
        Assert.False(reader.CanWrite);
        Assert.Throws<NotSupportedException>(() => reader.SetLength(0));
        Assert.Throws<IOException>(reader.Dispose);
        Assert.False(reader.CanRead);
        Assert.Equal(1, pin.releases);
        reader.Dispose();
        reader.Dispose();
        Assert.Equal(2, pin.releases);
    }

    private sealed class Pin(MemoryStream input) : IDisposable
    {
        internal int releases;

        public void Dispose()
        {
            Assert.False(input.CanRead);
            if (++releases == 1)
                throw new IOException("Expected retryable release.");
        }
    }
}
