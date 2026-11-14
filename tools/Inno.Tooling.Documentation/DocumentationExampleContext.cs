using System.Collections.Generic;

namespace Inno.Tooling.Documentation;

/// <summary>
/// Identifies the authored compilation context for a page's executable examples without preparing it.
/// </summary>
/// <param name="mode">
/// The explicit reference mode: host, runtime-script or editor-script.
/// Script modes require the actual projected assemblies, never substituted host references.
/// </param>
/// <param name="projects">
/// Actual project identities supplying the SDK context and host dependencies.
/// For script modes they identify ownership only; their host assemblies must not enter the example's references.
/// </param>
public sealed record DocumentationExampleContext(
    string mode,
    IReadOnlyList<string> projects
);
