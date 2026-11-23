using System.Collections.Generic;

namespace Inno.Core.Serialization.Tests;

internal sealed class MapHost : ISerializable
{
    [SerializableProperty]
    public Dictionary<int, string> values { get; set; } = [];
}
