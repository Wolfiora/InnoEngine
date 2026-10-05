using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Managed.DotNet;

/// <summary>
/// Publishes explicitly linked game code with the .NET desktop CoreCLR runtime.
/// </summary>
public sealed class CoreClrDeploymentCompiler : IManagedDeploymentCompiler
{
    private readonly string m_hostPath;

    /// <summary>
    /// Selects the SDK host used for project-scoped publication.
    /// </summary>
    /// <param name="hostPath">
    /// The .NET executable selected by the build composition root.
    /// </param>
    public CoreClrDeploymentCompiler(string hostPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(hostPath);
        m_hostPath = hostPath;
    }

    /// <inheritdoc />
    public ManagedDeploymentId id => ManagedDeploymentId.coreClr;

    /// <inheritdoc />
    public ManagedDeploymentCapabilities capabilities { get; } = new(
        ["win-x64", "osx-arm64", "linux-x64", "linux-arm64"],
        dynamicCode: true, aheadOfTime: false, nativeStaticLinking: false);

    /// <inheritdoc />
    public ValueTask<ManagedDeploymentResult> CompileAsync(
        ManagedDeploymentRequest request,
        CancellationToken cancellationToken = default
    ) => DotNetDeploymentPublisher.PublishAsync(id, capabilities, m_hostPath, request,
        ["SelfContained=true", "PublishSingleFile=true", "PublishTrimmed=true", "TrimMode=full",
            "IncludeNativeLibrariesForSelfExtract=true"], cancellationToken);
}
