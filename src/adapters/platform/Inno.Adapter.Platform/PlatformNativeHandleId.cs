using System;

namespace Inno.Adapter.Platform;

/// <summary>
/// Identifies an open native surface ABI understood by cooperating window and graphics adapters.
/// </summary>
public readonly record struct PlatformNativeHandleId
{
    /// <summary>
    /// Creates an ordinal, case-sensitive ABI identifier.
    /// </summary>
    /// <param name="value">
    /// Stable nonempty identifier without whitespace.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The identifier is empty or contains whitespace.
    /// </exception>
    public PlatformNativeHandleId(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        foreach (char character in value)
            if (char.IsWhiteSpace(character))
                throw new ArgumentException("A native surface identifier cannot contain whitespace.", nameof(value));
        this.value = value;
    }

    /// <summary>
    /// Gets the Win32 HWND surface ABI.
    /// </summary>
    public static PlatformNativeHandleId win32 { get; } = new("inno.surface.win32");

    /// <summary>
    /// Gets the Cocoa window surface ABI.
    /// </summary>
    public static PlatformNativeHandleId cocoa { get; } = new("inno.surface.cocoa");

    /// <summary>
    /// Gets the UTF-8 canvas selector surface ABI.
    /// </summary>
    public static PlatformNativeHandleId browserCanvas { get; } = new("inno.surface.browser-canvas");

    /// <summary>
    /// Gets the stable identifier; the default value is unassigned.
    /// </summary>
    public string value { get; } = string.Empty;

    /// <summary>
    /// Gets whether a surface ABI was assigned.
    /// </summary>
    public bool isValid => !string.IsNullOrWhiteSpace(value);

    /// <summary>
    /// Returns the identifier without resolving an implementation.
    /// </summary>
    /// <returns>
    /// The stable identifier, or an empty string when unassigned.
    /// </returns>
    public override string ToString() => value ?? string.Empty;
}
