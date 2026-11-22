namespace Inno.UI;

/// <summary>
/// Describes one draw of a previously published mesh.
/// </summary>
/// <param name="mesh">
/// The ui mesh handle value used to initialize this instance.
/// </param>
/// <param name="texture">
/// The ui texture handle value used to initialize this instance.
/// </param>
/// <param name="scissorEnabled">
/// The bool value used to initialize this instance.
/// </param>
/// <param name="scissor">
/// The ui clip rectangle value used to initialize this instance.
/// </param>
public readonly record struct UIDrawCommand(
    UIMeshHandle mesh,
    UITextureHandle texture,
    bool scissorEnabled,
    UIClipRectangle scissor
);
