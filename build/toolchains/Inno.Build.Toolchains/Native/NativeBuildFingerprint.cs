using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Inno.Build.Toolchains;

/// <summary>
/// Derives native build identities from explicit toolchain declarations and source bytes.
/// </summary>
public static class NativeBuildFingerprint
{
    /// <summary>
    /// Hashes declarations and complete source files in a deterministic order.
    /// </summary>
    /// <param name="declarations">
    /// Target, configuration, SDK and generation identities selected by the owning toolchain.
    /// </param>
    /// <param name="files">
    /// Absolute source and tool files; duplicates are read once and missing files fail.
    /// </param>
    /// <returns>
    /// A lowercase SHA-256 identity that changes when any declared input changes.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// An input collection is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// An input path is empty or relative.
    /// </exception>
    /// <exception cref="IOException">
    /// An input file cannot be read.
    /// </exception>
    public static string Create(
        IEnumerable<string> declarations,
        IEnumerable<string> files
    ) {
        ArgumentNullException.ThrowIfNull(declarations);
        ArgumentNullException.ThrowIfNull(files);
        using IncrementalHash hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        foreach (string declaration in declarations.Order(StringComparer.Ordinal))
            Append(declaration);
        StringComparer paths = OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
        foreach (string path in files.Distinct(paths).Order(paths))
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path);
            if (!Path.IsPathFullyQualified(path))
                throw new ArgumentException("Native fingerprint inputs must be absolute paths.", nameof(files));
            Append(Path.GetFullPath(path));
            using FileStream input = File.OpenRead(path);
            hash.AppendData(SHA256.HashData(input));
        }
        return Convert.ToHexStringLower(hash.GetHashAndReset());

        void Append(string value)
        {
            ArgumentNullException.ThrowIfNull(value);
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            Span<byte> length = stackalloc byte[4];
            System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(length, bytes.Length);
            hash.AppendData(length);
            hash.AppendData(bytes);
        }
    }
}
