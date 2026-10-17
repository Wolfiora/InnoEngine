using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Inno.Build.Toolchains;

/// <summary>
/// Specifies the linkage required by a product's native component operation.
/// </summary>
public enum NativeLibraryKind
{
    /// <summary>
    /// Produces archives for the selected final linker.
    /// </summary>
    Static,

    /// <summary>
    /// Produces libraries loaded by the deployed product.
    /// </summary>
    Shared
}

/// <summary>
/// Freezes product-owned linkage, ordered component definitions and their configuration inputs.
/// </summary>
public sealed class NativeComponentBuildOptions
{
    private static readonly Regex S_DEFINITION = new("^-D[A-Za-z_][A-Za-z0-9_]*(?::(?:BOOL|STRING|PATH|FILEPATH|INTERNAL))?=", RegexOptions.CultureInvariant);

    /// <summary>
    /// Captures component configuration without selecting an SDK or starting a build.
    /// </summary>
    /// <param name="libraryKind">
    /// The explicitly requested linkage; a recipe must support it before execution starts.
    /// </param>
    /// <param name="cmakeArguments">
    /// Ordered, unique CMake definitions belonging to this component, excluding SDK and linkage selection.
    /// </param>
    /// <param name="inputPaths">
    /// Portable checkout-relative configuration files included in content hashing and stability checks.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Linkage is invalid, definitions are ambiguous or override SDK selection, or an input escapes the checkout.
    /// </exception>
    public NativeComponentBuildOptions(
        NativeLibraryKind libraryKind,
        IEnumerable<string>? cmakeArguments = null,
        IEnumerable<string>? inputPaths = null
    ) {
        if (!Enum.IsDefined(libraryKind))
            throw new ArgumentException("A supported native library kind is required.", nameof(libraryKind));
        string[] arguments = (cmakeArguments ?? []).ToArray();
        var names = new HashSet<string>(StringComparer.Ordinal);
        foreach (string argument in arguments)
        {
            if (argument is null || !S_DEFINITION.IsMatch(argument) || argument.Contains('\0'))
                throw new ArgumentException("Component arguments must be explicit CMake definitions.", nameof(cmakeArguments));
            string name = argument[2..argument.IndexOf('=')].Split(':')[0];
            if (name.StartsWith("CMAKE_", StringComparison.Ordinal) || name is "INNO_ROOT" or "INNO_NATIVE_TARGET" or "INNO_LIBRARY_KIND"
                || name.StartsWith("INNO_COMPONENT_", StringComparison.Ordinal)
                || !names.Add(name))
                throw new ArgumentException("Component definitions must be unique and cannot override SDK or linkage selection.", nameof(cmakeArguments));
        }
        string[] inputs = (inputPaths ?? []).Select(ValidatePath).Distinct(StringComparer.Ordinal).ToArray();
        this.libraryKind = libraryKind;
        this.cmakeArguments = Array.AsReadOnly(arguments);
        this.inputPaths = Array.AsReadOnly(inputs);
    }

    /// <summary>
    /// Gets the linkage selected by the product rather than by the execution host.
    /// </summary>
    public NativeLibraryKind libraryKind { get; }

    /// <summary>
    /// Gets the immutable ordered component definitions; order contributes to artifact identity.
    /// </summary>
    public IReadOnlyList<string> cmakeArguments { get; }

    /// <summary>
    /// Gets logical configuration input paths whose actual bytes participate in the recipe.
    /// </summary>
    public IReadOnlyList<string> inputPaths { get; }

    internal IEnumerable<string> declarations => new[] { "libraryKind=" + libraryKind }
        .Concat(cmakeArguments.Select(static argument => "componentArgument=" + argument));

    private static string ValidatePath(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        string portable = path.Replace('\\', '/');
        if (portable.StartsWith('/') || portable.Contains(':') || portable.Split('/').Any(static segment => segment is "" or "." or ".."))
            throw new ArgumentException("A component input must be a portable checkout-relative path.", nameof(path));
        return portable;
    }
}
