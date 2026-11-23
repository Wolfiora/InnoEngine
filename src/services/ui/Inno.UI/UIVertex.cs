namespace Inno.UI;

/// <summary>
/// Describes one vertex in backend-neutral UI draw geometry.
/// </summary>
/// <param name="x">
/// The horizontal or first component.
/// </param>
/// <param name="y">
/// The vertical or second component.
/// </param>
/// <param name="u">
/// The float value used to initialize this instance.
/// </param>
/// <param name="v">
/// The concrete value read or transformed by this operation.
/// </param>
/// <param name="color">
/// The uint value used to initialize this instance.
/// </param>
public readonly record struct UIVertex(
    float x,
    float y,
    float u,
    float v,
    uint color
);
