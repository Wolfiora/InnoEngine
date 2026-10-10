using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Inno.Build.Toolchains;

/// <summary>
/// Derives recipe identities from ordered declarations, logical input names and complete source bytes.
/// </summary>
public static class NativeBuildFingerprint
{
    /// <summary>
    /// Hashes a complete declared input closure independently of checkout location and file timestamps.
    /// </summary>
    /// <param name="declarations">
    /// Ordered target, configuration, SDK, compiler and linker arguments. Order is significant.
    /// </param>
    /// <param name="inputs">
    /// Files and directories with explicit logical identities; overlapping reading locations are hashed once.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels enumeration or complete content hashing before a fingerprint is returned.
    /// </param>
    /// <returns>
    /// A lowercase SHA-256 identity that changes when a declaration, input inventory or input byte changes.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// A required input collection is null.
    /// </exception>
    /// <exception cref="System.IO.IOException">
    /// An input cannot be read or contains a directory-link cycle.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Content hashing was canceled.
    /// </exception>
    public static string Create(
        IEnumerable<string> declarations,
        IEnumerable<NativeBuildInput> inputs,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(declarations);
        ArgumentNullException.ThrowIfNull(inputs);
        NativeBuildInputState state = new();
        return Create(declarations, state.CaptureInitial(inputs, cancellationToken));
    }

    /// <summary>
    /// Hashes declarations using the current operation's initial snapshot or explicit fresh verification phase.
    /// </summary>
    /// <param name="context">
    /// The owner of this phase's complete input inventory and content hashes.
    /// </param>
    /// <param name="declarations">
    /// Ordered non-file inputs; declaration order remains significant.
    /// </param>
    /// <param name="inputs">
    /// Complete inputs with stable logical identities and physical read locations.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels complete enumeration and hashing.
    /// </param>
    /// <returns>
    /// The SHA-256 identity; repeated physical reads within this phase are reused.
    /// </returns>
    public static string Create(
        NativeBuildContext context,
        IEnumerable<string> declarations,
        IEnumerable<NativeBuildInput> inputs,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(declarations);
        ArgumentNullException.ThrowIfNull(inputs);
        return Create(declarations, context.CaptureInputs(inputs, cancellationToken));
    }

    internal static string Create(
        IEnumerable<string> declarations,
        NativeInputSnapshot snapshot
    ) {
        using IncrementalHash hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        foreach (string declaration in declarations)
            Append(hash, declaration);
        foreach (NativeInputSnapshot.Entry entry in snapshot.entries)
        {
            Append(hash, entry.logicalPath);
            hash.AppendData(entry.hash);
        }
        return Convert.ToHexStringLower(hash.GetHashAndReset());
    }

    private static void Append(
        IncrementalHash hash,
        string value
    ) {
        ArgumentNullException.ThrowIfNull(value);
        byte[] bytes = Encoding.UTF8.GetBytes(value);
        Span<byte> length = stackalloc byte[4];
        BinaryPrimitives.WriteInt32LittleEndian(length, bytes.Length);
        hash.AppendData(length);
        hash.AppendData(bytes);
    }
}
