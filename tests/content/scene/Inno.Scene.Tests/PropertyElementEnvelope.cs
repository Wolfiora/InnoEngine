using System;
using Inno.Core.Serialization;

namespace Inno.Scene.Tests;

/// <summary>
/// Creates valid and deliberately invalid current element envelopes through the public serializer.
/// </summary>
internal sealed class PropertyElementEnvelope : ISerializable
{
    [SerializableProperty]
    public Guid typeId { get; set; }

    [SerializableProperty]
    public string typeName { get; set; } = string.Empty;

    [SerializableProperty]
    public byte[] properties { get; set; } = [];

    [SerializableProperty]
    public Guid[] dependencyIds { get; set; } = [];

    [SerializableProperty]
    public Guid[] dependencyTypes { get; set; } = [];

    [SerializableProperty]
    public string[] dependencyPaths { get; set; } = [];

    [SerializableProperty]
    public Guid[] aliasIds { get; set; } = [];

    [SerializableProperty]
    public Guid[] aliasTargets { get; set; } = [];
}
