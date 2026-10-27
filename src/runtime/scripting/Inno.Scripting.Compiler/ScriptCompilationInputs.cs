using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Inno.Scripting.Compiler;

/// <summary>
/// Describes framework references and predefined symbols selected by the authoring host's SDK.
/// </summary>
public sealed class ScriptCompilationInputs
{
    /// <summary>
    /// Freezes a complete C# 14 and .NET 10 reference contract without retaining SDK objects.
    /// </summary>
    /// <param name="sdkIdentity">
    /// The exact SDK identity that evaluated this contract.
    /// </param>
    /// <param name="frameworkReferences">
    /// Absolute framework assembly paths selected by SDK reference resolution.
    /// </param>
    /// <param name="predefinedSymbols">
    /// Framework and configuration symbols produced by SDK target execution.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The identity, reference set or a reference path is invalid.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// A collection is null.
    /// </exception>
    public ScriptCompilationInputs(
        string sdkIdentity,
        IEnumerable<string> frameworkReferences,
        IEnumerable<string> predefinedSymbols
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(sdkIdentity);
        ArgumentNullException.ThrowIfNull(frameworkReferences);
        ArgumentNullException.ThrowIfNull(predefinedSymbols);
        string[] references = frameworkReferences.Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray();
        if (references.Length == 0 || references.Any(static path => !Path.IsPathFullyQualified(path)))
            throw new ArgumentException("SDK framework references must be a nonempty set of absolute paths.", nameof(frameworkReferences));
        this.sdkIdentity = sdkIdentity;
        this.frameworkReferences = Array.AsReadOnly(references);
        this.predefinedSymbols = Array.AsReadOnly(predefinedSymbols.Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray());
    }

    /// <summary>
    /// Gets the exact SDK identity used for IDE consistency checks and artifact identity.
    /// </summary>
    public string sdkIdentity { get; }

    /// <summary>
    /// Gets the SDK-selected framework paths, whose content is frozen before Roslyn compilation.
    /// </summary>
    public IReadOnlyList<string> frameworkReferences { get; }

    /// <summary>
    /// Gets SDK-produced symbols shared by runtime compilation and IDE projects.
    /// </summary>
    public IReadOnlyList<string> predefinedSymbols { get; }
}
