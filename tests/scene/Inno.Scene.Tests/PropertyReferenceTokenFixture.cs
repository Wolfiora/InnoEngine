using System;
using Inno.Core.Serialization;

namespace Inno.Scene.Tests;

internal sealed class PropertyReferenceTokenFixture : ISerializable
{
    [SerializableProperty]
    public int kind { get; set; }

    [SerializableProperty]
    public Guid sourceId { get; set; }
}
