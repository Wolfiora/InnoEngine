using System;

namespace Inno.Editor.Scene;

internal sealed record SceneIncomingReferenceState(
    Guid ownerID,
    string propertyName,
    byte[] data
);
