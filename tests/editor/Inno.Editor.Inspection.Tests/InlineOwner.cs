using Inno.Core.Serialization;

namespace Inno.Editor.Inspection.Tests;

internal sealed class InlineOwner : ISerializable
{
    [SerializableProperty]
    public InlineParent value { get; set; } = new();
}
