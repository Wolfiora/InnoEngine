using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using Inno.Core.IO;

namespace Inno.Build.Toolchains;

/// <summary>
/// Persists explicit publisher registrations for maintenance without loading the production distribution.
/// </summary>
public static class BuildCacheOwnerCatalog
{
    /// <summary>
    /// Registers a publisher root; this never adopts unrecorded artifacts inside that root.
    /// </summary>
    /// <param name="engineRoot">
    /// The absolute checkout owning the registration.
    /// </param>
    /// <param name="directory">
    /// The exact publisher root beneath the checkout.
    /// </param>
    /// <param name="id">
    /// The distribution's unique owner identity.
    /// </param>
    /// <param name="role">
    /// The publisher's actual artifact lifetime.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels before registration commits.
    /// </param>
    /// <exception cref="IOException">
    /// A registration crosses a link or ownership boundary.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// An existing root has a different registration.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Cancellation precedes commit.
    /// </exception>
    public static void Register(
        string engineRoot,
        string directory,
        string id,
        BuildCacheArtifactRole role,
        CancellationToken cancellationToken = default
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        if (!Enum.IsDefined(role))
            throw new ArgumentOutOfRangeException(nameof(role));
        string root = Path.GetFullPath(engineRoot);
        string owned = PathBoundary.RequireUnlinkedPath(root, directory);
        if (owned == root)
            throw new ArgumentException("The checkout cannot be registered as a cache.");
        string relative = Path.GetRelativePath(root, owned).Replace('\\', '/');
        string catalog = CatalogRoot(root);
        string key = Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(relative)));
        string file = Path.Combine(catalog, key + ".json");
        using FileLease writing = FileLease.AcquireAsync(Path.Combine(catalog, "owner.lock"),
            TimeSpan.FromSeconds(30), cancellationToken).AsTask().GetAwaiter().GetResult();
        var registration = new Registration(id, relative, role);
        if (File.Exists(file) && Read(file) != registration)
            throw new InvalidDataException("A cache root cannot change its registered owner or lifetime.");
        cancellationToken.ThrowIfCancellationRequested();
        AtomicFile.WriteAllBytes(file, JsonSerializer.SerializeToUtf8Bytes(registration));
    }

    /// <summary>
    /// Reads explicitly registered roots using each publisher's existing inventory and protection rules.
    /// </summary>
    /// <param name="engineRoot">
    /// The absolute checkout containing registrations.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels reading before a cleanup plan exists.
    /// </param>
    /// <returns>
    /// Frozen owner descriptors; an unprepared checkout returns an empty list.
    /// </returns>
    /// <exception cref="InvalidDataException">
    /// A registration is malformed or has an ambiguous identity.
    /// </exception>
    /// <exception cref="IOException">
    /// A registered path crosses a link or leaves the checkout.
    /// </exception>
    public static IReadOnlyList<BuildCacheOwnerDescriptor> ReadOwners(
        string engineRoot,
        CancellationToken cancellationToken = default
    ) {
        string root = Path.GetFullPath(engineRoot);
        string catalog = CatalogRoot(root);
        if (!Directory.Exists(catalog))
            return [];
        using FileLease reading = FileLease.AcquireSharedAsync(Path.Combine(catalog, "owner.lock"),
            TimeSpan.FromSeconds(30), cancellationToken).AsTask().GetAwaiter().GetResult();
        var result = new List<BuildCacheOwnerDescriptor>();
        var identities = new HashSet<string>(StringComparer.Ordinal);
        foreach (string file in PathBoundary.EnumerateFiles(catalog).Where(static file => file.EndsWith(".json", StringComparison.Ordinal)))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Registration value = Read(file);
            if (!identities.Add(value.id))
                throw new InvalidDataException("Cache registration identities must be unique.");
            string owned = PathBoundary.RequireUnlinkedPath(root, PathBoundary.Resolve(root, value.directory));
            result.Add(new BuildCachePublisher(owned, value.role).Describe(value.id));
        }
        return result.AsReadOnly();
    }

    private static string CatalogRoot(string root)
        => PathBoundary.RequireUnlinkedPath(root, Path.Combine(root, "artifacts", "cache-owners"));

    private static Registration Read(string path)
    {
        using JsonDocument document = JsonDocument.Parse(File.ReadAllBytes(path));
        JsonElement root = document.RootElement;
        if (root.ValueKind != JsonValueKind.Object || root.EnumerateObject().Count() != 3
            || !root.TryGetProperty("id", out JsonElement id) || id.ValueKind != JsonValueKind.String
            || !root.TryGetProperty("directory", out JsonElement directory) || directory.ValueKind != JsonValueKind.String
            || !root.TryGetProperty("role", out JsonElement role) || !role.TryGetInt32(out int lifetime))
            throw new InvalidDataException("A cache registration lacks its exact owner, directory and lifetime.");
        var value = new Registration(id.GetString()!, directory.GetString()!, (BuildCacheArtifactRole)lifetime);
        if (string.IsNullOrWhiteSpace(value.id) || string.IsNullOrWhiteSpace(value.directory)
            || Path.IsPathRooted(value.directory) || value.directory == "." || !Enum.IsDefined(value.role))
            throw new InvalidDataException("A cache registration has invalid ownership metadata.");
        string key = Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(value.directory)));
        if (Path.GetFileName(path) != key + ".json")
            throw new InvalidDataException("A cache registration has a mismatched publisher key.");
        return value;
    }

    private sealed record Registration(
        string id,
        string directory,
        BuildCacheArtifactRole role
    );
}
