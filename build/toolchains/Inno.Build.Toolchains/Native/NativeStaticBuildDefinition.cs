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
    /// <param name="logicalOutputs">
    /// The exact archive file names required after installation.
    /// </param>
    /// <param name="nativeSourceDirectory">
    /// The declared checkout-relative root used by the component's native CMake fragment, or null when no such fragment is consumed.
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
        IEnumerable<string> logicalOutputs,
        string? nativeSourceDirectory = null
    ) {
        ArgumentNullException.ThrowIfNull(inputPaths);
        ArgumentNullException.ThrowIfNull(logicalOutputs);
        this.cmakeFile = ValidatePath(cmakeFile);
        this.inputPaths = Array.AsReadOnly(inputPaths.Select(ValidatePath).Distinct(StringComparer.Ordinal).ToArray());
        this.nativeSourceDirectory = nativeSourceDirectory is null ? null : ValidatePath(nativeSourceDirectory);
        if (this.nativeSourceDirectory is not null && !this.inputPaths.Any(path => path == this.nativeSourceDirectory
            || path.StartsWith(this.nativeSourceDirectory + "/", StringComparison.Ordinal)))
            throw new ArgumentException("The native source root must contain declared static recipe inputs.", nameof(nativeSourceDirectory));
        string[] archives = logicalOutputs.Select(ValidatePath).ToArray();
        if (archives.Length == 0 || archives.Any(static name => name.Contains('/'))
            || archives.Distinct(StringComparer.OrdinalIgnoreCase).Count() != archives.Length)
            throw new ArgumentException("A static recipe requires unique portable archive file names.", nameof(logicalOutputs));
        this.logicalOutputs = Array.AsReadOnly(archives);

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
    /// Gets the declared native fragment's source root, or null when the recipe only consumes its other inputs.
    /// </summary>
    public string? nativeSourceDirectory { get; }

    /// <summary>
    /// Gets the exact required output closure within this component's target directory.
    /// </summary>
    public IReadOnlyList<string> logicalOutputs { get; }


    private static string ValidatePath(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        string portable = path.Replace('\\', '/');
        if (portable.StartsWith('/') || portable.Contains(':') || portable.Split('/').Any(static part => part is "" or "." or ".."))
            throw new ArgumentException("A static recipe path must remain within its declared root.", nameof(path));
        return portable;
    }
}
