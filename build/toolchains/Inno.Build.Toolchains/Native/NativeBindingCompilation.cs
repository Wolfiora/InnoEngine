using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using Inno.Core.IO;

namespace Inno.Build.Toolchains;

/// <summary>
/// Describes a selected component's native requirements independently of its source generator.
/// </summary>
public sealed class NativeBindingCompilation
{
    private static readonly JsonSerializerOptions S_OPTIONS = new()
    {
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
        RespectRequiredConstructorParameters = true
    };

    /// <summary>
    /// Gets root bindings whose identities are local to this component.
    /// </summary>
    public required NativeBindingSourceRoot[] roots { get; init; }

    /// <summary>
    /// Gets all exact input files; only files marked compile are translation units.
    /// </summary>
    public required NativeBindingSourceFile[] files { get; init; }

    /// <summary>
    /// Gets include requirements in their original search order.
    /// </summary>
    public required NativeBindingIncludeDirectory[] includeDirectories { get; init; }

    /// <summary>
    /// Gets ordered definitions for manifest translation units; upstream target configuration remains component-owned.
    /// </summary>
    public required string[] defines { get; init; }

    /// <summary>
    /// Gets parser target macro facts; these must not override the consuming compiler's builtins.
    /// </summary>
    public required string[] compilerDefines { get; init; }

    /// <summary>
    /// Gets the minimum C++ standard, or zero when no C++ translation unit is required.
    /// </summary>
    public required int cxxStandard { get; init; }

    /// <summary>
    /// Gets the minimum C standard, or zero when no C translation unit is required.
    /// </summary>
    public required int cStandard { get; init; }

    /// <summary>
    /// Gets the exact native symbols that the component must provide.
    /// </summary>
    public required string[] requiredExports { get; init; }

    /// <summary>
    /// Gets static table entry points, or an empty array for dynamic-only bindings.
    /// </summary>
    public required string[] staticExportSymbols { get; init; }

    /// <summary>
    /// Gets the static table ABI digest, or an empty value when no static table exists.
    /// </summary>
    public required string staticExportAbi { get; init; }

    /// <summary>
    /// Gets static table entries in address-resolution order.
    /// </summary>
    public required string[] staticExportNames { get; init; }

    /// <summary>
    /// Reads the current engine-owned compilation description from a selected native output.
    /// </summary>
    /// <param name="nativeDirectory">
    /// The complete selected generation's native directory.
    /// </param>
    /// <returns>
    /// The validated description; missing or malformed descriptions fail explicitly.
    /// </returns>
    /// <exception cref="IOException">
    /// The description cannot be read.
    /// </exception>
    /// <exception cref="JsonException">
    /// The description does not match the current contract.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// A root, path or content identity is invalid.
    /// </exception>
    public static NativeBindingCompilation Load(string nativeDirectory)
    {
        NativeBindingCompilation result = JsonSerializer.Deserialize<NativeBindingCompilation>(
            File.ReadAllBytes(Path.Combine(nativeDirectory, "compilation.json")), S_OPTIONS)
            ?? throw new InvalidDataException("A native compilation description is required.");
        result.Validate();
        return result;
    }

    /// <summary>
    /// Resolves a component-local root using the current checkout and selected output.
    /// </summary>
    /// <param name="engineRoot">
    /// The current checkout's absolute root.
    /// </param>
    /// <param name="nativeDirectory">
    /// The selected generation's absolute native output.
    /// </param>
    /// <param name="rootId">
    /// An ordinal root identity from this description.
    /// </param>
    /// <returns>
    /// The physical root; no other component's root table participates.
    /// </returns>
    /// <exception cref="InvalidDataException">
    /// The root identity is unknown.
    /// </exception>
    public string ResolveRoot(
        string engineRoot,
        string nativeDirectory,
        string rootId
    ) {
        NativeBindingSourceRoot root = roots.SingleOrDefault(root => root.id == rootId)
            ?? throw new InvalidDataException("Unknown native compilation root: " + rootId);
        return root.generated ? Path.GetFullPath(nativeDirectory) : Path.GetFullPath(root.directory, engineRoot);
    }

    /// <summary>
    /// Verifies the exact bytes at original or remapped roots before their use.
    /// </summary>
    /// <param name="resolveRoot">
    /// Resolves a root identity to the selected physical or copied directory.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels verification without publishing a result.
    /// </param>
    /// <exception cref="InvalidDataException">
    /// A required file has a different length or digest.
    /// </exception>
    /// <exception cref="IOException">
    /// A required input is missing or cannot be read.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Verification was canceled.
    /// </exception>
    public void VerifyInputs(
        Func<string, string> resolveRoot,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(resolveRoot);
        foreach (NativeBindingSourceFile input in files)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string path = PathBoundary.Resolve(resolveRoot(input.rootId), input.relativePath);
            using FileStream stream = File.OpenRead(path);
            if (stream.Length != input.length || Convert.ToHexStringLower(SHA256.HashData(stream)) != input.sha256)
                throw new InvalidDataException("Native compilation input changed: " + path);
        }
    }

    /// <summary>
    /// Enumerates exact file declarations for standalone or aggregate recipe fingerprinting.
    /// </summary>
    /// <param name="engineRoot">
    /// The current checkout's absolute root.
    /// </param>
    /// <param name="nativeDirectory">
    /// The selected native artifact directory.
    /// </param>
    /// <returns>
    /// All generated, user and SDK inputs with their current physical locations.
    /// </returns>
    public IEnumerable<NativeBuildInput> GetInputs(
        string engineRoot,
        string nativeDirectory
    ) => files.Select(input => NativeBuildInput.FromPath(engineRoot,
        PathBoundary.Resolve(ResolveRoot(engineRoot, nativeDirectory, input.rootId), input.relativePath)));

    private void Validate()
    {
        if (roots is null || files is null || includeDirectories is null || defines is null || compilerDefines is null || requiredExports is null
            || staticExportSymbols is null || staticExportNames is null || staticExportAbi is null)
            throw new InvalidDataException("Native compilation requirements are incomplete.");
        var ids = new HashSet<string>(StringComparer.Ordinal);
        foreach (NativeBindingSourceRoot root in roots)
        {
            if (root is null || string.IsNullOrWhiteSpace(root.id) || !ids.Add(root.id) || root.directory is null
                || root.generated && (!root.frozen || root.directory.Length != 0)
                || !root.generated && string.IsNullOrWhiteSpace(root.directory))
                throw new InvalidDataException("Native compilation roots must have unique, complete identities.");
        }
        if (roots.Count(static root => root.generated) != 1)
            throw new InvalidDataException("A native compilation must identify exactly one generated root.");
        var paths = new HashSet<(string, string)>();
        foreach (NativeBindingSourceFile file in files)
        {
            if (file is null || !ids.Contains(file.rootId) || !IsRelative(file.relativePath, false)
                || !paths.Add((file.rootId, file.relativePath)) || file.length < 0 || file.sha256 is null
                || file.sha256.Length != 64 || file.sha256.Any(static value => value is not (>= '0' and <= '9' or >= 'a' and <= 'f')))
                throw new InvalidDataException("Native compilation files require unique paths and exact content identities.");
        }
        if (includeDirectories.Any(directory => directory is null || !ids.Contains(directory.rootId)
            || !IsRelative(directory.relativePath, true)) || defines.Any(string.IsNullOrWhiteSpace)
            || compilerDefines.Any(string.IsNullOrWhiteSpace) || requiredExports.Any(string.IsNullOrWhiteSpace) || staticExportSymbols.Any(string.IsNullOrWhiteSpace)
            || staticExportNames.Any(string.IsNullOrWhiteSpace) || cxxStandard < 0 || cStandard < 0
            || staticExportSymbols.Length == 0 && (staticExportAbi.Length != 0 || staticExportNames.Length != 0)
            || staticExportSymbols.Length != 0 && (staticExportAbi.Length != 64 || staticExportSymbols.Length != 5
                || staticExportAbi.Any(static value => value is not (>= '0' and <= '9' or >= 'a' and <= 'f'))))
            throw new InvalidDataException("Native compilation search, language or export requirements are invalid.");
    }

    private static bool IsRelative(
        string? value,
        bool allowEmpty
    ) => value is not null && (allowEmpty && value.Length == 0
        || value.Length > 0 && !Path.IsPathRooted(value) && !value.Contains('\\') && !value.Contains(':')
            && value.Split('/').All(static segment => segment.Length > 0 && segment is not ("." or "..")
                && segment.AsSpan().IndexOfAnyInRange('\0', '\x1f') < 0));
}
