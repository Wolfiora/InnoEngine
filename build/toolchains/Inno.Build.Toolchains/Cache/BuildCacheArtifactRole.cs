namespace Inno.Build.Toolchains;

/// <summary>
/// Declares the producer's file lifetime independently of target identity and directory naming.
/// </summary>
public enum BuildCacheArtifactRole
{
    /// <summary>
    /// A complete immutable generation remains protected by selected products and active readers.
    /// </summary>
    ImmutableArtifact,

    /// <summary>
    /// A mutable compiler workspace may retire when no producer or consumer is active.
    /// </summary>
    ReusableWorkspace,

    /// <summary>
    /// An operation directory remains protected until its exact process owners have exited.
    /// </summary>
    ProcessWorkingDirectory
}
