using System;

namespace Inno.Text;

/// <summary>
/// Defines language and script hints for one shaping operation.
/// </summary>
public readonly record struct TextShapingOptions
{
    /// <summary>
    /// Creates shaping hints.
    /// </summary>
    /// <param name="direction">
    /// The logical text direction.
    /// </param>
    /// <param name="language">
    /// An optional BCP 47 language code.
    /// </param>
    /// <param name="script">
    /// An optional ISO 15924 script code.
    /// </param>
    public TextShapingOptions(
        TextDirection direction = TextDirection.Automatic,
        string? language = null,
        string? script = null
    ) {
        if (!Enum.IsDefined(direction))
        {
            throw new ArgumentOutOfRangeException(nameof(direction));
        }
        this.direction = direction;
        this.language = string.IsNullOrWhiteSpace(language) ? null : language.Trim();
        this.script = string.IsNullOrWhiteSpace(script) ? null : script.Trim();
    }

    /// <summary>
    /// Gets automatic shaping hints.
    /// </summary>
    public static TextShapingOptions automatic { get; } = new();

    /// <summary>
    /// Gets the requested logical direction.
    /// </summary>
    public TextDirection direction { get; }

    /// <summary>
    /// Gets the optional BCP 47 language code.
    /// </summary>
    public string? language { get; }

    /// <summary>
    /// Gets the optional ISO 15924 script code.
    /// </summary>
    public string? script { get; }
}
