namespace Reloadable.PrivateDependency;

/// <summary>
/// Provides a private module dependency whose value exposes the selected generation's dependency resolution.
/// </summary>
public static class VersionSource
{
    /// <summary>
    /// Gets the fixed base value used to distinguish the two reloadable module implementations.
    /// </summary>
    public static int baseVersion => 1;
}
