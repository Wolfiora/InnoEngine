using System.Collections.Generic;

namespace Inno.Core.Serialization;

internal sealed record MapSerializationNode : SerializationNode
{
    internal List<KeyValuePair<SerializationNode, SerializationNode>> values { get; } = [];
}
