using Inno.Extensibility.Types;
using Reloadable.PrivateDependency;

namespace Inno.Extensibility.Modules.TestModule;

/// <summary>
/// Supplies a replacement collectible implementation while retaining the first fixture generation's stable identity.
/// </summary>
[StableTypeId("44a4cda2-a03e-4918-8db2-f37048a9e4f1")]
public sealed class ReloadableExtension
{
    /// <summary>
    /// Gets the private dependency's base value plus one, making the replacement generation observable.
    /// </summary>
    public int version => VersionSource.baseVersion + 1;
}
