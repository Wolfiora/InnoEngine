using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Inno.Core.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build;

/// <summary>
/// Resolves and validates installed Player Support Packs without loading authoring services.
/// </summary>
public sealed class PlayerSupportPackCatalog
{
    private static readonly HashSet<string> S_FORBIDDEN_EXTENSIONS = new(StringComparer.OrdinalIgnoreCase)
    {
        ".cs", ".csproj", ".dbg", ".map", ".pdb", ".rsp", ".sln", ".xml"
    };

    private static readonly string[] S_FORBIDDEN_NAME_TOKENS =
    [
        "Microsoft.CodeAnalysis",
        "Roslyn",
        "Inno.Editor",
        "Inno.Build",
        "Inno.Scripting.Compiler",
        "Inno.Assets.Pipeline",
        "Inno.Plugins.Authoring",
        "shaderc",
        "texturec"
    ];

    private readonly string m_root;
    private readonly ConcurrentDictionary<BuildTargetId, SemaphoreSlim> m_provisionGates = new();

    /// <summary>
    /// Creates a catalog rooted at the directory containing target-specific Player Support Packs.
    /// </summary>
    /// <param name="root">
    /// The directory whose target children contain immutable fingerprint directories and a current index.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="root"/> is empty.
    /// </exception>
    public PlayerSupportPackCatalog(string root)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(root);
        m_root = Path.GetFullPath(root);
    }

    /// <summary>
    /// Resolves one installed Support Pack after validating its deployment-only closure.
    /// </summary>
    /// <param name="target">
    /// The platform and architecture identity of the required Support Pack.
    /// </param>
    /// <returns>
    /// The normalized directory containing the verified Support Pack.
    /// </returns>
    /// <exception cref="DirectoryNotFoundException">
    /// Thrown when no Support Pack is installed for <paramref name="target"/>.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// Thrown when the pack is empty, contains build-time payload, or lacks its Player executable or native runtimes.
    /// </exception>
    /// <param name="validator">
    /// The target-owned validator for runtime or linker inputs.
    /// </param>
    public string Resolve(
        BuildTargetId target,
        IPlayerSupportPackValidator validator
    ) {
        ArgumentNullException.ThrowIfNull(validator);
        string targetRoot = Path.Combine(m_root, target.value);
        string index = Path.Combine(targetRoot, "current");
        if (!File.Exists(index))
        {
            throw new DirectoryNotFoundException(
                $"Player Support Pack '{target}' is not installed at '{targetRoot}'. " +
                "Install or generate the target Support Pack before exporting.");
        }
        string fingerprint = File.ReadAllText(index);
        if (fingerprint.Length != 64 || fingerprint.Any(static character => !char.IsAsciiHexDigitLower(character)))
            throw new InvalidDataException($"Player Support Pack '{target}' has an invalid current fingerprint.");
        string directory = Path.Combine(targetRoot, fingerprint);
        ValidateDirectory(directory, target, validator);
        if (ComputeFingerprint(directory, CancellationToken.None) != fingerprint)
            throw new InvalidDataException($"Player Support Pack '{target}' no longer matches its immutable fingerprint.");
        return directory;
    }

    /// <summary>
    /// Validates and publishes an immutable pack, then atomically selects it for new readers.
    /// </summary>
    /// <param name="target">
    /// The platform and architecture owning the staged inputs.
    /// </param>
    /// <param name="stagingDirectory">
    /// A complete, exclusively owned staging tree on the same filesystem as this catalog.
    /// Successful publication consumes this directory unless identical inputs already exist.
    /// </param>
    /// <param name="validator">
    /// The target validator for runtime and linker inputs.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels validation or lease acquisition before selecting the completed pack.
    /// </param>
    /// <returns>
    /// The immutable directory selected for subsequent builds; earlier reader directories are retained.
    /// </returns>
    /// <exception cref="InvalidDataException">
    /// The candidate is incomplete, contains forbidden inputs or collides with a damaged installed artifact.
    /// </exception>
    /// <exception cref="IOException">
    /// The completed directory or its atomic current index cannot be installed.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Cancellation occurred before the current index was committed.
    /// </exception>
    public async ValueTask<string> PublishAsync(
        BuildTargetId target,
        string stagingDirectory,
        IPlayerSupportPackValidator validator,
        CancellationToken cancellationToken = default
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(stagingDirectory);
        ArgumentNullException.ThrowIfNull(validator);
        cancellationToken.ThrowIfCancellationRequested();
        string staging = Path.GetFullPath(stagingDirectory);
        ValidateDirectory(staging, target, validator);
        string fingerprint = ComputeFingerprint(staging, cancellationToken);
        string targetRoot = Path.Combine(m_root, target.value);
        Directory.CreateDirectory(targetRoot);
        using FileLease ownership = await FileLease.AcquireAsync(
            Path.Combine(targetRoot, "current.lock"), Timeout.InfiniteTimeSpan, cancellationToken).ConfigureAwait(false);
        string destination = Path.Combine(targetRoot, fingerprint);
        if (Directory.Exists(destination))
        {
            ValidateDirectory(destination, target, validator);
            if (ComputeFingerprint(destination, cancellationToken) != fingerprint)
                throw new InvalidDataException($"Installed Support Pack artifact '{destination}' has been modified.");
        }
        else
        {
            cancellationToken.ThrowIfCancellationRequested();
            AtomicDirectory.Publish(staging, destination, cancellationToken);
        }
        cancellationToken.ThrowIfCancellationRequested();
        AtomicFile.WriteAllBytes(Path.Combine(targetRoot, "current"), Encoding.UTF8.GetBytes(fingerprint));
        return destination;
    }

    private static void ValidateDirectory(
        string directory,
        BuildTargetId target,
        IPlayerSupportPackValidator validator
    ) {
        if (!Directory.Exists(directory))
            throw new InvalidDataException($"Player Support Pack '{target}' points to a missing artifact '{directory}'.");
        string[] files = PathBoundary.EnumerateFiles(directory).ToArray();
        if (files.Length == 0)
            throw new InvalidDataException($"Player Support Pack '{target}' is empty.");
        foreach (string file in files)
        {
            string relativePath = Path.GetRelativePath(directory, file);
            if (relativePath.StartsWith("PlayerLink" + Path.DirectorySeparatorChar, StringComparison.Ordinal))
                continue;
            string name = Path.GetFileName(file);
            if (S_FORBIDDEN_EXTENSIONS.Contains(Path.GetExtension(name))
                || S_FORBIDDEN_NAME_TOKENS.Any(token => name.Contains(token, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidDataException(
                    $"Player Support Pack '{target}' contains forbidden build-time file '{name}'.");
            }
        }
        string referenceDirectory = Path.Combine(directory, "References");
        if (!Directory.Exists(referenceDirectory)
            || !Directory.EnumerateFiles(referenceDirectory, "Inno.*.dll", SearchOption.TopDirectoryOnly).Any())
        {
            throw new InvalidDataException($"Player Support Pack '{target}' has no target compilation references.");
        }
        validator.Validate(directory);
    }

    internal async ValueTask<string> ResolveOrProvisionAsync(
        BuildTargetId target,
        IPlayerSupportPackValidator validator,
        IPlayerSupportPackProvisioner? provisioner,
        CancellationToken cancellationToken
    ) {
        cancellationToken.ThrowIfCancellationRequested();
        if (provisioner is null)
            return Resolve(target, validator);
        if (File.Exists(Path.Combine(m_root, target.value, "current")))
            _ = Resolve(target, validator);

        SemaphoreSlim gate = m_provisionGates.GetOrAdd(target, static _ => new SemaphoreSlim(1, 1));
        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            // The source provider checks its current SDK, code and native inputs before selecting a pack.
            await provisioner.ProvisionAsync(target, m_root, cancellationToken).ConfigureAwait(false);
            return Resolve(target, validator);
        }
        finally
        {
            gate.Release();
        }
    }

    private static string ComputeFingerprint(
        string directory,
        CancellationToken cancellationToken
    ) {
        using IncrementalHash hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        Span<byte> length = stackalloc byte[sizeof(int)];
        foreach (string file in PathBoundary.EnumerateFiles(directory)
            .OrderBy(path => Path.GetRelativePath(directory, path).Replace('\\', '/'), StringComparer.Ordinal))
        {
            cancellationToken.ThrowIfCancellationRequested();
            string relative = Path.GetRelativePath(directory, file).Replace('\\', '/');
            byte[] path = Encoding.UTF8.GetBytes(relative);
            System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(length, path.Length);
            hash.AppendData(length);
            hash.AppendData(path);
            using FileStream input = File.OpenRead(file);
            hash.AppendData(SHA256.HashData(input));
        }
        return Convert.ToHexStringLower(hash.GetHashAndReset());
    }
}
