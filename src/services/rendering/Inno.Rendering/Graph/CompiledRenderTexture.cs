namespace Inno.Rendering;

/// <summary>
/// Describes one texture allocation selected by render-graph compilation.
/// </summary>
public sealed class CompiledRenderTexture
{
    internal CompiledRenderTexture(
        RenderTextureHandle handle,
        string name,
        RenderTextureDescriptor descriptor,
        bool imported,
        PersistentTextureHandle persistentHandle,
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
    public RenderTextureHandle handle { get; }

    /// <summary>
    /// Gets the debug and diagnostic name.
    /// </summary>
    public string name { get; }

    /// <summary>
    /// Gets texture requirements.
    /// </summary>
    public RenderTextureDescriptor descriptor { get; }

    /// <summary>
    /// Gets whether the resource was imported from persistent device state.
    /// </summary>
    public bool imported { get; }

    /// <summary>
    /// Gets the persistent device texture for imported resources.
    /// </summary>
    public PersistentTextureHandle persistentHandle { get; }

    /// <summary>
    /// Gets the aliasing allocation slot, or negative one for imported resources.
    /// </summary>
    public int physicalSlot { get; }
}
