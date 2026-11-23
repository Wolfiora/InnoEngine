namespace Inno.Rendering;

internal readonly record struct RenderResourceKey(
    bool isTexture,
    int index
);
