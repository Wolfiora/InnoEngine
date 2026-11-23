namespace Inno.Rendering;

/// <summary>
/// Executes one frame-scoped pass payload.
/// </summary>
/// <typeparam name="TPassData">
/// Neutral pass payload type.
/// </typeparam>
/// <param name="passData">
/// Pass payload captured for this frame only.
/// </param>
/// <param name="context">
/// Current pass execution context.
/// </param>
public delegate void RenderPassExecute<in TPassData>(
    TPassData passData,
    RenderPassContext context
);
