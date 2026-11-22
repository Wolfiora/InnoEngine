using Inno.Core.Serialization;

namespace Inno.Editor.Inspection.Tests;

internal sealed class TextStateOwner : ISerializable
{
    [SerializableProperty]
    public TextStateValue value { get; set; } = new("Unset");
}
