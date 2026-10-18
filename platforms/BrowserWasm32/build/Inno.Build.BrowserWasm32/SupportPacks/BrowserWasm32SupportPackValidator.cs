using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Text;
using Inno.Build.Toolchains;

namespace Inno.Build.BrowserWasm32;

/// <summary>
/// Validates the platform runtime or linker closure before compilation and installation.
/// </summary>
public sealed class BrowserWasm32SupportPackValidator : IPlayerSupportPackValidator
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
        string linker = Path.Combine(directory, "PlayerLink");
        foreach (string input in new[]
        {
            "Player.csproj", "Program.cs", "BrowserWasm32PlayerComposition.cs", "HttpPlayerContentSource.cs", "BrowserWasm32Bridge.cs", "global.json",
            Path.Combine("Analyzers", "Inno.Runtime.Generators.dll"),
            Path.Combine("Analyzers", "Inno.Core.Serialization.Generators.dll"),
            Path.Combine("wwwroot", "index.html"), Path.Combine("wwwroot", "main.js"),
            Path.Combine("Native", "wasm_sjlj_shim.c"),
            Path.Combine("References", "Inno.Player.Runtime.dll")
        })
        {
            if (!File.Exists(Path.Combine(linker, input)))
                throw new InvalidDataException($"The browser Support Pack lacks linker input '{input}'.");
        }
        string nativeDirectory = Path.Combine(linker, "Native");
        string[] foreignBinaries = Directory.EnumerateFiles(nativeDirectory, "*", SearchOption.AllDirectories)
            .Where(path => new[] { ".dll", ".so", ".dylib", ".lib" }
                .Contains(Path.GetExtension(path), StringComparer.OrdinalIgnoreCase))
            .OrderBy(path => path, StringComparer.Ordinal)
            .ToArray();
        if (foreignBinaries.Length != 0)
            throw new InvalidDataException($"The browser Support Pack contains a foreign native binary '{Path.GetFileName(foreignBinaries[0])}'.");
        foreach (string archive in Directory.EnumerateFiles(nativeDirectory, "*.a", SearchOption.AllDirectories))
            ValidateArchive(archive);
    }

    private static void ValidateArchive(string file)
    {
        using FileStream input = File.OpenRead(file);
        Span<byte> magic = stackalloc byte[8];
        if (input.Length < magic.Length)
            throw new InvalidDataException($"BrowserWasm32 archive '{file}' has no archive header.");
        input.ReadExactly(magic);
        if (!magic.SequenceEqual("!<arch>\n"u8))
            throw new InvalidDataException($"BrowserWasm32 archive '{file}' is not an owned object archive.");
        Span<byte> header = stackalloc byte[60];
        Span<byte> objectHeader = stackalloc byte[8];
        bool containsObject = false;
        while (input.Position < input.Length)
        {
            if (input.Length - input.Position < header.Length)
                throw new InvalidDataException($"BrowserWasm32 archive '{file}' has a truncated member header.");
            input.ReadExactly(header);
            string name = Encoding.ASCII.GetString(header[..16]).TrimEnd();
            if (!header[58..].SequenceEqual("`\n"u8)
                || !long.TryParse(Encoding.ASCII.GetString(header.Slice(48, 10)).Trim(),
                    NumberStyles.None, CultureInfo.InvariantCulture, out long length)
                || length < 0 || length > input.Length - input.Position)
                throw new InvalidDataException($"BrowserWasm32 archive '{file}' has an invalid member size.");
            long end = input.Position + length;
            bool metadata = name is "/" or "//" or "/SYM64/" || name.StartsWith("__.SYMDEF", StringComparison.Ordinal);
            if (!metadata)
            {
                if (name.StartsWith("#1/", StringComparison.Ordinal))
                {
                    if (!int.TryParse(name[3..], NumberStyles.None, CultureInfo.InvariantCulture, out int prefix)
                        || prefix < 0 || prefix > length - objectHeader.Length)
                        throw new InvalidDataException($"BrowserWasm32 archive '{file}' has an invalid member name.");
                    input.Position += prefix;
                }
                if (end - input.Position < objectHeader.Length)
                    throw new InvalidDataException($"BrowserWasm32 archive '{file}' contains a truncated object.");
                input.ReadExactly(objectHeader);
                if (!objectHeader.SequenceEqual("\0asm\u0001\0\0\0"u8))
                    throw new InvalidDataException($"BrowserWasm32 archive '{file}' contains a foreign object ABI.");
                containsObject = true;
            }
            input.Position = end + (length & 1);
            if (input.Position > input.Length)
                throw new InvalidDataException($"BrowserWasm32 archive '{file}' lacks member alignment padding.");
        }
        if (!containsObject)
            throw new InvalidDataException($"BrowserWasm32 archive '{file}' contains no Wasm objects.");
    }
}
