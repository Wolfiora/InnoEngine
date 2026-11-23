namespace Inno.Core.Serialization;

internal sealed record NullSerializationNode : SerializationNode
{
    private NullSerializationNode() {}

    internal static NullSerializationNode instance { get; } = new();
}
