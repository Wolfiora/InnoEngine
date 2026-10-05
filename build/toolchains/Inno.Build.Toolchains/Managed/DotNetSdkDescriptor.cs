namespace Inno.Build.Toolchains;

/// <summary>
/// Records the executable and SDK identity selected by resolving one managed entry project.
/// </summary>
public sealed class DotNetSdkDescriptor
{
    internal DotNetSdkDescriptor(
        string hostPath,
        string sdkIdentity
    ) {
        this.hostPath = hostPath;
        this.sdkIdentity = sdkIdentity;
    }

    /// <summary>
    /// Gets the executable used for project-scoped SDK and workload resolution.
    /// </summary>
    public string hostPath { get; }

    /// <summary>
    /// Gets the exact SDK identity selected by the project's global.json resolution rules.
    /// </summary>
    public string sdkIdentity { get; }
}
