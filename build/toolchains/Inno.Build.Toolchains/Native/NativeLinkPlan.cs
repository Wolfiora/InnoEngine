using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Build.Toolchains;

/// <summary>
/// Freezes platform library formats and linker configuration independently of component recipes.
/// </summary>
public sealed class NativeLinkPlan
{
    /// <summary>
    /// Captures explicitly supported library formats and ordered linker configuration.
    /// </summary>
    /// <param name="sharedLibraryExtension">
    /// The shared-library suffix, or null when shared linkage is unsupported.
    /// </param>
    /// <param name="importLibraryExtension">
    /// The separate import-library suffix, or null when the shared library is the link input.
    /// </param>
    /// <param name="staticLibraryExtension">
    /// The archive suffix, or null when static linkage is not contributed.
    /// </param>
    /// <param name="cmakeArguments">
    /// Ordered platform-owned CMake linker definitions.
    /// </param>
    /// <exception cref="ArgumentException">
    /// No format is supported, an import format lacks shared linkage, a suffix is invalid, or an argument is null.
    /// </exception>
    public NativeLinkPlan(
        string? sharedLibraryExtension = null,
        string? importLibraryExtension = null,
        string? staticLibraryExtension = null,
        IEnumerable<string>? cmakeArguments = null
    ) {
        if (sharedLibraryExtension is null && staticLibraryExtension is null)
            throw new ArgumentException("A linker plan must contribute at least one library format.");
        if (importLibraryExtension is not null && sharedLibraryExtension is null)
            throw new ArgumentException("Import libraries require a shared-library format.", nameof(importLibraryExtension));
        this.sharedLibraryExtension = sharedLibraryExtension is null ? null : Validate(sharedLibraryExtension);
        this.importLibraryExtension = importLibraryExtension is null ? null : Validate(importLibraryExtension);
        this.staticLibraryExtension = staticLibraryExtension is null ? null : Validate(staticLibraryExtension);
        string[] arguments = (cmakeArguments ?? []).ToArray();
        if (arguments.Any(static argument => argument is null))
            throw new ArgumentException("Linker arguments cannot contain null entries.", nameof(cmakeArguments));
        this.cmakeArguments = Array.AsReadOnly(arguments);
    }

    /// <summary>
    /// Gets the shared-library suffix, or null when shared linkage is unsupported.
    /// </summary>
    public string? sharedLibraryExtension { get; }
    /// <summary>
    /// Gets the separate import suffix, or null when no separate import library is used.
    /// </summary>
    public string? importLibraryExtension { get; }
    /// <summary>
    /// Gets the static archive suffix, or null when static linkage is unavailable.
    /// </summary>
    public string? staticLibraryExtension { get; }
    /// <summary>
    /// Gets ordered platform linker definitions.
    /// </summary>
    public IReadOnlyList<string> cmakeArguments { get; }

    /// <summary>
    /// Formats an explicitly declared logical archive using the selected platform format.
    /// </summary>
    /// <param name="logicalName">
    /// The component-owned logical archive name.
    /// </param>
    /// <returns>
    /// The exact platform file name.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// Static linkage has no registered archive format.
    /// </exception>
    public string GetStaticFileName(string logicalName) => logicalName + GetFileExtension(NativeLibraryKind.Static);

    /// <summary>
    /// Resolves an explicitly supported linkage format before staging or generation begins.
    /// </summary>
    /// <param name="kind">
    /// The component's requested linkage.
    /// </param>
    /// <returns>
    /// The selected file extension; unsupported linkage fails immediately.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// The platform does not contribute this linkage format.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The linkage kind is not defined.
    /// </exception>
    public string GetFileExtension(NativeLibraryKind kind) => (kind switch
    {
        NativeLibraryKind.Shared => sharedLibraryExtension,
        NativeLibraryKind.Static => staticLibraryExtension,
        _ => throw new ArgumentOutOfRangeException(nameof(kind))
    }) ?? throw new NotSupportedException($"This target does not contribute '{kind}' linkage.");

    internal IEnumerable<string> declarations => new[] { "sharedSuffix=" + sharedLibraryExtension,
        "importSuffix=" + importLibraryExtension, "staticSuffix=" + staticLibraryExtension }.Concat(cmakeArguments);

    private static string Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.StartsWith('.') || value.IndexOfAny(['/', '\\', ':']) >= 0)
            throw new ArgumentException("A native output format requires a file suffix.", nameof(value));
        return value;
    }
}
