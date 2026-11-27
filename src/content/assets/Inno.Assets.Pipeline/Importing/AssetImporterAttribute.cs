using System;

namespace Inno.Assets.Pipeline;

/// <summary>
/// Declares the immutable protocol identity of an automatically discovered asset importer.
/// </summary>
/// <remarks>
/// The authoring catalog resolves this ID in the active type generation. Imported artifacts must use a
/// representation that the consuming runtime can open.
/// </remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class AssetImporterAttribute : Attribute
{
    /// <summary>
    /// Creates importer discovery metadata.
    /// </summary>
    /// <param name="id">
    /// Globally stable importer protocol identifier.
    /// </param>
    public AssetImporterAttribute(string id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        this.id = id.Trim();
    }

    /// <summary>
    /// Gets the globally stable importer protocol identifier.
    /// </summary>
    public string id { get; }
}
