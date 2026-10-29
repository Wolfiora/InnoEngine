using System;
using System.Linq;
using Inno.Assets;
using Inno.Core.Serialization;
using Xunit;

namespace Inno.Scene.Tests;

public sealed partial class SceneSerializationTests
{
    [Fact]
    public void ElementPropertyAliasesResolveThroughTheCurrentSceneIdentityOwner()
    {
        var scene = new GameScene("Property aliases");
        PropertyReferenceProbe probe = scene.CreateObject("Owner").AddComponent<PropertyReferenceProbe>();
        GameObject original = scene.CreateObject("Original");
        GameObject replacement = scene.CreateObject("Replacement");
        probe.targetObject = original;
        IAssetReferenceResolver assets = m_serializationContext.GetRequired<IAssetReferenceResolver>();
        byte[] source = SceneElementSerialization.CaptureState(probe, m_serialization, assets);
        // The fixture uses the ordinary serializer and verifies its complete current envelope before mutation.
        PropertyElementEnvelope envelope = m_serialization.Deserialize<PropertyElementEnvelope>(source, m_serializationContext);
        Assert.Equal(m_types.GetTypeRef(typeof(PropertyReferenceProbe)).stableId, envelope.typeId);
        probe.targetObject = null;
        SceneElementSerialization.RestoreState(probe, m_serialization.Serialize(envelope, m_serializationContext), m_serialization, assets);
        Assert.Same(original, probe.targetObject);
        envelope.aliasIds = [original.identity.persistentId];
        envelope.aliasTargets = [replacement.identity.persistentId];
        scene.DestroyObject(original);
        probe.targetObject = null;
        SceneElementSerialization.RestoreState(probe, m_serialization.Serialize(envelope, m_serializationContext), m_serialization, assets);
        Assert.Same(replacement, probe.targetObject);
        Assert.NotSame(original, probe.targetObject);
    }

    [Fact]
    public void ElementPropertyAliasesRejectCollisionsMissingTargetsAndMalformedMetadata()
    {
        var scene = new GameScene("Invalid property aliases");
        PropertyReferenceProbe probe = scene.CreateObject("Owner").AddComponent<PropertyReferenceProbe>();
        GameObject original = scene.CreateObject("Original");
        GameObject replacement = scene.CreateObject("Replacement");
        probe.targetObject = original;
        IAssetReferenceResolver assets = m_serializationContext.GetRequired<IAssetReferenceResolver>();
        PropertyElementEnvelope envelope = m_serialization.Deserialize<PropertyElementEnvelope>(
            SceneElementSerialization.CaptureState(probe, m_serialization, assets), m_serializationContext);
        envelope.aliasIds = [original.identity.persistentId];
        envelope.aliasTargets = [replacement.identity.persistentId];
        probe.targetObject = null;
        Exception collision = Assert.ThrowsAny<Exception>(Restore);
        Assert.Contains("Duplicate scene graph local identity", collision.ToString());
        Assert.Null(probe.targetObject);
        envelope.aliasIds = [Guid.NewGuid()];
        envelope.aliasTargets = [Guid.NewGuid()];
        Exception missing = Assert.ThrowsAny<Exception>(Restore);
        Assert.Contains("targets missing object", missing.ToString());
        Assert.Null(probe.targetObject);
        envelope.aliasIds = [Guid.Empty];
        envelope.aliasTargets = [replacement.identity.persistentId];
        Exception malformed = Assert.ThrowsAny<Exception>(Restore);
        Assert.Contains("invalid type, dependency or alias metadata", malformed.ToString());
        Assert.Null(probe.targetObject);

        void Restore()
            => SceneElementSerialization.RestoreState(probe, m_serialization.Serialize(envelope, m_serializationContext), m_serialization, assets);
    }

    [Fact]
    public void ElementPropertyRestoreRejectsReferencesOfTheWrongCurrentComponentType()
    {
        var scene = new GameScene("Wrong property reference type");
        GameObject owner = scene.CreateObject("Owner");
        PropertyReferenceProbe probe = owner.AddComponent<PropertyReferenceProbe>();
        ReferenceComponent target = owner.AddComponent<ReferenceComponent>();
        probe.targetComponent = probe;
        IAssetReferenceResolver assets = m_serializationContext.GetRequired<IAssetReferenceResolver>();
        PropertyElementEnvelope envelope = m_serialization.Deserialize<PropertyElementEnvelope>(
            SceneElementSerialization.CaptureState(target, m_serialization, assets), m_serializationContext);
        envelope.properties = ScenePropertySerialization.CaptureProperty(probe, nameof(PropertyReferenceProbe.targetComponent), m_serialization, assets);
        Exception failure = Assert.ThrowsAny<Exception>(() => SceneElementSerialization.RestoreState(
            target, m_serialization.Serialize(envelope, m_serializationContext), m_serialization, assets));
        Assert.Contains("targetComponent", failure.ToString());
        Assert.Null(target.targetComponent);
    }

    [Fact]
    public void PropertyRestoreRejectsAnInvalidReferenceKindBeforeAssigningTheTarget()
    {
        var scene = new GameScene("Invalid reference kind");
        GameObject owner = scene.CreateObject("Owner");
        PropertyReferenceProbe probe = owner.AddComponent<PropertyReferenceProbe>();
        probe.targetComponent = owner.transform;
        IAssetReferenceResolver assets = m_serializationContext.GetRequired<IAssetReferenceResolver>();
        SerializationPropertySnapshot property = ScenePropertySerialization.CapturePropertySnapshots(probe, m_serialization, assets)
            .Single(value => value.name == nameof(PropertyReferenceProbe.targetComponent));
        byte[] original = property.data.ToArray();
        PropertyReferenceTokenFixture token = m_serialization.Deserialize<PropertyReferenceTokenFixture>(original, m_serializationContext);
        Assert.Equal(owner.transform.identity.persistentId, token.sourceId);
        token.kind = int.MaxValue;
        byte[] invalid = m_serialization.Serialize(token, m_serializationContext);
        Assert.Equal(original.Length, invalid.Length);
        byte[] payload = ScenePropertySerialization.CaptureProperty(probe, nameof(PropertyReferenceProbe.targetComponent), m_serialization, assets);
        int offset = payload.AsSpan().IndexOf(original);
        Assert.True(offset >= 0, "The public property document did not contain its independently encoded value.");
        invalid.CopyTo(payload.AsSpan(offset, invalid.Length));
        probe.targetComponent = null;
        Exception failure = Assert.ThrowsAny<Exception>(() => ScenePropertySerialization.RestoreProperties(probe, payload, m_serialization, assets));
        Assert.Contains("not a GameComponent reference", failure.ToString());
        Assert.Null(probe.targetComponent);
    }
}

internal sealed class PropertyReferenceTokenFixture : ISerializable
{
    [SerializableProperty]
    public int kind { get; set; }

    [SerializableProperty]
    public Guid sourceId { get; set; }
}

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
