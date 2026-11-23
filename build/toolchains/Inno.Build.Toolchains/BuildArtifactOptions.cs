using System;
using System.Collections.Generic;

namespace Inno.Build.Toolchains;

/// <summary>
/// Defines the deterministic filter and naming policy used to collect one native product.
/// </summary>
/// <param name="buildDirName">
/// The dependency-relative directory searched for native build outputs.
/// </param>
/// <param name="libraryTokens">
/// File-name tokens that identify artifacts belonging to the product.
/// </param>
/// <param name="extensions">
/// The accepted native artifact extensions.
/// </param>
/// <param name="requiredPathTokens">
/// Optional normalized path tokens used to reject unrelated intermediate files.
/// </param>
/// <param name="normalizeOutputName">
/// The deterministic output naming policy.
/// </param>
public sealed record BuildArtifactOptions(
    string buildDirName,
    IReadOnlyCollection<string> libraryTokens,
    IReadOnlyCollection<string> extensions,
    IReadOnlyCollection<string>? requiredPathTokens,
    Func<string, string, string> normalizeOutputName
);
