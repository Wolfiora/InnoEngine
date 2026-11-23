namespace Inno.Rendering;

/// <summary>
/// Describes one buffer allocation selected by render-graph compilation.
/// </summary>
public sealed class CompiledRenderBuffer
{
    internal CompiledRenderBuffer(
        RenderBufferHandle handle,
        string name,
        RenderBufferDescriptor descriptor,
        bool imported,
        PersistentBufferHandle persistentHandle,
        int physicalSlot
    ) {
        this.handle = handle;
        this.name = name;
        this.descriptor = descriptor;
        this.imported = imported;
        this.persistentHandle = persistentHandle;
        this.physicalSlot = physicalSlot;
    }

    /// <summary>
    /// Gets the frame-scoped logical handle.
    /// </summary>
    public RenderBufferHandle handle { get; }

    /// <summary>
    /// Gets the debug and diagnostic name.
    /// </summary>
    public string name { get; }

    /// <summary>
    /// Gets buffer requirements.
    /// </summary>
    public RenderBufferDescriptor descriptor { get; }

    /// <summary>
    /// Gets whether the resource was imported from persistent device state.
    /// </summary>
    public bool imported { get; }

    /// <summary>
    /// Gets the persistent device buffer for imported resources.
    /// </summary>
    public PersistentBufferHandle persistentHandle { get; }

    /// <summary>
    /// Gets the aliasing allocation slot, or negative one for imported resources.
    /// </summary>
    public int physicalSlot { get; }
}
