using System;
using System.IO;
using System.Security.Cryptography;
using Inno.Native.ImGui;
using ImGuiNative = Inno.Native.ImGui.ImGui;

namespace Inno.Adapter.Presentation.ImGui;

public sealed unsafe partial class PlatformImGuiContext
{
    private const string C_SUPPLEMENTAL_FONT_FILE = "NotoSansCJKsc-Regular.otf";
    private const int C_SUPPLEMENTAL_FONT_BYTES = 16_437_364;
    private const string C_SUPPLEMENTAL_FONT_HASH = "2c76254f6fc379fddfce0a7e84fb5385bb135d3e399294f6eeb6680d0365b74b";

    private static byte[] ReadSupplementalFontData()
    {
        foreach (string directory in ResolveAssetDirectories(Path.Combine(DEFAULT_FONTS_DIRECTORY_RELATIVE_PATH, "Supplemental")))
        {
            string path = Path.Combine(directory, C_SUPPLEMENTAL_FONT_FILE);
            if (!File.Exists(path))
                continue;
            using FileStream source = File.OpenRead(path);
            if (source.Length != C_SUPPLEMENTAL_FONT_BYTES)
                throw new InvalidDataException($"The bundled supplemental text font '{path}' has an unexpected length.");
            byte[] data = GC.AllocateUninitializedArray<byte>(C_SUPPLEMENTAL_FONT_BYTES, pinned: true);
            source.ReadExactly(data);
            if (source.ReadByte() != -1 || !string.Equals(Convert.ToHexStringLower(SHA256.HashData(data)), C_SUPPLEMENTAL_FONT_HASH, StringComparison.Ordinal))
                throw new InvalidDataException($"The bundled supplemental text font '{path}' does not match its declared content identity.");
            return data;
        }
        throw new FileNotFoundException("The bundled supplemental text font is required for readable CJK labels.", C_SUPPLEMENTAL_FONT_FILE);
    }

    private static void MergeSupplementalFont(
        ImFontAtlas* fonts,
        byte[] data
    ) {
        ImFontConfig* configuration = ImGuiNative.Raw.ImFontConfig();
        try
        {
            configuration->MergeMode = 1;
            // All faces borrow the same permanently pinned buffer until their owning context is destroyed.
            configuration->FontDataOwnedByAtlas = 0;
            fixed (byte* pointer = data)
                if (ImGuiNative.Raw.AddFontFromMemoryTTF(fonts, pointer, data.Length, DEFAULT_FONT_SIZE_PIXELS, configuration, null) is null)
                    throw new InvalidOperationException("The bundled supplemental text font could not be added to the native atlas.");
        }
        finally
        {
            ImGuiNative.Raw.Destroy(configuration);
        }
    }
}
