namespace Inno.Extensibility.Types;

internal interface ITypeRegistry
{
    /// <summary>
    /// Builds and validates candidate state without changing the active generation.
    /// </summary>
    /// <param name="types">
    /// The active type catalog generation used for extension resolution.
    /// </param>
    /// <returns>
    /// The validated itype registry transaction that represents the completed operation.
    /// </returns>
    ITypeRegistryTransaction Prepare(TypeCacheSnapshot types);
}
