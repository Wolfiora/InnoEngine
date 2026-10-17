using Inno.Rendering;
using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Inno.Rendering.Assets;

/// <summary>
/// Defines deterministic relative paths shared by target-artifact producers and runtime consumers.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("85da30f1-a679-56d0-9cdf-7d6cf19b83f0")]
public static class RenderTargetArtifactPath
{
    /// <summary>
    /// Gets the relative deployment path for one target shader variant.
    /// </summary>
    /// <param name="shaderId">
    /// The persistent shader asset identity.
    /// </param>
    /// <param name="backend">
    /// The graphics backend selected by the target Player.
    /// </param>
    /// <param name="variant">
    /// The canonical static keyword selection.
    /// </param>
    /// <returns>
    /// A platform-neutral path beneath the runtime content root.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="shaderId"/> is empty.
    /// </exception>
    public static string GetShaderPath(
        Guid shaderId,
        GraphicsApi backend,
        RenderShaderVariant variant
    ) {
        if (shaderId == Guid.Empty)
            throw new ArgumentException("A target shader path requires a persistent asset identity.", nameof(shaderId));
        return string.Join('/',
            "TargetArtifacts",
            "Shaders",
            backend.ToString(),
            shaderId.ToString("D", CultureInfo.InvariantCulture),
            HashVariant(variant.value) + ".shader");
    }

    /// <summary>
    /// Gets the relative deployment path for one portable texture artifact.
    /// </summary>
    /// <param name="texture">
    /// Stable texture artifact reference.
    /// </param>
    /// <returns>
    /// A platform-neutral path beneath the runtime content root.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="texture"/> is invalid.
    /// </exception>
    public static string GetTexturePath(RenderTextureArtifactReference texture)
    {
        if (texture.assetId == Guid.Empty || string.IsNullOrWhiteSpace(texture.slot.id))
            throw new ArgumentException("A target texture path requires a valid artifact reference.", nameof(texture));
        return string.Join('/',
            "TargetArtifacts",
            "Textures",
            texture.assetId.ToString("D", CultureInfo.InvariantCulture),
            HashSlot(texture.slot.id) + ".ktx");
    }

    private static string HashVariant(string value)
        => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value))).ToLowerInvariant();

    private static string HashSlot(string value) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value))).ToLowerInvariant();
}

