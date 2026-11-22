using System.Collections.Generic;

namespace Inno.Core.Serialization.Tests;

internal sealed class MissingClassCollectionHost : ISerializable
{
    [SerializableProperty]
    public List<MissingChild> children { get; set; } = [];
}
