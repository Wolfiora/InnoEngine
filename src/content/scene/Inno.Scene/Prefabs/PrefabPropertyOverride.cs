using System;

namespace Inno.Scene;

internal sealed record PrefabPropertyOverride(
    Guid sourceComponentID,
    string propertyName,
    byte[] value,
    bool isOrphaned = false
);
