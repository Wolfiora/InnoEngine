namespace Inno.Rendering;

internal sealed record RenderResourceUse(
    RenderResourceKey key,
    RenderResourceAccess access,
    RenderResourceUseKind kind
);
