using System;

namespace Inno.Assets.Pipeline;

/// <summary>
/// Registers a source-language sample transformer through the reloadable type catalog.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class AssetSampleSourceRewriterAttribute : Attribute
{
    /// <summary>
    /// Creates one ordered source-language transformer declaration.
    /// </summary>
    /// <param name="id">
    /// Stable extension identity used for diagnostics and ordering.
    /// </param>
    public AssetSampleSourceRewriterAttribute(string id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        this.id = id;
    }

    /// <summary>
    /// Gets the stable extension identity.
    /// </summary>
    public string id { get; }
}
