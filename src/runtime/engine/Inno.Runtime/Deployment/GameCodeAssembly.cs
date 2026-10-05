using System;

namespace Inno.Runtime;

/// <summary>
/// Identifies one immutable code input independently of its physical deployment representation.
/// </summary>
public sealed record GameCodeAssembly
{
    /// <summary>
    /// Validates and freezes one logical assembly identity.
    /// </summary>
    /// <param name="name">
    /// The exact simple assembly name.
    /// </param>
    /// <param name="contentFingerprint">
    /// The lowercase SHA-256 fingerprint of the frozen compiler output.
    /// </param>
    /// <exception cref="System.IO.InvalidDataException">
    /// The supplied code identity is malformed.
    /// </exception>
    public GameCodeAssembly(
        string name,
        string contentFingerprint
    ) {
        new GameRuntimeAssembly { name = name, contentFingerprint = contentFingerprint }.Validate();
        this.name = name;
        this.contentFingerprint = contentFingerprint;
    }

    /// <summary>
    /// Gets the exact simple assembly name.
    /// </summary>
    public string name { get; }

    /// <summary>
    /// Gets the build input's content fingerprint.
    /// </summary>
    public string contentFingerprint { get; }
}
