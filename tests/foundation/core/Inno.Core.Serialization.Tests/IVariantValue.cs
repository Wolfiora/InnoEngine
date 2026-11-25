namespace Inno.Core.Serialization.Tests;

internal interface IVariantValue<out T>
{
    T value { get; }
}
