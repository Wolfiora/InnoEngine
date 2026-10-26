using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;

namespace Inno.Build.Managed.DotNet;

/// <summary>
/// Publishes explicitly linked game code with the .NET desktop CoreCLR runtime.
/// </summary>
public sealed class CoreClrDeploymentCompiler : IManagedDeploymentCompiler
{
    private readonly string m_hostPath;
    private readonly IToolchainWorkspaceProvider m_workspaces;

    /// <summary>
    /// Selects the SDK host used for project-scoped publication.
    /// </summary>
    /// <param name="hostPath">
    /// The .NET executable selected by the build composition root.
    /// </param>
    /// <param name="runtimeIdentifiers">
    /// The platform-contributed managed targets authorized for this implementation.
    /// </param>
    /// <param name="workspaces">
    /// The explicit host execution path provider.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The SDK host is empty or runtime targets are empty, unassigned, or repeated.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The target collection or workspace provider is null.
    /// </exception>
    public CoreClrDeploymentCompiler(
        string hostPath,
        IReadOnlyList<string> runtimeIdentifiers,
        IToolchainWorkspaceProvider workspaces
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(hostPath);
        ArgumentNullException.ThrowIfNull(workspaces);
        m_hostPath = hostPath;
        m_workspaces = workspaces;
        capabilities = new ManagedDeploymentCapabilities(runtimeIdentifiers,
            dynamicCode: true, aheadOfTime: false, nativeStaticLinking: false);
    }

    /// <inheritdoc />
    public ManagedDeploymentId id => ManagedDeploymentId.coreClr;

    /// <inheritdoc />
    public ManagedDeploymentCapabilities capabilities { get; }

    /// <inheritdoc />
    public ValueTask<ManagedDeploymentResult> CompileAsync(
        ManagedDeploymentRequest request,
        CancellationToken cancellationToken = default
    ) => DotNetDeploymentPublisher.PublishAsync(id, capabilities, m_hostPath, m_workspaces, request,
        ["SelfContained=true", "PublishSingleFile=true", "PublishTrimmed=true", "TrimMode=full",
            "IncludeNativeLibrariesForSelfExtract=true"], cancellationToken);
}
