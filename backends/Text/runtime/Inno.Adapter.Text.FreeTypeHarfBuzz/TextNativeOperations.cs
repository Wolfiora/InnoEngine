using System;
using System.Runtime.InteropServices;
using Inno.Native.Text;

namespace Inno.Adapter.Text.FreeTypeHarfBuzz;

internal static unsafe class TextNativeOperations
{
    internal static InnoTextFontAdapterRuntime* CreateSession()
    {
        InnoTextFontAdapterRuntime* runtime = null;
        BgcsInnoTextError* error = null;
        CheckBridge(TextNative.Raw.CreateRuntime(&runtime, &error), error);
        return runtime;
    }

    internal static void DestroySession(InnoTextFontAdapterRuntime* runtime)
    {
        BgcsInnoTextError* error = null;
        CheckBridge(TextNative.Raw.DestroyRuntime(runtime, &error), error);
    }

    internal static Result LoadFont(
        InnoTextFontAdapterRuntime* runtime,
        ReadOnlySpan<byte> data,
        int faceIndex,
        ref ulong handle
    ) {
        Result result = default;
        BgcsInnoTextError* error = null;
        fixed (byte* bytes = data)
        fixed (ulong* destination = &handle)
            CheckBridge(TextNative.Raw.LoadFont(runtime, bytes, checked((uint)data.Length), faceIndex, destination, &result, &error), error);
        return result;
    }

    internal static Result ReleaseFont(
        InnoTextFontAdapterRuntime* runtime,
        ulong handle
    ) {
        Result result = default;
        BgcsInnoTextError* error = null;
        CheckBridge(TextNative.Raw.ReleaseFont(runtime, handle, &result, &error), error);
        return result;
    }

    internal static Result ShapeUtf8(
        InnoTextFontAdapterRuntime* runtime,
        ulong handle,
        byte* text,
        uint textLength,
        float fontSize,
        Direction direction,
        byte* language,
        byte* script,
        InnoTextFontAdapterGlyph* glyphs,
        uint capacity,
        uint* count
    ) {
        Result result = default;
        BgcsInnoTextError* error = null;
        // BGCS verifies every DTO field, offset, size and alignment in the native bridge.
        CheckBridge(TextNative.Raw.ShapeUtf8(runtime, handle, (sbyte*)text, textLength, fontSize, direction,
            (sbyte*)language, (sbyte*)script, (InnoTextFontAdapterGlyphNative*)glyphs, capacity, count, &result, &error), error);
        return result;
    }

    internal static Result GetMetrics(
        InnoTextFontAdapterRuntime* runtime,
        ulong handle,
        float fontSize,
        ref InnoTextFontAdapterMetrics metrics
    ) {
        Result result = default;
        BgcsInnoTextError* error = null;
        fixed (InnoTextFontAdapterMetrics* destination = &metrics)
            CheckBridge(TextNative.Raw.GetMetrics(runtime, handle, fontSize,
                (InnoTextFontAdapterMetricsNative*)destination, &result, &error), error);
        return result;
    }

    internal static Result RasterizeGlyph(
        InnoTextFontAdapterRuntime* runtime,
        ulong handle,
        uint glyphId,
        float fontSize,
        byte* pixels,
        uint capacity,
        InnoTextFontAdapterBitmap* bitmap
    ) {
        Result result = default;
        BgcsInnoTextError* error = null;
        CheckBridge(TextNative.Raw.RasterizeGlyph(runtime, handle, glyphId, fontSize, pixels, capacity,
            (InnoTextFontAdapterBitmapNative*)bitmap, &result, &error), error);
        return result;
    }

    internal static string? ResultMessage(Result result)
    {
        sbyte* message = null;
        BgcsInnoTextError* error = null;
        CheckBridge(TextNative.Raw.ResultMessage(result, &message, &error), error);
        return Marshal.PtrToStringUTF8((nint)message);
    }

    private static void CheckBridge(
        int status,
        BgcsInnoTextError* error
    ) {
        if (status == 0 && error == null)
            return;
        try
        {
            string? message = error == null ? null : Marshal.PtrToStringUTF8((nint)TextNative.Raw.BgcsInnoTextErrorMessage(error));
            throw new InvalidOperationException($"The text bridge failed with status {status}: {message ?? "No native error text is available."}");
        }
        finally
        {
            if (error != null)
                TextNative.Raw.BgcsInnoTextErrorRelease(error);
        }
    }
}
