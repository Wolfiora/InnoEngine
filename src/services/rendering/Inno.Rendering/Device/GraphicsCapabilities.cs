using System;
using System.Collections.Generic;

namespace Inno.Rendering;

/// <summary>
/// Provides an immutable capability snapshot for one device generation.
/// </summary>
public sealed class GraphicsCapabilities
{
    private readonly HashSet<RenderTextureFormat> m_sampledFormats;
    private readonly HashSet<RenderTextureFormat> m_sampled3DFormats;
    private readonly HashSet<RenderTextureFormat> m_sampledCubeFormats;
    private readonly HashSet<RenderTextureFormat> m_renderTargetFormats;
    private readonly HashSet<RenderTextureFormat> m_multisampleRenderTargetFormats;
    private readonly HashSet<RenderTextureFormat> m_storageReadFormats;
    private readonly HashSet<RenderTextureFormat> m_storageWriteFormats;

    /// <summary>
    /// Creates a device capability snapshot.
    /// </summary>
    /// <param name="backend">
    /// Active graphics API family.
    /// </param>
    /// <param name="features">
    /// Supported optional features.
    /// </param>
    /// <param name="limits">
    /// Device limits.
    /// </param>
    /// <param name="sampledFormats">
    /// Formats valid for sampled two-dimensional textures.
    /// </param>
    /// <param name="renderTargetFormats">
    /// Formats valid as raster attachments.
    /// </param>
    /// <param name="storageReadFormats">
    /// Formats valid for unordered shader reads.
    /// </param>
    /// <param name="storageWriteFormats">
    /// Formats valid for unordered shader writes.
    /// </param>
    /// <param name="originBottomLeft">
    /// Whether render-target coordinates start at the bottom-left.
    /// </param>
    /// <param name="homogeneousDepth">
    /// Whether clip-space depth uses the negative-one-to-one range.
    /// </param>
    /// <param name="sampled3DFormats">
    /// Formats valid for sampled volume textures.
    /// </param>
    /// <param name="sampledCubeFormats">
    /// Formats valid for sampled cubemaps.
    /// </param>
    /// <param name="multisampleRenderTargetFormats">
    /// Formats valid as multisampled raster attachments.
    /// </param>
    public GraphicsCapabilities(
        GraphicsApi backend,
        GraphicsCapability features,
        GraphicsLimits limits,
        IEnumerable<RenderTextureFormat> sampledFormats,
        IEnumerable<RenderTextureFormat> renderTargetFormats,
        IEnumerable<RenderTextureFormat> storageReadFormats,
        IEnumerable<RenderTextureFormat> storageWriteFormats,
        bool originBottomLeft,
        bool homogeneousDepth,
        IEnumerable<RenderTextureFormat>? sampled3DFormats = null,
        IEnumerable<RenderTextureFormat>? sampledCubeFormats = null,
        IEnumerable<RenderTextureFormat>? multisampleRenderTargetFormats = null
    ) {
        ArgumentNullException.ThrowIfNull(limits);
        ArgumentNullException.ThrowIfNull(sampledFormats);
        ArgumentNullException.ThrowIfNull(renderTargetFormats);
        ArgumentNullException.ThrowIfNull(storageReadFormats);
        ArgumentNullException.ThrowIfNull(storageWriteFormats);
        if (!backend.isValid)
        {
            throw new ArgumentException("Graphics capabilities require a backend identity.", nameof(backend));
        }
        this.backend = backend;
        this.features = features;
        this.limits = limits;
        this.originBottomLeft = originBottomLeft;
        this.homogeneousDepth = homogeneousDepth;
        m_sampledFormats = [.. sampledFormats];
        m_sampled3DFormats = [.. sampled3DFormats ?? []];
        m_sampledCubeFormats = [.. sampledCubeFormats ?? []];
        m_renderTargetFormats = [.. renderTargetFormats];
        m_multisampleRenderTargetFormats = [.. multisampleRenderTargetFormats ?? []];
        m_storageReadFormats = [.. storageReadFormats];
        m_storageWriteFormats = [.. storageWriteFormats];
    }

    /// <summary>
    /// Gets the active graphics API family.
    /// </summary>
    public GraphicsApi backend { get; }

    /// <summary>
    /// Gets all supported optional features.
    /// </summary>
    public GraphicsCapability features { get; }

    /// <summary>
    /// Gets device limits.
    /// </summary>
    public GraphicsLimits limits { get; }

    /// <summary>
    /// Gets whether render-target coordinates start at the bottom-left.
    /// </summary>
    public bool originBottomLeft { get; }

    /// <summary>
    /// Gets whether clip-space depth uses the negative-one-to-one range.
    /// </summary>
    public bool homogeneousDepth { get; }

    /// <summary>
    /// Tests whether every requested optional feature is supported.
    /// </summary>
    /// <param name="required">
    /// The required consumed by supports; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when every requested feature is available.
    /// </returns>
    public bool Supports(GraphicsCapability required) => (features & required) == required;

    /// <summary>
    /// Tests whether a format is valid for sampled two-dimensional textures.
    /// </summary>
    /// <param name="format">
    /// Texture format to query.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the format can be sampled.
    /// </returns>
    public bool SupportsSampled(RenderTextureFormat format) => SupportsSampled(format, RenderTextureDimension.Texture2D);

    /// <summary>
    /// Tests whether a format is valid for sampled textures of one dimensional shape.
    /// </summary>
    /// <param name="format">
    /// Texture format to query.
    /// </param>
    /// <param name="dimension">
    /// Texture shape to query.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the format and dimension can be sampled.
    /// </returns>
    public bool SupportsSampled(
        RenderTextureFormat format,
        RenderTextureDimension dimension
    ) => dimension switch
    {
        RenderTextureDimension.Texture2D => m_sampledFormats.Contains(format),
        RenderTextureDimension.Texture3D => m_sampled3DFormats.Contains(format),
        RenderTextureDimension.Cube => m_sampledCubeFormats.Contains(format),
        _ => false
    };

    /// <summary>
    /// Tests whether a format is valid as a raster attachment.
    /// </summary>
    /// <param name="format">
    /// The format consumed by supports render target; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the format can be attached.
    /// </returns>
    public bool SupportsRenderTarget(RenderTextureFormat format) => m_renderTargetFormats.Contains(format);

    /// <summary>
    /// Tests whether a format is valid as a multisampled raster attachment.
    /// </summary>
    /// <param name="format">
    /// Texture format to query.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the format can be attached with multisampling.
    /// </returns>
    public bool SupportsMultisampleRenderTarget(RenderTextureFormat format) => m_multisampleRenderTargetFormats.Contains(format);

    /// <summary>
    /// Tests whether a format supports the requested unordered shader access.
    /// </summary>
    /// <param name="format">
    /// Texture format to query.
    /// </param>
    /// <param name="access">
    /// Required storage access.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the format supports every requested access direction.
    /// </returns>
    public bool SupportsStorage(
        RenderTextureFormat format,
        RenderStorageAccess access
    ) => access switch
    {
        RenderStorageAccess.Read => m_storageReadFormats.Contains(format),
        RenderStorageAccess.Write => m_storageWriteFormats.Contains(format),
        RenderStorageAccess.ReadWrite => m_storageReadFormats.Contains(format)
                                         && m_storageWriteFormats.Contains(format),
        _ => false
    };
}

/// <summary>
/// Declares backend-neutral optional graphics functionality.
/// </summary>
[Flags]
public enum GraphicsCapability
{
    /// <summary>
    /// No optional feature.
    /// </summary>
    None = 0,
    /// <summary>
    /// Compute shader dispatch.
    /// </summary>
    Compute = 1 << 0,
    /// <summary>
    /// Shader-readable and writable storage buffers.
    /// </summary>
    StorageBuffer = 1 << 1,
    /// <summary>
    /// Indirect draw and dispatch commands.
    /// </summary>
    Indirect = 1 << 2,
    /// <summary>
    /// Independent blend state per color attachment.
    /// </summary>
    IndependentBlend = 1 << 3,
    /// <summary>
    /// Multiple command encoders may record concurrently.
    /// </summary>
    ConcurrentEncoders = 1 << 4,
    /// <summary>
    /// Texture copy operations are supported directly.
    /// </summary>
    TextureBlit = 1 << 5,
    /// <summary>
    /// General GPU buffer copy operations are supported directly.
    /// </summary>
    BufferCopy = 1 << 6,
    /// <summary>
    /// Alpha-to-coverage rasterization is supported.
    /// </summary>
    AlphaToCoverage = 1 << 7,
    /// <summary>
    /// Unsigned 32-bit index buffers are supported.
    /// </summary>
    Index32 = 1 << 8,
    /// <summary>
    /// Instanced draw input is supported.
    /// </summary>
    Instancing = 1 << 9,
    /// <summary>
    /// Additional native presentation surfaces are supported.
    /// </summary>
    SwapChain = 1 << 10,
    /// <summary>
    /// Two-dimensional texture arrays are supported.
    /// </summary>
    Texture2DArray = 1 << 11,
    /// <summary>
    /// Three-dimensional textures are supported.
    /// </summary>
    Texture3D = 1 << 12,
    /// <summary>
    /// Cubemap texture arrays are supported.
    /// </summary>
    TextureCubeArray = 1 << 13,
    /// <summary>
    /// Half-precision vertex attributes are supported.
    /// </summary>
    VertexAttributeHalf = 1 << 14,
    /// <summary>
    /// Packed 10:10:10:2 vertex attributes are supported.
    /// </summary>
    VertexAttributeUInt10 = 1 << 15,
    /// <summary>
    /// Procedural draws using shader vertex identifiers are supported.
    /// </summary>
    ProceduralDraw = 1 << 16,
    /// <summary>
    /// Fragment shaders may write depth explicitly.
    /// </summary>
    FragmentDepth = 1 << 17,
    /// <summary>
    /// Shader-readable and writable storage textures are supported.
    /// </summary>
    StorageTexture = 1 << 18,
    /// <summary>
    /// Asynchronous texture transfer from GPU memory to CPU-visible bytes.
    /// </summary>
    TextureReadback = 1 << 19
}
