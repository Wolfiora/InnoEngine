using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Inno.Build.Toolchains;

/// <summary>
/// Identifies one complete, integrity-checked native artifact and its exact published file closure.
/// </summary>
public sealed class NativeBuildProduct : IDisposable
{
    private readonly BuildArtifactReadLease m_readLease;

    internal NativeBuildProduct(
        string component,
        string targetId,
        string fingerprint,
        string directory,
        NativeBindingGenerationDescriptor? bindingGeneration,
        string? sharedLibraryExtension,
        BuildArtifactReadLease readLease
    ) {
        this.component = component;
        this.targetId = targetId;
        this.fingerprint = fingerprint;
        this.directory = directory;
        this.bindingGeneration = bindingGeneration;
        this.sharedLibraryExtension = sharedLibraryExtension;
        m_readLease = readLease;
        try
        {
            artifacts = NativeArtifact.Read(directory);
            files = Array.AsReadOnly(artifacts.Select(static artifact => artifact.file).Order(StringComparer.Ordinal).ToArray());
        }
        catch
        {
            readLease.Dispose();
            throw;
        }
    }

    /// <summary>
    /// Gets the component identity supplied by the owning toolchain.
    /// </summary>
    public string component { get; }

    /// <summary>
    /// Gets the target ABI selected for this build.
    /// </summary>
    public string targetId { get; }

    /// <summary>
    /// Gets the content fingerprint covering sources, tool executables and toolchain declarations.
    /// </summary>
    public string fingerprint { get; }

    /// <summary>
    /// Gets the immutable artifact root containing Outputs and its integrity manifest.
    /// </summary>
    public string directory { get; }

    /// <summary>
    /// Gets the exact binding generation compiled into this product, or null for a component without bindings.
    /// Managed consumers must retain this selection rather than resolve a new generation after native preparation.
    /// </summary>
    public NativeBindingGenerationDescriptor? bindingGeneration { get; }

    /// <summary>
    /// Gets the shared-library suffix declared by the selected SDK, or null for an SDK-independent producer.
    /// </summary>
    public string? sharedLibraryExtension { get; }

    /// <summary>
    /// Gets the exact absolute output file paths validated by the publisher.
    /// </summary>
    public IReadOnlyList<string> files { get; }

    /// <summary>
    /// Gets the exact producer-declared output and link roles.
    /// </summary>
    public IReadOnlyList<NativeArtifact> artifacts { get; }

    /// <summary>
    /// Releases the artifact after all deployment, compilation and linking consumers finish.
    /// File paths must not be used afterwards; repeated disposal has no effect.
    /// </summary>
    public void Dispose() => m_readLease.Dispose();
}
