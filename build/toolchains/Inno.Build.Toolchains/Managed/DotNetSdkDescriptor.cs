namespace Inno.Build.Toolchains;

/// <summary>
/// Freezes the host, SDK identity and managed CLI entry selected by one managed entry project.
/// </summary>
public sealed class DotNetSdkDescriptor
{
    internal DotNetSdkDescriptor(
        string hostPath,
        string sdkIdentity,
        string cliPath
    ) {
        this.hostPath = hostPath;
        this.sdkIdentity = sdkIdentity;
        this.cliPath = cliPath;
    }

    /// <summary>
    /// Gets the executable used for project-scoped SDK and workload resolution.
    /// </summary>
    public string hostPath { get; }

    /// <summary>
    /// Gets the exact SDK identity selected by the project's global.json resolution rules.
    /// </summary>
    public string sdkIdentity { get; }

    /// <summary>
    /// Gets the selected SDK's managed CLI entry assembly, invoked with the recorded host
    /// without resolving another SDK from a temporary execution directory.
    /// </summary>
    public string cliPath { get; }
}
