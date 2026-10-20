namespace Inno.Build.Toolchains;

/// <summary>
/// Preserves the order and search semantics of one native include requirement.
/// </summary>
/// <param name="rootId">
/// The component-local input root.
/// </param>
/// <param name="relativePath">
/// The root-relative directory, or empty for the root itself.
/// </param>
/// <param name="system">
/// Whether this directory contains system headers.
/// </param>
/// <param name="framework">
/// Whether the compiler must use framework search semantics.
/// </param>
public sealed record NativeBindingIncludeDirectory(
    string rootId,
    string relativePath,
    bool system,
    bool framework
);
