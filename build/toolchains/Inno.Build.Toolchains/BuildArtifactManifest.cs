using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
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
        File.WriteAllText(Path.Combine(directory, C_FILE_NAME), JsonSerializer.Serialize(
            new Manifest { fingerprint = fingerprint, files = files }));
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
        Manifest? manifest;
        try
        {
            manifest = JsonSerializer.Deserialize<Manifest>(File.ReadAllText(path));
        }
        catch (JsonException)
        {
            return false;
        }
        if (manifest?.fingerprint != fingerprint || manifest.files is null || manifest.files.Count == 0)
            return false;
        Dictionary<string, string> files = ReadFiles(directory, outputDirectories, context);
        return files.Count == manifest.files.Count
            && files.All(pair => manifest.files.TryGetValue(pair.Key, out string? value) && pair.Value == value);
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
        return files;
    }

    private sealed class Manifest
    {
        /// <summary>
        /// Gets the immutable input identity assigned by the artifact owner.
        /// </summary>
        public required string fingerprint { get; init; }

        /// <summary>
        /// Gets each relative output path and the SHA-256 of its completed bytes.
        /// </summary>
        public required Dictionary<string, string> files { get; init; }
    }
}
