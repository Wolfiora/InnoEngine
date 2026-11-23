namespace Inno.Rendering;

internal sealed record RenderBufferRecord(
    string name,
    RenderBufferDescriptor descriptor,
    bool imported,
    PersistentBufferHandle persistentHandle,
    ulong allocationID
);
