using System;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Scripting.Api;

namespace Inno.Editor.Assets;

/// <summary>
/// Provides source encoding services to one detached Asset creation template invocation.
/// </summary>
public sealed class AssetCreationContext
{
    private readonly AssetSourceStore m_sources;

    /// <summary>
    /// Creates a context over the source serializer owned by the active Asset pipeline.
    /// </summary>
    /// <param name="sources">
    /// Source serializer carrying the active reference context.
    /// </param>
    [ScriptingApiIgnore]
    public AssetCreationContext(AssetSourceStore sources)
    {
        ArgumentNullException.ThrowIfNull(sources);
        m_sources = sources;
    }

    /// <summary>
    /// Encodes an Asset value through the current native structured source serializer.
    /// </summary>
    /// <param name="asset">
    /// Detached Asset value to encode.
    /// </param>
    /// <returns>
    /// Complete native source bytes, including the concrete serialized type identity.
    /// </returns>
    public byte[] EncodeNative(AssetObject asset)
    {
        ArgumentNullException.ThrowIfNull(asset);
        return m_sources.Encode(asset);
    }
}
