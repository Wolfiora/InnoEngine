namespace Inno.Core.Serialization.Tests;

internal sealed class CycleValue
{
    public CycleValue? next { get; set; }
}
