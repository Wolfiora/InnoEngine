using System;
using System.IO;

namespace Inno.Build.Managed;

/// <summary>
/// Freezes the build-owned project, code closure and output locations supplied to a managed publisher.
/// </summary>
public sealed class ManagedDeploymentRequest
{
    /// <summary>
    /// Resolves all filesystem locations without starting processes or creating output directories.
    /// </summary>
    /// <param name="projectPath">
    /// The prepared entry project with explicit code references, generated registrations and native inputs.
    /// </param>
    /// <param name="runtimeIdentifier">
    /// The managed toolchain's exact target identifier.
    /// </param>
    /// <param name="codeInputDirectory">
    /// The isolated directory containing the frozen code inputs referenced by the entry project.
    /// </param>
    /// <param name="outputDirectory">
    /// The empty build-owned staging directory; final installation remains the pipeline's responsibility.
    /// </param>
    /// <param name="logDirectory">
    /// The build-owned directory receiving process output and SDK selection evidence.
    /// </param>
    public ManagedDeploymentRequest(
        string projectPath,
        string runtimeIdentifier,
        string codeInputDirectory,
        string outputDirectory,
        string logDirectory
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(projectPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(runtimeIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(codeInputDirectory);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputDirectory);
        ArgumentException.ThrowIfNullOrWhiteSpace(logDirectory);
        this.projectPath = Path.GetFullPath(projectPath);
        this.runtimeIdentifier = runtimeIdentifier;
        this.codeInputDirectory = Path.GetFullPath(codeInputDirectory);
        this.outputDirectory = Path.GetFullPath(outputDirectory);
        this.logDirectory = Path.GetFullPath(logDirectory);
    }

    /// <summary>
    /// Gets the prepared entry project owned by this build.
    /// </summary>
    public string projectPath { get; }

    /// <summary>
    /// Gets the exact target identifier accepted by the selected managed toolchain.
    /// </summary>
    public string runtimeIdentifier { get; }

    /// <summary>
    /// Gets the isolated frozen code input directory.
    /// </summary>
    public string codeInputDirectory { get; }

    /// <summary>
    /// Gets the managed publication staging directory.
    /// </summary>
    public string outputDirectory { get; }

    /// <summary>
    /// Gets the directory receiving process output and SDK selection evidence.
    /// </summary>
    public string logDirectory { get; }
}
