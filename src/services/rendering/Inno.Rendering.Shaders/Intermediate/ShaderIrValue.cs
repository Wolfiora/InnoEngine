namespace Inno.Rendering.Shaders;

/// <summary>
/// Identifies a typed single-assignment value within one intermediate block.
/// </summary>
public sealed class ShaderIrValue
{
    internal ShaderIrValue(
        object owner,
        int index,
        ShaderSourceType type
    ) {
        this.owner = owner;
        this.index = index;
        this.type = type;
    }

    /// <summary>
    /// Gets the deterministic block-local value index, not a GPU register or resource slot.
    /// </summary>
    public int index { get; }

    /// <summary>
    /// Gets the complete language-independent value type.
    /// </summary>
    public ShaderSourceType type { get; }

    internal object owner { get; }
}
