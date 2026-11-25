using System;
using Inno.Core.Serialization;

namespace Inno.Rendering.Assets.Authoring.Tests;

internal sealed class RenderingImportSettingsEnvelope : ISerializable
{
    [SerializableProperty]
    public Guid stableTypeId { get; set; }

    [SerializableProperty]
    public byte[] properties { get; set; } = [];

    [SerializableProperty]
    public Guid[] dependencies { get; set; } = [];
}
