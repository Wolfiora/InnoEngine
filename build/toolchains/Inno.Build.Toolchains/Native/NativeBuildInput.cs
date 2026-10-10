using System;
using System.IO;

namespace Inno.Build.Toolchains;

/// <summary>
/// Separates the reproducible identity of an input from its physical reading location.
/// </summary>
public sealed record NativeBuildInput
{
    /// <summary>
    /// Declares one file or directory consumed by a native recipe.
    /// </summary>
    /// <param name="logicalPath">
    /// A nonempty, slash-separated identity relative to a declared logical root.
    /// </param>
    /// <param name="physicalPath">
    /// The absolute file or directory used only for reading and execution.
    /// </param>
    /// <exception cref="ArgumentException">
    /// An identity is empty, contains a control character or backslash, or the physical path is relative.
    /// </exception>
    public NativeBuildInput(
        string logicalPath,
        string physicalPath
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(logicalPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(physicalPath);
        if (logicalPath.Contains('\\') || logicalPath.AsSpan().IndexOfAnyInRange('\0', '\x1f') >= 0)
            throw new ArgumentException("A native input identity must use portable, printable path segments.", nameof(logicalPath));
        if (!Path.IsPathFullyQualified(physicalPath))
            throw new ArgumentException("A native input reading location must be absolute.", nameof(physicalPath));
        this.logicalPath = logicalPath.TrimEnd('/');
        if (this.logicalPath.Length == 0)
            throw new ArgumentException("A native input identity requires a logical root.", nameof(logicalPath));
        this.physicalPath = Path.GetFullPath(physicalPath);
    }

    /// <summary>
    /// Gets the identity included in the recipe fingerprint.
    /// </summary>
    public string logicalPath { get; }

    /// <summary>
    /// Gets the reading location, which is not implicitly included in the fingerprint.
    /// </summary>
    public string physicalPath { get; }

    /// <summary>
    /// Declares a checkout-relative input, or an external SDK location whose path affects tool selection.
    /// </summary>
    /// <param name="engineRoot">
    /// The absolute checkout root assigned the logical prefix <c>engine/</c>.
    /// </param>
    /// <param name="path">
    /// A checkout-relative input or an absolute SDK input.
    /// </param>
    /// <returns>
    /// A normalized declaration preserving external SDK location identity.
    /// </returns>
    public static NativeBuildInput FromPath(
        string engineRoot,
        string path
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(engineRoot);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        string physical = Path.GetFullPath(path, engineRoot);
        string relative = Path.GetRelativePath(engineRoot, physical).Replace('\\', '/');
        string logical = relative != ".." && !relative.StartsWith("../", StringComparison.Ordinal)
            && !Path.IsPathRooted(relative)
            ? "engine/" + relative
            : "external/" + physical.Replace('\\', '/').TrimStart('/');
        return new NativeBuildInput(logical, physical);
    }
}
