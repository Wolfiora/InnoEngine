using System;
using Inno.Core.Serialization;

namespace Inno.Editor.Scripting.Tests;

internal sealed class SampleIdentitySettings : ISerializable
{
    /// <summary>
    /// Gets or sets one neutral identity stored inside an import settings payload.
    /// </summary>
    [SerializableProperty]
    public Guid referenceId { get; set; }
}
