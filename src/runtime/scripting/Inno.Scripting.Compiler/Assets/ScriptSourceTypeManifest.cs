using System;
using Inno.Core.Serialization;

namespace Inno.Scripting.Compiler;

internal sealed class ScriptSourceTypeManifest : ISerializable
{
    /// <summary>
    /// Creates a validated script source type manifest instance.
    /// </summary>
    public ScriptSourceTypeManifest() {}

    /// <summary>
    /// Creates a validated script source type manifest instance.
    /// </summary>
    /// <param name="sourcePersistentID">
    /// The source persistent id consumed by script source type manifest; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="sourcePath">
    /// The normalized file-system path used by this operation.
    /// </param>
    /// <param name="declarations">
    /// The declarations consumed by script source type manifest; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public ScriptSourceTypeManifest(
        Guid sourcePersistentID,
        string sourcePath,
        ScriptSourceTypeDeclaration[] declarations
    ) {
        this.sourcePersistentId = sourcePersistentID;
        this.sourcePath = sourcePath;
        this.declarations = declarations;
    }

    /// <summary>
    /// Gets the persistent identity of the script source represented by this manifest.
    /// </summary>
    [SerializableProperty]
    public Guid sourcePersistentId { get; set; }

    /// <summary>
    /// Gets the normalized source path used by the current operation.
    /// </summary>
    [SerializableProperty]
    public string sourcePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets the stable attachable-type declarations discovered in the script source.
    /// </summary>
    [SerializableProperty]
    public ScriptSourceTypeDeclaration[] declarations { get; set; } = [];
}
