namespace Inno.Core.Serialization.Tests;

internal sealed class HookDerivedSample : HookBaseSample
{
    [OnSerializableRestored]
    private void AfterDerivedRestore() => calls.Add("derived");
}
