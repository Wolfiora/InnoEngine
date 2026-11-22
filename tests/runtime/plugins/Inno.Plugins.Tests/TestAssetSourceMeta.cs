using System;
using Inno.Core.Serialization;

namespace Inno.Plugins.Authoring.Tests;

internal sealed class TestAssetSourceMeta : ISerializable
{
    [SerializableProperty]
    public Guid persistentId { get; set; }

    [SerializableProperty]
    public int sourceKind { get; set; }

    [SerializableProperty]
    public string importerId { get; set; } = string.Empty;

    [SerializableProperty]
    public byte[] importerSettingsBytes { get; set; } = [];
}
