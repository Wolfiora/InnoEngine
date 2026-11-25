using System;
using Inno.Editor.Interactions;

namespace Inno.Editor.Core.Tests;

[EditorHistoryHandler(NeutralHistoryHandler.C_KIND)]
public sealed class NeutralHistoryHandler : EditorHistoryHandler
{
    public const string C_KIND = "tests/neutral-value";
    public static int value;

    public static EditorHistoryChange CreateChange(
        int before,
        int after
    ) {
        var bytes = new byte[sizeof(int) * 2];
        BitConverter.GetBytes(before).CopyTo(bytes, 0);
        BitConverter.GetBytes(after).CopyTo(bytes, sizeof(int));
        return new EditorHistoryChange(C_KIND, EditorHistoryPayload.FromBytes(bytes));
    }

    protected override EditorHistoryAvailability Query(
        EditorHistoryContext context,
        EditorHistoryChange change,
        EditorHistoryDirection direction
    ) => change.payload.length >= sizeof(int) * 2
         ? EditorHistoryAvailability.Available()
         : EditorHistoryAvailability.Unavailable("The neutral value payload is truncated.");

    protected override EditorHistoryResult Apply(
        EditorHistoryContext context,
        EditorHistoryChange change,
        EditorHistoryDirection direction
    ) {
        byte[] bytes = change.payload.ReadBytes();
        value = BitConverter.ToInt32(bytes, direction == EditorHistoryDirection.Undo ? 0 : sizeof(int));
        return EditorHistoryResult.Success();
    }
}
