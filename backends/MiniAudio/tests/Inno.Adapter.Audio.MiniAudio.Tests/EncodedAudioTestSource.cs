using System;
using System.IO;
using System.Security.Cryptography;
using Inno.Audio;

namespace Inno.Adapter.Audio.MiniAudio.Tests;

internal sealed class EncodedAudioTestSource : IAudioClipSource
{
    private readonly byte[] m_bytes;

    internal EncodedAudioTestSource(byte[] bytes)
    {
        m_bytes = (byte[])bytes.Clone();
        contentHash = Convert.ToHexString(SHA256.HashData(m_bytes));
    }

    public string contentHash { get; }
    public long length => m_bytes.LongLength;
    public Stream OpenRead() => new MemoryStream(m_bytes, writable: false);
}
