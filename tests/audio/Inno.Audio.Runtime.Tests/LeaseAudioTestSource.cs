using System.IO;
using Inno.Assets;

namespace Inno.Audio.Runtime.Tests;

internal sealed class LeaseAudioTestSource(ArtifactLease artifact) : IAudioClipSource
{
    public string contentHash => artifact.info.contentHash;
    public long length => artifact.info.length;
    public Stream OpenRead() => artifact.OpenRead();
}
