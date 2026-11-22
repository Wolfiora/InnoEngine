namespace Inno.Rendering;

/// <summary>
/// Supplies the command encoder and immutable frame identity to a pass callback.
/// </summary>
public sealed class RenderPassContext
{
    internal RenderPassContext(
        RenderCommandEncoder commands,
        ulong frameIndex
    ) {
        this.commands = commands;
        this.frameIndex = frameIndex;
    }

    /// <summary>
    /// Gets the command encoder scoped to the current pass.
    /// </summary>
    public RenderCommandEncoder commands { get; }

    /// <summary>
    /// Gets the monotonic frame index.
    /// </summary>
    public ulong frameIndex { get; }
}
