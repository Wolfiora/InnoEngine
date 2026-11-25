using System;
using Inno.Core.Serialization;

namespace Inno.Testing.Scripting;

/// <summary>
/// Mirrors the asset source metadata that the standard metadata writer records beside a packaged script source.
/// </summary>
public sealed class ScriptingAssetSourceMeta : ISerializable
{
    /// <summary>
    /// Gets or sets the persistent identity of the packaged script source.
    /// </summary>
    [SerializableProperty]
    public Guid persistentId { get; set; }

    /// <summary>
    /// Gets or sets the asset source kind recorded by the standard metadata writer.
    /// </summary>
    [SerializableProperty]
    public int sourceKind { get; set; }

    /// <summary>
    /// Gets or sets the importer responsible for the packaged source.
    /// </summary>
    [SerializableProperty]
    public string importerId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the neutral importer configuration bytes.
    /// </summary>
    [SerializableProperty]
    public byte[] importerSettingsBytes { get; set; } = [];
}
