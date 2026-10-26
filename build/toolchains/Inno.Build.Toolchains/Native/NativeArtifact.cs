using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Inno.Core.IO;

namespace Inno.Build.Toolchains;

/// <summary>
/// Describes the link or deployment role explicitly assigned by a native producer.
/// </summary>
public enum NativeArtifactRole
{
    /// <summary>
    /// A library loaded by the runtime.
    /// </summary>
    SharedLibrary,
    /// <summary>
    /// A separate linker input referring to a shared library.
    /// </summary>
    ImportLibrary,
    /// <summary>
    /// An archive linked into the final executable.
    /// </summary>
    StaticArchive,
    /// <summary>
    /// A tool, header or other supporting payload.
    /// </summary>
    SupportFile
}

/// <summary>
/// Identifies one exact output and its producer-declared role.
/// </summary>
public sealed record NativeArtifact
{
    /// <summary>
    /// Captures an output independently of platform suffix conventions.
    /// </summary>
    /// <param name="logicalId">
    /// The unique producer-owned output identity.
    /// </param>
    /// <param name="file">
    /// The absolute output path retained by the product's reading ownership.
    /// </param>
    /// <param name="role">
    /// The explicitly declared link or deployment role.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The identity, path or role is invalid.
    /// </exception>
    public NativeArtifact(
        string logicalId,
        string file,
        NativeArtifactRole role
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(logicalId);
        if (!Path.IsPathFullyQualified(file) || !Enum.IsDefined(role))
            throw new ArgumentException("Native outputs require an absolute file and an explicit supported role.");
        this.logicalId = logicalId;
        this.file = Path.GetFullPath(file);
        this.role = role;
    }

    /// <summary>
    /// Gets the producer-owned output identity.
    /// </summary>
    public string logicalId { get; }
    /// <summary>
    /// Gets the exact absolute output path.
    /// </summary>
    public string file { get; }
    /// <summary>
    /// Gets the explicit link or deployment role.
    /// </summary>
    public NativeArtifactRole role { get; }

    internal static void Write(
        string directory,
        IReadOnlyList<NativeArtifact> artifacts
    ) {
        string output = Path.Combine(directory, "Outputs");
        if (artifacts.Count == 0 || artifacts.Select(static item => item.logicalId).Distinct(StringComparer.Ordinal).Count() != artifacts.Count)
            throw new InvalidDataException("A Native output inventory must contain unique logical identities.");
        foreach (NativeArtifact artifact in artifacts)
            PathBoundary.RequireUnlinkedPath(output, artifact.file);
        if (!artifacts.Select(static artifact => artifact.file).ToHashSet(StringComparer.Ordinal)
            .SetEquals(PathBoundary.EnumerateFiles(output)))
            throw new InvalidDataException("The declared Native output inventory must match every produced file exactly.");
        Directory.CreateDirectory(Path.Combine(directory, "Metadata"));
        File.WriteAllBytes(Path.Combine(directory, "Metadata", "artifacts.json"), JsonSerializer.SerializeToUtf8Bytes(
            artifacts.Select(artifact => new Entry(artifact.logicalId, Path.GetRelativePath(output, artifact.file).Replace('\\', '/'), artifact.role)).ToArray()));
    }

    internal static IReadOnlyList<NativeArtifact> Read(string directory)
    {
        Entry[] entries = JsonSerializer.Deserialize<Entry[]>(File.ReadAllBytes(Path.Combine(directory, "Metadata", "artifacts.json")))
            ?? throw new InvalidDataException("The Native output inventory is absent.");
        string output = Path.Combine(directory, "Outputs");
        return Array.AsReadOnly(entries.Select(entry => new NativeArtifact(entry.logicalId,
            PathBoundary.Resolve(output, entry.path), entry.role)).ToArray());
    }

    private sealed record Entry(
        string logicalId,
        string path,
        NativeArtifactRole role
    );
}
