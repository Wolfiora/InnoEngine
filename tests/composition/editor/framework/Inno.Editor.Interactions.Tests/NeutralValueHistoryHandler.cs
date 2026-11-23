using System;
using System.Collections.Generic;

namespace Inno.Editor.Interactions.Tests;

[EditorHistoryHandler(NeutralValueHistoryHandler.C_KIND)]
public sealed class NeutralValueHistoryHandler : EditorHistoryHandler
{
    public const string C_KIND = "tests/neutral-values";
    private static readonly HashSet<(int slot, EditorHistoryDirection direction)> BLOCKED = [];
    private static readonly Dictionary<int, int> VALUES = [];

    public static EditorHistoryChange CreateChange(
        int slot,
        int before,
        int after,
        string? mergeKey = null
    ) {
        var bytes = new byte[sizeof(int) * 3];
        BitConverter.GetBytes(slot).CopyTo(bytes, 0);
        BitConverter.GetBytes(before).CopyTo(bytes, sizeof(int));
        BitConverter.GetBytes(after).CopyTo(bytes, sizeof(int) * 2);
        return new EditorHistoryChange(C_KIND, EditorHistoryPayload.FromBytes(bytes), mergeKey);
    }

    public static int GetValue(int slot) => VALUES.TryGetValue(slot, out int value) ? value : 0;

    public static void SetValue(
        int slot,
        int value
    ) => VALUES[slot] = value;

    public static void Block(
        int slot,
        EditorHistoryDirection direction
    ) => BLOCKED.Add((slot, direction));

    public static void Unblock(
        int slot,
        EditorHistoryDirection direction
    ) => BLOCKED.Remove((slot, direction));

    public static void Reset()
    {
        BLOCKED.Clear();
        VALUES.Clear();
    }

    protected override EditorHistoryAvailability Query(
        EditorHistoryContext context,
        EditorHistoryChange change,
        EditorHistoryDirection direction
    ) => change.payload.length >= sizeof(int) * 3
         ? EditorHistoryAvailability.Available()
         : EditorHistoryAvailability.Unavailable("The neutral value payload is truncated.");

    protected override EditorHistoryResult Apply(
        EditorHistoryContext context,
        EditorHistoryChange change,
        EditorHistoryDirection direction
    ) {
        byte[] bytes = change.payload.ReadBytes();
        int slot = BitConverter.ToInt32(bytes, 0);
        if (BLOCKED.Contains((slot, direction)))
        {
            return EditorHistoryResult.Failure($"Slot {slot} is blocked for {direction}.");
        }
        int offset = direction == EditorHistoryDirection.Undo ? sizeof(int) : sizeof(int) * 2;
        VALUES[slot] = BitConverter.ToInt32(bytes, offset);
        return EditorHistoryResult.Success();
    }

    protected override bool TryMerge(
        EditorHistoryChange older,
        EditorHistoryChange newer,
        out EditorHistoryChange? merged
    ) {
        byte[] olderBytes = older.payload.ReadBytes();
        byte[] newerBytes = newer.payload.ReadBytes();
        int slot = BitConverter.ToInt32(olderBytes, 0);
        if (slot != BitConverter.ToInt32(newerBytes, 0))
        {
            merged = null;
            return false;
        }

        merged = CreateChange(
            slot,
            BitConverter.ToInt32(olderBytes, sizeof(int)),
            BitConverter.ToInt32(newerBytes, sizeof(int) * 2),
            older.mergeKey
        );
        return true;
    }
}
