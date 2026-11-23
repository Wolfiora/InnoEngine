namespace Inno.Core.Serialization.Tests;

internal sealed class ConvertedHookSample : ISerializable
{
    public int value { get; set; }
    public int hookCount { get; private set; }

    [OnSerializableRestored]
    private void AfterRestore() => hookCount++;
}
