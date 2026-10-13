using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Build.SupportPacks;

/// <summary>
/// Freezes distribution-selected files and native deployment groups independently of platform packaging.
/// </summary>
public sealed class PlayerSupportPackRequirements
{
    /// <summary>
    /// Captures the exact publication inputs and native groups supplied by the selected product closure.
    /// </summary>
    /// <param name="target">
    /// The stable target whose inputs are being validated.
    /// </param>
    /// <param name="requiredFiles">
    /// Portable Pack-relative files required by the selected managed and binding graph.
    /// </param>
    /// <param name="nativeRoot">
    /// The Pack-relative root containing the selected runtime libraries or static archives.
    /// </param>
    /// <param name="nativeDirectories">
    /// Native-root-relative deployment groups, each requiring at least one selected library.
    /// </param>
    /// <param name="nativeExtension">
    /// The explicit SDK library suffix; it is not inferred from this process.
    /// </param>
    /// <param name="nativeFiles">
    /// The exact native-root-relative library set, or null when deployment metadata validates filenames.
    /// </param>
    /// <exception cref="ArgumentException">
    /// A target, extension or portable relative path is invalid or repeated.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// A required collection is null.
    /// </exception>
    public PlayerSupportPackRequirements(
        BuildTargetId target,
        IReadOnlyList<string> requiredFiles,
        string nativeRoot,
        IReadOnlyList<string> nativeDirectories,
        string nativeExtension,
        IReadOnlyList<string>? nativeFiles = null
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(target.value);
        ArgumentNullException.ThrowIfNull(requiredFiles);
        ArgumentNullException.ThrowIfNull(nativeDirectories);
        ArgumentException.ThrowIfNullOrWhiteSpace(nativeExtension);
        if (!nativeExtension.StartsWith(".", StringComparison.Ordinal) || nativeExtension.Contains('/') || nativeExtension.Contains('\\'))
            throw new ArgumentException("A selected native suffix must be one file extension.", nameof(nativeExtension));
        ValidatePath(nativeRoot);
        foreach (string path in requiredFiles.Concat(nativeDirectories))
            ValidatePath(path);
        foreach (string path in nativeFiles ?? [])
            ValidatePath(path);
        if (requiredFiles.Distinct(StringComparer.Ordinal).Count() != requiredFiles.Count
            || nativeDirectories.Distinct(StringComparer.Ordinal).Count() != nativeDirectories.Count
            || nativeFiles is not null && nativeFiles.Distinct(StringComparer.Ordinal).Count() != nativeFiles.Count)
            throw new ArgumentException("Selected publication inputs cannot repeat ownership paths.");
        if (nativeFiles is not null && nativeFiles.Any(path => !path.EndsWith(nativeExtension, StringComparison.Ordinal)
            || !nativeDirectories.Any(group => path.StartsWith(group + "/", StringComparison.Ordinal))))
            throw new ArgumentException("Exact selected libraries must belong to their declared native groups and suffix.", nameof(nativeFiles));
        this.target = target;
        this.requiredFiles = Array.AsReadOnly(requiredFiles.ToArray());
        this.nativeRoot = nativeRoot;
        this.nativeDirectories = Array.AsReadOnly(nativeDirectories.ToArray());
        this.nativeExtension = nativeExtension;
        this.nativeFiles = nativeFiles is null ? null : Array.AsReadOnly(nativeFiles.ToArray());
    }

    /// <summary>
    /// Gets the explicitly selected target.
    /// </summary>
    public BuildTargetId target { get; }

    /// <summary>
    /// Gets the immutable required Pack-relative input paths.
    /// </summary>
    public IReadOnlyList<string> requiredFiles { get; }

    /// <summary>
    /// Gets the portable Pack-relative native deployment root.
    /// </summary>
    public string nativeRoot { get; }

    /// <summary>
    /// Gets the immutable native deployment groups supplied by the selected steps.
    /// </summary>
    public IReadOnlyList<string> nativeDirectories { get; }

    /// <summary>
    /// Gets the library suffix selected by the target SDK.
    /// </summary>
    public string nativeExtension { get; }

    /// <summary>
    /// Gets the exact selected library filenames, or null when an implementation validates its deployment metadata.
    /// </summary>
    public IReadOnlyList<string>? nativeFiles { get; }

    private static void ValidatePath(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        if (path.Contains('\\') || path.Contains(':') || path.StartsWith('/')
            || path.Split('/').Any(static segment => segment is "" or "." or ".."))
            throw new ArgumentException("Publication requirements must use portable owned relative paths.", nameof(path));
    }
}
