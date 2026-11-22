namespace Inno.Core.Serialization.Tests;

internal sealed class GenericBox<T>
{
    public GenericBox(T value)
    {
        this.value = value;
    }

    public T value { get; }
}
