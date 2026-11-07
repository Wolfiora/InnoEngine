using System;
using System.IO;
using System.Security.Cryptography;
using Inno.Engine.Default;
using Xunit;

namespace Inno.Runtime.Tests;

public sealed class EngineBrandingTests
{
    [Fact]
    public void LogoIsTheDistributedPngAndDoesNotRequireAnEngineSession()
    {
        using Stream source = EngineBranding.OpenLogoPng();
        Assert.True(source.CanRead);
        Assert.False(source.CanWrite);
        Assert.Equal(0, source.Position);
        using var bytes = new MemoryStream();
        source.CopyTo(bytes);
        byte[] png = bytes.ToArray();
        Assert.Equal(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }, png[..8]);
        Assert.Equal("5AC155BAC41EB5AC99F157635B187FD27A84D8E6CAF6F77733A748346C6CE2DF",
            Convert.ToHexString(SHA256.HashData(png)));
    }

    [Fact]
    public void StreamsHaveIndependentPositionsAndDisposal()
    {
        using Stream first = EngineBranding.OpenLogoPng();
        using Stream second = EngineBranding.OpenLogoPng();
        Assert.NotSame(first, second);
        Assert.Equal(137, first.ReadByte());
        Assert.Equal(0, second.Position);
        first.Dispose();
        Assert.Equal(137, second.ReadByte());
        using Stream replacement = EngineBranding.OpenLogoPng();
        Assert.Equal(0, replacement.Position);
        Assert.Equal(137, replacement.ReadByte());
        Assert.Throws<NotSupportedException>(() => replacement.WriteByte(0));
    }
}
