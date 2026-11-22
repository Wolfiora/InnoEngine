namespace Inno.Core.Serialization.Tests;

internal sealed class VariantValue(string value) : IVariantValue<string>
{
    public string value { get; } = value;
}
