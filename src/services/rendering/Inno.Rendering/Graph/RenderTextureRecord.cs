namespace Inno.Rendering;

internal sealed record RenderTextureRecord(
    string name,
    RenderTextureDescriptor descriptor,
    bool imported,
    PersistentTextureHandle persistentHandle,
    ulong allocationID
);
