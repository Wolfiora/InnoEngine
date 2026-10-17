using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Build.Managed;

/// <summary>
/// Describes the execution and native-link capabilities of one publisher over explicit runtime targets.
/// </summary>
public sealed class ManagedDeploymentCapabilities
{
    /// <summary>
    /// Freezes the execution policy and supported runtime identifiers declared by a provider.
    /// </summary>
    /// <param name="runtimeIdentifiers">
    /// The exact runtime identifiers accepted by the selected managed toolchain.
    /// </param>
    /// <param name="dynamicCode">
    /// Whether the deployed runtime supports generating executable managed code.
    /// </param>
    /// <param name="aheadOfTime">
    /// Whether this publisher compiles managed code before deployment.
    /// </param>
    /// <param name="nativeStaticLinking">
    /// Whether native object inputs can participate in the runtime's final link.
    /// </param>
    public ManagedDeploymentCapabilities(
        IReadOnlyList<string> runtimeIdentifiers,
        bool dynamicCode,
        bool aheadOfTime,
        bool nativeStaticLinking
    ) {
        ArgumentNullException.ThrowIfNull(runtimeIdentifiers);
        if (runtimeIdentifiers.Count == 0 || runtimeIdentifiers.Any(string.IsNullOrWhiteSpace)
            || runtimeIdentifiers.Distinct(StringComparer.Ordinal).Count() != runtimeIdentifiers.Count)
            throw new ArgumentException("A managed provider requires unique runtime targets.", nameof(runtimeIdentifiers));
        this.runtimeIdentifiers = Array.AsReadOnly(runtimeIdentifiers.ToArray());
        this.dynamicCode = dynamicCode;
        this.aheadOfTime = aheadOfTime;
        this.nativeStaticLinking = nativeStaticLinking;
    }

    /// <summary>
    /// Gets the immutable supported runtime identifiers.
    /// </summary>
    public IReadOnlyList<string> runtimeIdentifiers { get; }

    /// <summary>
    /// Gets whether the deployed runtime permits dynamic executable code generation.
    /// </summary>
    public bool dynamicCode { get; }

    /// <summary>
    /// Gets whether managed code is compiled ahead of runtime execution.
    /// </summary>
    public bool aheadOfTime { get; }

    /// <summary>
    /// Gets whether native objects participate in final linking.
    /// </summary>
    public bool nativeStaticLinking { get; }
}
