using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Inno.Assets.Pipeline;
using Inno.Build.Toolchains;
using Inno.Core.IO;

namespace Inno.Build.Toolchains.Bgfx.Shaders;

/// <summary>
/// Fixes the complete authored shader file inventory and content identity for one compilation operation.
/// </summary>
/// <remarks>
/// Permanent import metadata participates in the identity; derived artifacts, source noise and atomic
/// publication candidates do not. Compilation copies these inputs into its owned source tree. This
/// value neither locks the authoring source nor authorizes its modification or cleanup.
/// </remarks>
public sealed class ShaderBuildInputs
{
    private readonly string m_sourceRoot;
    private readonly NativeBuildInput[] m_files;
    private readonly string[] m_directories;

    private ShaderBuildInputs(
        string sourceRoot,
        Inventory inventory,
        CancellationToken cancellationToken
    ) {
        m_sourceRoot = sourceRoot;
        m_files = inventory.files;
        m_directories = inventory.directories;
        fingerprint = ComputeFingerprint(inventory, cancellationToken);
    }

    /// <summary>
    /// Gets the complete logical directory, file and content identity used by the enclosing build.
    /// </summary>
    public string fingerprint { get; }

    /// <summary>
    /// Captures permanent authoring files and their import metadata without consuming temporary publications.
    /// </summary>
    /// <param name="assetRoot">
    /// The existing physical authoring root, containing no source links.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels discovery and complete content hashing.
    /// </param>
    /// <returns>
    /// A fixed input inventory and content identity; physical source files remain owned by their author.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The source root is blank.
    /// </exception>
    /// <exception cref="IOException">
    /// A source cannot be read, does not exist, or crosses a link.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Capture was canceled.
    /// </exception>
    public static ShaderBuildInputs Capture(
        string assetRoot,
        CancellationToken cancellationToken = default
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(assetRoot);
        string root = Path.TrimEndingDirectorySeparator(Path.GetFullPath(assetRoot));
        return new ShaderBuildInputs(root, Discover(root, cancellationToken), cancellationToken);
    }

    /// <summary>
    /// Rechecks the complete current inventory and bytes against this operation's captured identity.
    /// </summary>
    /// <param name="cancellationToken">
    /// Cancels discovery and complete content verification.
    /// </param>
    /// <exception cref="InvalidDataException">
    /// A source or permanent metadata file was added, removed or changed.
    /// </exception>
    /// <exception cref="IOException">
    /// The source tree cannot be safely inspected or read.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Verification was canceled.
    /// </exception>
    public void VerifyUnchanged(CancellationToken cancellationToken = default)
    {
        string actual = ComputeFingerprint(Discover(m_sourceRoot, cancellationToken), cancellationToken);
        if (!string.Equals(actual, fingerprint, StringComparison.Ordinal))
            throw new InvalidDataException("Shader authoring inputs changed during compilation; the candidate cannot be published.");
    }

    internal string Materialize(
        string operationDirectory,
        CancellationToken cancellationToken
    ) {
        string destination = PathBoundary.Resolve(operationDirectory, "Source");
        Directory.CreateDirectory(destination);
        foreach (string relative in m_directories)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Directory.CreateDirectory(PathBoundary.RequireUnlinkedPath(destination, PathBoundary.Resolve(destination, relative)));
        }
        List<NativeBuildInput> copies = [];
        foreach (NativeBuildInput input in m_files)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string relative = Path.GetRelativePath(m_sourceRoot, input.physicalPath);
            string source = PathBoundary.RequireUnlinkedPath(m_sourceRoot, input.physicalPath);
            string copy = PathBoundary.RequireUnlinkedPath(destination, PathBoundary.Resolve(destination, relative));
            Directory.CreateDirectory(Path.GetDirectoryName(copy)!);
            using (var reading = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var writing = new FileStream(copy, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                reading.CopyToAsync(writing, cancellationToken).GetAwaiter().GetResult();
            copies.Add(new NativeBuildInput(input.logicalPath, copy));
        }
        if (ComputeFingerprint(new Inventory([.. copies], m_directories), cancellationToken) != fingerprint)
            throw new InvalidDataException("Shader inputs changed while copying the frozen compilation tree.");
        VerifyUnchanged(cancellationToken);
        return destination;
    }

    private static string ComputeFingerprint(
        Inventory inventory,
        CancellationToken cancellationToken
    ) => NativeBuildFingerprint.Create(
        inventory.directories.Select(static path => "shader-directory/" + path), inventory.files, cancellationToken);

    private static Inventory Discover(
        string root,
        CancellationToken cancellationToken
    ) {
        if (!Directory.Exists(root))
            throw new DirectoryNotFoundException($"Shader authoring root '{root}' does not exist.");
        var pending = new Stack<string>();
        List<NativeBuildInput> files = [];
        List<string> directories = [];
        pending.Push(root);
        AssetSourcePolicy policy = AssetSourcePolicy.defaultPolicy;
        while (pending.TryPop(out string? directory))
        {
            cancellationToken.ThrowIfCancellationRequested();
            PathBoundary.RequireUnlinkedPath(root, directory);
            directories.Add(directory == root ? string.Empty : Path.GetRelativePath(root, directory).Replace('\\', '/'));
            foreach (string child in Directory.EnumerateDirectories(directory))
            {
                if (!policy.IsIgnored(Path.GetRelativePath(root, child), isDirectory: true))
                    pending.Push(child);
            }
            foreach (string path in Directory.EnumerateFiles(directory))
            {
                cancellationToken.ThrowIfCancellationRequested();
                string relative = Path.GetRelativePath(root, path).Replace('\\', '/');
                if (AtomicFile.IsCandidatePath(relative))
                    continue;
                bool metadata = relative.EndsWith(".imeta", StringComparison.OrdinalIgnoreCase);
                if (!metadata && policy.IsIgnored(relative, isDirectory: false))
                    continue;
                PathBoundary.RequireUnlinkedPath(root, path);
                files.Add(new NativeBuildInput("shader-source/" + relative, path));
            }
        }
        return new Inventory(files.OrderBy(static input => input.logicalPath, StringComparer.Ordinal).ToArray(),
            directories.Order(StringComparer.Ordinal).ToArray());
    }

    private sealed record Inventory(
        NativeBuildInput[] files,
        string[] directories
    );
}
