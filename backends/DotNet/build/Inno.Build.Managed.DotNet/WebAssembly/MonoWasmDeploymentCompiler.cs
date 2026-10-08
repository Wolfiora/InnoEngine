using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Managed.DotNet;

/// <summary>
/// Publishes explicitly linked code through the SDK's Mono WebAssembly interpreter or AOT toolchain.
/// </summary>
public sealed class MonoWasmDeploymentCompiler : IManagedDeploymentCompiler
{
    private readonly string m_hostPath;
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
    public MonoWasmDeploymentCompiler(
        string hostPath,
        bool aheadOfTime
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(hostPath);
        m_hostPath = hostPath;
        m_aheadOfTime = aheadOfTime;
        capabilities = new ManagedDeploymentCapabilities(["browser-wasm"],
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
    ) => DotNetDeploymentPublisher.PublishAsync(id, capabilities, m_hostPath, request,
        ["WasmBuildNative=true", "PublishTrimmed=true", "TrimMode=full",
            "RunAOTCompilation=" + (m_aheadOfTime ? "true" : "false")], cancellationToken);
}
