namespace Inno.Build.Toolchains;

/// <summary>
/// Identifies one immutable native archive selected for a backend product closure.
/// </summary>
/// <param name = "asset">
/// The logical archive name in the native component inventory.
/// </param>
/// <param name = "sha256">
/// The complete lowercase SHA-256 digest verified before import.
/// </param>
public sealed record NativeDependencyAsset(
    string asset,
    string sha256
);
