using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;

namespace Inno.Build.Managed.DotNet;

internal static class DotNetDeploymentPublisher
{
    internal static async ValueTask<ManagedDeploymentResult> PublishAsync(
        ManagedDeploymentId id,
        ManagedDeploymentCapabilities capabilities,
        string hostPath,
        ManagedDeploymentRequest request,
        IReadOnlyList<string> properties,
        CancellationToken cancellationToken
    ) {
        ArgumentNullException.ThrowIfNull(request);
        if (!capabilities.runtimeIdentifiers.Contains(request.runtimeIdentifier, StringComparer.Ordinal))
            throw new InvalidOperationException($"Managed deployment '{id}' does not support '{request.runtimeIdentifier}'.");
        DotNetSdkDescriptor sdk = await DotNetSdkResolver.ResolveAsync(hostPath,
            request.projectPath, cancellationToken).ConfigureAwait(false);
        return await DotNetPublishExecutor.PublishAsync(new DotNetPublishRequest(sdk,
            id, request, properties), cancellationToken).ConfigureAwait(false);
    }
}
