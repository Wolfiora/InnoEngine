using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Buffers;
using System.Text.Unicode;
using BGCS.Runtime;
using Inno.Native.ImGui;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui;
internal static unsafe class ImGuiUtf8Buffer
{
    private static readonly UTF8Encoding S_TEXT_ENCODING = new(false, true);

    internal static Vector2 MeasureText(
        string text,
        float wrapWidth
    ) => PresentText(text, wrapWidth, measure: true);

    internal static void DrawText(string text) => _ = PresentText(text, 0f, measure: false);

    [SkipLocalsInit]
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static Vector2 PresentText(
        string text,
        float wrapWidth,
        bool measure
    ) {
        ArgumentNullException.ThrowIfNull(text);
        if (text.AsSpan().Contains('\0'))
            throw new ArgumentException("Literal native text cannot contain zero code units.", nameof(text));
        int length = checked(S_TEXT_ENCODING.GetByteCount(text) + 1);
        byte[]? rented = length > 1024 ? ArrayPool<byte>.Shared.Rent(length) : null;
        Span<byte> buffer = rented is null ? stackalloc byte[length] : rented.AsSpan(0, length);
        try
        {
            int count = S_TEXT_ENCODING.GetBytes(text.AsSpan(), buffer);
            buffer[count] = 0;
            fixed (byte* value = buffer)
            {
                ImVec2C size = default;
                if (measure)
                    NativeImGui.Raw.CalcTextSize((sbyte*)value, (sbyte*)value + count, 0, wrapWidth, &size);
                else
                    NativeImGui.Raw.TextUnformatted((sbyte*)value, (sbyte*)value + count);
                return new Vector2(size.X, size.Y);
            }
        }
        finally
        {
            if (rented is not null)
                ArrayPool<byte>.Shared.Return(rented, clearArray: true);
        }
    }

    internal static bool InputText(
        string label,
        string? hint,
        ref string value,
        nuint capacity,
        ImGuiInputTextFlags flags
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(label);
        if (capacity is 0 or > int.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(capacity));
        const ImGuiInputTextFlags callbacks = ImGuiInputTextFlags.CallbackCompletion | ImGuiInputTextFlags.CallbackHistory | ImGuiInputTextFlags.CallbackAlways | ImGuiInputTextFlags.CallbackCharFilter | ImGuiInputTextFlags.CallbackResize | ImGuiInputTextFlags.CallbackEdit;
        if ((flags & callbacks) != 0)
            throw new ArgumentException("Managed text controls do not accept native callback flags.", nameof(flags));
        int length = (int)capacity;
        byte[]? rented = length > 1024 ? ArrayPool<byte>.Shared.Rent(length) : null;
        Span<byte> buffer = rented is null ? stackalloc byte[length] : rented.AsSpan(0, length);
        try
        {
            _ = Utf8.FromUtf16(value.AsSpan(), buffer[..^1], out _, out int bytesUsed, replaceInvalidSequences: true, isFinalBlock: true);
            buffer[bytesUsed] = 0;
            bool changed;
            using NativeStringLease nativeLabel = Utf8Codec.EncodeNullTerminated(label, allowsNull: false);
            using NativeStringLease nativeHint = Utf8Codec.EncodeNullTerminated(hint, allowsNull: true);
            fixed (byte* nativeBuffer = buffer)
            {
                changed = hint is null ? NativeImGui.Raw.InputText(
                    (sbyte*)nativeLabel.pointer,
                    (sbyte*)nativeBuffer,
                    (uint)capacity,
                    (int)flags,
                    null,
                    null
                ) != 0 : NativeImGui.Raw.InputTextWithHint(
                    (sbyte*)nativeLabel.pointer,
                    (sbyte*)nativeHint.pointer,
                    (sbyte*)nativeBuffer,
                    (uint)capacity,
                    (int)flags,
                    null,
                    null
                ) != 0;
            }

            if (changed)
            {
                int terminator = buffer.IndexOf((byte)0);
                value = Encoding.UTF8.GetString(buffer[..(terminator < 0 ? buffer.Length : terminator)]);
            }

            return changed;
        }
        finally
        {
            if (rented is not null)
                ArrayPool<byte>.Shared.Return(rented, clearArray: true);
        }
    }
}
