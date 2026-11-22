using System;
using System.IO;

namespace Inno.Editor.Scene;

internal sealed record SceneHierarchyHistoryData(
    SceneObjectPlacement[] before,
    SceneObjectPlacement[] after,
    Guid selectedID
) {
    internal byte[] Encode()
    {
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        writer.Write(selectedID.ToByteArray());
        WritePlacements(writer, before);
        WritePlacements(writer, after);
        writer.Flush();
        return stream.ToArray();
    }

    internal static SceneHierarchyHistoryData Decode(ReadOnlySpan<byte> bytes)
    {
        using var stream = new MemoryStream(bytes.ToArray(), writable: false);
        using var reader = new BinaryReader(stream);
        var selectedID = new Guid(reader.ReadBytes(16));
        SceneObjectPlacement[] before = ReadPlacements(reader);
        SceneObjectPlacement[] after = ReadPlacements(reader);
        if (stream.Position != stream.Length)
        {
            throw new InvalidDataException("Scene hierarchy history payload contains trailing data.");
        }
        return new SceneHierarchyHistoryData(before, after, selectedID);
    }

    private static void WritePlacements(
        BinaryWriter writer,
        SceneObjectPlacement[] placements
    ) {
        writer.Write(placements.Length);
        for (int i = 0; i < placements.Length; i++)
        {
            SceneObjectPlacement placement = placements[i];
            writer.Write(placement.sceneID.ToByteArray());
            writer.Write(placement.objectID.ToByteArray());
            writer.Write(placement.parentID.HasValue);
            if (placement.parentID.HasValue)
            {
                writer.Write(placement.parentID.Value.ToByteArray());
            }
            writer.Write(placement.siblingIndex);
        }
    }

    private static SceneObjectPlacement[] ReadPlacements(BinaryReader reader)
    {
        int count = reader.ReadInt32();
        if (count < 0)
        {
            throw new InvalidDataException("Scene hierarchy placement count cannot be negative.");
        }
        var placements = new SceneObjectPlacement[count];
        for (int i = 0; i < placements.Length; i++)
        {
            var sceneID = new Guid(reader.ReadBytes(16));
            var objectID = new Guid(reader.ReadBytes(16));
            Guid? parentID = reader.ReadBoolean() ? new Guid(reader.ReadBytes(16)) : null;
            placements[i] = new SceneObjectPlacement(sceneID, objectID, parentID, reader.ReadInt32());
        }
        return placements;
    }
}

internal readonly record struct SceneObjectPlacement(
    Guid sceneID,
    Guid objectID,
    Guid? parentID,
    int siblingIndex
);
