namespace Inno.Rendering;

/// <summary>
/// Stores a linear clear color without depending on an engine math assembly.
/// </summary>
public readonly record struct RenderClearColor
{
    /// <summary>
    /// Creates a linear clear color.
    /// </summary>
    /// <param name="r">
    /// Red channel.
    /// </param>
    /// <param name="g">
    /// Green channel.
    /// </param>
    /// <param name="b">
    /// Blue channel.
    /// </param>
    /// <param name="a">
    /// Alpha channel.
    /// </param>
    public RenderClearColor(
        float r,
        float g,
        float b,
        float a = 1f
    ) {
        this.r = r;
        this.g = g;
        this.b = b;
        this.a = a;
    }

    /// <summary>
    /// Gets the red channel.
    /// </summary>
    public float r { get; }

    /// <summary>
    /// Gets the green channel.
    /// </summary>
    public float g { get; }

    /// <summary>
    /// Gets the blue channel.
    /// </summary>
    public float b { get; }

    /// <summary>
    /// Gets the alpha channel.
    /// </summary>
    public float a { get; }
}
