namespace Inno.Build.Toolchains;
/// <summary>
/// Identifies one immutable download selected from the current repository's dependency releases.
/// </summary>
/// <param name = "release">
/// The exact release tag, never a mutable latest selector.
/// </param>
/// <param name = "asset">
/// The exact archive name in that release.
/// </param>
/// <param name = "sha256">
/// The complete lowercase SHA-256 digest verified before import.
/// </param>
public sealed record NativeDependencyAsset(
    string release,
    string asset,
    string sha256
);
