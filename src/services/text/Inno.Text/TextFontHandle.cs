namespace Inno.Text;

/// <summary>
/// Identifies one font face loaded into a text backend generation.
/// </summary>
/// <param name="value">
/// The concrete value read or transformed by this operation.
/// </param>
public readonly record struct TextFontHandle(ulong value)
{
    /// <summary>
    /// Gets whether this handle identifies a loaded face.
    /// </summary>
    public bool isValid => value != 0;
}
