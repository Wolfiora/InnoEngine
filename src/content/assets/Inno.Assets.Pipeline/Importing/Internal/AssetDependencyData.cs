using System;
using Inno.Core.Serialization;

namespace Inno.Assets.Pipeline;

internal struct AssetDependencyData
{
    [SerializableProperty]
    internal Guid persistentId { get; set; }

    [SerializableProperty]
    internal Guid stableTypeId { get; set; }

    [SerializableProperty]
    internal string lastKnownPath { get; set; }
}
