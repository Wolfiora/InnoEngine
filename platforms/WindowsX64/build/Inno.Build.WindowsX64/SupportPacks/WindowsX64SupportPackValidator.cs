using System;
using System.IO;
using System.Linq;

namespace Inno.Build.WindowsX64;

/// <summary>
/// Validates the platform runtime or linker closure before compilation and installation.
/// </summary>
public sealed class WindowsX64SupportPackValidator : IPlayerSupportPackValidator
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
        string[] foreignNativeExtensions = [".dylib", ".so"];
        foreach (string file in files.Where(file => file.StartsWith(nativeRoot, StringComparison.Ordinal)))
        {
            if (foreignNativeExtensions.Contains(Path.GetExtension(file), StringComparer.OrdinalIgnoreCase))
            {
                throw new InvalidDataException(
                    $"Player Support Pack '{Inno.Build.WindowsX64.WindowsX64BuildModule.target.id}' contains foreign native runtime '{Path.GetFileName(file)}'.");
            }
            if (Path.GetExtension(file).Equals(".dll", StringComparison.OrdinalIgnoreCase))
                ValidateLibrary(file);
        }
        foreach (string input in new[]
        {
            "Player.csproj", "Program.cs", "WindowsX64PlayerComposition.cs", "WindowsX64PlayerContentSource.cs", "global.json",
            Path.Combine("Analyzers", "Inno.Runtime.Generators.dll"),
            Path.Combine("Analyzers", "Inno.Core.Serialization.Generators.dll"),
            Path.Combine("References", "Inno.Player.Runtime.dll")
        })
            if (!File.Exists(Path.Combine(directory, "PlayerLink", input)))
                throw new InvalidDataException($"The Windows Support Pack lacks publication input '{input}'.");
    }

    private static void ValidateLibrary(string file)
    {
        using var input = new BinaryReader(File.OpenRead(file));
        if (input.BaseStream.Length < 64 || input.ReadUInt16() != 0x5a4d)
            throw new InvalidDataException($"WindowsX64 native library '{file}' has no PE header.");
        input.BaseStream.Position = 60;
        int offset = input.ReadInt32();
        if (offset < 64 || offset > input.BaseStream.Length - 26)
            throw new InvalidDataException($"WindowsX64 native library '{file}' has an invalid PE header offset.");
        input.BaseStream.Position = offset;
        if (input.ReadUInt32() != 0x00004550 || input.ReadUInt16() != 0x8664)
            throw new InvalidDataException($"WindowsX64 native library '{file}' does not target the AMD64 ABI.");
        input.BaseStream.Position = offset + 22;
        if ((input.ReadUInt16() & 0x2000) == 0 || input.ReadUInt16() != 0x020b)
            throw new InvalidDataException($"WindowsX64 native library '{file}' is not a PE32+ dynamic library.");
    }
}
