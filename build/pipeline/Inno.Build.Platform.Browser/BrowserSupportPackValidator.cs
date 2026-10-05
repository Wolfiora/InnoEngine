using System;
using System.IO;
using System.Linq;

namespace Inno.Build.Platform.Browser;

/// <summary>
/// Validates the platform runtime or linker closure before compilation and installation.
/// </summary>
public sealed class BrowserSupportPackValidator : IPlayerSupportPackValidator
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
            "Player.csproj", "Program.cs", "BrowserPlayerComposition.cs", "BrowserContentLoader.cs", "BrowserBridge.cs", "global.json",
            Path.Combine("Analyzers", "Inno.Runtime.Generators.dll"),
            Path.Combine("Analyzers", "Inno.Core.Serialization.Generators.dll"),
            Path.Combine("wwwroot", "index.html"), Path.Combine("wwwroot", "main.js"),
            Path.Combine("Native", "wasm_sjlj_shim.c"),
            Path.Combine("References", "BGCS.Runtime.dll"),
            Path.Combine("References", "Inno.Player.Runtime.dll"),
            Path.Combine("References", "Inno.Native.Bgfx.dll"),
            Path.Combine("References", "Inno.Native.MiniAudio.dll"),
            Path.Combine("References", "Inno.Native.Sdl3.dll"),
            Path.Combine("References", "Inno.Native.Text.dll"),
            Path.Combine("References", "Inno.Native.UI.dll")
        })
        {
            if (!File.Exists(Path.Combine(linker, input)))
                throw new InvalidDataException($"The browser Support Pack lacks linker input '{input}'.");
        }
        foreach (string archive in new[]
        {
            "bgfxRelease.a", "bxRelease.a", "bimgRelease.a", "bimg_decodeRelease.a",
            "libSDL3.a", "libminiaudio.a", "libinno-text.a", "libinno-ui.a",
            "librmlui.a", "libfreetype.a", "libharfbuzz.a"
        })
        {
            if (!File.Exists(Path.Combine(linker, "Native", archive)))
                throw new InvalidDataException($"The browser Support Pack lacks native archive '{archive}'.");
        }
    }
}
