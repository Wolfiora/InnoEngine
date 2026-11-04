using System;

namespace Inno.Scripting.Compiler;

/// <summary>
/// Identifies a type declaration in one completed compiler candidate without retaining its assembly.
/// </summary>
/// <remarks>
/// Locations describe the captured source generation. They do not grant access to live assets or
/// imply that the candidate was activated. Consumers must use the result of the failed operation.
/// </remarks>
public sealed class ScriptTypeSource
{
    internal ScriptTypeSource(
        string assemblyName,
        ScriptTypeManifestEntry declaration
    ) {
        this.assemblyName = assemblyName;
        typeName = declaration.typeName;
        typeId = declaration.stableTypeId == Guid.Empty ? null : declaration.stableTypeId;
        sourceId = declaration.sourcePersistentId;
        sourcePath = declaration.sourcePath;
        line = declaration.line;
        column = declaration.column;
    }

    /// <summary>
    /// Gets the assembly's simple identity within this compiler candidate.
    /// </summary>
    public string assemblyName { get; }

    /// <summary>
    /// Gets the fully qualified metadata type name, including nested type separators.
    /// </summary>
    public string typeName { get; }

    /// <summary>
    /// Gets the compiler-declared stable identity, or null when identity belongs to runtime type discovery.
    /// </summary>
    public Guid? typeId { get; }

    /// <summary>
    /// Gets the persistent identity of the canonical source asset.
    /// </summary>
    public Guid sourceId { get; }

    /// <summary>
    /// Gets the source-aware logical asset path captured for the declaration.
    /// </summary>
    public string sourcePath { get; }

    /// <summary>
    /// Gets the one-based declaration line in the captured source.
    /// </summary>
    public int line { get; }

    /// <summary>
    /// Gets the one-based declaration column in the captured source.
    /// </summary>
    public int column { get; }
}
