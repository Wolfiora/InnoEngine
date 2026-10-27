using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Inno.Scripting.Compiler;

/// <summary>
/// Freezes the framework metadata and compiler identity borrowed by one compilation or IDE projection.
/// </summary>
internal sealed class ScriptCompilationEnvironment
{
    internal const string C_TARGET_FRAMEWORK = "net10.0";
    internal const string C_LANGUAGE_VERSION = "14.0";

    internal ScriptCompilationEnvironment(
        IReadOnlyList<MetadataReference> frameworkReferences,
        IReadOnlySet<string> frameworkAssemblyNames,
        string fingerprint,
        ScriptCompilationInputs inputs
    ) {
        this.frameworkReferences = frameworkReferences;
        this.frameworkAssemblyNames = frameworkAssemblyNames;
        this.fingerprint = fingerprint;
        this.inputs = inputs;
    }

    internal IReadOnlyList<MetadataReference> frameworkReferences { get; }
    internal IReadOnlySet<string> frameworkAssemblyNames { get; }
    internal string fingerprint { get; }
    internal ScriptCompilationInputs inputs { get; }
}
