namespace Inno.Rendering;

internal sealed record RenderAttachment(
    RenderTextureHandle texture,
    int slot,
    bool isDepth,
    int mipLevel,
    int arrayLayer,
    RenderLoadAction loadAction,
    RenderStoreAction storeAction,
    RenderClearColor clearColor,
    float clearDepth,
    byte clearStencil
);
