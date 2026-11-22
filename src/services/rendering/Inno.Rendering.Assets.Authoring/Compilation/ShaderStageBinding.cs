using Inno.Extensibility.Types;

namespace Inno.Rendering.Assets.Authoring;

/// <summary>
/// Maps one logical binding to the adapter-generated native name, without exposing a GPU handle.
/// </summary>
/// <param name="id">
/// Stable logical binding identity from the typed stage.
/// </param>
/// <param name="nativeName">
/// Generated name used for reflection and runtime uniform binding.
/// </param>
/// <param name="location">
/// Assigned texture slot; uniform locations are resolved by the runtime.
/// </param>
[StableTypeID("00dff34d-f800-51e6-8a37-0141761958c8")]
public sealed record ShaderStageBinding(
    string id,
    string nativeName,
    int location
);
