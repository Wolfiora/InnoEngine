using System;
using System.Buffers.Binary;
using System.IO;
using Inno.Editor.Interactions;

namespace Inno.Editor.Settings;

internal static class EditorSettingsHistory
{
    internal const string C_KIND = "editor.settings.apply";

    internal static EditorHistoryChange CreateChange(
        ReadOnlySpan<byte> before,
        ReadOnlySpan<byte> after
    ) {
        var payload = new byte[sizeof(int) + before.Length + after.Length];
        BinaryPrimitives.WriteInt32LittleEndian(payload, before.Length);
        before.CopyTo(payload.AsSpan(sizeof(int), before.Length));
        after.CopyTo(payload.AsSpan(sizeof(int) + before.Length));
        return new EditorHistoryChange(C_KIND, EditorHistoryPayload.FromBytes(payload));
    }

    internal static (ReadOnlyMemory<byte> before, ReadOnlyMemory<byte> after) Read(EditorHistoryChange change)
    {
        byte[] payload = change.payload.ReadBytes();
        if (payload.Length < sizeof(int))
        {
            throw new InvalidDataException("The Settings history payload is truncated.");
        }
        int beforeLength = BinaryPrimitives.ReadInt32LittleEndian(payload);
        if (beforeLength < 0 || beforeLength > payload.Length - sizeof(int))
        {
            throw new InvalidDataException("The Settings history payload has an invalid boundary.");
        }
        ReadOnlyMemory<byte> before = payload.AsMemory(sizeof(int), beforeLength);
        ReadOnlyMemory<byte> after = payload.AsMemory(sizeof(int) + beforeLength);
        return (before, after);
    }
}
