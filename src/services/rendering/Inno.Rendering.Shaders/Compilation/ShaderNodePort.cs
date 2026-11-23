using Inno.Core.Graphs;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Declares a typed shader port independently of editor presentation and backend source syntax.
/// </summary>
/// <param name="id">
/// Stable node-local port identity.
/// </param>
/// <param name="type">
/// Complete value type, including all aggregate fields.
/// </param>
/// <param name="direction">
/// Input or output value flow.
/// </param>
/// <param name="required">
/// Whether an input must be connected; alternative aggregate/member inputs validate in their compiler.
/// </param>
public sealed record ShaderNodePort(
    string id,
    ShaderSourceType type,
    GraphPortDirection direction,
    bool required = true
);
