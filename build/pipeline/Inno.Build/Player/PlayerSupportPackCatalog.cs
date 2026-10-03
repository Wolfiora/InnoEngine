using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
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
    /// The directory whose child names are stable <see cref="BuildTargetId"/> values.
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
        string directory = Path.Combine(m_root, target.value);
        if (!Directory.Exists(directory))
        {
            throw new DirectoryNotFoundException(
                $"Player Support Pack '{target}' is not installed at '{directory}'. " +
                "Install or generate the target Support Pack before exporting.");
        }
        string[] files = Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories).ToArray();
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
        return directory;
    }

    internal async ValueTask<string> ResolveOrProvisionAsync(
        BuildTargetId target,
        IPlayerSupportPackValidator validator,
        IPlayerSupportPackProvisioner? provisioner,
        CancellationToken cancellationToken
    ) {
        try
        {
            return Resolve(target, validator);
        }
        catch (DirectoryNotFoundException) when (provisioner is not null)
        {
            // A missing pack can be produced; an invalid installed pack must remain an explicit failure.
        }

        if (provisioner is null)
            return Resolve(target, validator);

        SemaphoreSlim gate = m_provisionGates.GetOrAdd(target, static _ => new SemaphoreSlim(1, 1));
        await gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            if (Directory.Exists(Path.Combine(m_root, target.value)))
                return Resolve(target, validator);
            await provisioner.ProvisionAsync(target, m_root, cancellationToken).ConfigureAwait(false);
            return Resolve(target, validator);
        }
        finally
        {
            gate.Release();
        }
    }
}
