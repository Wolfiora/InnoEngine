using System;
using System.Text;
using BGCS.Runtime;

using Inno.Native.ImGui;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui;

internal static unsafe class ImGuiUtf8Buffer
{
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

        byte[] buffer = new byte[(int)capacity];
        Encoding.UTF8.GetEncoder().Convert(
            value.AsSpan(),
            buffer.AsSpan(0, buffer.Length - 1),
            flush: true,
            out _,
            out int bytesUsed,
            out _);
        buffer[bytesUsed] = 0;

        bool changed;
        using NativeStringLease nativeLabel = Utf8Codec.EncodeNullTerminated(label, allowsNull: false);
        using NativeStringLease nativeHint = Utf8Codec.EncodeNullTerminated(hint, allowsNull: true);
        fixed (byte* nativeBuffer = buffer)
        {
            changed = hint is null
                ? NativeImGui.Raw.InputText((sbyte*)nativeLabel.pointer, (sbyte*)nativeBuffer, (uint)capacity, (int)flags, null, null) != 0
                : NativeImGui.Raw.InputTextWithHint(
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
            int terminator = Array.IndexOf(buffer, (byte)0);
            value = Encoding.UTF8.GetString(buffer, 0, terminator < 0 ? buffer.Length : terminator);
        }
        return changed;
    }
}
