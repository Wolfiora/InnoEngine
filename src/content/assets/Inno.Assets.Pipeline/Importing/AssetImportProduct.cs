using System;
using System.Collections.Generic;

namespace Inno.Assets.Pipeline;

internal readonly struct AssetImportProduct(
    AssetObject asset,
    IReadOnlyDictionary<string, ReadOnlyMemory<byte>> outputs,
    IReadOnlyList<string> diagnostics,
    IReadOnlySet<string> authoringOutputs,
    AssetDeploymentScope? deploymentScope
) {
    internal AssetObject asset { get; } = asset ?? throw new ArgumentNullException(nameof(asset));
    internal IReadOnlyDictionary<string, ReadOnlyMemory<byte>> outputs { get; } = outputs;
    internal IReadOnlyList<string> diagnostics { get; } = diagnostics;
    internal IReadOnlySet<string> authoringOutputs { get; } = authoringOutputs;
    internal AssetDeploymentScope? deploymentScope { get; } = deploymentScope;

    internal ReadOnlyMemory<byte> runtimePayload
        => outputs.TryGetValue("runtime", out ReadOnlyMemory<byte> bytes)
           ? bytes
           : ReadOnlyMemory<byte>.Empty;
}
