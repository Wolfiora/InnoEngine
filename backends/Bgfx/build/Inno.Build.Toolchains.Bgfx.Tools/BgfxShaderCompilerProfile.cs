using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Inno.Rendering;

namespace Inno.Build.Toolchains.Bgfx.Tools;

/// <summary>
/// Freezes one renderer's vendor dialect, capability facts and semantic compiler defines.
/// </summary>
public sealed class BgfxShaderCompilerProfile
{
    /// <summary>
    /// Validates and snapshots platform-supplied compiler inputs.
    /// </summary>
    /// <param name="capabilities">
    /// The immutable target capabilities.
    /// </param>
    /// <param name="shadercPlatform">
    /// The explicit vendor platform argument.
    /// </param>
    /// <param name="vertexProfile">
    /// The vertex shader dialect.
    /// </param>
    /// <param name="fragmentProfile">
    /// The fragment shader dialect.
    /// </param>
    /// <param name="computeProfile">
    /// The compute dialect, or empty when compute is unsupported.
    /// </param>
    /// <param name="defines">
    /// Optional ordered defines; copied on construction.
    /// </param>
    /// <exception cref="ArgumentException">
    /// A required string is blank, a define is null, or the compute declaration disagrees with capabilities.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Capabilities are null.
    /// </exception>
    public BgfxShaderCompilerProfile(
        GraphicsCapabilities capabilities,
        string shadercPlatform,
        string vertexProfile,
        string fragmentProfile,
        string computeProfile,
        IEnumerable<string>? defines = null
    ) {
        ArgumentNullException.ThrowIfNull(capabilities);
        ArgumentException.ThrowIfNullOrWhiteSpace(shadercPlatform);
        ArgumentException.ThrowIfNullOrWhiteSpace(vertexProfile);
        ArgumentException.ThrowIfNullOrWhiteSpace(fragmentProfile);
        ArgumentNullException.ThrowIfNull(computeProfile);
        string[] snapshot = defines?.ToArray() ?? [];
        if (snapshot.Any(string.IsNullOrWhiteSpace) || capabilities.backend == GraphicsApi.Noop
            || capabilities.Supports(GraphicsCapability.Compute) != !string.IsNullOrWhiteSpace(computeProfile))
            throw new ArgumentException("A shader profile requires valid defines, a renderer and consistent compute support.");
        this.capabilities = capabilities;
        this.shadercPlatform = shadercPlatform;
        this.vertexProfile = vertexProfile;
        this.fragmentProfile = fragmentProfile;
        this.computeProfile = computeProfile;
        this.defines = Array.AsReadOnly(snapshot);
        key = CreateKey(capabilities);
    }

    /// <summary>
    /// Gets the immutable offline validation capabilities.
    /// </summary>
    public GraphicsCapabilities capabilities { get; }

    /// <summary>
    /// Gets the vendor shader compiler platform argument.
    /// </summary>
    public string shadercPlatform { get; }

    /// <summary>
    /// Gets the vertex stage dialect.
    /// </summary>
    public string vertexProfile { get; }

    /// <summary>
    /// Gets the fragment stage dialect.
    /// </summary>
    public string fragmentProfile { get; }

    /// <summary>
    /// Gets the compute dialect, or an empty string when compute is unavailable.
    /// </summary>
    public string computeProfile { get; }

    /// <summary>
    /// Gets immutable ordered compiler defines.
    /// </summary>
    public IReadOnlyList<string> defines { get; }

    /// <summary>
    /// Gets a deterministic identity covering dialects, defines and offline capabilities.
    /// </summary>
    public string key { get; }

    internal string GetStageProfile(ShaderStage stage) => stage switch
    {
        ShaderStage.Vertex => vertexProfile,
        ShaderStage.Fragment => fragmentProfile,
        ShaderStage.Compute when computeProfile.Length > 0 => computeProfile,
        ShaderStage.Compute => throw new NotSupportedException("The selected shader profile does not support compute."),
        _ => throw new ArgumentOutOfRangeException(nameof(stage), stage, "A single shader stage is required.")
    };

    internal string CreateKey(GraphicsCapabilities requested)
    {
        using var bytes = new MemoryStream();
        using (var writer = new BinaryWriter(bytes, Encoding.UTF8, leaveOpen: true))
        {
            writer.Write(shadercPlatform);
            writer.Write(vertexProfile);
            writer.Write(fragmentProfile);
            writer.Write(computeProfile);
            writer.Write(defines.Count);
            foreach (string define in defines)
                writer.Write(define);
            writer.Write(requested.backend.value);
            writer.Write((int)requested.features);
            writer.Write(requested.limits.maxViews);
            writer.Write(requested.limits.maxColorAttachments);
            writer.Write(requested.limits.maxTextureSize);
            writer.Write(requested.limits.maxComputeBindings);
            writer.Write(requested.originBottomLeft);
            writer.Write(requested.homogeneousDepth);
            foreach (RenderTextureFormat format in Enum.GetValues<RenderTextureFormat>())
            {
                writer.Write((int)format);
                writer.Write(requested.SupportsSampled(format));
                writer.Write(requested.SupportsSampled(format, RenderTextureDimension.Texture3D));
                writer.Write(requested.SupportsSampled(format, RenderTextureDimension.Cube));
                writer.Write(requested.SupportsRenderTarget(format));
                writer.Write(requested.SupportsMultisampleRenderTarget(format));
                writer.Write(requested.SupportsStorage(format, RenderStorageAccess.Read));
                writer.Write(requested.SupportsStorage(format, RenderStorageAccess.Write));
            }
        }
        return Convert.ToHexString(SHA256.HashData(bytes.ToArray()));
    }
}
