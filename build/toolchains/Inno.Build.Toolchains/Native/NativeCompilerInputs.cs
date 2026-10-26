using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Inno.Build.Toolchains;

/// <summary>
/// Freezes SDK-resolved native compiler and header inputs without interpreting environment conventions.
/// </summary>
public sealed class NativeCompilerInputs
{
    /// <summary>
    /// Captures explicit parser inputs supplied by the target SDK provider.
    /// </summary>
    /// <param name="cCompiler">
    /// The selected C compiler, or null when this selection does not provide one.
    /// </param>
    /// <param name="cppCompiler">
    /// The selected C++ compiler, or null when this selection does not provide one.
    /// </param>
    /// <param name="sysRoot">
    /// The SDK root, or null when the compiler does not use a sysroot.
    /// </param>
    /// <param name="systemIncludeDirectories">
    /// Ordered system header directories.
    /// </param>
    /// <param name="cppSystemIncludeDirectories">
    /// Ordered C++ standard library directories.
    /// </param>
    /// <param name="defines">
    /// Ordered target preprocessor definitions.
    /// </param>
    /// <param name="arguments">
    /// Ordered parser arguments supplied by the SDK.
    /// </param>
    /// <exception cref="ArgumentException">
    /// An assigned physical path is not absolute.
    /// </exception>
    public NativeCompilerInputs(
        string? cCompiler = null,
        string? cppCompiler = null,
        string? sysRoot = null,
        IEnumerable<string>? systemIncludeDirectories = null,
        IEnumerable<string>? cppSystemIncludeDirectories = null,
        IEnumerable<string>? defines = null,
        IEnumerable<string>? arguments = null
    ) {
        this.cCompiler = Validate(cCompiler);
        this.cppCompiler = Validate(cppCompiler);
        this.sysRoot = Validate(sysRoot);
        this.systemIncludeDirectories = Array.AsReadOnly((systemIncludeDirectories ?? []).Select(path => Validate(path)!).ToArray());
        this.cppSystemIncludeDirectories = Array.AsReadOnly((cppSystemIncludeDirectories ?? []).Select(path => Validate(path)!).ToArray());
        this.defines = Array.AsReadOnly((defines ?? []).ToArray());
        this.arguments = Array.AsReadOnly((arguments ?? []).ToArray());
    }

    /// <summary>
    /// Gets the explicit C compiler, or null when unavailable.
    /// </summary>
    public string? cCompiler { get; }
    /// <summary>
    /// Gets the explicit C++ compiler, or null when unavailable.
    /// </summary>
    public string? cppCompiler { get; }
    /// <summary>
    /// Gets the target SDK root, or null when unnecessary.
    /// </summary>
    public string? sysRoot { get; }
    /// <summary>
    /// Gets ordered SDK header directories.
    /// </summary>
    public IReadOnlyList<string> systemIncludeDirectories { get; }
    /// <summary>
    /// Gets ordered C++ standard library directories.
    /// </summary>
    public IReadOnlyList<string> cppSystemIncludeDirectories { get; }
    /// <summary>
    /// Gets ordered target definitions.
    /// </summary>
    public IReadOnlyList<string> defines { get; }
    /// <summary>
    /// Gets ordered target parser arguments.
    /// </summary>
    public IReadOnlyList<string> arguments { get; }

    internal IEnumerable<string> declarations => new[] { "cCompiler=" + cCompiler, "cppCompiler=" + cppCompiler, "sysRoot=" + sysRoot }
        .Concat(systemIncludeDirectories.Select(static path => "systemInclude=" + path))
        .Concat(cppSystemIncludeDirectories.Select(static path => "cppSystemInclude=" + path))
        .Concat(defines.Select(static value => "compilerDefine=" + value))
        .Concat(arguments.Select(static value => "compilerArgument=" + value));

    private static string? Validate(string? path)
    {
        if (path is not null && (string.IsNullOrWhiteSpace(path) || !Path.IsPathFullyQualified(path)))
            throw new ArgumentException("Native compiler inputs require explicit absolute paths.", nameof(path));
        return path;
    }
}
