using System;

namespace Inno.UI;

/// <summary>
/// Associates a frontend-owned source name with one frozen imported image dependency.
/// </summary>
public sealed class UiDocumentImage
{
    /// <summary>
    /// Declares an image used by canonical document source.
    /// </summary>
    /// <param name="source">
    /// Opaque resource name emitted by the language frontend.
    /// </param>
    /// <param name="image">
    /// Imported artifact identity and dimensions.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The source is empty.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The image is null.
    /// </exception>
    public UiDocumentImage(
        string source,
        UiImageSource image
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(source);
        this.image = image ?? throw new ArgumentNullException(nameof(image));
        this.source = source;
    }

    /// <summary>
    /// Gets the frontend-owned source name.
    /// </summary>
    public string source { get; }

    /// <summary>
    /// Gets the frozen imported resource.
    /// </summary>
    public UiImageSource image { get; }
}
