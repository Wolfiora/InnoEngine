using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Toolchains;

/// <summary>
/// Binds one product component to an owned build operation and explicit deployment layout.
/// </summary>
public sealed class ProductNativeBuildStep
{
    /// <summary>
    /// Freezes a component step without embedding any backend list in the shared executor.
    /// </summary>
    /// <param name="id">
    /// The expected published component identity.
    /// </param>
    /// <param name="component">
    /// The unique source and recipe owners.
    /// </param>
    /// <param name="options">
    /// Explicit product linkage, ordered component parameters and their input closure.
    /// </param>
    /// <param name="dependencies">
    /// Earlier component identities required by this operation.
    /// </param>
    /// <param name="build">
    /// The awaited standalone producer, or null when the component is executed by a static aggregate.
    /// </param>
    /// <param name="deploymentPath">
    /// Maps a relative output and target to a deployment path, or null for build-only inputs.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The identity or dependency set is invalid.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// A required owner, producer or path policy is null.
    /// </exception>
    public ProductNativeBuildStep(
        string id,
        NativeComponentDescriptor component,
        NativeComponentBuildOptions options,
        IReadOnlyList<string> dependencies,
        Func<NativeBuildContext, IReadOnlyDictionary<string, NativeBuildProduct>, CancellationToken, Task<NativeBuildProduct>>? build,
        Func<string, string, string?> deploymentPath
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentNullException.ThrowIfNull(component);
        ArgumentNullException.ThrowIfNull(options);
        if (build is null && options.libraryKind != NativeLibraryKind.Static)
            throw new ArgumentException("Aggregate-only steps require explicit static linkage.", nameof(options));
        if (build is not null && options.libraryKind != NativeLibraryKind.Shared)
            throw new ArgumentException("Standalone steps support shared linkage; archive recipes use aggregation.", nameof(options));
        ArgumentNullException.ThrowIfNull(dependencies);
        if (build is null && component.staticBuild is null)
            throw new ArgumentException("Aggregate-only steps require a real static build definition.", nameof(build));
        ArgumentNullException.ThrowIfNull(deploymentPath);
        if (dependencies.Any(string.IsNullOrWhiteSpace) || dependencies.Distinct(StringComparer.Ordinal).Count() != dependencies.Count)
            throw new ArgumentException("Component dependencies must be assigned and unique.", nameof(dependencies));
        this.id = id;
        this.component = component;
        this.options = options;
        this.dependencies = Array.AsReadOnly(dependencies.ToArray());
        this.build = build;
        this.deploymentPath = deploymentPath;
    }

    /// <summary>
    /// Gets the exact expected component identity.
    /// </summary>
    public string id { get; }

    /// <summary>
    /// Gets the explicit source and recipe owners.
    /// </summary>
    public NativeComponentDescriptor component { get; }

    /// <summary>
    /// Gets product-owned configuration independently of the selected SDK.
    /// </summary>
    public NativeComponentBuildOptions options { get; }

    /// <summary>
    /// Gets dependencies that must precede this step.
    /// </summary>
    public IReadOnlyList<string> dependencies { get; }

    /// <summary>
    /// Gets the standalone producer, or null for a component owned by a static aggregate executor.
    /// </summary>
    public Func<NativeBuildContext, IReadOnlyDictionary<string, NativeBuildProduct>, CancellationToken, Task<NativeBuildProduct>>? build { get; }

    /// <summary>
    /// Gets the explicit output layout policy; null results exclude build-only files.
    /// </summary>
    public Func<string, string, string?> deploymentPath { get; }
}
