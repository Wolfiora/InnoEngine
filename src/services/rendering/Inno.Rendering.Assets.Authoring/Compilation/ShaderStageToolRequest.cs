using Inno.Extensibility.Types;
using Inno.Rendering.Shaders;

namespace Inno.Rendering.Assets.Authoring;

/// <summary>
/// Supplies typed stage semantics and frozen source modules to a target compiler.
/// </summary>
/// <param name="stage">
/// Typed stage with target-assigned interface/resource locations.
/// </param>
/// <param name="target">
/// Exact toolchain profile and capability snapshot.
/// </param>
[StableTypeID("8a37ee08-a048-52fc-bbf4-bbc47fe425d4")]
public sealed record ShaderStageToolRequest(
    ShaderIrStage stage,
    ShaderCompileTarget target
);
