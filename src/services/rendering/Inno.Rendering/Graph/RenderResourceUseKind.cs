namespace Inno.Rendering;

internal enum RenderResourceUseKind
{
    GenericRead,
    StorageRead,
    StorageWrite,
    StorageReadWrite,
    CopySource,
    CopyDestination,
    ColorAttachment,
    DepthStencilAttachment
}
