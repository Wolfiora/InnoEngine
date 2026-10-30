using System;
using System.Text;
using System.Buffers;
using System.Text.Unicode;
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
