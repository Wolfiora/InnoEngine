using Inno.Core.Serialization;

namespace Inno.Editor.Scripting.Tests;

internal sealed class InlineOwner : ISerializable
{
    [SerializableProperty]
    public InlineParent value { get; set; } = new();
}
