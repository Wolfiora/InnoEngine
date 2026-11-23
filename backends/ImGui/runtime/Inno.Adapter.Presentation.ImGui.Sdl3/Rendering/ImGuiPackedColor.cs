using System;
using System.Numerics;
using Inno.Native.Sdl3;

using ImGuiNative = Inno.Native.ImGui.ImGui;

namespace Inno.Adapter.Presentation.ImGui;

internal static class ImGuiPackedColor
{
    private static bool s_initialized;
    private static int s_rShift;
    private static int s_gShift;
    private static int s_bShift;
    private static int s_aShift;

    internal static void EnsureInitialized()
    {
        if (s_initialized)
        {
            return;
        }

        var probe = new Vector4(0.125f, 0.5f, 0.875f, 1f);
        uint packed = ImGuiNative.GetColorU32(probe);
        int expectedR = (int)(probe.X * 255f + 0.5f);
        int expectedG = (int)(probe.Y * 255f + 0.5f);
        int expectedB = (int)(probe.Z * 255f + 0.5f);
        int expectedA = (int)(probe.W * 255f + 0.5f);

        int b0 = (int)(packed & 0xFF);
        int b1 = (int)((packed >> 8) & 0xFF);
        int b2 = (int)((packed >> 16) & 0xFF);
        int b3 = (int)((packed >> 24) & 0xFF);
        Span<int> packedBytes = [b0, b1, b2, b3];

        int usedMask = 0;
        s_rShift = SelectBestShift(packedBytes, expectedR, ref usedMask);
        s_gShift = SelectBestShift(packedBytes, expectedG, ref usedMask);
        s_bShift = SelectBestShift(packedBytes, expectedB, ref usedMask);
        s_aShift = SelectBestShift(packedBytes, expectedA, ref usedMask);
        s_initialized = true;
    }

    internal static SDLFColor ToSdlFColor(uint packedColor)
    {
        EnsureInitialized();

        const float C_INV255 = 1f / 255f;
        float r = ((packedColor >> s_rShift) & 0xFF) * C_INV255;
        float g = ((packedColor >> s_gShift) & 0xFF) * C_INV255;
        float b = ((packedColor >> s_bShift) & 0xFF) * C_INV255;
        float a = ((packedColor >> s_aShift) & 0xFF) * C_INV255;
        return new SDLFColor { R = r, G = g, B = b, A = a };
    }

    private static int SelectBestShift(
        ReadOnlySpan<int> packedBytes,
        int expected,
        ref int usedMask
    ) {
        int bestIndex = 0;
        int bestError = int.MaxValue;
        for (int i = 0; i < 4; i++)
        {
            if ((usedMask & (1 << i)) != 0)
            {
                continue;
            }

            int error = Math.Abs(packedBytes[i] - expected);
            if (error < bestError)
            {
                bestError = error;
                bestIndex = i;
            }
        }

        usedMask |= 1 << bestIndex;
        return bestIndex * 8;
    }
}
