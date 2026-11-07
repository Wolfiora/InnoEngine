using System.IO;

namespace Inno.Engine.Default;

/// <summary>
/// Provides the original brand artwork bundled with the default engine distribution.
/// </summary>
public static class EngineBranding
{
    /// <summary>
    /// Opens the transparent, 512-by-512 grayscale engine logo without requiring an engine session.
    /// </summary>
    /// <returns>
    /// A new read-only PNG stream positioned at its beginning. The caller owns and must dispose
    /// the stream; separate calls have independent positions and lifetimes.
    /// </returns>
    /// <exception cref="InvalidDataException">
    /// The installed assembly does not contain its required logo resource.
    /// </exception>
    public static Stream OpenLogoPng()
        => typeof(EngineBranding).Assembly.GetManifestResourceStream("Inno.Engine.Branding.Logo.png")
           ?? throw new InvalidDataException("The default engine distribution is missing its embedded logo PNG.");
}
