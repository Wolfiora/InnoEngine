using System;
using System.IO;
using System.Collections.Generic;
using Inno.Core.Serialization;
using Inno.Content;

using Inno.Rendering;
using Inno.Rendering.Assets;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Reads immutable render target artifacts from one verified logical content store.
/// </summary>
public sealed class ContentRenderTargetArtifactProvider : IRenderTargetArtifactProvider
{
    private readonly IRuntimeContentStore m_content;
    private readonly SerializationRegistry m_serialization;
    private readonly SerializationContext m_context;
    private readonly Dictionary<string, RenderShaderArtifact> m_shaders = new(StringComparer.Ordinal);

    /// <summary>
    /// Creates a provider borrowing one source-free content store.
    /// </summary>
    /// <param name="content">
    /// The verified immutable content store, borrowed for this provider's lifetime.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// A required content or serialization service is null.
    /// </exception>
    /// <param name="serialization">
    /// The runtime owner serialization registry.
    /// </param>
    /// <param name="context">
    /// Complete runtime asset/reference context used to resolve captured texture defaults.
    /// </param>
    public ContentRenderTargetArtifactProvider(
        IRuntimeContentStore content,
        SerializationRegistry serialization,
        SerializationContext context
    ) {
        m_content = content ?? throw new ArgumentNullException(nameof(content));
        m_serialization = serialization ?? throw new ArgumentNullException(nameof(serialization));
        m_context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Reads and validates the shader definition value from its authoritative source.
    /// </summary>
    /// <param name="artifact">
    /// The resolved immutable artifact payload returned to the caller.
    /// </param>
    /// <returns>
    /// The validated shader definition that represents the completed operation.
    /// </returns>
    public ShaderDefinition ReadShaderDefinition(RenderShaderArtifact artifact)
    {
        ArgumentNullException.ThrowIfNull(artifact);
        return m_serialization.Deserialize<ShaderDefinition>(artifact.definitionData.Span, m_context);
    }

    /// <summary>
    /// Loads and validates one packaged shader target artifact when it exists.
    /// </summary>
    /// <param name="shader">
    /// The imported backend-neutral shader asset.
    /// </param>
    /// <param name="variant">
    /// The exact material keyword selection.
    /// </param>
    /// <param name="capabilities">
    /// The active graphics capability snapshot.
    /// </param>
    /// <param name="artifact">
    /// Receives the decoded artifact when present.
    /// </param>
    /// <returns>
    /// <see cref="RenderTargetArtifactStatus.Ready"/> when the deployed artifact exists; otherwise,
    /// <see cref="RenderTargetArtifactStatus.Unavailable"/>.
    /// </returns>
    /// <exception cref="InvalidDataException">
    /// Thrown when a deployed artifact exists but is corrupt or mismatched.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="shader"/> or <paramref name="capabilities"/> is <see langword="null"/>.
    /// </exception>
    public RenderTargetArtifactStatus GetShaderArtifact(
        ShaderAsset shader,
        RenderShaderVariant variant,
        GraphicsCapabilities capabilities,
        out RenderShaderArtifact? artifact
    ) {
        ArgumentNullException.ThrowIfNull(shader);
        ArgumentNullException.ThrowIfNull(capabilities);
        string path = RenderTargetArtifactPath.GetShaderPath(
            shader.identity.persistentId,
            capabilities.backend,
            variant);
        if (m_shaders.TryGetValue(path, out artifact))
            return RenderTargetArtifactStatus.Ready;
        if (!m_content.index.TryGetEntry(new ContentKey(path), out _))
        {
            artifact = null;
            return RenderTargetArtifactStatus.Unavailable;
        }
        try
        {
            ShaderDefinition definition = shader.definition
                ?? throw new InvalidDataException(
                    $"Runtime shader '{shader.assetPath}' has no committed definition.");
            artifact = RenderShaderArtifactCodec.Decode(
                ReadContent(new ContentKey(path)),
                definition.name,
                variant);
            m_shaders.Add(path, artifact);
            return RenderTargetArtifactStatus.Ready;
        }
        catch (Exception exception) when (exception is not InvalidDataException)
        {
            throw new InvalidDataException(
                $"Deployed shader artifact '{path}' failed integrity validation.",
                exception);
        }
    }

    /// <summary>
    /// Loads one packaged portable texture artifact when it exists.
    /// </summary>
    /// <param name="texture">
    /// The imported texture description.
    /// </param>
    /// <param name="artifact">
    /// Receives immutable KTX bytes when present.
    /// </param>
    /// <returns>
    /// <see cref="RenderTargetArtifactStatus.Ready"/> when the deployed artifact exists; otherwise,
    /// <see cref="RenderTargetArtifactStatus.Unavailable"/>.
    /// </returns>
    /// <exception cref="InvalidDataException">
    /// Thrown when a deployed texture artifact is empty.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="texture"/> is <see langword="null"/>.
    /// </exception>
    public RenderTargetArtifactStatus GetTextureArtifact(
        RenderTextureArtifactReference texture,
        out ReadOnlyMemory<byte> artifact
    ) {
        if (texture.assetId == Guid.Empty || string.IsNullOrWhiteSpace(texture.slot.id))
            throw new ArgumentException("A valid texture artifact reference is required.", nameof(texture));
        string path = RenderTargetArtifactPath.GetTexturePath(texture);
        if (!m_content.index.TryGetEntry(new ContentKey(path), out _))
        {
            artifact = ReadOnlyMemory<byte>.Empty;
            return RenderTargetArtifactStatus.Unavailable;
        }
        byte[] bytes = ReadContent(new ContentKey(path));
        if (bytes.Length == 0)
            throw new InvalidDataException($"Deployed texture artifact '{path}' is empty.");
        artifact = bytes;
        return RenderTargetArtifactStatus.Ready;
    }

    private byte[] ReadContent(ContentKey key)
    {
        using ContentReadLease lease = m_content.Acquire(key);
        if (lease.entry.length > Array.MaxLength)
            throw new InvalidDataException($"Target artifact '{key}' exceeds the managed payload limit.");
        using Stream stream = lease.OpenRead();
        byte[] bytes = new byte[checked((int)lease.entry.length)];
        stream.ReadExactly(bytes);
        if (stream.ReadByte() != -1)
            throw new InvalidDataException($"Target artifact '{key}' exceeds its indexed length.");
        return bytes;
    }
}
