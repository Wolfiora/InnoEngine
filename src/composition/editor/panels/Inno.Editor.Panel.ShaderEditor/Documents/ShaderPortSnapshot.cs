using Inno.Core.Graphs;
using Inno.Rendering.Shaders;

namespace Inno.Editor.Panel.ShaderEditor;

internal struct ShaderPortSnapshot
{
    /// <summary>
    /// Gets the stable identity used to reference this value across subsystem boundaries.
    /// </summary>
    public string id { get; set; }

    /// <summary>
    /// Gets the graph port's shader value type.
    /// </summary>
    public ShaderGraphType type { get; set; }

    /// <summary>
    /// Gets the graph port's input or output direction.
    /// </summary>
    public GraphPortDirection direction { get; set; }

    /// <summary>
    /// Gets whether required is active for the current instance.
    /// </summary>
    public bool required { get; set; }
}
