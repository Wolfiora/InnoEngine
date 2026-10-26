using System;
using System.IO;
using System.Linq;

namespace Inno.Build.MacOSArm64;

/// <summary>
/// Validates the platform runtime or linker closure before compilation and installation.
/// </summary>
public sealed class MacOSArm64SupportPackValidator : IPlayerSupportPackValidator
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
                    $"Player Support Pack '{Inno.Build.MacOSArm64.MacOSArm64BuildModule.target.id}' contains foreign native runtime '{Path.GetFileName(file)}'.");
            }
            if (Path.GetExtension(file).Equals(".dylib", StringComparison.OrdinalIgnoreCase))
                ValidateLibrary(file);
        }
        foreach (string input in new[]
        {
            "Player.csproj", "Program.cs", "MacOSArm64PlayerComposition.cs", "MacOSArm64PlayerContentSource.cs", "global.json",
            Path.Combine("Analyzers", "Inno.Runtime.Generators.dll"),
            Path.Combine("Analyzers", "Inno.Core.Serialization.Generators.dll"),
            Path.Combine("References", "Inno.Player.Runtime.dll")
        })
            if (!File.Exists(Path.Combine(directory, "PlayerLink", input)))
                throw new InvalidDataException($"The MacOS Support Pack lacks publication input '{input}'.");
    }

    private static void ValidateLibrary(string file)
    {
        using var input = new BinaryReader(File.OpenRead(file));
        if (input.BaseStream.Length < 32 || input.ReadUInt32() != 0xfeedfacf
            || input.ReadUInt32() != 0x0100000c)
            throw new InvalidDataException($"MacOSArm64 native library '{file}' does not target the Mach-O ARM64 ABI.");
        _ = input.ReadUInt32();
        if (input.ReadUInt32() != 6)
            throw new InvalidDataException($"MacOSArm64 native library '{file}' is not a dynamic library.");
    }
}
