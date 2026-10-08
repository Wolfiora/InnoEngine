using System;

namespace Inno.Build;

/// <summary>
/// Declares immutable product target facts independently of the machine executing build tools.
/// </summary>
public sealed class PlatformTargetDescriptor
{
    /// <summary>
    /// Creates an explicit platform, processor, ABI and managed-runtime mapping.
    /// </summary>
    /// <param name="id">
    /// The stable publication target identity.
    /// </param>
    /// <param name="platform">
    /// The platform package owning system integration.
    /// </param>
    /// <param name="architecture">
    /// The target processor architecture, such as x64, arm64 or wasm32.
    /// </param>
    /// <param name="abi">
    /// The native ABI selected by this platform implementation.
    /// </param>
    /// <param name="runtimeIdentifier">
    /// The exact managed toolchain runtime identifier.
    /// </param>
    /// <param name="supportsEditor">
    /// Whether this implemented target supports the shared Editor product.
    /// </param>
    /// <exception cref="ArgumentException">
    /// An identity or required target fact is unassigned.
    /// </exception>
    public PlatformTargetDescriptor(
        BuildTargetId id,
        string platform,
        string architecture,
        string abi,
        string runtimeIdentifier,
        bool supportsEditor
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(id.value);
        ArgumentException.ThrowIfNullOrWhiteSpace(platform);
        ArgumentException.ThrowIfNullOrWhiteSpace(architecture);
        ArgumentException.ThrowIfNullOrWhiteSpace(abi);
        ArgumentException.ThrowIfNullOrWhiteSpace(runtimeIdentifier);
        this.id = id;
        this.platform = platform;
        this.architecture = architecture;
        this.abi = abi;
        this.runtimeIdentifier = runtimeIdentifier;
        this.supportsEditor = supportsEditor;
    }

    /// <summary>
    /// Gets the stable publication target identity.
    /// </summary>
    public BuildTargetId id { get; }

    /// <summary>
    /// Gets the owning platform package.
    /// </summary>
    public string platform { get; }

    /// <summary>
    /// Gets the explicitly declared processor architecture.
    /// </summary>
    public string architecture { get; }

    /// <summary>
    /// Gets the explicitly selected native ABI.
    /// </summary>
    public string abi { get; }

    /// <summary>
    /// Gets the managed runtime mapping without deriving it from the target identity.
    /// </summary>
    public string runtimeIdentifier { get; }

    /// <summary>
    /// Gets whether this target provides an implemented Editor host.
    /// </summary>
    public bool supportsEditor { get; }
}
