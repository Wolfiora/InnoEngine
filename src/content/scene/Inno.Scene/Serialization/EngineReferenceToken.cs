using System;

namespace Inno.Scene;

internal readonly record struct EngineReferenceToken(
    EngineReferenceKind kind,
    Guid sourceID
);

internal enum EngineReferenceKind
{
    GameObject = 1,
    GameComponent = 2,
    GameSystem = 3
}
