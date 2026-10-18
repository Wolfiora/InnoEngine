using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using Inno.Core.IO;

namespace Inno.Native.LibraryLoading;

/// <summary>
/// Describes exact native library and tool locations selected by a product's deployment owner.
/// </summary>
public sealed class NativeDeploymentIndex
{
    /// <summary>
    /// Gets the index filename inside the explicitly deployed native directory.
    /// </summary>
    public const string C_FILE_NAME = "Deployment.index";

    /// <summary>
    /// Freezes one product target and its explicit aliases without detecting the host OS.
    /// </summary>
    /// <param name="targetId">
    /// The exact deployment target selected by the product.
    /// </param>
    /// <param name="libraries">
    /// Logical import names mapped to contained paths relative to the native directory.
    /// </param>
    /// <param name="files">
    /// Exact tool or resource aliases mapped to contained deployment paths.
    /// </param>
    /// <exception cref="ArgumentException">
    /// A target, alias or contained path is invalid.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// A mapping is absent.
    /// </exception>
    public NativeDeploymentIndex(
        string targetId,
        IReadOnlyDictionary<string, string> libraries,
        IReadOnlyDictionary<string, string> files
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(targetId);
        this.targetId = targetId;
        this.libraries = Freeze(libraries);
        this.files = Freeze(files);
    }

    /// <summary>
    /// Gets the explicit product target, without reconstructing it from OS or CPU names.
    /// </summary>
    public string targetId { get; }

    /// <summary>
    /// Gets immutable logical library mappings selected by composition.
    /// </summary>
    public IReadOnlyDictionary<string, string> libraries { get; }

    /// <summary>
    /// Gets immutable exact tool and resource mappings selected by composition.
    /// </summary>
    public IReadOnlyDictionary<string, string> files { get; }

    /// <summary>
    /// Encodes the deployment layout without recording SDK or build cache locations.
    /// </summary>
    /// <returns>
    /// Deterministic UTF-8 bytes for the current strict index format.
    /// </returns>
    public byte[] Encode()
    {
        using var stream = new MemoryStream();
        using (var writer = new Utf8JsonWriter(stream))
        {
            writer.WriteStartObject();
            writer.WriteString("targetId", targetId);
            WriteMapping(writer, "libraries", libraries);
            WriteMapping(writer, "files", files);
            writer.WriteEndObject();
        }
        return stream.ToArray();
    }

    /// <summary>
    /// Reads only the current index format and validates its complete path mappings.
    /// </summary>
    /// <param name="bytes">
    /// Complete encoded bytes borrowed for this call.
    /// </param>
    /// <returns>
    /// An immutable layout; malformed or incomplete input is rejected.
    /// </returns>
    /// <exception cref="InvalidDataException">
    /// The index is malformed, incomplete or contains invalid mappings.
    /// </exception>
    public static NativeDeploymentIndex Decode(ReadOnlySpan<byte> bytes)
    {
        try
        {
            using JsonDocument document = JsonDocument.Parse(bytes.ToArray());
            JsonElement root = document.RootElement;
            string[] names = root.EnumerateObject().Select(static entry => entry.Name).Order(StringComparer.Ordinal).ToArray();
            if (!names.SequenceEqual(new[] { "files", "libraries", "targetId" }, StringComparer.Ordinal))
                throw new InvalidDataException("The native deployment index must contain its exact current fields.");
            return new(root.GetProperty("targetId").GetString()!, ReadMapping(root.GetProperty("libraries")),
                ReadMapping(root.GetProperty("files")));
        }
        catch (Exception failure) when (failure is JsonException or ArgumentException or InvalidOperationException)
        {
            throw new InvalidDataException("The native deployment index is malformed or incomplete.", failure);
        }
    }

    /// <summary>
    /// Resolves a selected library without scanning directories or guessing suffixes.
    /// </summary>
    /// <param name="nativeDirectory">
    /// The absolute directory whose owner deployed the index and payload.
    /// </param>
    /// <param name="name">
    /// The exact logical import name.
    /// </param>
    /// <returns>
    /// The existing contained library path.
    /// </returns>
    /// <exception cref="FileNotFoundException">
    /// The name is absent or the deployed payload is missing.
    /// </exception>
    public string ResolveLibrary(
        string nativeDirectory,
        string name
    ) => Resolve(nativeDirectory, name, libraries);

    /// <summary>
    /// Resolves a tool or resource alias from the explicit deployment mappings.
    /// </summary>
    /// <param name="nativeDirectory">
    /// The absolute directory containing the index and payload.
    /// </param>
    /// <param name="name">
    /// The exact registered alias.
    /// </param>
    /// <returns>
    /// The existing contained file path.
    /// </returns>
    /// <exception cref="FileNotFoundException">
    /// The alias is absent or its payload is missing.
    /// </exception>
    public string ResolveFile(
        string nativeDirectory,
        string name
    ) => Resolve(nativeDirectory, name, files);

    private static IReadOnlyDictionary<string, string> Freeze(IReadOnlyDictionary<string, string> mapping)
    {
        ArgumentNullException.ThrowIfNull(mapping);
        var result = new SortedDictionary<string, string>(StringComparer.Ordinal);
        foreach ((string alias, string path) in mapping)
        {
            if (string.IsNullOrWhiteSpace(alias) || alias.IndexOfAny(['/', '\\', ':', '*', '?']) >= 0
                || alias is "." or ".." || string.IsNullOrWhiteSpace(path) || path.Contains('\\')
                || path.Contains(':') || path.Split('/').Any(static part => part is "" or "." or ".."))
                throw new ArgumentException("Native deployment aliases and paths must be assigned and portable.", nameof(mapping));
            result.Add(alias, path);
        }
        return new ReadOnlyDictionary<string, string>(result);
    }

    private static string Resolve(
        string directory,
        string name,
        IReadOnlyDictionary<string, string> mapping
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (!mapping.TryGetValue(name, out string? relative))
            throw new FileNotFoundException($"No native deployment entry is registered for '{name}'.");
        string file = PathBoundary.RequireUnlinkedPath(directory, PathBoundary.Resolve(directory, relative));
        if (!File.Exists(file))
            throw new FileNotFoundException("An explicitly deployed native payload is absent.", file);
        return file;
    }

    private static IReadOnlyDictionary<string, string> ReadMapping(JsonElement element)
        => element.EnumerateObject().ToDictionary(static entry => entry.Name, static entry => entry.Value.GetString()!, StringComparer.Ordinal);

    private static void WriteMapping(
        Utf8JsonWriter writer,
        string name,
        IReadOnlyDictionary<string, string> mapping
    ) {
        writer.WriteStartObject(name);
        foreach ((string alias, string path) in mapping)
            writer.WriteString(alias, path);
        writer.WriteEndObject();
    }
}
