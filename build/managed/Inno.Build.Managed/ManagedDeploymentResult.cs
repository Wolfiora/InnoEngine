using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Inno.Build.Managed;

/// <summary>
/// Describes verified staging output produced by one managed deployment compiler.
/// </summary>
public sealed class ManagedDeploymentResult
{
    /// <summary>
    /// Freezes successful publication evidence without granting ownership of the pipeline's staging root.
    /// </summary>
    /// <param name="deployment">
    /// The compiler identity that produced this output.
    /// </param>
    /// <param name="outputDirectory">
    /// The directory containing the completed managed publication.
    /// </param>
    /// <param name="sdkIdentity">
    /// The exact managed SDK selected from the entry project's configuration.
    /// </param>
    /// <param name="files">
    /// The nonempty set of relative output file paths, copied before returning to the pipeline.
    /// </param>
    public ManagedDeploymentResult(
        ManagedDeploymentId deployment,
        string outputDirectory,
        string sdkIdentity,
        IReadOnlyList<string> files
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(deployment.value);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputDirectory);
        ArgumentException.ThrowIfNullOrWhiteSpace(sdkIdentity);
        ArgumentNullException.ThrowIfNull(files);
        if (files.Count == 0 || files.Any(static file => string.IsNullOrWhiteSpace(file)
            || Path.IsPathRooted(file) || file.Split('/', '\\').Any(static segment => segment is "." or ".."))
            || files.Distinct(StringComparer.Ordinal).Count() != files.Count)
            throw new ArgumentException("Managed output requires unique relative artifact paths.", nameof(files));
        this.deployment = deployment;
        this.outputDirectory = Path.GetFullPath(outputDirectory);
        this.sdkIdentity = sdkIdentity;
        this.files = Array.AsReadOnly(files.ToArray());
    }

    /// <summary>
    /// Gets the managed compiler identity that produced this publication.
    /// </summary>
    public ManagedDeploymentId deployment { get; }

    /// <summary>
    /// Gets the completed build-owned publication directory.
    /// </summary>
    public string outputDirectory { get; }

    /// <summary>
    /// Gets the project-selected managed SDK identity.
    /// </summary>
    public string sdkIdentity { get; }

    /// <summary>
    /// Gets the frozen relative output file paths.
    /// </summary>
    public IReadOnlyList<string> files { get; }
}
