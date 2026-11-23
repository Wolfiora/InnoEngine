using Inno.Core.Identity;

namespace Inno.Core.Serialization.Tests;

internal sealed class CaptureBlobRoot : IdentityObject, ISerializable
{
    internal byte[] data = [];
}
