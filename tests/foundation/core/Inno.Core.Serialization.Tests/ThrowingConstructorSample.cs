using System;

namespace Inno.Core.Serialization.Tests;

internal sealed class ThrowingConstructorSample : ISerializable
{
    private ThrowingConstructorSample()
    {
        throw new ApplicationException("constructor failure");
    }
}
