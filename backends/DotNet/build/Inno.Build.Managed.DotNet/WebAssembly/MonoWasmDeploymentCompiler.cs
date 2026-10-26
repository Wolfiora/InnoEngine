using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;

namespace Inno.Build.Managed.DotNet;

/// <summary>
/// Publishes explicitly linked code through the SDK's Mono WebAssembly interpreter or AOT toolchain.
/// </summary>
public sealed class MonoWasmDeploymentCompiler : IManagedDeploymentCompiler
{
    private readonly string m_hostPath;
    private readonly IToolchainWorkspaceProvider m_workspaces;
    private readonly bool m_aheadOfTime;

    /// <summary>
    /// Selects the project-scoped SDK host and one explicit compilation policy.
    /// </summary>
    /// <param name="hostPath">
    /// The .NET executable whose project resolution selects the matching WebAssembly workload.
    /// </param>
    /// <param name="aheadOfTime">
    /// Whether managed code is compiled ahead of browser execution.
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
    public MonoWasmDeploymentCompiler(
        string hostPath,
        IReadOnlyList<string> runtimeIdentifiers,
        IToolchainWorkspaceProvider workspaces,
        bool aheadOfTime
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(hostPath);
        ArgumentNullException.ThrowIfNull(workspaces);
        m_hostPath = hostPath;
        m_workspaces = workspaces;
        m_aheadOfTime = aheadOfTime;
        capabilities = new ManagedDeploymentCapabilities(runtimeIdentifiers,
            dynamicCode: false, aheadOfTime: aheadOfTime, nativeStaticLinking: true);
    }

    /// <inheritdoc />
    public ManagedDeploymentId id => m_aheadOfTime ? ManagedDeploymentId.monoWasmAot : ManagedDeploymentId.monoWasm;

    /// <inheritdoc />
    public ManagedDeploymentCapabilities capabilities { get; }

    /// <inheritdoc />
    public ValueTask<ManagedDeploymentResult> CompileAsync(
        ManagedDeploymentRequest request,
        CancellationToken cancellationToken = default
    ) => DotNetDeploymentPublisher.PublishAsync(id, capabilities, m_hostPath, m_workspaces, request,
        ["WasmBuildNative=true", "PublishTrimmed=true", "TrimMode=full",
            "RunAOTCompilation=" + (m_aheadOfTime ? "true" : "false")], cancellationToken);
}
