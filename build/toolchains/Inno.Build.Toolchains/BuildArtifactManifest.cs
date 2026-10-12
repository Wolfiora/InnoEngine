using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;
using Inno.Core.IO;

namespace Inno.Build.Toolchains;

/// <summary>
/// Validates complete build outputs against their input identity and recorded file hashes.
/// </summary>
public static class BuildArtifactManifest
{
    private const string C_FILE_NAME = "generation-manifest.json";

    /// <summary>
    /// Records a completed staging tree before its owner publishes the directory.
    /// </summary>
    /// <param name="directory">
    /// The complete staging root owned exclusively by the caller.
    /// </param>
    /// <param name="fingerprint">
    /// The immutable identity of every input used to produce these outputs.
    /// </param>
    /// <param name="outputDirectories">
    /// Relative output subdirectories included in this artifact; each must stay within the root.
    /// </param>
    /// <param name="context">
    /// The optional operation collecting independent output-read statistics.
    /// </param>
    /// <exception cref="ArgumentException">
    /// An identity or output path is invalid.
    /// </exception>
    /// <exception cref="IOException">
    /// An output or manifest cannot be read or written.
    /// </exception>
    public static void Write(
        string directory,
        string fingerprint,
        IReadOnlyList<string> outputDirectories,
        NativeBuildContext? context = null
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(fingerprint);
        Dictionary<string, string> files = ReadFiles(directory, outputDirectories, context);
        if (files.Count == 0)
            throw new InvalidDataException("A completed artifact must contain output files.");
        using var bytes = new MemoryStream();
        using (var writer = new Utf8JsonWriter(bytes))
        {
            writer.WriteStartObject();
            writer.WriteString("fingerprint", fingerprint);
            writer.WriteStartObject("files");
            foreach (var file in files.OrderBy(static pair => pair.Key, StringComparer.Ordinal))
                writer.WriteString(file.Key, file.Value);
            writer.WriteEndObject();
            writer.WriteEndObject();
        }
        File.WriteAllText(Path.Combine(directory, C_FILE_NAME), Encoding.UTF8.GetString(bytes.ToArray()));
    }

    /// <summary>
    /// Checks the identity, exact output file set and bytes before reusing a cached artifact.
    /// </summary>
    /// <param name="directory">
    /// The published artifact directory.
    /// </param>
    /// <param name="fingerprint">
    /// The requested input identity.
    /// </param>
    /// <param name="outputDirectories">
    /// The same relative output subdirectories supplied when recording the artifact.
    /// </param>
    /// <param name="context">
    /// The optional operation collecting actual output integrity reads.
    /// </param>
    /// <returns>
    /// True only for complete, unmodified outputs; false for a missing, malformed or mismatched manifest.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// An output path escapes the artifact root.
    /// </exception>
    /// <exception cref="IOException">
    /// An existing output cannot be read.
    /// </exception>
    public static bool IsComplete(
        string directory,
        string fingerprint,
        IReadOnlyList<string> outputDirectories,
        NativeBuildContext? context = null
    ) {
        string path = Path.Combine(directory, C_FILE_NAME);
        if (!File.Exists(path))
            return false;
        Dictionary<string, string> expected;
        try
        {
            using JsonDocument manifest = JsonDocument.Parse(File.ReadAllBytes(path));
            JsonElement data = manifest.RootElement;
            if (!data.EnumerateObject().Select(static entry => entry.Name).Order(StringComparer.Ordinal)
                .SequenceEqual(new[] { "files", "fingerprint" }, StringComparer.Ordinal)
                || data.GetProperty("fingerprint").GetString() != fingerprint)
                return false;
            expected = data.GetProperty("files").EnumerateObject()
                .ToDictionary(static entry => entry.Name, static entry => entry.Value.GetString()!, StringComparer.Ordinal);
        }
        catch (Exception failure) when (failure is JsonException or InvalidOperationException or ArgumentException)
        {
            return false;
        }
        if (expected.Count == 0)
            return false;
        Dictionary<string, string> files = ReadFiles(directory, outputDirectories, context);
        return files.Count == expected.Count
            && PathBoundary.EnumerateFiles(directory).Count() == files.Count + 1
            && files.All(pair => expected.TryGetValue(pair.Key, out string? value) && pair.Value == value);
    }

    private static Dictionary<string, string> ReadFiles(
        string directory,
        IReadOnlyList<string> outputDirectories,
        NativeBuildContext? context
    ) {
        ArgumentNullException.ThrowIfNull(outputDirectories);
        string root = Path.GetFullPath(directory);
        Dictionary<string, string> files = new(StringComparer.Ordinal);
        foreach (string relative in outputDirectories)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(relative);
            string output = Path.GetFullPath(Path.Combine(root, relative));
            string check = Path.GetRelativePath(root, output);
            if (Path.IsPathRooted(check) || check is "." or ".."
                || check.StartsWith(".." + Path.DirectorySeparatorChar, StringComparison.Ordinal))
                throw new ArgumentException("An artifact output directory must be contained in its root.", nameof(outputDirectories));
            if (!Directory.Exists(output))
                continue;
            foreach (string path in PathBoundary.EnumerateFiles(output).Order(StringComparer.Ordinal))
            {
                using FileStream input = File.OpenRead(path);
                files.Add(Path.GetRelativePath(root, path).Replace('\\', '/'),
                    Convert.ToHexStringLower(SHA256.HashData(input)));
                context?.RecordOutputRead(input.Position);
            }
        }
        if (files.Count == 0)
            return files;
        foreach (string path in PathBoundary.EnumerateFiles(root).Order(StringComparer.Ordinal))
        {
            string relative = Path.GetRelativePath(root, path).Replace('\\', '/');
            if (relative == C_FILE_NAME || files.ContainsKey(relative))
                continue;
            using FileStream input = File.OpenRead(path);
            files.Add(relative, Convert.ToHexStringLower(SHA256.HashData(input)));
            context?.RecordOutputRead(input.Position);
        }
        return files;
    }

}
