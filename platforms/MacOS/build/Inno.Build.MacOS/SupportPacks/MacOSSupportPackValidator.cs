using System;
using System.IO;
using System.Linq;

namespace Inno.Build.MacOS;

/// <summary>
/// Validates the platform runtime or linker closure before compilation and installation.
/// </summary>
public sealed class MacOSSupportPackValidator : IPlayerSupportPackValidator
{
    /// <summary>
    /// Rejects incomplete or foreign platform inputs in the supplied Support Pack.
    /// </summary>
    /// <param name="directory">
    /// The isolated or installed target directory to validate.
    /// </param>
    /// <exception cref="InvalidDataException">
    /// A required input is absent or the closure contains a foreign native binary.
    /// </exception>
    public void Validate(string directory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(directory);
        string[] files = Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories).ToArray();
        string nativeRoot = Path.Combine(directory, "native") + Path.DirectorySeparatorChar;
        string[] foreignNativeExtensions = [".dll", ".so"];
        foreach (string file in files.Where(file => file.StartsWith(nativeRoot, StringComparison.Ordinal)))
        {
            if (foreignNativeExtensions.Contains(Path.GetExtension(file), StringComparer.OrdinalIgnoreCase))
            {
                throw new InvalidDataException(
                    $"Player Support Pack '{BuildTargetId.macOSArm64}' contains foreign native runtime '{Path.GetFileName(file)}'.");
            }
        }
        foreach (string input in new[]
        {
            "Player.csproj", "Program.cs", "MacOSPlayerComposition.cs", "MacOSPlayerContentSource.cs", "global.json",
            Path.Combine("Analyzers", "Inno.Runtime.Generators.dll"),
            Path.Combine("Analyzers", "Inno.Core.Serialization.Generators.dll"),
            Path.Combine("References", "Inno.Player.Runtime.dll"), Path.Combine("References", "BGCS.Runtime.dll")
        })
            if (!File.Exists(Path.Combine(directory, "PlayerLink", input)))
                throw new InvalidDataException($"The MacOS Support Pack lacks publication input '{input}'.");
        string[] requiredNativeFiles = [
                "libbgfx-shared-lib-release.dylib",
                "SDL3-release.dylib",
                "libminiaudio-release.dylib",
                "libinno-text-release.dylib",
                "libinno-ui-release.dylib"
            ];
        foreach (string requiredNativeFile in requiredNativeFiles)
        {
            if (!files.Where(file => file.StartsWith(nativeRoot, StringComparison.Ordinal)).Any(file => string.Equals(
                    Path.GetFileName(file),
                    requiredNativeFile,
                    StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidDataException(
                    $"Player Support Pack '{BuildTargetId.macOSArm64}' is missing native runtime '{requiredNativeFile}'.");
            }
        }

    }
}
