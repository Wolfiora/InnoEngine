using System;

namespace Inno.Rendering;

/// <summary>
/// Identifies the graphics API family selected by the host without exposing a backend-native enum.
/// Pipelines use this value only for capability-aware choices and target artifact selection;
/// it does not require separate pipeline or shader source implementations.
/// </summary>
public readonly record struct GraphicsApi : IComparable<GraphicsApi>
{
    /// <summary>
    /// Creates an open, stable backend identity without registering a backend implementation.
    /// </summary>
    /// <param name="value">
    /// A portable case-sensitive identifier used in target artifact paths.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The identifier is empty or contains nonportable characters.
    /// </exception>
    public GraphicsApi(string value)
    {
        if (!IsPortableIdentifier(value))
        {
            throw new ArgumentException(
                "Graphics API IDs must begin with a letter or digit and use only portable letters, numbers, dots, underscores or hyphens.",
                nameof(value)
            );
        }
        this.value = value;
    }

    /// <summary>
    /// Headless validation backend.
    /// </summary>
    public static GraphicsApi noop { get; } = new("Noop");

    /// <summary>
    /// Direct3D 11 renderer.
    /// </summary>
    public static GraphicsApi direct3D11 { get; } = new("Direct3D11");

    /// <summary>
    /// Direct3D 12 renderer.
    /// </summary>
    public static GraphicsApi direct3D12 { get; } = new("Direct3D12");

    /// <summary>
    /// Apple Metal renderer.
    /// </summary>
    public static GraphicsApi metal { get; } = new("Metal");

    /// <summary>
    /// Khronos Vulkan renderer.
    /// </summary>
    public static GraphicsApi vulkan { get; } = new("Vulkan");

    /// <summary>
    /// Desktop OpenGL renderer.
    /// </summary>
    public static GraphicsApi openGL { get; } = new("OpenGL");

    /// <summary>
    /// OpenGL ES renderer.
    /// </summary>
    public static GraphicsApi openGLES { get; } = new("OpenGLES");

    /// <summary>
    /// WebGPU renderer.
    /// </summary>
    public static GraphicsApi webGPU { get; } = new("WebGPU");

    /// <summary>
    /// Gets the stable backend identity used by target artifacts and provider selection.
    /// </summary>
    public string value { get; }

    /// <summary>
    /// Gets whether this value contains an initialized backend identity.
    /// </summary>
    public bool isValid => !string.IsNullOrEmpty(value);

    /// <summary>
    /// Compares two backend identities using stable ordinal ordering.
    /// </summary>
    /// <param name="other">
    /// The other backend identity.
    /// </param>
    /// <returns>
    /// A negative, zero, or positive value according to ordinal identifier order.
    /// </returns>
    public int CompareTo(GraphicsApi other) => string.Compare(value, other.value, StringComparison.Ordinal);

    /// <summary>
    /// Formats the exact stable backend identity.
    /// </summary>
    /// <returns>
    /// The identity, or an empty string for an uninitialized value.
    /// </returns>
    public override string ToString() => value ?? string.Empty;

    /// <summary>
    /// Parses an open backend identity without consulting a closed engine-side backend list.
    /// </summary>
    /// <param name="value">
    /// The exact stable backend identity.
    /// </param>
    /// <param name="api">
    /// The parsed value, or default when parsing fails.
    /// </param>
    /// <returns>
    /// True when the identifier is portable and nonempty.
    /// </returns>
    public static bool TryParse(
        string? value,
        out GraphicsApi api
    ) {
        api = default;
        if (!IsPortableIdentifier(value))
        {
            return false;
        }
        api = new GraphicsApi(value!);
        return true;
    }

    private static bool IsPortableIdentifier(string? candidate)
    {
        if (string.IsNullOrEmpty(candidate) || !char.IsAsciiLetterOrDigit(candidate[0]))
        {
            return false;
        }
        foreach (char character in candidate)
        {
            if (!(char.IsAsciiLetterOrDigit(character) || character is '-' or '_' or '.'))
            {
                return false;
            }
        }
        return true;
    }
}
