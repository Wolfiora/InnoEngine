using System;

namespace Inno.Build.Managed;

/// <summary>
/// Identifies an open managed deployment implementation independently of the publication platform.
/// </summary>
public readonly record struct ManagedDeploymentId
{
    /// <summary>
    /// Identifies the desktop CoreCLR publisher.
    /// </summary>
    public static ManagedDeploymentId coreClr { get; } = new("coreclr");

    /// <summary>
    /// Identifies the Mono WebAssembly interpreter publisher.
    /// </summary>
    public static ManagedDeploymentId monoWasm { get; } = new("mono-wasm");

    /// <summary>
    /// Identifies the Mono WebAssembly ahead-of-time publisher.
    /// </summary>
    public static ManagedDeploymentId monoWasmAot { get; } = new("mono-wasm-aot");

    /// <summary>
    /// Identifies the .NET NativeAOT publisher.
    /// </summary>
    public static ManagedDeploymentId nativeAot { get; } = new("nativeaot");

    /// <summary>
    /// Validates a stable portable provider identity.
    /// </summary>
    /// <param name="value">
    /// A nonempty lowercase identifier composed of letters, digits and hyphens.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The identity is empty or contains nonportable characters.
    /// </exception>
    public ManagedDeploymentId(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        foreach (char character in value)
            if (!(character is >= 'a' and <= 'z' or >= '0' and <= '9' or '-'))
                throw new ArgumentException("Managed deployment IDs require lowercase portable characters.", nameof(value));
        this.value = value;
    }

    /// <summary>
    /// Gets the exact stable provider identity.
    /// </summary>
    public string value { get; }

    /// <inheritdoc />
    public override string ToString() => value ?? string.Empty;
}
