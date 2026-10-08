using System;
using System.IO;

namespace Inno.Platform.MacOS;

/// <summary>
/// Defines MacOS application layout and user data locations without implementing storage services.
/// </summary>
public static class MacOSApplicationLocations
{
    /// <summary>
    /// Gets the current user's application data root according to MacOS system conventions.
    /// </summary>
    /// <exception cref="PlatformNotSupportedException">
    /// The application is running on another operating system.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The system did not provide a usable user location.
    /// </exception>
    public static string userDataRoot
    {
        get
        {
            if (!OperatingSystem.IsMacOS())
                throw new PlatformNotSupportedException("This product requires MacOS.");
            string location = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Library", "Application Support");
            if (string.IsNullOrWhiteSpace(location) || !Path.IsPathFullyQualified(location))
                throw new InvalidOperationException("The system did not provide a usable application data root.");
            return location;
        }
    }

    /// <summary>
    /// Resolves the sole content location in a published MacOS application layout.
    /// </summary>
    /// <param name="applicationDirectory">
    /// The absolute product executable directory.
    /// </param>
    /// <returns>
    /// The explicit content directory, without probing alternative layouts.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The application directory is empty or relative.
    /// </exception>
    public static string GetPlayerContentDirectory(string applicationDirectory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(applicationDirectory);
        if (!Path.IsPathFullyQualified(applicationDirectory))
            throw new ArgumentException("An application directory must be absolute.", nameof(applicationDirectory));
        return Path.GetFullPath(Path.Combine(applicationDirectory, "..", "Resources", "Content"));
    }
}
