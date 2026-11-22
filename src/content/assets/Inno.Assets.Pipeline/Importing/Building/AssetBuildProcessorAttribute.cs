using System;

namespace Inno.Assets.Pipeline;

/// <summary>
/// Declares the immutable cache protocol identity of an automatically discovered asset build processor.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class AssetBuildProcessorAttribute : Attribute
{
    /// <summary>
    /// Creates build-processor discovery metadata.
    /// </summary>
    /// <param name="id">
    /// Globally stable build processor identifier.
    /// </param>
    public AssetBuildProcessorAttribute(string id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        this.id = id.Trim();
    }

    /// <summary>
    /// Gets the globally stable build processor identifier.
    /// </summary>
    public string id { get; }
}
