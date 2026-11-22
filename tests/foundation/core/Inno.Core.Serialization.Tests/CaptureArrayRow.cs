using Inno.Core.Identity;

namespace Inno.Core.Serialization.Tests;

internal sealed class CaptureArrayRow : IdentityObject, ISerializable
{
    internal int number;
    internal int writes;
    internal ulong? revision = 0;
    internal ConvertedCaptureProperties state = new();
}
