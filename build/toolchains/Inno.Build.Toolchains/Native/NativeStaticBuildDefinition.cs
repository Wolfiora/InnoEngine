using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Build.Toolchains;

/// <summary>
/// Declares a backend-owned static-library recipe and its complete checkout input closure.
/// </summary>
public sealed class NativeStaticBuildDefinition
{
    /// <summary>
    /// Freezes the component's CMake entry point, source dependencies and exact installed archive names.
    /// </summary>
    /// <param name="cmakeFile">
    /// The portable checkout-relative component definition.
    /// </param>
    /// <param name="inputPaths">
    /// The portable checkout-relative source and build dependencies.
    /// </param>
    /// <param name="targetIds">
    /// The exact ABI targets supported by this archive recipe.
    /// </param>
    /// <param name="archiveNames">
    /// The exact archive file names required after installation.
    /// </param>
    /// <exception cref="ArgumentException">
    /// A path escapes its root or an archive name is invalid or duplicated.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// A required collection is null.
    /// </exception>
    public NativeStaticBuildDefinition(
        string cmakeFile,
        IEnumerable<string> inputPaths,
        IEnumerable<string> archiveNames,
        IEnumerable<string> targetIds
    ) {
        ArgumentNullException.ThrowIfNull(inputPaths);
        ArgumentNullException.ThrowIfNull(archiveNames);
        this.cmakeFile = ValidatePath(cmakeFile);
        this.inputPaths = Array.AsReadOnly(inputPaths.Select(ValidatePath).Distinct(StringComparer.Ordinal).ToArray());
        string[] archives = archiveNames.Select(ValidatePath).ToArray();
        if (archives.Length == 0 || archives.Any(static name => name.Contains('/') || !name.EndsWith(".a", StringComparison.Ordinal))
            || archives.Distinct(StringComparer.OrdinalIgnoreCase).Count() != archives.Length)
            throw new ArgumentException("A static recipe requires unique portable archive file names.", nameof(archiveNames));
        this.archiveNames = Array.AsReadOnly(archives);
        ArgumentNullException.ThrowIfNull(targetIds);
        string[] targets = targetIds.Select(ValidatePath).ToArray();
        if (targets.Length == 0 || targets.Any(static id => id.Contains('/'))
            || targets.Distinct(StringComparer.Ordinal).Count() != targets.Length)
            throw new ArgumentException("Static recipes require explicit unique target identities.", nameof(targetIds));
        this.targetIds = Array.AsReadOnly(targets);
    }

    /// <summary>
    /// Gets the component-owned CMake definition, which participates in artifact identity.
    /// </summary>
    public string cmakeFile { get; }

    /// <summary>
    /// Gets the immutable complete source dependency paths relative to the checkout.
    /// </summary>
    public IReadOnlyList<string> inputPaths { get; }

    /// <summary>
    /// Gets the exact required output closure within this component's target directory.
    /// </summary>
    public IReadOnlyList<string> archiveNames { get; }

    /// <summary>
    /// Gets the exact supported target identities; other archive formats require a separate recipe.
    /// </summary>
    public IReadOnlyList<string> targetIds { get; }

    private static string ValidatePath(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        string portable = path.Replace('\\', '/');
        if (portable.StartsWith('/') || portable.Contains(':') || portable.Split('/').Any(static part => part is "" or "." or ".."))
            throw new ArgumentException("A static recipe path must remain within its declared root.", nameof(path));
        return portable;
    }
}
