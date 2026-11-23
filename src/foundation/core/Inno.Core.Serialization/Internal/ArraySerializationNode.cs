using System.Collections.Generic;

namespace Inno.Core.Serialization;

internal sealed record ArraySerializationNode : SerializationNode
{
    internal ArraySerializationNode()
        : this([]) {}

    internal ArraySerializationNode(List<SerializationNode> values)
    {
        this.values = values;
    }

    internal List<SerializationNode> values { get; }
}
