namespace Inno.Build.Toolchains;

/// <summary>
/// Identifies exact generated or external bytes needed by a native compilation.
/// </summary>
/// <param name="rootId">
/// The component-local input root.
/// </param>
/// <param name="relativePath">
/// The portable root-relative filename.
/// </param>
/// <param name="length">
/// The expected byte length.
/// </param>
/// <param name="sha256">
/// The lowercase SHA-256 content identity.
/// </param>
/// <param name="compile">
/// Whether this file is a translation unit rather than an include dependency.
/// </param>
public sealed record NativeBindingSourceFile(
    string rootId,
    string relativePath,
    long length,
    string sha256,
    bool compile
);
