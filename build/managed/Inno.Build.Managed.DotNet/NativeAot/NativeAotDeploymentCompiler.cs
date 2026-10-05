using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Managed.DotNet;

/// <summary>
/// Publishes explicitly linked game code as a native executable through .NET NativeAOT.
/// </summary>
public sealed class NativeAotDeploymentCompiler : IManagedDeploymentCompiler
{
    private readonly string m_hostPath;

    /// <summary>
    /// Selects the project-scoped .NET SDK host that invokes the installed native compiler toolchain.
    /// </summary>
    /// <param name="hostPath">
    /// The .NET executable selected by the build composition root.
    /// </param>
    public NativeAotDeploymentCompiler(string hostPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(hostPath);
        m_hostPath = hostPath;
    }

    /// <inheritdoc />
    public ManagedDeploymentId id => ManagedDeploymentId.nativeAot;

    /// <inheritdoc />
    public ManagedDeploymentCapabilities capabilities { get; } = new(
        ["win-x64", "osx-arm64", "linux-x64", "linux-arm64"],
        dynamicCode: false, aheadOfTime: true, nativeStaticLinking: true);

    /// <inheritdoc />
    public ValueTask<ManagedDeploymentResult> CompileAsync(
        ManagedDeploymentRequest request,
        CancellationToken cancellationToken = default
    ) => DotNetDeploymentPublisher.PublishAsync(id, capabilities, m_hostPath, request,
        ["SelfContained=true", "PublishAot=true", "PublishTrimmed=true", "TrimMode=full"], cancellationToken);
}
