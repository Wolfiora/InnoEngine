using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Inno.Build.Toolchains;

/// <summary>
/// Freezes one publisher-owned cache entry and the generations it requires.
/// </summary>
public sealed class BuildCacheArtifactDescriptor
{
    /// <summary>
    /// Freezes ownership metadata supplied by the artifact's actual publisher.
    /// </summary>
    /// <param name="directory">
    /// The absolute published artifact or producer-owned workspace directory.
    /// </param>
    /// <param name="leasePath">
    /// The absolute persistent file used by this entry's actual producer and readers for Core.IO coordination.
    /// </param>
    /// <param name="bytes">
    /// The publisher-recorded sum of owned file lengths, captured under writing ownership for cache budgeting.
    /// It does not measure allocated disk blocks or count shared hardlinks as recovered volume space.
    /// </param>
    /// <param name="targetId">
    /// The explicit product target, or null for a shared host artifact that target-scoped cleanup must preserve.
    /// </param>
    /// <param name="lastUseUtc">
    /// The publisher's last recorded use, used only to order retirement.
    /// </param>
    /// <param name="dependencies">
    /// Absolute artifact directories required by this generation.
    /// </param>
    /// <param name="role">
    /// The producer-declared lifetime; workspace preference alone does not pin an immutable product.
    /// </param>
    /// <exception cref="ArgumentException">
    /// A path is relative or the timestamp is not UTC.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The measured size is negative.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The dependency collection is absent.
    /// </exception>
    public BuildCacheArtifactDescriptor(
        string directory,
        string leasePath,
        string? targetId,
        long bytes,
        DateTime lastUseUtc,
        IReadOnlyList<string> dependencies,
        BuildCacheArtifactRole role
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(directory);
        ArgumentException.ThrowIfNullOrWhiteSpace(leasePath);
        ArgumentNullException.ThrowIfNull(dependencies);
        if (!Path.IsPathFullyQualified(directory) || !Path.IsPathFullyQualified(leasePath)
            || dependencies.Any(static path => !Path.IsPathFullyQualified(path)))
            throw new ArgumentException("Cache artifact paths must be absolute.");
        if (bytes < 0)
            throw new ArgumentOutOfRangeException(nameof(bytes));
        if (targetId is not null && string.IsNullOrWhiteSpace(targetId))
            throw new ArgumentException("A declared cache target cannot be blank.", nameof(targetId));
        if (lastUseUtc.Kind != DateTimeKind.Utc)
            throw new ArgumentException("Cache use timestamps must be UTC.", nameof(lastUseUtc));
        this.directory = Path.TrimEndingDirectorySeparator(Path.GetFullPath(directory));
        this.leasePath = Path.GetFullPath(leasePath);
        this.targetId = targetId;
        this.bytes = bytes;
        this.lastUseUtc = lastUseUtc;
        if (!Enum.IsDefined(role))
            throw new ArgumentOutOfRangeException(nameof(role));
        this.role = role;
        this.dependencies = new ReadOnlyCollection<string>(dependencies.Select(static path =>
            Path.TrimEndingDirectorySeparator(Path.GetFullPath(path))).ToArray());
    }

    /// <summary>
    /// Gets the absolute cache entry controlled by the declaring producer.
    /// </summary>
    public string directory { get; }

    /// <summary>
    /// Gets the exact existing producer/reader coordination path; retirement does not infer another lock protocol.
    /// </summary>
    public string leasePath { get; }

    /// <summary>
    /// Gets the producer-declared target, or null when this artifact is shared by host operations.
    /// </summary>
    public string? targetId { get; }

    /// <summary>
    /// Gets the logical file size used for budgeting, without weakening content validation.
    /// </summary>
    public long bytes { get; }

    /// <summary>
    /// Gets the declared lifetime used for retirement and preferred-workspace accounting.
    /// </summary>
    public BuildCacheArtifactRole role { get; }

    /// <summary>
    /// Gets the UTC use timestamp, which is never used to validate cached content.
    /// </summary>
    public DateTime lastUseUtc { get; }

    /// <summary>
    /// Gets the frozen dependency directories protected whenever this artifact is a live root.
    /// </summary>
    public IReadOnlyList<string> dependencies { get; }
}
