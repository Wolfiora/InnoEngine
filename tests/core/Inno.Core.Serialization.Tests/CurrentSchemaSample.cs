namespace Inno.Core.Serialization.Tests;

internal sealed class CurrentSchemaSample : ISerializable
{
    [SerializableProperty]
    public string changed { get; set; } = "default";

    [SerializableProperty(PropertyVisibility.Hide)]
    public string compatible { get; set; } = "new";

    [SerializableProperty]
    public int added { get; set; } = 17;

    public int restoreCount { get; private set; }

    [OnSerializableRestored]
    private void AfterRestore() => restoreCount++;
}
